using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LineupDataArray : IFlatBufferArchive<LineupData>
{
    [FlatBufferItem(0)] public LineupData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LineupData
{
    [FlatBufferItem(0)] public string Lineupid { get; set; }
    [FlatBufferItem(1)] public int Sortnum { get; set; }
    [FlatBufferItem(2)] public ItemID Item { get; set; }
    [FlatBufferItem(3)] public CondEnum ItemCondkind { get; set; }
    [FlatBufferItem(4)] public string ItemCondvalue { get; set; }
    [FlatBufferItem(5)] public int GymBadgeNum { get; set; }
}
