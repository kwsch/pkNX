using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ConditionType8a : ulong
{
    FlagComparison = 0x3DDD5FEB4A68A149, // "flag_comparison
    FlagComparisonMultiAnd = 0x1474C7C46A813955, // "flag_comparison_multi_and"
    FlagComparisonMultiOr = 0x99A60B9D95356789, // "flag_comparison_multi_or"
    Force = 0x344570C1301A7584, // "force"
    MkrgCondition = 0xF8ADF49E06E61C30, // "mkrg_condition
    PhaseCondition = 0x8E77D62D1E38721E, // "phase_condition"
    PokeGetSelf = 0x7AAB4D6FEBAC4C0E, // "poke_get_self"
    ProgressWorkCondition = 0x43C4820F12B5385C, // "progress_work_condition"
    Probability = 0x5E53C6367A2D36BC, // "probability"
    QuestProgressCondition = 0x2041C674206FE0A7, // "quest_progress_condition"
    WorkCondition = 0x2557279B949C81D6, // "work_condition
    None = 0xCBF29CE484222645, // ""
}
