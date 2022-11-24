using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SystemBgArray : IFlatBufferArchive<SystemBg>
{
    [FlatBufferItem(0)] public SystemBg[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SystemBg
{
    [FlatBufferItem(0)] public IdSystemBg ID { get; set; }
    [FlatBufferItem(1)] public bool IsValid { get; set; }
    [FlatBufferItem(2)] public int Priority { get; set; }
    [FlatBufferItem(3)] public BgType BgType { get; set; }
    [FlatBufferItem(4)] public PtclType PtclType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum BgType
{
    Non = 0,
    Common = 1,
    Fill = 2,
    BG_Box = 3,
    BG_Net = 4,
    BG_Title = 5,
    BG_YMap = 6,
    BG_Btlspot1 = 7,
    BG_Btlspot2 = 8,
    BG_Btlspot3 = 9,
    BG_Btlspot4 = 10,
    BG_ShopOther = 11,
    BG_BtlStadiumTop = 12,
    ForceTransparent = 13,
    BG_Shop2 = 14,
    BG_Shop3 = 15,
    BG_Shop4 = 16,
    BG_Shop5 = 17,
}

[FlatBufferEnum(typeof(int))]
public enum IdSystemBg
{
    NONE = 0,
    msg = 1,
    msg_sys = 2,
    msg_btl = 4,
    msg_dem = 5,
    msg_book = 6,
    msg_itemget = 7,
    title_menu = 11,
    btl_omakase = 13,
    popup_window = 14,
    emote = 15,
    field_btl_trainer = 16,
    ui_placename = 17,
    xmenu = 18,
    pokelist = 19,
    pokelist_btl = 20,
    bag = 21,
    status = 22,
    box = 26,
    ymap = 27,
    hud_notice = 28,
    schoolmap = 29,
    hud_minimap = 30,
    pokedex = 31,
    ymap_distribution = 32,
    pokedex_register = 33,
    leaguecard = 34,
    btl_app = 35,
    btl_raid = 38,
    team_circle = 42,
    result = 44,
    photomode = 45,
    net_topmenu = 46,
    net_btl = 49,
    btlspot1 = 57,
    btlspot_casual = 58,
    btlspot_rank = 59,
    btlspot_competition = 60,
    btlspot_rental = 63,
    mystery = 65,
    badge_get = 70,
    okozukai = 74,
    shop = 75,
    shop_lp = 76,
    shop_wazamachine = 77,
    hairsalon = 78,
    dressup = 79,
    syoujou = 80,
    ichimaie = 82,
    event_skip = 90,
    quest_start = 91,
    quest_clear = 92,
    msg_call = 94,
    pokepicnic_main = 96,
    pokepicnic_cooking = 97,
    msg_sub = 100,
    btl_dan = 101,
    hud_itemget = 102,
    boutique = 103,
    hud_announce = 104,
    event_gym_koori = 105,
    event_gym_denki = 106,
    event_gym_esp = 107,
    fukidashiicon = 108,
    msg_noise = 109,
    btlspot2 = 110,
    btlspot3 = 111,
    btlspot4 = 112,
    tipslist = 113,
    btl_app_info = 114,
    other_shop = 115,
    option = 116,
    vsdemo_1 = 117,
    vsdemo_2 = 118,
    vsdemo_3 = 119,
    vsdemo_4 = 120,
    btlstadiumtop1 = 121,
    btlstadiumtop2 = 122,
    btlstadiumtop3 = 123,
    btlstadiumtop4 = 124,
    rental_team = 125,
    battle_field = 126,
    delibird = 127,
    delicatessen = 128,
    drug_store = 129,
    kanzume = 130,
    bakery = 131,
    supermarket = 132,
    koubai = 133,
    picnic = 134,
    resutaurant = 135,
    luxuryRestaurant = 136,
}

[FlatBufferEnum(typeof(int))]
public enum PtclType
{
    None = 0,
    General = 1,
    Btlspot1 = 2,
    Btlspot2 = 3,
    Btlspot3 = 4,
    Btlspot4 = 5,
    VsDemo1 = 6,
    VSDemo2 = 7,
    VSDemo3 = 8,
    VSDemo4 = 9,
}
