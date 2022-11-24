using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

// Root Type: gfl.audio.fb.BGMEventArray

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BGMEventArray : IFlatBufferArchive<BGMEvent>
{
    [FlatBufferItem(0)] public BGMEvent[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BGMEvent
{
    [FlatBufferItem(0)] public string EventName { get; set; }
    [FlatBufferItem(1)] public BGMEventType Type { get; set; }
    [FlatBufferItem(2)] public string LayerName { get; set; }
    [FlatBufferItem(3)] public string WwiseEventName { get; set; }
}
