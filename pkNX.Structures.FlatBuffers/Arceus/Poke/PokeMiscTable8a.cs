using FlatSharp.Attributes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeMiscTable8a : IFlatBufferArchive<PokeMisc8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokeMisc8a[] Table { get; set; } = Array.Empty<PokeMisc8a>();

    public PokeMisc8a GetEntry(ushort species, ushort form)
    {
        return Array.Find(Table, z => z.Species == species && z.Form == form) ??
            new PokeMisc8a { Value = $"{species}-{form} is not in {nameof(PokeMiscTable8a)}." };
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public PokeMisc8a AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The misc table already contains an entry for the same species + form!");

        var entry = new PokeMisc8a { Species = species, Form = form };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeMisc8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public int Form { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public float ScaleFactor { get; set; } = 1.0f; // scale factor when not alpha
    [FlatBufferItem(04)] public float AlphaScaleFactor { get; set; } = 2.0f; // scale factor when alpha (forced 255)
    [FlatBufferItem(05)] public int Field_05 { get; set; } // = 1; // similar scale amplification like Field 6, but not used for levels
    [FlatBufferItem(06)] public int OybnLevelIndex { get; set; } = 2;

    [FlatBufferItem(07)][TypeConverter(typeof(DropTableConverter))] public ulong DropTableRef { get; set; }
    public PokeDropItem8a? DropTable { get; set; }
    [FlatBufferItem(08)][TypeConverter(typeof(DropTableConverter))] public ulong AlphaDropTableRef { get; set; }
    public PokeDropItem8a? AlphaDropTable { get; set; }

    [FlatBufferItem(09)] public string Value { get; set; } = "pm0000_00_00";
    [FlatBufferItem(10)] public uint[] Field_10 { get; set; } = { 0, 1, 2, 3 };
    [FlatBufferItem(11)] public int Field_11 { get; set; } = 0;
    [FlatBufferItem(12)] public int Field_12 { get; set; } = 4;
    [FlatBufferItem(13)] public int Field_13 { get; set; } = 10;
    [FlatBufferItem(14)] public bool Field_14 { get; set; } = false;
    [FlatBufferItem(15)] public int Field_15 { get; set; } = 0;
    [FlatBufferItem(16)] public float Field_16 { get; set; } = 0.01f;
    [FlatBufferItem(17)] public float Field_17 { get; set; } = 1.0f;
    [FlatBufferItem(18)] public bool Field_18 { get; set; } = true; // false for zubat, psyduck
    [FlatBufferItem(19)] public float Field_19 { get; set; } = 0;
    [FlatBufferItem(20)] public float Field_20 { get; set; } = 1.0f;
    [FlatBufferItem(21)] public uint Field_21 { get; set; } // None have this
    [FlatBufferItem(22)] public float Field_22 { get; set; }
}
