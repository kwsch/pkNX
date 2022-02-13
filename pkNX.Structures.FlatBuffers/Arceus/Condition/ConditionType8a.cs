using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ConditionType8a : ulong
{
    BattleWin = 0xB2EE750DBCD8FFD6, // "battle_win" // UNUSED
    EventEndFlagComparison = 0xB1D54CA3ACFD951C, // "event_end_flag_comparison"
    FlagComparison = 0x3DDD5FEB4A68A149, // "flag_comparison"
    FlagComparisonMultiAnd = 0x1474C7C46A813955, // "flag_comparison_multi_and"
    FlagComparisonMultiOr = 0x99A60B9D95356789, // "flag_comparison_multi_or"
    Force = 0x344570C1301A7584, // "force"
    IsRide = 0xE1DBDA285AC7E0C4, // "is_ride"
    ItemNum = 0x4218EEEA9BFE8019, // "item_num"
    MkrgCondition = 0xF8ADF49E06E61C30, // "mkrg_condition
    MoonType = 0x9C765A1AAAC34605, // "moon_type"
    MydressupParts = 0x6E4AB6860240F386, // "mydressup_parts"
    NpcPassPokeForm = 0xDD65C7864951449B, // "npc_pass_poke_form"
    NpcPassPokeIndividualNum = 0xCE6FB5F32B6297C5, // "npc_pass_poke_individual_num"
    NpcPassPokeMonsNum = 0x26D399D556EE1C2D, // "npc_pass_poke_mons_num"
    NpcPassPokeOybn = 0x769FC8C156B7CDBD, // "npc_pass_poke_oybn"
    NpcPassPokeSeikaku = 0x4CED96920003025E, // "npc_pass_poke_seikaku" // UNUSED
    NpcPassPokeSex = 0xABE95BDC028692F3, // "npc_pass_poke_sex"
    NpcPassPokeSizeAbsolute = 0xA6F824297967B404, // "npc_pass_poke_size_absolute" // UNUSED
    NpcPassPokeSizeIndividual = 0x4CED96920003025E, // "npc_pass_poke_size_individual" // UNUSED
    NpcPassPokeType = 0xC4BB08F0E0BB1979, // "npc_pass_poke_type"
    NpcPassPokeWaza = 0xF26110F743861738, // "npc_pass_poke_waza"
    PartyMonsNoNum = 0x0B830B240F28E61C, // "party_mons_no_num"
    PartyPokeNum = 0x313CB8E1DEBB01DE, // "party_poke_num"
    PhaseCondition = 0x8E77D62D1E38721E, // "phase_condition"
    PlayerSex = 0xFA75906B85AE950F, // "player_sex"
    PokeGetNum = 0x56E50D419BFB3A2C, // "poke_get_num"
    PokeGetSelf = 0x7AAB4D6FEBAC4C0E, // "poke_get_self"
    PokeResearchComplete = 0x5B017A2B0C07CD1A, // "poke_research_complete" // UNUSED
    PokeTotalNum = 0x892E20D02629F1FA, // "poke_total_num"
    Probability = 0x5E53C6367A2D36BC, // "probability"
    ProgressWorkCondition = 0x43C4820F12B5385C, // "progress_work_condition"
    ProgressWorkConditionHash = 0x409A7F367B037545, // "progress_work_condition_hash"
    ProgressResidentWorkCondition = 0xD0D514A85FC0B50F, // "progress_resident_work_condition" // UNUSED
    ProgressResidentWorkConditionHash = 0x1C5747049C3B1920, // "progress_resident_work_condition_hash"
    QuestProgressCondition = 0x2041C674206FE0A7, // "quest_progress_condition"
    QuestTaskClearCheck = 0x4BBF494C6CE53FEA, // "quest_task_clear_check" // UNUSED
    ThrowItemCondition = 0x15C4DB49A2D05975, // "throw_item_condition" // UNUSED
    TimeZone = 0x46EBE59F660B132B, // "time_zone"
    TimeZoneAfternoon = 0x8E0F436D4E20E310, // "time_zone_afternoon"
    TimeZoneNight = 0xD95B681BFE1BB762, // "time_zone_night"
    WorkCondition = 0x2557279B949C81D6, // "work_condition"
    ZukanResearchCondition = 0x854B2B1FDB9AC9B4, // "zukan_research_condition"
    ZukanStageCondition = 0x9A193E701C7F9A61, // "zukan_stage_condition"

    None = 0xCBF29CE484222645, // ""
}
