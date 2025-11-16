namespace pkNX.Structures;

public static partial class Legal
{
    public const int MaxSpeciesID_7_GG = 809; // Melmetal
    public const int MaxMoveID_7_GG = 742; // Double Iron Bash
    public const int MaxItemID_7_GG = 1057; // Magmar Candy
    public const int MaxAbilityID_7_GG = MaxAbilityID_7_USUM;
    public const byte AwakeningMax = 200;

    #region Items

    public static readonly ushort[] Pouch_Candy_GG_Regular =
    [
        050, // Rare Candy
        960, 961, 962, 963, 964, 965, // S
        966, 967, 968, 969, 970, 971, // L
        972, 973, 974, 975, 976, 977, // XL
    ];

    public static readonly ushort[] Pouch_Candy_GG_Species =
    [
        978, 979,
        980, 981, 982, 983, 984, 985, 986, 987, 988, 989,
        990, 991, 992, 993, 994, 995, 996, 997, 998, 999,
        1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009,
        1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 1019,
        1020, 1021, 1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029,
        1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039,
        1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049,
        1050, 1051, 1052, 1053, 1054, 1055, 1056,
        1057,
    ];

    public static readonly ushort[] Pouch_Candy_GG = [.. Pouch_Candy_GG_Regular, .. Pouch_Candy_GG_Species];

    public static readonly ushort[] Pouch_Medicine_GG =
    [
        017, 018, 019, 020, 021, 022, 023, 024, 025, 026, 027, 028, 029, 030, 031, 032, 038, 039, 040, 041, 709, 903,
    ];

    public static readonly ushort[] Pouch_PowerUp_GG =
    [
        051, 053, 081, 082, 083, 084, 085,
        849,
    ];

    public static readonly ushort[] Pouch_Catching_GG =
    [
        001, 002, 003, 004, 012, 164, 166, 168,
        861, 862, 863, 864, 865, 866,
    ];

    public static readonly ushort[] Pouch_Battle_GG =
    [
        055, 056, 057, 058, 059, 060, 061, 062,
        656, 659, 660, 661, 662, 663, 671, 672, 675, 676, 678, 679,
        760, 762, 770, 773,
    ];

    public static readonly ushort[] Pouch_Regular_GG =
    [
        076, 077, 078, 079, 086, 087, 088, 089,
        090, 091, 092, 093, 101, 102, 103, 113, 115,
        121, 122, 123, 124, 125, 126, 127, 128,
        442,
        571,
        632, 651,
        795, 796,
        872, 873, 874, 875, 876, 877, 878, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 900, 901, 902,
    ];

    public static readonly ushort[] Pouch_Regular_GG_Item =
    [
        076, // Super Repel
        077, // Max Repel
        078, // Escape Rope
        079, // Repel
        086, // Tiny Mushroom
        087, // Big Mushroom
        088, // Pearl
        089, // Big Pearl
        090, // Stardust
        091, // Star Piece
        092, // Nugget
        093, // Heart Scale

        571, // Pretty Wing
        795, // Bottle Cap
        796, // Gold Bottle Cap

        900, // Lure
        901, // Super Lure
        902, // Max Lure
    ];

    public static readonly ushort[] HeldItems_GG = [.. Pouch_Candy_GG, .. Pouch_Medicine_GG, .. Pouch_PowerUp_GG, .. Pouch_Catching_GG, .. Pouch_Battle_GG, .. Pouch_Regular_GG_Item];

    #endregion

    #region Moves

    private static readonly int[] AllowedMovesGG =
    [
        000, 001, 002, 003, 004, 005, 006, 007, 008, 009,
        010, 011, 012, 013, 014, 015, 016, 017, 018, 019,
        020, 021, 022, 023, 024, 025, 026, 027, 028, 029,
        030, 031, 032, 033, 034, 035, 036, 037, 038, 039,
        040, 041, 042, 043, 044, 045, 046, 047, 048, 049,
        050, 051, 052, 053, 054, 055, 056, 057, 058, 059,
        060, 061, 062, 063, 064, 065, 066, 067, 068, 069,
        070, 071, 072, 073, 074, 075, 076, 077, 078, 079,
        080, 081, 082, 083, 084, 085, 086, 087, 088, 089,
        090, 091, 092, 093, 094, 095, 096, 097, 098, 099,
        100, 101, 102, 103, 104, 105, 106, 107, 108, 109,
        110, 111, 112, 113, 114, 115, 116, 117, 118, 119,
        120, 121, 122, 123, 124, 125, 126, 127, 128, 129,
        130, 131, 132, 133, 134, 135, 136, 137, 138, 139,
        140, 141, 142, 143, 144, 145, 146, 147, 148, 149,
        150, 151, 152, 153, 154, 155, 156, 157, 158, 159,
        160, 161, 162, 163, 164,

        182, 188, 200, 224, 227, 231, 242, 243, 247, 252,
        257, 261, 263, 269, 270, 276, 280, 281, 339, 347,
        355, 364, 369, 389, 394, 398, 399, 403, 404, 405,
        406, 417, 420, 430, 438, 446, 453, 483, 492, 499,
        503, 504, 525, 529, 583, 585, 603, 605, 606, 607,
        729, 730, 731, 733, 734, 735, 736, 737, 738, 739,
        740, 742,
    ];

    public static readonly ushort[] TMHM_GG =
    [
        029, 269, 270, 100, 156, 113, 182, 164, 115, 091,
        261, 263, 280, 019, 069, 086, 525, 369, 231, 399,
        492, 157, 009, 404, 127, 398, 092, 161, 503, 339,
        007, 605, 347, 406, 008, 085, 053, 087, 200, 094,
        089, 120, 247, 583, 076, 126, 057, 063, 276, 355,
        059, 188, 072, 430, 058, 446, 006, 529, 138, 224,
        // rest are same as SM, unused

        // No HMs
    ];

    #endregion
}
