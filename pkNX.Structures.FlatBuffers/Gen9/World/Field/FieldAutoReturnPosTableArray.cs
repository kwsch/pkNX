using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldAutoReturnPosTableArray : IFlatBufferArchive<FieldAutoReturnPosTable>
{
    [FlatBufferItem(0)] public FieldAutoReturnPosTable[] Table{ get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldAutoReturnPosTable
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public float PosX { get; set; }
    [FlatBufferItem(2)] public float PosY { get; set; }
    [FlatBufferItem(3)] public float PosZ { get; set; }
}
