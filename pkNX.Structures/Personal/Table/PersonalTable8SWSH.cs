using System;
using System.Diagnostics;

namespace pkNX.Structures;

/// <summary>
/// Personal Table storing <see cref="PersonalTable8SWSH"/> used in <see cref="GameVersion.SWSH"/>.
/// </summary>
public sealed class PersonalTable8SWSH : IPersonalTable, IPersonalTable<PersonalInfo8SWSH>
{
    public PersonalInfo8SWSH[] Table { get; }
    private const int SIZE = PersonalInfo8SWSH.SIZE;
    public int MaxSpeciesID => Legal.MaxSpeciesID_8;

    public PersonalTable8SWSH(ReadOnlySpan<byte> data)
    {
        Table = data.GetArray(x => new PersonalInfo8SWSH(x.ToArray()), SIZE);
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public PersonalInfo8SWSH this[int index] => Table[(uint)index < Table.Length ? index : 0];
    public PersonalInfo8SWSH this[ushort species, byte form] => Table[GetFormIndex(species, form)];
    public PersonalInfo8SWSH GetFormEntry(ushort species, byte form) => Table[GetFormIndex(species, form)];

    public int GetFormIndex(ushort species, byte form)
    {
        if ((uint)species <= MaxSpeciesID)
            return Table[species].FormIndex(species, form);
        return 0;
    }

    public bool IsSpeciesInGame(ushort species)
    {
        if ((uint)species > MaxSpeciesID)
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
        if ((uint)species > MaxSpeciesID)
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
        int sFormCount = 0;
        for (ushort i = 1; i <= Legal.MaxSpeciesID_7_USUM; i++)
        {
            var s = Table[i];
            s.DexIndexNational = i;

            var u = ResourcesUtil.USUM.Table[i];

            if (s.FormCount == 0)
            {
                s.SetPersonalInfo(u);
                s.FormCount = Math.Max((byte)1, FormInfo.GetOutOfBattleFormCount(i, u.FormCount, 7));
            }

            if (s.FormCount == 1)
                continue;

            if (s.FormStatsIndex != 0)
                Debug.Assert(s.FormStatsIndex == (MaxSpeciesID + 1) + sFormCount);

            s.FormStatsIndex = (MaxSpeciesID + 1) + sFormCount;
            sFormCount += s.FormCount - 1;

            for (byte f = 1; f < s.FormCount; f++)
            {
                var formS = Table[s.FormStatsIndex + (f - 1)];

                if (formS.FormCount == 0)
                {
                    // Check if USUM table has form data for this entry
                    if (f < u.FormCount)
                    {
                        if (u.FormCount <= s.FormCount || (FormInfo.HasTotemForm(i) && !FormInfo.IsTotemForm(i, f)))
                        {
                            var formU = ResourcesUtil.USUM.GetFormEntry(i, f);
                            formS.SetPersonalInfo(formU);
                        }
                    }
                    else
                    {
                        // No form data was found, just write the base form data
                        formS.SetPersonalInfo(s);
                    }

                    formS.FormCount = s.FormCount;
                    formS.FormStatsIndex = s.FormStatsIndex;
                }

                formS.DexIndexNational = i;
                formS.Form = f;
            }
        }

        // Fix form number
        for (ushort i = Legal.MaxSpeciesID_7_USUM; i <= MaxSpeciesID; i++)
        {
            var s = Table[i];
            s.DexIndexNational = i;

            for (byte f = 1; f < s.FormCount; f++)
            {
                var formS = Table[s.FormStatsIndex + (f - 1)];
                formS.DexIndexNational = i;
                formS.Form = f;
            }
        }

        Debug.WriteLine("Auto fix for SWSH data succeded");
    }
}
