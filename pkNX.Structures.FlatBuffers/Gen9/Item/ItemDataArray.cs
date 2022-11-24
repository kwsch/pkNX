using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemDataArray : IFlatBufferArchive<ItemData>
{
    [FlatBufferItem(0, Required = true)] public ItemData[] Table { get; set; } = Array.Empty<ItemData>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemData
{
    [FlatBufferItem(00)] public int Id { get; set; }
    [FlatBufferItem(01)] public ItemType ItemType { get; set; }
    [FlatBufferItem(02, Required = true)] public string IconName { get; set; } = string.Empty;
    [FlatBufferItem(03)] public int Price { get; set; }
    [FlatBufferItem(04)] public int BP { get; set; }
    [FlatBufferItem(05)] public EquipEffect EquipEffect { get; set; }
    [FlatBufferItem(06)] public int EquipPower { get; set; }
    [FlatBufferItem(07)] public int ThrowPower { get; set; }
    [FlatBufferItem(08)] public bool ThrowEffect { get; set; }
    [FlatBufferItem(09)] public int NaturalGiftPower { get; set; }
    [FlatBufferItem(10)] public int NaturalGiftType { get; set; }
    [FlatBufferItem(11)] public PluckEffect PluckEffect { get; set; }
    [FlatBufferItem(12)] public WazaID MachineWaza { get; set; }
    [FlatBufferItem(13)] public int SortNum { get; set; }
    [FlatBufferItem(14)] public ItemGroup ItemGroup { get; set; }
    [FlatBufferItem(15)] public int GroupID { get; set; }
    [FlatBufferItem(16)] public FieldPocket FieldPocket { get; set; }
    [FlatBufferItem(17)] public FieldFunctionType FieldFunctionType { get; set; }
    [FlatBufferItem(18)] public BattleFunctionType BattleFunctionType { get; set; }
    [FlatBufferItem(19)] public bool BattleUseLost { get; set; }
    [FlatBufferItem(20)] public bool BattleBagSelect { get; set; }
    [FlatBufferItem(21)] public bool BattleBagSelectTarget { get; set; }
    [FlatBufferItem(22)] public bool NoSpend { get; set; }
    [FlatBufferItem(23)] public bool SetToPoke { get; set; }
    [FlatBufferItem(24)] public int SlotMaxNum { get; set; }
    [FlatBufferItem(25)] public WorkType WorkType { get; set; }
    [FlatBufferItem(26)] public int WorkCommon { get; set; }
    [FlatBufferItem(27)] public int WorkEffectGuard { get; set; }
    [FlatBufferItem(28)] public int WorkCritical { get; set; }
    [FlatBufferItem(29)] public int WorkAttack { get; set; }
    [FlatBufferItem(30)] public int WorkDefense { get; set; }
    [FlatBufferItem(31)] public int WorkSpeed { get; set; }
    [FlatBufferItem(32)] public int WorkAccuracy { get; set; }
    [FlatBufferItem(33)] public int WorkSpAttack { get; set; }
    [FlatBufferItem(34)] public int WorkSpDefense { get; set; }
    [FlatBufferItem(35)] public int WorkLevel { get; set; }
    [FlatBufferItem(36)] public WorkPpSelTgt WorkPpSelTgt { get; set; }
    [FlatBufferItem(37)] public int WorkPpRcv { get; set; }
    [FlatBufferItem(38)] public int WorkPpUp { get; set; }
    [FlatBufferItem(39)] public int WorkStatusLimitCtrl { get; set; }
    [FlatBufferItem(40)] public int WorkStatusHp { get; set; }
    [FlatBufferItem(41)] public int WorkStatusAtk { get; set; }
    [FlatBufferItem(42)] public int WorkStatusDef { get; set; }
    [FlatBufferItem(43)] public int WorkStatusSpd { get; set; }
    [FlatBufferItem(44)] public int WorkStatusSAtk { get; set; }
    [FlatBufferItem(45)] public int WorkStatusSDef { get; set; }
    [FlatBufferItem(46)] public int WorkFriendly1 { get; set; }
    [FlatBufferItem(47)] public int WorkFriendly2 { get; set; }
    [FlatBufferItem(48)] public int WorkFriendly3 { get; set; }
    [FlatBufferItem(49)] public int WorkRecvSleep { get; set; }
    [FlatBufferItem(50)] public int WorkRecvPoison { get; set; }
    [FlatBufferItem(51)] public int WorkRecvBurn { get; set; }
    [FlatBufferItem(52)] public int WorkRecvFreeze { get; set; }
    [FlatBufferItem(53)] public int WorkRecvParalyze { get; set; }
    [FlatBufferItem(54)] public int WorkRecvConfuse { get; set; }
    [FlatBufferItem(55)] public int WorkRecvMero { get; set; }
    [FlatBufferItem(56)] public int WorkRecvPower { get; set; }
    [FlatBufferItem(57)] public int WorkRevival { get; set; }
    [FlatBufferItem(58)] public int WorkEvolutional { get; set; }
    [FlatBufferItem(59)] public int WorkRecvNemuke { get; set; }
    [FlatBufferItem(60)] public int WorkRecvTousyou { get; set; }
    [FlatBufferItem(61)] public int WorkWazaDrunk { get; set; }
    [FlatBufferItem(62)] public int WorkAvoidUp { get; set; }
    [FlatBufferItem(63)] public int WorkOffenseUp { get; set; }
    [FlatBufferItem(64)] public int WorkOffDefInv { get; set; }
}
