using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NetBattleRuleDataArray : IFlatBufferArchive<NetBattleRuleData>
{
    [FlatBufferItem(0)] public NetBattleRuleData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NetBattleRuleData
{
    [FlatBufferItem(00)] public int BattleRuleId { get; set; }
    [FlatBufferItem(01)] public int BattleteamPokeMin { get; set; }
    [FlatBufferItem(02)] public int BattleteamPokeMax { get; set; }
    [FlatBufferItem(03)] public int SelectPokeMin { get; set; }
    [FlatBufferItem(04)] public int SelectPokeMax { get; set; }
    [FlatBufferItem(05)] public int JoinLevel { get; set; }
    [FlatBufferItem(06)] public LevelRateRule LevelRateRule { get; set; }
    [FlatBufferItem(07)] public UseCheck SamePokeCheck { get; set; }
    [FlatBufferItem(08)] public UseCheck SameItemCheck { get; set; }
    [FlatBufferItem(09)] public TimeLimitType TimelimitType { get; set; }
    [FlatBufferItem(10)] public int LimitTime { get; set; }
    [FlatBufferItem(11)] public int WaitTime { get; set; }
    [FlatBufferItem(12)] public int DecideTime { get; set; }
    [FlatBufferItem(13)] public PokeShowType PokeShowType { get; set; }
    [FlatBufferItem(14)] public int PokeShowTime { get; set; }
    [FlatBufferItem(15)] public GemModeLimit GemModeFlag { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum GemModeLimit
{
    USABLE = 0,
    PROHIBITION = 1,
}

[FlatBufferEnum(typeof(int))]
public enum LevelRateRule
{
    NONE = 0,
    OR_MORE = 1,
    OR_LESS = 2,
    OR_MORE_REVISE = 3,
    ALL_REVISE = 4,
    OR_LESS_REVISE = 5,
}

[FlatBufferEnum(typeof(int))]
public enum PokeShowType
{
    NOT_SHOW = 0,
    SHOW = 1,
}

[FlatBufferEnum(typeof(int))]
public enum TimeLimitType
{
    TOTAL = 0,
    WAIT = 1,
    TOTALWAIT = 2,
}

[FlatBufferEnum(typeof(int))]
public enum UseCheck
{
    USABLE = 0,
    NOT_USE = 1,
}
