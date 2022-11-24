using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GymQuestClearArray : IFlatBufferArchive<GymQuestClear>
{
    [FlatBufferItem(0)] public GymQuestClear[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GymQuestClear
{
    [FlatBufferItem(0)] public GymQuestClearNo ID { get; set; }
    [FlatBufferItem(1)] public string EventNpcSceneName { get; set; }
    [FlatBufferItem(2)] public string EventNpcObjName { get; set; }
    [FlatBufferItem(3)] public string GymClearQuestType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum GymQuestClearNo
{
    NONE = 0,
    MUSHI = 1,
    KUSA = 2,
    MIZU = 3,
    DENKI = 4,
    NORMAL = 5,
    KOORI = 6,
    GHOST = 7,
    ESPER = 8,
}
