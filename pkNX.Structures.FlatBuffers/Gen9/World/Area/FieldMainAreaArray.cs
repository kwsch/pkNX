using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldMainAreaArray : IFlatBufferArchive<FieldMainArea>
{
    [FlatBufferItem(0)] public FieldMainArea[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldMainArea
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public AreaInfo AreaInfo { get; set; }
}
