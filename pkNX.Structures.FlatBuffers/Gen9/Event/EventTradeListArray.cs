using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventTradeListArray : IFlatBufferArchive<EventTradeList>
{
    [FlatBufferItem(0)] public EventTradeList[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventTradeList
{
    [FlatBufferItem(0)] public string Label { get; set; }
    [FlatBufferItem(1)] public string ReceivePoke { get; set; }
    [FlatBufferItem(2)] public DevID SendPokeDevId { get; set; }
    [FlatBufferItem(3)] public short SendPokeFormId { get; set; }
}
