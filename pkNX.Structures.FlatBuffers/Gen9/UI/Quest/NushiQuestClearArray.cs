using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NushiQuestClearArray : IFlatBufferArchive<NushiQuestClear>
{
    [FlatBufferItem(0)] public NushiQuestClear[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NushiQuestClear
{
    [FlatBufferItem(00)] public NushiQuestClearNo ID { get; set; }
    [FlatBufferItem(01)] public string NushiClearQuestType { get; set; }
    [FlatBufferItem(02)] public string TrackName { get; set; }
    [FlatBufferItem(03)] public string EventNpcSceneName1 { get; set; }
    [FlatBufferItem(04)] public string EventNpcObjectName1 { get; set; }
    [FlatBufferItem(05)] public string EventNpcSceneName2 { get; set; }
    [FlatBufferItem(06)] public string EventNpcObjectName2 { get; set; }
    [FlatBufferItem(07)] public string EventNpcSceneName3 { get; set; }
    [FlatBufferItem(08)] public string EventNpcObjectName3 { get; set; }
    [FlatBufferItem(09)] public string EventNpcSceneName4 { get; set; }
    [FlatBufferItem(10)] public string EventNpcObjectName4 { get; set; }
    [FlatBufferItem(11)] public string EventNpcSceneName5 { get; set; }
    [FlatBufferItem(12)] public string EventNpcObjectName5 { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum NushiQuestClearNo
{
    NONE = 0,
    IWA = 1,
    HAGANE = 2,
    JIMEN = 3,
    HIKOU = 4,
    DRAGON = 5,
}
