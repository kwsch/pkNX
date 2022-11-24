using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVDex
{
    [FlatBufferItem(0)] public ushort Index { get; set; }
    [FlatBufferItem(1)] public byte   Group { get; set; }
}
