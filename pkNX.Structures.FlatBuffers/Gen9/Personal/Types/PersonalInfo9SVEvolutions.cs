using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVEvolutions
{
    [FlatBufferItem(0)] public ushort Level { get; set; }
    [FlatBufferItem(1)] public ushort Method { get; set; }
    [FlatBufferItem(2)] public ushort Argument { get; set; }
    [FlatBufferItem(3)] public ushort Reserved3 { get; set; }
    [FlatBufferItem(4)] public ushort Reserved4 { get; set; }
    [FlatBufferItem(5)] public ushort Reserved5 { get; set; }
    [FlatBufferItem(6)] public ushort SpeciesInternal { get; set; }
    [FlatBufferItem(7)] public ushort Form { get; set; }
}
