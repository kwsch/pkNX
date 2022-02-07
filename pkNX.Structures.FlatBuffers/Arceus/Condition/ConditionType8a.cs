using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ConditionType8a : ulong
{
    EventEndFlagComparison = 0xB1D54CA3ACFD951C, // "event_end_flag_comparison"
    FlagComparison = 0x3DDD5FEB4A68A149, // "flag_comparison"
    FlagComparisonMultiAnd = 0x1474C7C46A813955, // "flag_comparison_multi_and"
    FlagComparisonMultiOr = 0x99A60B9D95356789, // "flag_comparison_multi_or"
    Force = 0x344570C1301A7584, // "force"
    IsRide = 0xE1DBDA285AC7E0C4, // "is_ride"
    ItemNum = 0x4218EEEA9BFE8019, // "item_num"
    MkrgCondition = 0xF8ADF49E06E61C30, // "mkrg_condition
    MoonType = 0x9C765A1AAAC34605, // "moon_type"
    NpcPassPokeForm = 0xDD65C7864951449B, // "npc_pass_poke_form"
    NpcPassPokeIndividualNum = 0xCE6FB5F32B6297C5, // "npc_pass_poke_individual_num"
    NpcPassPokeMonsNum = 0x26D399D556EE1C2D, // "npc_pass_poke_mons_num"
    NpcPassPokeOybn = 0x769FC8C156B7CDBD, // "npc_pass_poke_oybn"
    NpcPassPokeSex = 0xABE95BDC028692F3, // "npc_pass_poke_sex"
    NpcPassPokeType = 0xC4BB08F0E0BB1979, // "npc_pass_poke_type"
    NpcPassPokeWaza = 0xF26110F743861738, // "npc_pass_poke_waza"
    PartyMonsNoNum = 0x0B830B240F28E61C, // "party_mons_no_num"
    PartyPokeNum = 0x313CB8E1DEBB01DE, // "party_poke_num"
    PhaseCondition = 0x8E77D62D1E38721E, // "phase_condition"
    PlayerSex = 0xFA75906B85AE950F, // "player_sex"
    PokeGetNum = 0x56E50D419BFB3A2C, // "poke_get_num"
    PokeGetSelf = 0x7AAB4D6FEBAC4C0E, // "poke_get_self"
    PokeTotalNum = 0x892E20D02629F1FA, // "poke_total_num"
    Probability = 0x5E53C6367A2D36BC, // "probability"
    ProgressWorkCondition = 0x43C4820F12B5385C, // "progress_work_condition"
    QuestProgressCondition = 0x2041C674206FE0A7, // "quest_progress_condition"
    TimeZone = 0x46EBE59F660B132B, // "time_zone"
    TimeZoneAfternoon = 0x8E0F436D4E20E310, // "time_zone_afternoon"
    TimeZoneNight = 0xD95B681BFE1BB762, // "time_zone_night"
    WorkCondition = 0x2557279B949C81D6, // "work_condition"
    ZukanResearchCondition = 0x854B2B1FDB9AC9B4, // "zukan_research_condition"
    ZukanStageCondition = 0x9A193E701C7F9A61, // "zukan_stage_condition"

    None = 0xCBF29CE484222645, // ""
}
