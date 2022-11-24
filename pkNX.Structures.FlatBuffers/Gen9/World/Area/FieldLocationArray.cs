using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldLocationArray : IFlatBufferArchive<FieldLocation>
{
    [FlatBufferItem(0)] public FieldLocation[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldLocation
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public AreaInfo AreaInfo { get; set; }
}
