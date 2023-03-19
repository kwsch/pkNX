using System;
using System.Collections;
using pkNX.Containers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlatSharp;

namespace pkNX.Structures.FlatBuffers.SV;

/// <summary>
/// Personal Table storing <see cref="PersonalInfo9SV"/> used in <see cref="GameVersion.SV"/>.
/// </summary>
public sealed class PersonalTable9SV : IPersonalTable, IPersonalTable<PersonalInfo9SV>
{
    public PersonalInfo9SV[] Table { get; }
    private const ushort MaxSpecies = Legal.MaxSpeciesID_9;
    public int MaxSpeciesID => MaxSpecies;

    private readonly IFileContainer File;
    public PersonalTable Root { get; }

    public PersonalTable9SV(IFileContainer file)
    {
        File = file;
        Root = PersonalTable.Serializer.Parse(file[0], FlatBufferDeserializationOption.GreedyMutable);

        var baseForms = new PersonalInfo9SV[MaxSpecies + 1];
        var formTable = new List<PersonalInfo9SV>();

        var formGrouped = Root.Table
            .GroupBy(x => x.Info.SpeciesNational)
            .OrderBy(x => x.Key).ToArray();

        for (int i = 0; i <= MaxSpecies; i++)
        {
            var item = formGrouped[i];
            var forms = item.ToArray();

            baseForms[i] = GetObj(forms[0], forms, MaxSpecies, formTable);
            for (int f = 1; f < forms.Length; f++)
                formTable.Add(GetObj(forms[f], forms, MaxSpecies, formTable, f));
        }

        Table = baseForms.Concat(formTable).ToArray();
    }

    private static PersonalInfo9SV GetObj(PersonalInfo e, ICollection forms, ushort max, ICollection formTable, int f = 0)
    {
        return new PersonalInfo9SV(e)
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

    public PersonalInfo9SV this[int index] => Table[(uint)index < Table.Length ? index : 0];
    public PersonalInfo9SV this[ushort species, byte form] => Table[GetFormIndex(species, form)];
    public PersonalInfo9SV GetFormEntry(ushort species, byte form) => Table[GetFormIndex(species, form)];

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

    public byte[] Write()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        foreach (var entry in Table)
            entry.Write(bw);
        return ms.ToArray();
    }
}
