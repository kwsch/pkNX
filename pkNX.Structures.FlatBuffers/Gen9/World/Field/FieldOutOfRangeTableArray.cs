using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldOutOfRangeTableArray : IFlatBufferArchive<FieldOutOfRangeTable>
{
    [FlatBufferItem(0)] public FieldOutOfRangeTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldOutOfRangeTable
{
    [FlatBufferItem(0)] public float OutOfRangeLowerX { get; set; }
    [FlatBufferItem(1)] public float OutOfRangeLowerY { get; set; }
    [FlatBufferItem(2)] public float OutOfRangeLowerZ { get; set; }
    [FlatBufferItem(3)] public float OutOfRangeUpperX { get; set; }
    [FlatBufferItem(4)] public float OutOfRangeUpperY { get; set; }
    [FlatBufferItem(5)] public float OutOfRangeUpperZ { get; set; }
}
