using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Waza9EntityStat
{
    [FlatBufferItem(0), Browsable(false)] public sbyte FStat1 { get; set; }
    [FlatBufferItem(1), Browsable(false)] public sbyte FStat2 { get; set; }
    [FlatBufferItem(2), Browsable(false)] public sbyte FStat3 { get; set; }
    [FlatBufferItem(3), Browsable(false)] public sbyte FStat1Stage { get; set; }
    [FlatBufferItem(4), Browsable(false)] public sbyte FStat2Stage { get; set; }
    [FlatBufferItem(5), Browsable(false)] public sbyte FStat3Stage { get; set; }
    [FlatBufferItem(6), Browsable(false)] public byte FStat1Percent { get; set; }
    [FlatBufferItem(7), Browsable(false)] public byte FStat2Percent { get; set; }
    [FlatBufferItem(8), Browsable(false)] public byte FStat3Percent { get; set; }
}
