namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class SingleShop
{
    public override string ToString() => $"{Hash:X16} - {Inventories}";

    public static readonly Dictionary<ulong, string> SWSH = new()
    {
        // Galar
        { 0x1F3FF031A3A24490, "Poké Mart [0 Badges, Before Catching Tutorial]" },
        { 0x8E308F85B43038B4, "Motostoke [Upper Tier, TMs]" },
        { 0x8E309085B4303A67, "Hammerlocke [West, TMs]" },
        { 0x8E309185B4303C1A, "Hammerlocke [East, TMs]" },
        { 0x8E309285B4303DCD, "Wyndon [North, TMs]" },
        { 0x8E308B85B43031E8, "Battle Tower [TMs]" },
        { 0xCBD67969D873539B, "Motostoke [Lower Tier, Miscellaneous]" },
        { 0xCBD67869D87351E8, "Hammerlocke [South, Miscellaneous]" },
        { 0xCBD669D8735701, "Wyndon [South, Miscellaneous]" },
        { 0x04D7046DA09D3C78, "Hulbury [Herb Shop]" },
        { 0x4B2F9E98DDCB0707, "Hulbury [Incense Shop]" },
        { 0xE379CDF67A297070, "Wedgehurst [Berry Shop]" },
        { 0x3FD7A44219BF30BB, "Hammerlocke [South, BP Shop]" },
        { 0x3FD7A34219BF2F08, "Battle Tower [Battle Items]" },
        { 0x3FD7A64219BF3421, "Battle Tower [Nature Mints]" },
        // 0xD1BEA92EAAE52B5A -- Wishing Piece + X Items... unused?

        // Wild Area
        // next 84 tables are all Ingredient Sellers with similar inventories that rotate daily; don't bother labeling (why hardcode everything GF?)
        { 0xD1BEAA2EAAE52D0D, "Watt Trader 1 [Net Ball]" },
        { 0xD1BEA72EAAE527F4, "Watt Trader 1 [Dive Ball]" },
        { 0xD1BEA82EAAE529A7, "Watt Trader 1 [Nest Ball]" },
        { 0xD1BEA52EAAE5248E, "Watt Trader 1 [Repeat Ball]" },
        { 0xD1BEA62EAAE52641, "Watt Trader 1 [Timer Ball]" },
        { 0xD1BEA32EAAE52128, "Watt Trader 1 [Luxury Ball]" },
        { 0xD1BEA42EAAE522DB, "Watt Trader 1 [Dusk Ball]" },
        { 0xD1BEA12EAAE51DC2, "Watt Trader 1 [Heal Ball]" },
        { 0xD1BEA22EAAE51F75, "Watt Trader 1 [Quick Ball]" },

        { 0xD1C20F2EAAE80E83, "Watt Trader 2 [Net Ball]" },
        { 0xD1C20E2EAAE80CD0, "Watt Trader 2 [Dive Ball]" },
        { 0xD1C2112EAAE811E9, "Watt Trader 2 [Nest Ball]" },
        { 0xD1C2102EAAE81036, "Watt Trader 2 [Repeat Ball]" },
        { 0xD1C2132EAAE8154F, "Watt Trader 2 [Timer Ball]" },
        { 0xD1C2122EAAE8139C, "Watt Trader 2 [Luxury Ball]" },
        { 0xD1C2152EAAE818B5, "Watt Trader 2 [Dusk Ball]" },
        { 0xD1C2142EAAE81702, "Watt Trader 2 [Heal Ball]" },
        { 0xD1C2172EAAE81C1B, "Watt Trader 2 [Quick Ball]" },

        { 0xD1C2162EAAE81A68, "Watt Trader 3 [Net Ball]" },
        { 0xD1B79D2EAADEF848, "Watt Trader 3 [Dive Ball]" },
        { 0xD1B79E2EAADEF9FB, "Watt Trader 3 [Nest Ball]" },
        { 0xD1B79F2EAADEFBAE, "Watt Trader 3 [Repeat Ball]" },
        { 0xD1B7A02EAADEFD61, "Watt Trader 3 [Timer Ball]" },
        { 0xD1B7A12EAADEFF14, "Watt Trader 3 [Luxury Ball]" },
        { 0xD1B7A22EAADF00C7, "Watt Trader 3 [Dusk Ball]" },
        { 0xD1B7A32EAADF027A, "Watt Trader 3 [Heal Ball]" },
        { 0xD1B7A42EAADF042D, "Watt Trader 3 [Quick Ball]" },

        { 0xD1B7952EAADEEAB0, "Watt Trader 4 [Net Ball]" },
        { 0xD1B7962EAADEEC63, "Watt Trader 4 [Dive Ball]" },
        { 0xD1BB232EAAE211D1, "Watt Trader 4 [Nest Ball]" },
        { 0xD1BB222EAAE2101E, "Watt Trader 4 [Repeat Ball]" },
        { 0xD1BB212EAAE20E6B, "Watt Trader 4 [Timer Ball]" },
        { 0xD1BB202EAAE20CB8, "Watt Trader 4 [Luxury Ball]" },
        { 0xD1BB272EAAE2189D, "Watt Trader 4 [Dusk Ball]" },
        { 0xD1BB262EAAE216EA, "Watt Trader 4 [Heal Ball]" },
        { 0xD1BB252EAAE21537, "Watt Trader 4 [Quick Ball]" },

        { 0xD1BB242EAAE21384, "Watt Trader 5 [Net Ball]" },
        { 0xD1BB1B2EAAE20439, "Watt Trader 5 [Dive Ball]" },
        { 0xD1BB1A2EAAE20286, "Watt Trader 5 [Nest Ball]" },
        { 0xD1CC212EAAF0819E, "Watt Trader 5 [Repeat Ball]" },
        { 0xD1CC222EAAF08351, "Watt Trader 5 [Timer Ball]" },
        { 0xD1CC1F2EAAF07E38, "Watt Trader 5 [Luxury Ball]" },
        { 0xD1CC202EAAF07FEB, "Watt Trader 5 [Dusk Ball]" },
        { 0xD1CC252EAAF0886A, "Watt Trader 5 [Heal Ball]" },
        { 0xD1CC262EAAF08A1D, "Watt Trader 5 [Quick Ball]" },

        { 0xD1CC232EAAF08504, "Watt Trader 6 [Net Ball]" },
        { 0xD1CC242EAAF086B7, "Watt Trader 6 [Dive Ball]" },
        { 0xD1CC192EAAF07406, "Watt Trader 6 [Repeat Ball]" },
        { 0xD1CC1A2EAAF075B9, "Watt Trader 6 [Quick Ball]" },
        { 0xD1CFA72EAAF39B27, "Watt Trader 6 [Heal Ball]" },

        // Isle of Armor
        { 0x5870C0165650F6A5, "Fields of Honor [Berry Shop]" },

        // Crown Tundra
        { 0x81DA6390A03C7E3F, "Freezington [Peddler]" },
        { 0x813C350B0B777943, "Snowslide Slope [Today's Highlight, TR00-TR09]" },
        { 0x813C360B0B777AF6, "Snowslide Slope [Today's Highlight, TR10-TR19]" },
        { 0x813C370B0B777CA9, "Snowslide Slope [Today's Highlight, TR20-TR29]" },
        { 0x813C380B0B777E5C, "Snowslide Slope [Today's Highlight, TR30-TR39]" },
        { 0x813C390B0B77800F, "Snowslide Slope [Today's Highlight, TR40-TR49]" },
        { 0x813C3A0B0B7781C2, "Snowslide Slope [Today's Highlight, TR50-TR59]" },
        { 0x813C3B0B0B778375, "Snowslide Slope [Today's Highlight, TR60-TR69]" },
        { 0x813C3C0B0B778528, "Snowslide Slope [Today's Highlight, TR70-TR79]" },
        { 0x813C3D0B0B7786DB, "Snowslide Slope [Today's Highlight, TR80-TR89]" },
        { 0x813F3A0B0B79B799, "Snowslide Slope [Today's Highlight, TR90-TR99]" },
        { 0xF49C86F8683842BF, "Max Lair [Dynite Ore Trader]" },
    };
}

public partial class MultiShop
{
    public override string ToString() => $"{Hash:X16} - {string.Join(", ", Inventories.Select(z => z.ToString()))}";

    public static readonly Dictionary<ulong, string> SWSH = new()
    {
        { 0x66CA73B2966BB871, "Poké Mart Inventories [0-8 Badges]" },
        { 0x5870BD165650F18C, "Fields of Honor [Watt Trader, 0-8 Badges]" },
    };
}

public partial class Inventory
{
    public override string ToString() => $"{string.Join(",", Items.Select(z => z.ToString()))}";
}
