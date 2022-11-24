using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class OverrideBiome
{
    [FlatBufferItem(0)] public FieldAttrKey Attr1 { get; set; }
    [FlatBufferItem(1)] public EncBiome Biome1 { get; set; }
    [FlatBufferItem(2)] public FieldAttrKey Attr2 { get; set; }
    [FlatBufferItem(3)] public EncBiome Biome2 { get; set; }
    [FlatBufferItem(4)] public FieldAttrKey Attr3 { get; set; }
    [FlatBufferItem(5)] public EncBiome Biome3 { get; set; }
}
