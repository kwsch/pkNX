using FlatSharp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace pkNX.Structures.FlatBuffers;

// resident_release.bin -> bin/field/param/placement/common/pokemon_model_set.bin
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelSet8a : IFlatBufferArchive<PokeModelSetEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeModelSetEntry8a[] Table { get; set; } = Array.Empty<PokeModelSetEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelSetEntry8a
{
    [FlatBufferItem(00)] public ulong PokeModelHash { get; set; }
    [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(02)] public uint Species { get; set; }
    [FlatBufferItem(03)] public uint Form { get; set; }
    [FlatBufferItem(04)] public string VariantDesc { get; set; } = string.Empty;
    [FlatBufferItem(05)] public bool Field_05 { get; set; } // Species 77 is true
    [FlatBufferItem(06)] public bool Field_06 { get; set; } // Species 95 is true
    [FlatBufferItem(07)] public bool Field_07 { get; set; } // Species 95 is true
    [FlatBufferItem(08)] public uint Field_08 { get; set; } // 1, 127 or 255  Buizel has an entry for both. Single entry with 0 (Ponyta)
}
