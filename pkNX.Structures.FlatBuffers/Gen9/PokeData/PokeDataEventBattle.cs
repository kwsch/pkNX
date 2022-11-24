using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDataEventBattle
{
    [FlatBufferItem(00)] public DevID DevId { get; set; }
    [FlatBufferItem(01)] public short FormId { get; set; }
    [FlatBufferItem(02)] public SexType Sex { get; set; }
    [FlatBufferItem(03)] public int Level { get; set; }
    [FlatBufferItem(04)] public RareType RareType { get; set; }
    [FlatBufferItem(05)] public TalentType TalentType { get; set; }
    [FlatBufferItem(06)] public sbyte TalentVnum { get; set; }
    [FlatBufferItem(07)] public ParamSet TalentValue { get; set; }
    [FlatBufferItem(08)] public ParamSet EffortValue { get; set; }
    [FlatBufferItem(09)] public ItemID Item { get; set; }
    [FlatBufferItem(10)] public ItemID DropItem { get; set; }
    [FlatBufferItem(11)] public sbyte DropItemNum { get; set; }
    [FlatBufferItem(12)] public SeikakuType Seikaku { get; set; }
    [FlatBufferItem(13)] public SeikakuType SeikakuHosei { get; set; }
    [FlatBufferItem(14)] public TokuseiType Tokusei { get; set; }
    [FlatBufferItem(15)] public WazaType WazaType { get; set; }
    [FlatBufferItem(16)] public WazaSet Waza1 { get; set; }
    [FlatBufferItem(17)] public WazaSet Waza2 { get; set; }
    [FlatBufferItem(18)] public WazaSet Waza3 { get; set; }
    [FlatBufferItem(19)] public WazaSet Waza4 { get; set; }
    [FlatBufferItem(20)] public GemType GemType { get; set; }
    [FlatBufferItem(21)] public SizeType ScaleType { get; set; }
    [FlatBufferItem(22)] public short ScaleValue { get; set; }
    [FlatBufferItem(23)] public RibbonType SetRibbon { get; set; }
}
