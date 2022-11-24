using FlatSharp.Attributes;
using System.ComponentModel;
#nullable disable
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrainerTypeArray : IFlatBufferArchive<TrainerType>
{
    [FlatBufferItem(0)] public TrainerType[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrainerType
{
    [FlatBufferItem(00)] public string NameLabel { get; set; }
    [FlatBufferItem(01)] public string MsgLabel { get; set; }
    [FlatBufferItem(02)] public Sex Sex { get; set; }
    [FlatBufferItem(03)] public TrainerCategory Category { get; set; }
    [FlatBufferItem(04)] public string IntroTml { get; set; }
    [FlatBufferItem(05)] public string ThrowTml { get; set; }
    [FlatBufferItem(06)] public string CameraObjectName { get; set; }
    [FlatBufferItem(07)] public string LoseTml { get; set; }
    [FlatBufferItem(08)] public string IntroObjectName { get; set; }
    [FlatBufferItem(09)] public string LoseObjectName { get; set; }
    [FlatBufferItem(10)] public string BGMEventName { get; set; }
    [FlatBufferItem(11)] public TrainerBodySize TrainerBodySize { get; set; }
}
