using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PopupWindowTableArray : IFlatBufferArchive<PopupWindowTable>
{
    [FlatBufferItem(0)] public PopupWindowTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PopupWindowTable
{
    [FlatBufferItem(0)] public string Id { get; set; }
    [FlatBufferItem(1)] public string Label { get; set; }
    [FlatBufferItem(2)] public WindowTypeEnum WindowType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum WindowTypeEnum
{
    normal = 0,
    burst = 1,
    thinking = 2,
    sub_quest = 3,
    signboard = 4,
    trainer_normal = 5,
    trainer_strong = 6,
}
