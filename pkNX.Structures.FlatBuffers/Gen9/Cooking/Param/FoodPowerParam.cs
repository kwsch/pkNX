using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class FoodPowerParam
{
    [FlatBufferItem(0)] public int EGG { get; set; }
    [FlatBufferItem(1)] public int CAPTURE { get; set; }
    [FlatBufferItem(2)] public int EXP { get; set; }
    [FlatBufferItem(3)] public int LOSTPROPERTY { get; set; }
    [FlatBufferItem(4)] public int RAID { get; set; }
    [FlatBufferItem(5)] public int ANOTHERNAME { get; set; }
    [FlatBufferItem(6)] public int RARE { get; set; }
    [FlatBufferItem(7)] public int GIGANT { get; set; }
    [FlatBufferItem(8)] public int MIINIMUM { get; set; }
    [FlatBufferItem(9)] public int ENCOUNT { get; set; }
}
