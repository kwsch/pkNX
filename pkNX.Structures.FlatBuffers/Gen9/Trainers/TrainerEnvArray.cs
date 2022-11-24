using FlatSharp.Attributes;
using System.ComponentModel;
#nullable disable
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrainerEnvArray : IFlatBufferArchive<TrainerEnv>
{
    [FlatBufferItem(0)] public TrainerEnv[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrainerEnv
{
    [FlatBufferItem(00)] public string TrEnvId { get; set; }
    [FlatBufferItem(01)] public TalkData TalkData1 { get; set; }
    [FlatBufferItem(02)] public TalkData TalkData2 { get; set; }
    [FlatBufferItem(03)] public TalkData TalkData3 { get; set; }
    [FlatBufferItem(04)] public TalkData TalkData4 { get; set; }
    [FlatBufferItem(05)] public TalkData TalkData5 { get; set; }
    [FlatBufferItem(06)] public TalkData TalkData6 { get; set; }
    [FlatBufferItem(07)] public TalkData TalkData7 { get; set; }
    [FlatBufferItem(08)] public TalkData TalkData8 { get; set; }
    [FlatBufferItem(09)] public TalkData TalkData9 { get; set; }
    [FlatBufferItem(10)] public TalkData TalkData10 { get; set; }
    [FlatBufferItem(11)] public TalkData TalkData11 { get; set; }
    [FlatBufferItem(12)] public TalkData TalkData12 { get; set; }
    [FlatBufferItem(13)] public TalkData TalkData13 { get; set; }
    [FlatBufferItem(14)] public TalkData TalkData14 { get; set; }
    [FlatBufferItem(15)] public TalkData TalkData15 { get; set; }
    [FlatBufferItem(16)] public string IntroTml { get; set; }
    [FlatBufferItem(17)] public string ThrowTml { get; set; }
    [FlatBufferItem(18)] public string CameraObjectName { get; set; }
    [FlatBufferItem(19)] public string LoseTml { get; set; }
    [FlatBufferItem(20)] public string IntroObjectName { get; set; }
    [FlatBufferItem(21)] public string LoseObjectName { get; set; }
    [FlatBufferItem(22)] public string bgmEventName { get; set; }
}
