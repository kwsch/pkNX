using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDataFull
{
    [FlatBufferItem(00)] public DevID DevId { get; set; }
    [FlatBufferItem(01)] public short FormId { get; set; }
    [FlatBufferItem(02)] public ItemID Item { get; set; }
    [FlatBufferItem(03)] public int Level { get; set; }
    [FlatBufferItem(04)] public SexType Sex { get; set; }
    [FlatBufferItem(05)] public SeikakuType Seikaku { get; set; }
    [FlatBufferItem(06)] public SeikakuType SeikakuHosei { get; set; }
    [FlatBufferItem(07)] public TokuseiType Tokusei { get; set; }
    [FlatBufferItem(08)] public RareType RareType { get; set; }
    [FlatBufferItem(09)] public int RareTryCount { get; set; }
    [FlatBufferItem(10)] public TalentType TalentType { get; set; }
    [FlatBufferItem(11)] public ParamSet TalentValue { get; set; }
    [FlatBufferItem(12)] public sbyte TalentVnum { get; set; }
    [FlatBufferItem(13)] public ParamSet EffortValue { get; set; }
    [FlatBufferItem(14)] public int Friendship { get; set; }
    [FlatBufferItem(15)] public SizeType HeightType { get; set; }
    [FlatBufferItem(16)] public short HeightValue { get; set; }
    [FlatBufferItem(17)] public SizeType WeightType { get; set; }
    [FlatBufferItem(18)] public short WeightValue { get; set; }
    [FlatBufferItem(19)] public SizeType ScaleType { get; set; }
    [FlatBufferItem(20)] public short ScaleValue { get; set; }
    [FlatBufferItem(21)] public bool SetPersonalRand { get; set; }
    [FlatBufferItem(22)] public ulong PersonalRand { get; set; }
    [FlatBufferItem(23)] public bool SetRandSeed { get; set; }
    [FlatBufferItem(24)] public ulong RandSeed { get; set; }
    [FlatBufferItem(25)] public WazaType WazaType { get; set; }
    [FlatBufferItem(26)] public WazaSet Waza1 { get; set; }
    [FlatBufferItem(27)] public WazaSet Waza2 { get; set; }
    [FlatBufferItem(28)] public WazaSet Waza3 { get; set; }
    [FlatBufferItem(29)] public WazaSet Waza4 { get; set; }
    [FlatBufferItem(30)] public bool UseNickName { get; set; }
    [FlatBufferItem(31)] public ulong NicknameLabel { get; set; }
    [FlatBufferItem(32)] public ulong ParentNameLabel { get; set; }
    [FlatBufferItem(33)] public SexType ParentSex { get; set; }
    [FlatBufferItem(34)] public LangType ParentLangId { get; set; }
    [FlatBufferItem(35)] public int ParentMemoryCode { get; set; }
    [FlatBufferItem(36)] public int ParentMemoryData { get; set; }
    [FlatBufferItem(37)] public int ParentMemoryFeel { get; set; }
    [FlatBufferItem(38)] public int ParentMemoryLevel { get; set; }
    [FlatBufferItem(39)] public LangType LangId { get; set; }
    [FlatBufferItem(40)] public BallType BallId { get; set; }
    [FlatBufferItem(41)] public RibbonType SetRibbon { get; set; }
    [FlatBufferItem(42)] public bool EventFlg { get; set; }
    [FlatBufferItem(43)] public GemType GemType { get; set; }
    [FlatBufferItem(44)] public sbyte WazaConfirmLevel { get; set; }
    [FlatBufferItem(45)] public PokeMemoType PokeMemo { get; set; }
    [FlatBufferItem(46)] public int PokeMemoPlace { get; set; }
    [FlatBufferItem(47)] public long TrainerId { get; set; }
}
