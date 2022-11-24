using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Waza9Inflict
{
    [FlatBufferItem(00)] public ushort Value { get; set; }
    [FlatBufferItem(01)] public byte Chance { get; set; }
    [FlatBufferItem(02)] public byte Turn1 { get; set; }
    [FlatBufferItem(03)] public byte Turn2 { get; set; }
    [FlatBufferItem(04)] public byte Turn3 { get; set; }
}
