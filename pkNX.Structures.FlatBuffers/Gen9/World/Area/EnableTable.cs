using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class EnableTable
{
    [FlatBufferItem(0)] public bool Land { get; set; }
    [FlatBufferItem(1)] public bool UpWater { get; set; }
    [FlatBufferItem(2)] public bool Underwater { get; set; }
    [FlatBufferItem(3)] public bool Air1 { get; set; }
    [FlatBufferItem(4)] public bool Air2 { get; set; }
}
