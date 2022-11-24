using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemPointTypeBiomeTableArray : IFlatBufferArchive<ItemPointTypeBiomeTable>
{
    [FlatBufferItem(0)] public ItemPointTypeBiomeTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemPointTypeBiomeTable
{
    [FlatBufferItem(0)] public string Biome { get; set; }
    [FlatBufferItem(1)] public float WeightHiddenItem { get; set; }
    [FlatBufferItem(2)] public float WeightRummagingItem { get; set; }
}

