using FlatSharp.Attributes;
using System;
using System.ComponentModel;

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

    public PokeMisc8a GetEntry(int species, int form)
    {
        return Array.Find(Table, z => z.Species == species && z.Form == form) ??
            new PokeMisc8a { Value = $"{species}-{form} is not in {nameof(PokeMiscTable8a)}." };
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeMisc8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public int Form { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public float ScaleFactor { get; set; } // scale factor when not alpha
    [FlatBufferItem(04)] public float AlphaScaleFactor { get; set; } // scale factor when alpha (forced 255)
    [FlatBufferItem(05)] public int Field_05 { get; set; } // similar scale amplification like Field 6, but not used for levels
    [FlatBufferItem(06)] public int OybnLevelIndex { get; set; }

    [FlatBufferItem(07)][TypeConverter(typeof(DropTableConverter))] public ulong DropTableRef { get; set; }
    public PokeDropItem8a? DropTable { get; set; }
    [FlatBufferItem(08)][TypeConverter(typeof(DropTableConverter))] public ulong AlphaDropTableRef { get; set; }
    public PokeDropItem8a? AlphaDropTable { get; set; }

    [FlatBufferItem(09)] public string Value { get; set; } = string.Empty;
    [FlatBufferItem(10)] public int[] Field_10 { get; set; } = Array.Empty<int>();
    [FlatBufferItem(11)] public int Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public int Field_13 { get; set; }
    [FlatBufferItem(14)] public bool Field_14 { get; set; }
    [FlatBufferItem(15)] public int Field_15 { get; set; }
    [FlatBufferItem(16)] public float Field_16 { get; set; }
    [FlatBufferItem(17)] public float Field_17 { get; set; }
    [FlatBufferItem(18)] public bool Field_18 { get; set; }
    [FlatBufferItem(19)] public float Field_19 { get; set; }
    [FlatBufferItem(20)] public float Field_20 { get; set; }
}
