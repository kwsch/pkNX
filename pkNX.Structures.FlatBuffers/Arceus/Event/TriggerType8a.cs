using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum TriggerType8a : ulong
{
    AreaChange = 0x7E23ECD43CE201A1, // "area_change"
    BattleLose = 0x054B378194CDEB91, // "battle_lose"
    BattleWin = 0xB2EE750DBCD8FFD6, // "battle_win"
    BgEvent = 0x64224581923358E9, // "bg_event"
    BootUrankUpgrade = 0x77E1D0135FEDD0E0, // "boot_urank_upgrade"
    CaptureFailedPoke = 0xC09BBA82558CCB11, // "capture_failed_poke"
    CapturedEventPoke = 0x82DCDEC11E4B902C, // "captured_event_poke"
    CapturedPoke = 0xF9DCAFF7B9A4A1B5, // "captured_poke"
    DigHit = 0xDEDAFE9CD0903841, // "dig_hit"
    DoorEvent = 0xF8026805B0390A1E, // "door_event"
    EventBattleLose = 0x956AB57BCBE24AC2, // "event_battle_lose"
    EventBattleWin = 0x78891EA1EB1719B7, // "event_battle_win"
    FinishCraTutorial = 0xCB28FF671A2BF67C, // "finish_cra_tutorial"
    FinishEventKisekaeHome = 0xEC5C94E0F6AF0025, // "finish_event_kisekae_home"
    FinishEventScript = 0x2E632ED350D2BC4D, // "finish_event_script"
    FinishFastTravel = 0x114FE10EEDD9D9BE, // "finish_fast_travel"
    FinishNushiBattle = 0x3B78EA5ACF12B68F, // "finish_nushi_battle"
    FinishRideMiniGame = 0x43264D2AEB95F420, // "finish_ride_mini_game"
    FinishSelectArea = 0xAB29E81B22E196F1, // "finish_select_area"
    FinishThrowMiniGame = 0x474D107BF9969EDC, // "finish_throw_mini_game"
    GameOver = 0x57BA8020944AD222, // "game_over"
    GameOverAfterward = 0x5F2F50327A48756D, // "game_over_afterward"
    GetMkrg = 0x9FE3F0D2163599C7, // "get_mkrg"
    NpcTalk = 0xAB00E1A6F5D27687, // "npc_talk"
    PokeSelectMenuAfter = 0x3C6FC069B47DBF4C, // "poke_select_menu_after"
    PosEnter = 0xBD5314EC693575D4, // "pos_enter"
    PosExit = 0xC01E67CA2AAD4DFA, // "pos_exit"
    PosStay = 0xDB75191D739E26DF, // "pos_stay"
    ThrowNg = 0x63CFDE902FA005E7, // "throw_ng"
    TrainerBanditLose = 0xA4E0055DFAB6E157, // "trainer_bandit_lose"
    TrainerBanditWin = 0x2E11B56C5A55E9E8, // "trainer_bandit_win"
    TrainerBattleLose = 0x7DACDFC0F3964597, // "trainer_battle_lose"
    TrainerBattleWin = 0xB7942BC60882A228, // "trainer_battle_win"
    WildPokeAvoidThrowable = 0xCAAD203D7ABFF4F6, // "wild_poke_avoid_throwable"
    WildPokeEscape = 0xE22F524AAE64337F, // "wild_poke_escape"
    WildPokeHitThrowable = 0x4A182F2782FFD34E, // "wild_poke_hit_throwable"

    None = 0xCBF29CE484222645, // ""
}
