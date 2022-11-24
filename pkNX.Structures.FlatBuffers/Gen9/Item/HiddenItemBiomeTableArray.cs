using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemBiomeTableArray : IFlatBufferArchive<HiddenItemBiomeTable>
{
    [FlatBufferItem(0)] public HiddenItemBiomeTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemBiomeTable
{
    [FlatBufferItem(0)] public HiddenItemBiomeKey Key { get; set; }
    [FlatBufferItem(1)] public string TableId { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemBiomeKey
{
    [FlatBufferItem(0)] public int Area { get; set; }
    [FlatBufferItem(1)] public string Biome { get; set; }
}
