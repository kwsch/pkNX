using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupCategoryDataArray : IFlatBufferArchive<DressupCategoryData>
{
    [FlatBufferItem(0)] public DressupCategoryData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupCategoryData
{
    [FlatBufferItem(0)] public string Shopid { get; set; }
    [FlatBufferItem(1)] public bool Uniform { get; set; }
    [FlatBufferItem(2)] public bool Leg { get; set; }
    [FlatBufferItem(3)] public bool Foot { get; set; }
    [FlatBufferItem(4)] public bool Glove { get; set; }
    [FlatBufferItem(5)] public bool Bag { get; set; }
    [FlatBufferItem(6)] public bool Head { get; set; }
    [FlatBufferItem(7)] public bool Eye { get; set; }
    [FlatBufferItem(8)] public bool Rotom { get; set; }
}
