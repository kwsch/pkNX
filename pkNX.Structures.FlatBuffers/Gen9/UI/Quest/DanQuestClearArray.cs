using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DanQuestClearArray : IFlatBufferArchive<DanQuestClear>
{
    [FlatBufferItem(0)] public DanQuestClear[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DanQuestClear
{
    [FlatBufferItem(0)] public DanQuestClearNo ID { get; set; }
    [FlatBufferItem(1)] public string EventNpcSceneName { get; set; }
    [FlatBufferItem(2)] public string EventNpcObjName { get; set; }
    [FlatBufferItem(3)] public string DanClearQuestType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum DanQuestClearNo
{
    NONE = 0,
    HONOO = 1,
    AKU = 2,
    DOKU = 3,
    KAKUTOU = 4,
    FAIRY = 5,
}
