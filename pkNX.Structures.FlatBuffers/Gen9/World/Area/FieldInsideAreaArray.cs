using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldInsideAreaArray : IFlatBufferArchive<FieldInsideArea>
{
    [FlatBufferItem(0)] public FieldInsideArea[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldInsideArea
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public AreaInfo AreaInfo { get; set; }
}
