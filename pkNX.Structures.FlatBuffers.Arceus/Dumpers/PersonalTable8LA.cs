using System.Collections;
using pkNX.Containers;
using System.Diagnostics;
using System.Collections.Immutable;
using FlatSharp;

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary>
/// Personal Table storing <see cref="PersonalInfo8LA"/> used in <see cref="GameVersion.PLA"/>.
/// </summary>
public sealed class PersonalTable8LA : IPersonalTable, IPersonalTable<PersonalInfo8LA>
{
    public PersonalInfo8LA[] Table { get; }
    private const int MaxSpecies = Legal.MaxSpeciesID_8a;
    public int MaxSpeciesID => MaxSpecies;

    private readonly IFileContainer File;
    public PersonalTable Root { get; }

    public PersonalTable8LA(IFileContainer file)
    {
        File = file;
        Root = PersonalTable.Serializer.Parse(file[0], FlatBufferDeserializationOption.GreedyMutable);

        var baseForms = new PersonalInfo8LA[MaxSpecies + 1];
        var formTable = new List<PersonalInfo8LA>();

        var formGrouped = Root.Table
            .OrderBy(z => z.DexIndexNational)
            .ThenBy(z => z.Form)
            .GroupBy(x => x.DexIndexNational)
            .Select(group => group.ToImmutableArray())
            .ToArray();

        for (int i = 0; i <= MaxSpecies; i++)
        {
            var forms = formGrouped[i];

            baseForms[i] = GetObj(forms[0], forms, MaxSpecies, formTable);
            for (int f = 1; f < forms.Length; f++)
                formTable.Add(GetObj(forms[f], forms, MaxSpecies, formTable, f));
        }

        Table = baseForms.Concat(formTable).ToArray();
    }

    private static PersonalInfo8LA GetObj(PersonalInfo e, ICollection forms, ushort max, ICollection formTable, int f = 0)
    {
        return new PersonalInfo8LA(e)
        {
            FormCount = (byte)forms.Count,
            FormStatsIndex = (f != 0 ? 0 : forms.Count == 1 ? 0 : max + formTable.Count + 1),
        };
    }

    public void Save()
    {
        var pool = System.Buffers.ArrayPool<byte>.Shared;
        var serializer = PersonalTable.Serializer;
        var size = serializer.GetMaxSize(Root);
        var arr = pool.Rent(size);
        var len = serializer.Write(arr, Root);
        var data = arr.AsSpan(0, len).ToArray();
        pool.Return(arr);
        File[0] = data;
    }

    public PersonalInfo8LA this[int index] => Table[(uint)index < Table.Length ? index : 0];
    public PersonalInfo8LA this[ushort species, byte form] => Table[GetFormIndex(species, form)];
    public PersonalInfo8LA GetFormEntry(ushort species, byte form) => Table[GetFormIndex(species, form)];

    public int GetFormIndex(ushort species, byte form)
    {
        if ((uint)species <= MaxSpecies)
            return Table[species].FormIndex(species, form);
        return 0;
    }

    public bool IsSpeciesInGame(ushort species)
    {
        if ((uint)species > MaxSpecies)
            return false;

        var form0 = Table[species];
        if (form0.IsPresentInGame)
            return true;

        var fc = form0.FormCount;
        for (byte i = 1; i < fc; i++)
        {
            var entry = GetFormEntry(species, i);
            if (entry.IsPresentInGame)
                return true;
        }
        return false;
    }

    public bool IsPresentInGame(ushort species, byte form)
    {
        if ((uint)species > MaxSpecies)
            return false;

        var form0 = Table[species];
        if (form == 0)
            return form0.IsPresentInGame;
        if (!form0.HasForm(form))
            return false;

        var entry = GetFormEntry(species, form);
        return entry.IsPresentInGame;
    }

    IPersonalInfo[] IPersonalTable.Table => Table;
    IPersonalInfo IPersonalTable.this[int index] => this[index];
    IPersonalInfo IPersonalTable.this[ushort species, byte form] => this[species, form];
    IPersonalInfo IPersonalTable.GetFormEntry(ushort species, byte form) => GetFormEntry(species, form);

    public void FixMissingData()
    {
        // Fix all base forms
        int laFormCount = 0;
        for (ushort i = 1; i <= Legal.MaxSpeciesID_8; i++)
        {
            var l = Table[i];
            l.DexIndexNational = i;

            var s = ResourcesUtil.SWSH.Table[i];

            Debug.Assert(l.DexIndexNational == s.DexIndexNational);

            if (l.HP == 0)
                l.SetPersonalInfo(s);

            if (l.FormCount == 1)
                continue;

            var expectFormIndex = MaxSpeciesID + 1 + laFormCount;
            if (l.FormStatsIndex != 0)
                Debug.Assert(l.FormStatsIndex == expectFormIndex);

            l.FormStatsIndex = expectFormIndex;
            laFormCount += l.FormCount - 1;

            for (byte f = 1; f < l.FormCount; f++)
            {
                var formL = Table[l.FormStatsIndex + (f - 1)];
                if (formL.HP != 0)
                    continue;

                // Check if SWSH table has form data for this entry
                if (f < s.FormCount)
                {
                    if (s.FormCount <= l.FormCount || (FormInfo.HasBattleOnlyForm(i) && !FormInfo.IsBattleOnlyForm(i, f, 8)))
                    {
                        var formS = ResourcesUtil.SWSH.GetFormEntry(i, f);

                        Debug.Assert(formL.DexIndexNational == formS.DexIndexNational);
                        formL.SetPersonalInfo(formS);
                    }
                }
                else
                {
                    // No form data was found, just write the base form data
                    formL.SetPersonalInfo(l);
                }
            }
        }

        Debug.WriteLine("Auto fix for PLA data succeeded");
    }
}
