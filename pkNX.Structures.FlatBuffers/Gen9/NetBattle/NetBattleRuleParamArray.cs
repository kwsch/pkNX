using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NetBattleRuleParamArray : IFlatBufferArchive<NetBattleRuleParam>
{
    [FlatBufferItem(0)] public NetBattleRuleParam[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NetBattleRuleParam
{
    [FlatBufferItem(0)] public int BattleRuleId { get; set; }
    [FlatBufferItem(1)] public BattleType BattleType { get; set; }
    [FlatBufferItem(2)] public LevelType LevelType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum LevelType
{
    NORMAL = 0,
    FLAT = 1,
    NON_LIMITED = 2,
}
