using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum TriggerCommandType8a : ulong
{
    AgunomuThrowEnd = 0xFA1DFCCE926904E0, // "agunomu_throw_end"
    AgunomuThrowStart = 0xB0F3255086108A01, // "agunomu_throw_start"
    AdventureResultCall = 0x3D79D6D0058D4D92, // "adventure_result_call" // UNUSED
    AdventureResultInit = 0xE716459F5EBE2E94, // "adventure_result_init"
    AiChange = 0x909AE490410B6058, // "ai_change" // UNUSED
    AiChangeTarget = 0xC0A167CE4DAEFCA8, // "ai_change_target" // UNUSED
    AllPokeRecover = 0x35AE11B202B33FAB, // "all_poke_recover"
    AutoSaveNg = 0x396A9D10B0C06938, // "auto_save_ng"
    AutoSaveNgReset = 0x5A23A70C90B2E3C0, // "auto_save_ng_reset"
    BagCall = 0x633E78A0C793496E, // "bag_call" // UNUSED
    BagLostCall = 0x8CF3D91B280B82C9, // "bag_lost_call"
    BgmPlay = 0x1EBC09E17E96F774, // "bgm_play"
    BgmStop = 0xD5092EDB9DAFA972, // "bgm_stop"
    BgmLock = 0x672CF23B33EA0505, // "bgm_lock" // UNUSED
    BgmUnlock = 0xF428A39FF598D116, // "bgm_unlock"
    BoutiqueCall = 0x8212CBCB399C7F54, // "boutique_call"
    CallBattle = 0xB54D19CA9F2B27A8, // "call_battle"
    CallEvent = 0x466C5DFFFA6047DE, // "call_event"
    CallRealTimeEvent = 0x1608695374913BA9, // "call_real_time_event"
    CallStaffroll = 0x39A9DB57ED643E63, // "call_staffroll" // UNUSED
    CallTips = 0x5B249F4C1C8A2800, // "call_tips" // UNUSED
    CallTrainerBattle = 0x81FE18C0B68BFC12, // "call_trainer_battle"
    CallTriggerBoot = 0xA6C73C7E028A667B, // "call_trigger_boot"
    ClearVigilance = 0xD458C18CE2A7E801, // "clear_vigilance" // UNUSED
    CraCall = 0x8520F45674D19E44, // "cra_call"
    CreateFieldObjectRequest = 0x0D9BB98942D0BCF6, // "create_field_object_request"
    CreateFieldObjectRequestCategory = 0x4E3742139786576F, // "create_field_object_request_category"
    CreateFieldObjectRequestCategoryExecute = 0xF6D071052A9A65DF, // "create_field_object_request_category_execute" // UNUSED
    CreateFieldObjectRequestSet = 0xA7C532B0D5AF6F59, // "create_field_object_request_set"
    DemoPlay = 0xC68C92A921029B81, // "demo_play"
    DemoPlayD220 = 0xB651089382810DEA, // "demo_play_d220"
    EasyItemEvent = 0xCDCB1E4485BD64AC, // "easy_item_event" // UNUSED
    EventBattleHookEnd = 0xEF0F31CCE66E7CBA, // "event_battle_hook_end" // UNUSED
    EndNushiBattle = 0xF4F2BE33D270D1F7, // "end_nushi_battle"
    FadeIn = 0xFB3E07E0FFD1B25B, // "fade_in"
    FadeOut = 0xA9BA3A52A69DD7A8, // "fade_out
    FadeWait = 0xECC548B7B9448DD5, // "fade_wait"
    FinishRealTimeEvent = 0x6C0A8D2940D8F34A, // "finish_real_time_event"
    FlagReset = 0x861AC906050FBC07, // "flag_reset"
    FlagSet = 0x2719CB6B0BCC8398, // "flag_set"
    ForceReport = 0x949ED0E99A38A7AF, // "force_report"
    ForceReportExecute = 0x65852F23188ED61F, // "force_report_execute" // UNUSED
    HugeOutbreakClear = 0x5B3F92D4849AB3AE, // "huge_outbreak_clear"
    HugeOutbreakLottery = 0x1D29BC9A7BA7D242, // "huge_outbreak_lottery"
    ImageShow = 0xF8218ADC727F3F92, // "image_show" // UNUSED
    ItemAdd = 0x6D35E2EAB45C0F6E, // "item_add"
    ItemBoxCall = 0x2D01C6B3EE7EC58D, // "item_box_call"
    ItemDelete = 0xC59A0CC763D2A7A8, // "item_delete"
    ItemGet = 0x7F6616EABEE84FB5, // "item_get" // UNUSED
    ItemSelectCall = 0x79C506A26ABE4C0C, // "item_select_call" // UNUSED
    ItemSub = 0xD20CF4EA5CB0BEEF, // "item_sub"
    LeaveWildPokemon = 0x635B9BF9C7B6BEA3, // "leave_wild_pokemon"
    MapChange = 0x1223758E1F82495E, // "map_change"
    MapChangeGameOver = 0xA93595533A427B98, // "map_change_game_over"
    MapChangeDoorId = 0xF688862E367CF429, // "map_change_door_id"
    MoneyAdd = 0x5195731EE2BCA693, // "money_add"
    MoneyGet = 0x8496771EFF5A370C, // "money_get" // UNUSED
    MoneySub = 0xD73D391E9D2276B2, // "money_sub" // UNUSED
    NpcTalkCall = 0x4393E4FD10599008, // "npc_talk_call" // UNUSED
    PhaseUpdate = 0x0145D669ED9CCD14, // "phase_update" // UNUSED
    PlayerAnimationStateReset = 0x3221105676A499B3, // "player_animation_state_reset"
    PlayerPinchRecover = 0x836C7B20B1BC4A14, // "player_pinch_recover"
    PlayerWarp = 0x4CC0659046F12EF7, // "player_warp" // UNUSED
    PlayerWarpXyz = 0x5F5F59AA1A454253, // "player_warp_xyz"
    PlayreportAddvalue = 0x36BD72700FE8335A, // "playreport_addvalue" // UNUSED
    PlayreportSetvalue = 0x09FBF53A8261403F, // "playreport_setvalue" // UNUSED
    PokeAdd = 0x82E032DE25CE78E0, // "poke_add"
    PokeBoxCall = 0x1F24A4FCF859527B, // "poke_box_call"
    PokePassEvolution = 0x36C4EA8E0C07226A, // "poke_pass_evolution
    PokePassEvolutionAdaptation = 0xF6BD3323D7EC7F2E, // "poke_pass_evolution_adaptation"
    PokemonSelectCall = 0xE69AEB1CB355CFD4, // "pokemon_select_call" // UNUSED
    PokeSelectMenuCall = 0xE75986350917856A, // "poke_select_menu_call"
    PokeSelectResultAdaptation = 0x9CEE05CE2A7DF249, // "poke_select_result_adaptation"
    QuestActivationConditionCheck = 0xDA1ECE532B4484D3, // "quest_activation_condition_check" // UNUSED
    QuestAdvance = 0x1F9C392CEFBB1B58, // "quest_advance"
    QuestAdvanceExecute = 0x3739AA4DFE7142D8, // "quest_advance_execute" // UNUSED
    QuestCal = 0x91530CCB04EA81A2, // "quest_cal" // UNUSED, Typo?
    QuestEnd = 0x7F22B0CAFA5DFD63, // "quest_end"
    QuestProgressSet = 0x8F4ECBA903BE3CD2, // "quest_progress_set"
    QuestStart = 0xACDCFB9B749A3D26, // "quest_start"
    ResetWormhole = 0xEEE8243BDF3948B0, // "reset_wormhole"
    ResidentWorkClear = 0x908FC6969AB5250D, // "resident_work_clear"
    SelectAreaCall = 0xDA71107BF9807C04, // "select_area_call"
    SetMydressupItem = 0x28CF7A6F7273496A, // "set_mydressup_item" // UNUSED
    SetDressupItem = 0x2D7A8BD9982C8840, // "set_dressup_item"
    SetFocusItem = 0x3F669C91AF847B32, // "set_focus_item"
    SetFocusMyPoke = 0xF6CC19EBA86582D5, // "set_focus_mypoke" // UNUSED
    SetThrowItem = 0xDBEF9BBF00919F52, // "set_throw_item" // UNUSED
    ShopCall = 0x9E3A5EB597D7F7A6, // "shop_call"
    ShowExpResult = 0x6C9C2C4BF7EC3138, // "show_exp_result"
    SimpleMessageChara = 0xEA08F03627CA535F, // "simple_message_chara"
    SimpleMessagePokemon = 0x512653C97AF7F269, // "simple_message_pokemon" // UNUSED
    SimpleMessageSystem = 0x388689E19B68D6F9, // "simple_message_system"
    SleepWormhole = 0x6C50ACC5A3018C28, // "sleep_wormhole"
    SoundEventPost = 0x53D11252A6F42576, // "sound_event_post"
    SoundPostSaveStory = 0x2BAED33B647A894F, // "sound_post_save_story"
    SoundPostSaveVillage = 0x58D65502484672BE, // "sound_post_save_village"
    SoundSetRtpc = 0xAE39DFCB98B7358B, // "sound_set_rtpc" // UNUSED
    SoundSetState = 0xA4C6918F875B40FD, // "sound_set_state" // UNUSED
    SoundSituationMixer = 0x82EBFDB54C8107A5, // "sound_situation_mixer"
    SpawnWildPokemon = 0x39A6CB761A05967B, // "spawn_wild_pokemon"
    StartNsAi = 0xC2FC48F79D5F53BA, // "start_ns_ai" // UNUSED
    StartNushiBattle = 0x081BF500D5709C9C, // "start_nushi_battle"
    StartWormhole = 0x2156F9E8ED44730D, // "start_wormhole"
    TerrainSuspend = 0xAEC1A8869FBFFAB1, // "terrain_suspend"
    TimeChange = 0x48D69D8788B2FE61, // "time_change"
    TimeLock = 0xF7911B293C7F980E, // "time_lock"
    TimeRelease = 0x6A28CB95A4BBFF62, // "time_release"
    TriggerCommandPreset = 0x8CD4CD78EA36EF55, // "trigger_command_preset"
    WeatherOverrideEnd = 0x0B590FB0E4757ECE, // "weather_override_end"
    WeatherOverrideStart = 0xD7000A2DA20F2AEB, // "weather_override_start
    WeatherOverrideStartNowDepending = 0xE4B9EFCF8B1793D1, // "weather_override_start_now_depending"
    WorkAdd = 0xB956CAA67E5EB58C, // "work_add"
    WorkSet = 0x555FCEA6D6C83ADB, // "work_set"

    None = 0xCBF29CE484222645, // ""
}
