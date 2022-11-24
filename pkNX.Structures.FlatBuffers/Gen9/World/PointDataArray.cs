using FlatSharp.Attributes;
using System.ComponentModel;
#nullable disable
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PointDataArray : IFlatBufferArchive<PointData>
{
    [FlatBufferItem(0)] public PointData[] Table { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PointData
{
    [FlatBufferItem(0)] public SVector3 Position { get; set; }
    [FlatBufferItem(1)] public SVector2 LevelRange { get; set; }
    [FlatBufferItem(2)] public EncBiome Biome { get; set; }
    [FlatBufferItem(3)] public EncAttr Substance { get; set; }
    [FlatBufferItem(4)] public sbyte AreaNo { get; set; }
}
