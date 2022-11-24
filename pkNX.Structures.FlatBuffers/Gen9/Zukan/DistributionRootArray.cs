using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GridParam
{
    [FlatBufferItem(0)] public int X { get; set; }
    [FlatBufferItem(1)] public int Y { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DistributionRootArray : IFlatBufferArchive<DistributionRoot>
{
    [FlatBufferItem(0)] public DistributionRoot[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DistributionRoot : IFlatBufferArchive<DistributionData>
{
    [FlatBufferItem(0)] public DistributionData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DistributionData
{
    [FlatBufferItem(0)] public int DevNo { get; set; }
    [FlatBufferItem(1)] public int FormNo { get; set; }
    [FlatBufferItem(2)] public bool VersionA { get; set; }
    [FlatBufferItem(3)] public bool VersionB { get; set; }
    [FlatBufferItem(4)] public GridParam GridParam { get; set; }
}
