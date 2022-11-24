using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaFlyFlagArray : IFlatBufferArchive<AreaFlyFlag>
{
    [FlatBufferItem(0)] public AreaFlyFlag[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaFlyFlag
{
    [FlatBufferItem(0)] public string AreaName { get; set; }
    [FlatBufferItem(1)] public string FlyFlag1 { get; set; }
    [FlatBufferItem(2)] public string FlyFlag2 { get; set; }
}
