using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDataSymbol
{
    [FlatBufferItem(00)] public DevID DevId { get; set; }
    [FlatBufferItem(01)] public short FormId { get; set; }
    [FlatBufferItem(02)] public int Level { get; set; }
    [FlatBufferItem(03)] public SexType Sex { get; set; }
    [FlatBufferItem(04)] public RareType RareType { get; set; }
    [FlatBufferItem(05)] public TalentType TalentType { get; set; }
    [FlatBufferItem(06)] public ParamSet TalentValue { get; set; }
    [FlatBufferItem(07)] public sbyte TalentVNum { get; set; }
    [FlatBufferItem(08)] public WazaType WazaType { get; set; }
    [FlatBufferItem(09)] public WazaSet Waza1 { get; set; }
    [FlatBufferItem(10)] public WazaSet Waza2 { get; set; }
    [FlatBufferItem(11)] public WazaSet Waza3 { get; set; }
    [FlatBufferItem(12)] public WazaSet Waza4 { get; set; }
    [FlatBufferItem(13)] public TokuseiType TokuseiIndex { get; set; }
    [FlatBufferItem(14)] public SizeType ScaleType { get; set; }
    [FlatBufferItem(15)] public short ScaleValue { get; set; }
    [FlatBufferItem(16)] public GemType GemType { get; set; }
}
