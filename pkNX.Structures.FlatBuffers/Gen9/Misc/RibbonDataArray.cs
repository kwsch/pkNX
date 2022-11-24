using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RibbonDataArray : IFlatBufferArchive<RibbonData>
{
    [FlatBufferItem(0)] public RibbonData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RibbonData
{
    [FlatBufferItem(0)] public int Id { get; set; }
    [FlatBufferItem(1)] public int DispOrder { get; set; }
    [FlatBufferItem(2)] public string NameLabel { get; set; }
    [FlatBufferItem(3)] public string DescLabel { get; set; }
    [FlatBufferItem(4)] public string TitleLabel { get; set; }
}
