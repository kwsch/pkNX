using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TalkData
{
    [FlatBufferItem(0)] public string EventName { get; set; }
    [FlatBufferItem(1)] public CheckCategory Category { get; set; }
    [FlatBufferItem(2)] public int ValueA { get; set; }
    [FlatBufferItem(3)] public int ValueB { get; set; }
    [FlatBufferItem(4)] public int ValueC { get; set; }
    [FlatBufferItem(5)] public TalkRankEffect RankEffect { get; set; }
}
