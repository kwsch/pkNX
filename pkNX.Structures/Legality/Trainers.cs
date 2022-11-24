using System.Linq;

namespace pkNX.Structures;

public static partial class Legal
{
    #region Gen 6
    public static readonly ushort[] Mega_XY =
    {
        003, 006, 009, 065, 080, 115, 127, 130, 142, 150,
        181, 212, 214, 229, 248,
        257, 282, 303, 306, 308, 310, 354, 359, 380, 381,
        445, 448, 460,
    };

    public static readonly ushort[] Mega_ORAS = Mega_XY.Concat(new ushort[]
    {
        015, 018, 094,
        208,
        254, 260, 302, 319, 323, 334, 362, 373, 376, 384,
        428, 475,
        531,
        719,
    }).ToArray();

    public static readonly int[] SpecialClasses_XY =
    {
        #region Classes
        000, // Pokémon Trainer
        001, // Pokémon Trainer
        004, // Leader
        018, // Team Flare
        019, // Team Flare
        020, // Team Flare
        021, // Team Flare
        022, // Team Flare
        035, // Elite Four
        036, // Elite Four
        037, // Elite Four
        038, // Elite Four
        039, // Leader
        040, // Leader
        041, // Leader
        042, // Leader
        043, // Leader
        044, // Leader
        045, // Leader
        053, // Champion
        055, // Pokémon Trainer
        056, // Pokémon Trainer
        057, // Pokémon Trainer
        064, // Battle Chatelaine
        065, // Battle Chatelaine
        066, // Battle Chatelaine
        067, // Battle Chatelaine
        081, // Team Flare
        102, // Pokémon Trainer
        103, // Pokémon Trainer
        104, // Pokémon Trainer
        105, // Pokémon Professor
        139, // Marchioness
        140, // Marquis
        141, // Marchioness
        142, // Marquis
        143, // Marquis
        144, // Marchioness
        145, // Marchioness
        146, // Marquis
        151, // Grand Duchess
        160, // Pokémon Trainer
        161, // Pokémon Trainer
        170, // Pokémon Trainer
        171, // Pokémon Trainer
        172, // Pokémon Trainer
        173, // Team Flare
        174, // Team Flare
        175, // Team Flare Boss
        176, // Successor
        177, // Leader
        #endregion
    };

    public static readonly int[] SpecialClasses_ORAS =
    {
        #region Classes
        064, // Battle Chatelaine
        065, // Battle Chatelaine
        066, // Battle Chatelaine
        067, // Battle Chatelaine
        127, // Pokémon Trainer
        128, // Pokémon Trainer
        174, // Aqua Leader
        175, // Aqua Admin
        178, // Magma Leader
        180, // Magma Admin
        182, // Magma Admin
        186, // Aqua Admin
        187, // Magma Admin
        192, // Pokémon Trainer
        194, // Elite Four
        195, // Elite Four
        196, // Elite Four
        197, // Elite Four
        198, // Champion
        200, // Leader
        201, // Leader
        202, // Leader
        203, // Leader
        204, // Leader
        205, // Leader
        206, // Leaders
        207, // Leader
        219, // Pokémon Trainer
        221, // Lorekeeper
        232, // Pokémon Trainer
        233, // Pokémon Trainer
        234, // Pokémon Trainer
        236, // Secret Base Expert
        267, // Pokémon Trainer
        268, // Sootopolitan
        270, // Pokémon Trainer
        271, // Pokémon Trainer
        272, // Pokémon Trainer
        273, // Elite Four
        274, // Elite Four
        275, // Elite Four
        276, // Elite Four
        277, // Champion
        278, // Pokémon Trainer
        279, // Pokémon Trainer
        #endregion
    };
    #endregion

    #region Gen 7
    public static readonly int[] SpecialClasses_SM =
    {
        #region Classes
        000, // Pokémon Trainer
        001, // Pokémon Trainer
        030, // Pokémon Trainer
        031, // Island Kahuna
        038, // Captain
        040, // Pokémon Trainer
        041, // Pokémon Trainer
        043, // Captain
        044, // Captain
        045, // Captain
        046, // Captain
        047, // Captain
        048, // Captain
        049, // Island Kahuna
        050, // Island Kahuna
        051, // Island Kahuna
        071, // Aether President
        072, // Aether Branch Chief
        076, // Team Skull Boss
        077, // Pokémon Trainer
        078, // Team Skull Admin
        079, // Pokémon Trainer
        080, // Elite Four
        081, // Pokémon Trainer
        082, // Aether President
        083, // Pokémon Trainer
        084, // Pokémon Trainer
        085, // Pokémon Trainer
        086, // Pokémon Trainer
        087, // Pokémon Trainer
        088, // Pokémon Trainer
        089, // Pokémon Trainer
        090, // Pokémon Trainer
        091, // Pokémon Trainer
        092, // Pro Wrestler
        093, // Pokémon Trainer
        097, // Pokémon Trainer
        098, // Pokémon Trainer
        099, // Pokémon Trainer
        100, // Pokémon Trainer
        101, // Pokémon Trainer
        102, // Pokémon Trainer
        103, // Pokémon Trainer
        104, // Pokémon Trainer
        105, // Pokémon Trainer
        106, // Pokémon Trainer
        107, // Elite Four
        108, // Pokémon Trainer
        109, // Elite Four
        110, // Elite Four
        111, // Pokémon Professor
        128, // Pokémon Trainer
        139, // GAME FREAK
        140, // Pokémon Trainer
        141, // Island Kahuna
        142, // Captain
        143, // Pokémon Trainer
        150, // Pokémon Trainer
        153, // Captain
        154, // Pokémon Professor
        164, // Island Kahuna
        166, // Pokémon Trainer
        167, // Pokémon Trainer
        168, // Pokémon Trainer
        169, // Pokémon Trainer
        170, // Pokémon Trainer
        171, // Pokémon Trainer
        165, // Pokémon Professor
        183, // Battle Legend
        184, // Battle Legend
        185, // Aether Foundation
        #endregion
    };

    public static readonly int[] SpecialClasses_USUM =
    {
        #region Classes
        000, // Pokémon Trainer
        001, // Pokémon Trainer
        030, // Pokémon Trainer
        031, // Island Kahuna
        038, // Captain
        040, // Pokémon Trainer
        041, // Pokémon Trainer
        043, // Captain
        044, // Captain
        045, // Captain
        046, // Captain
        047, // Captain
        048, // Captain
        049, // Island Kahuna
        050, // Island Kahuna
        051, // Island Kahuna
        071, // Aether President
        072, // Aether Branch Chief
        076, // Team Skull Boss
        077, // Pokémon Trainer
        078, // Team Skull Admin
        079, // Pokémon Trainer
        080, // Elite Four
        081, // Pokémon Trainer
        082, // Aether President
        083, // Pokémon Trainer
        084, // Pokémon Trainer
        085, // Pokémon Trainer
        086, // Pokémon Trainer
        087, // Pokémon Trainer
        088, // Pokémon Trainer
        089, // Pokémon Trainer
        090, // Pokémon Trainer
        091, // Pokémon Trainer
        092, // Pro Wrestler
        093, // Pokémon Trainer
        097, // Pokémon Trainer
        098, // Pokémon Trainer
        099, // Pokémon Trainer
        100, // Pokémon Trainer
        101, // Pokémon Trainer
        102, // Pokémon Trainer
        103, // Pokémon Trainer
        104, // Pokémon Trainer
        105, // Pokémon Trainer
        106, // Pokémon Trainer
        107, // Elite Four
        108, // Pokémon Trainer
        109, // Elite Four
        110, // Elite Four
        111, // Pokémon Professor
        128, // Pokémon Trainer
        139, // GAME FREAK
        140, // Pokémon Trainer
        141, // Island Kahuna
        142, // Captain
        143, // Pokémon Trainer
        150, // Pokémon Trainer
        153, // Captain
        154, // Pokémon Professor
        164, // Island Kahuna
        166, // Pokémon Trainer
        167, // Pokémon Trainer
        168, // Pokémon Trainer
        169, // Pokémon Trainer
        170, // Pokémon Trainer
        171, // Pokémon Trainer
        165, // Pokémon Professor
        183, // Battle Legend
        184, // Battle Legend
        185, // Aether Foundation
        186, // Pokémon Trainer
        187, // Pokémon Trainer
        188, // Pokémon Trainer
        189, // Pokémon Trainer
        190, // Pokémon Trainer
        191, // Elite Four
        192, // Ultra Recon Squad
        193, // Ultra Recon Squad
        194, // Pokémon Trainer
        198, // Team Aqua
        199, // Team Galactic
        200, // Team Magma
        201, // Team Plasma
        202, // Team Flare
        205, // GAME FREAK
        206, // Team Rainbow Rocket
        207, // Pokémon Trainer
        219, // Pokémon Trainer
        220, // Aether President
        221, // Pokémon Trainer
        222, // Pokémon Trainer
        #endregion
    };

    public static readonly int[] SpecialClasses_GG =
    {
        #region Classes
        000, // Pokémon Trainer [Trace, Standard]
        001, // Gym Leader [Brock]
        002, // Gym Leader [Misty]
        003, // Gym Leader [Lt. Surge]
        004, // Gym Leader [Erika]
        005, // Gym Leader [Sabrina]
        006, // Gym Leader [Koga]
        007, // Gym Leader [Blaine]
        008, // Pokémon Trainer [Red]
        009, // Pokémon Trainer [Blue]
        010, // Pokémon Trainer [Green]
        011, // Pokémon Trainer [Mina]
        012, // Team Rocket Boss [Giovanni]
        013, // Team Rocket Admin [Archer]
        014, // Team Rocket [Jessie]
        017, // Elite Four [Lorelei]
        018, // Elite Four [Bruno]
        019, // Elite Four [Agatha]
        020, // Elite Four [Lance]
        027, // Team Rocket [James]
        028, // Gym Leader [Giovanni]
        057, // Gym Leader [Blue]
        058, // Pokémon Trainer [Archer]
        061, // Champion [Trace]
        383, // Pokémon Trainer [Trace, Champion Title Defense]
        #endregion
    };

    /// <summary>
    /// Unused Trainer Classes in Let's Go, Pikachu! &amp; Let's Go, Eevee!.
    /// Assigning these Trainer Classes to a Trainer crashes the game.
    /// A majority of these are Master Trainer related, and only used for multiplayer. They are not to be assigned to NPCs.
    /// </summary>
    public static readonly int[] BlacklistedClasses_GG = Enumerable.Range(072, 311).Concat(new[]
    {
        #region CrashClasses
        032, // Pokémon Trainer
        033, // Pokémon Trainer
        #endregion
    }).ToArray();
    #endregion

    #region Gen 8
    public static readonly int[] SpecialClasses_SWSH =
    {
        #region Classes
        004, // Champion [Leon]
        005, // Pokémon Trainer [Leon, Battle Tower]
        006, // Pokémon Trainer [Leon, Champion Cup Rematches]
        007, // Pokémon Trainer [Hop]
        008, // Pokémon Trainer [Hop, Gym Outfit]
        011, // Pokémon Trainer [Bede]
        012, // Gym Leader [Bede]
        013, // Pokémon Trainer [Marnie]
        014, // Pokémon Trainer [Marnie, Gym Outfit]
        015, // Gym Leader [Marnie]
        020, // Gym Leader [Milo]
        021, // Gym Leader [Nessa]
        022, // Gym Leader [Kabu]
        023, // Gym Leader [Bea]
        024, // Gym Leader [Allister]
        025, // Gym Leader [Opal]
        026, // Gym Leader [Gordie]
        027, // Gym Leader [Melony]
        028, // Gym Leader [Piers]
        029, // Gym Leader [Raihan]
        030, // Macro Cosmos’s [Oleana]
        032, // Macro Cosmos’s [Rose]
        074, // Pokémon Trainer [Sordward]
        075, // Pokémon Trainer [Shielbert]
        183, // GAME FREAK’s [Morimoto]
        184, // Pokémon Trainer [Max Raid Battle, Hop]
        185, // Pokémon Trainer [Max Raid Battle, Piers]
        188, // Pokémon Trainer [First Battle, Hop]
        199, // Pokémon Trainer [Final Battle, Hop]
        200, // Pokémon Trainer [Opal]
        205, // Gym Leader [Rematch, Nessa]
        206, // Gym Leader [Rematch, Raihan]
        207, // Gym Leader [Rematch, Allister]
        208, // Gym Leader [Rematch, Bea]
        209, // Gym Leader [Rematch, Milo]
        210, // Gym Leader [Champion Cup, Nessa]
        211, // Gym Leader [Champion Cup, Kabu]
        212, // Gym Leader [Champion Cup, Bea]
        213, // Gym Leader [Champion Cup, Allister]
        214, // Gym Leader [Champion Cup, Opal]
        215, // Gym Leader [Champion Cup, Gordie]
        216, // Gym Leader [Champion Cup, Melony]
        217, // Pokémon Trainer [Champion Cup, Piers]
        218, // Gym Leader [Champion Cup, Raihan]
        219, // Pokémon Trainer [Klara]
        220, // Pokémon Trainer [Avery]
        221, // Dojo Master [Mustard]
        222, // Dojo Master [Mustard, No Jacket]
        227, // Dojo Matron [Honey]
        228, // Pokémon Trainer [Peony]
        229, // Pokémon Trainer [Peonia]
        250, // Pokémon Trainer [Klara]
        251, // Pokémon Trainer [Avery]
        252, // Gym Leader [Avery]
        253, // Gym Leader [Klara]
        #endregion
    };

    public static readonly int[] DoubleBattleClasses_SWSH =
    {
        #region DoubleBattleClasses
        072, // Reporter
        073, // Cameraman
        170, // Musician
        171, // Dancer
        172, // Rail Staff
        173, // Beauty
        175, // Office Worker [Male]
        176, // Office Worker [Female]
        178, // Team Yell [Male]
        179, // Gym Trainer [Dark, Male]
        180, // Doctor [Male]
        181, // Doctor [Female]
        186, // Pokémon Trainer [Sordward]
        187, // Pokémon Trainer [Shielbert]
        202, // Team Yell [Female]
        203, // Macro Cosmos’s [Male]
        204, // Macro Cosmos’s [Female]
        230, // Dojo Master [Galarian Star Tournament, Mustard]
        231, // Gym Leader [Galarian Star Tournament, Bede]
        232, // Gym Leader [Galarian Star Tournament, Marnie]
        233, // Pokémon Trainer [Galarian Star Tournament, Leon]
        234, // Gym Leader [Galarian Star Tournament, Kabu]
        235, // Gym Leader [Galarian Star Tournament, Nessa]
        236, // Pokémon Trainer [Galarian Star Tournament, Piers]
        237, // Gym Leader [Galarian Star Tournament, Allister]
        238, // Gym Leader [Galarian Star Tournament, Raihan]
        239, // Gym Leader [Galarian Star Tournament, Bea]
        240, // Pokémon Trainer [Galarian Star Tournament, Shielbert]
        241, // Pokémon Trainer [Galarian Star Tournament, Hop]
        242, // Gym Leader [Galarian Star Tournament, Melony]
        243, // Gym Leader [Galarian Star Tournament, Gordie]
        244, // Gym Leader [Galarian Star Tournament, Avery]
        245, // Gym Leader [Galarian Star Tournament, Klara]
        246, // Pokémon Trainer [Galarian Star Tournament, Peony]
        247, // Pokémon Trainer [Galarian Star Tournament, Sordward]
        248, // Gym Leader [Galarian Star Tournament, Milo]
        249, // Pokémon Trainer [Galarian Star Tournament, Opal]

        // These Trainer Classes are never assigned to Trainers, they're purely for display
        168, // Interviewers  (Displayed when Trainer Classes 072 and 073 partake in a Double Battle)
        169, // Music Crew    (Displayed when Trainer Classes 170 and 171 partake in a Double Battle)
        174, // Daring Couple (Displayed when Trainer Classes 172 and 173 partake in a Double Battle)
        177, // Colleagues    (Displayed when Trainer Classes 175 and 176 partake in a Double Battle)
        182, // Medical Team  (Displayed when Trainer Classes 180 and 181 partake in a Double Battle)
        #endregion
    };

    /// <summary>
    /// Unused Trainer Classes in Sword &amp; Shield.
    /// Consists of NPCs you can interact with but never battle.
    /// </summary>
    public static readonly int[] UnusedClasses_SWSH =
    {
        #region UnusedClasses
        000, // Pokémon Trainer [Your Player]
        001, // Pokémon Trainer [Your Player]
        002, // きんにくじまん [T-Pose]
        003, // おかあさん [Mother]
        009, // じょしゅ [Sonia, Trench Coat]
        010, // じょしゅ [Sonia, Lab Coat]
        016, // おばさん [T-Pose]
        017, // ポケモンはかせ [Magnolia, Lab Coat]
        018, // ポケモンはかせ [Magnolia, Casual Dress]
        031, // Macro Cosmos’s [Rose, Casual Clothing]
        119, // Gym Challenger [T-Pose]
        121, // Gym Challenger [T-Pose]
        123, // Gym Challenger [T-Pose]
        125, // Gym Challenger [T-Pose]
        127, // Gym Challenger [T-Pose]
        129, // Gym Challenger [T-Pose]
        131, // Gym Challenger [T-Pose]
        133, // Gym Challenger [T-Pose]
        135, // Gym Challenger [T-Pose]
        137, // Gym Challenger [T-Pose]
        139, // Gym Challenger [T-Pose]
        141, // Gym Challenger [T-Pose]
        143, // Gym Challenger [T-Pose]
        145, // Gym Challenger [T-Pose]
        147, // Gym Challenger [T-Pose]
        148, // ＰＣじょう [Pokémon Center Lady]
        149, // リーグしんぱんいん [League Referee]
        150, // カセキはかせ [Cara Liss]
        151, // Ball Guy [T-Pose]
        152, // てんいん [Poké Mart Clerk]
        153, // てんいん [T-Pose]
        155, // えんじ [Preschooler]
        156, // えんじ [Preschooler]
        157, // じどう [T-Pose]
        158, // じどう [T-Pose]
        159, // わかもの [T-Pose]
        160, // ちゅうねん [T-Pose]
        161, // ちゅうねん [T-Pose]
        162, // ろうじん [T-Pose]
        163, // ろうじん [T-Pose]
        164, // ちゅうねん [T-Pose]
        167, // Young Man [T-Pose]
        224, // Master Dojo [Male] -- functionally identical to 223
        226, // Master Dojo [Female] -- functionally identical to 225
        #endregion
    };

    /// <summary>
    /// Unused Trainer Classes in Sword &amp; Shield.
    /// Assigning these Trainer Classes to a Trainer crashes the game.
    /// </summary>
    public static readonly int[] CrashClasses_SWSH =
    {
        #region CrashClasses
        019, // ベテラントレーナー
        047, // Waitress
        068, // Stylist
        094, // Gym Leader
        095, // Gym Trainer
        096, // Gym Trainer
        097, // Gym Leader
        098, // Gym Trainer
        099, // Gym Trainer
        100, // Gym Leader
        101, // Gym Trainer
        102, // Gym Trainer
        103, // Gym Leader
        104, // Gym Trainer
        105, // Gym Trainer
        106, // Gym Leader
        107, // Gym Trainer
        108, // Gym Trainer
        109, // Gym Leader
        110, // Gym Trainer
        111, // Gym Trainer
        112, // Gym Leader
        113, // Gym Trainer
        114, // Gym Trainer
        115, // Gym Leader
        116, // Gym Trainer
        117, // Gym Trainer
        154, // はいたついん
        222, // Dojo Master [Mustard] -- this is used, but crashes if assigned to any other trainers
        #endregion
    };

    /// <summary>
    /// Dummy Trainer Classes in Sword and Shield.
    /// No names are assigned to them. Could be preserved for future DLC, or could just be leftovers.
    /// </summary>
    public static readonly int[] DummyClasses_SWSH =
    {
        #region DummyClasses
        254, // [~ 254]
        255, // [~ 255]
        256, // [~ 256]
        257, // [~ 257]
        258, // [~ 258]
        259, // [~ 259]
        260, // [~ 260]
        261, // [~ 261]
        262, // [~ 262]
        263, // [~ 263]
        264, // [~ 264]
        265, // [~ 265]
        266, // [~ 266]
        267, // [~ 267]
        268, // [~ 268]
        269, // [~ 269]
        270, // [~ 270]
        #endregion
    };

    public static readonly int[] BlacklistedClasses_SWSH = DoubleBattleClasses_SWSH.Concat(UnusedClasses_SWSH).Concat(CrashClasses_SWSH).Concat(DummyClasses_SWSH).ToArray();
    #endregion

    #region Gen 9
    public static readonly int[] SpecialClasses_SV =
    {
        #region Classes
        #endregion
    };
    
    public static readonly int[] DoubleBattleClasses_SV =
    {
        #region DoubleBattleClasses
        #endregion
    };

    /// <summary>
    /// Unused Trainer Classes in Scarlet &amp; Violet.
    /// Consists of NPCs you can interact with but never battle.
    /// </summary>
    public static readonly int[] UnusedClasses_SV =
    {
        #region UnusedClasses
        #endregion
    };

    /// <summary>
    /// Unused Trainer Classes in Scarlet &amp; Violet.
    /// Assigning these Trainer Classes to a Trainer crashes the game.
    /// </summary>
    public static readonly int[] CrashClasses_SV =
    {
        #region CrashClasses
        #endregion
    };

    /// <summary>
    /// Dummy Trainer Classes in Scarlet &amp; Violet.
    /// No names are assigned to them. Could be preserved for future DLC, or could just be leftovers.
    /// </summary>
    public static readonly int[] DummyClasses_SV =
    {
        #region DummyClasses
        #endregion
    };
    #endregion

    public static readonly int[] BlacklistedClasses_SV = DoubleBattleClasses_SV.Concat(UnusedClasses_SV).Concat(CrashClasses_SV).Concat(DummyClasses_SV).ToArray();

    public static readonly int[] Model_XY =
    {
        #region Models
        018, // Aliana
        019, // Bryony
        020, // Celosia
        021, // Mable
        022, // Xerosic
        055, // Shauna
        056, // Tierno
        057, // Trevor
        081, // Lysandre
        102, // AZ
        103, // Calem
        104, // Serena
        105, // Sycamore
        175, // Lysandre (Mega Ring)
        #endregion
    };

    public static readonly int[] Model_AO =
    {
        #region Models
        127, // Brendan
        128, // May
        174, // Archie
        178, // Maxie
        192, // Wally
        198, // Steven
        219, // Steven (Multi Battle)
        221, // Zinnia (Lorekeeper)
        267, // Zinnia
        272, // Wally (Mega Pendant)
        277, // Steven (Rematch)
        278, // Brendan (Mega Bracelet)
        279, // May (Mega Bracelet)
        #endregion
    };

    public static readonly int[] Z_Moves =
    {
        622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658,
        695, 696, 697, 698, 699, 700, 701, 702, 703, 719, 723, 724, 725, 726, 727, 728,
    };

    public static readonly int[] Max_Moves =
    {
        743, // Max Guard
        757, // Max Flare
        758, // Max Flutterby
        759, // Max Lightning
        760, // Max Strike
        761, // Max Knuckle
        762, // Max Phantasm
        763, // Max Hailstorm
        764, // Max Ooze
        765, // Max Geyser
        766, // Max Airstream
        767, // Max Starfall
        768, // Max Wyrmwind
        769, // Max Mindstorm
        770, // Max Rockfall
        771, // Max Quake
        772, // Max Darkness
        773, // Max Overgrowth
        774, // Max Steelspike
    };

    public static readonly int[] Taboo_Moves =
    {
        165, // Struggle
        464, // Dark Void
        621, // Hyperspace Fury
        781, // Behemoth Blade
        782, // Behemoth Bash
    };

    public static readonly int[] ImportantTrainers_XY =
    {
        006, 021, 022, 023, 024, 025, 026, 076, 130, 131, 132, 175, 184, 185, 186, 187, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274,
        275, 276, 277, 279, 303, 321, 322, 323, 324, 325, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 237, 348, 349, 350, 351, 435, 436,
        437, 438, 439, 503, 504, 505, 507, 511, 512, 513, 514, 515, 519, 520, 521, 525, 526, 559, 560, 561, 562, 573, 575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 585, 586, 587, 588, 589,
        590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 604, 605, 606, 613,
    };

    public static readonly int[] ImportantTrainers_ORAS =
    {
        178, 231, 235, 236, 266, 271, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 518, 527, 528, 529, 530, 531, 532, 553, 554, 555, 556, 557, 561, 563, 567, 569, 570, 571, 572,
        583, 674, 675, 676, 677, 678, 679, 680, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 713, 856, 857, 898, 906, 907, 908, 909, 910, 911,
        912, 913, 942, 943, 944, 945, 946, 947,
    };

    public static readonly int[] ImportantTrainers_SM =
    {
        012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 129, 131, 132, 138, 144, 146, 149, 152, 153, 154, 155, 156, 158, 159, 160, 164, 167, 215, 216, 217, 218, 219, 220, 221,
        222, 235, 236, 238, 239, 240, 241, 349, 350, 351, 352, 356, 357, 358, 359, 360, 392, 396, 398, 400, 401, 403, 405, 409, 410, 412, 413, 414, 415, 416, 417, 418, 419, 435, 438, 439, 440,
        441, 447, 448, 449, 450, 451, 452, 467, 477, 478, 479, 480, 481, 482, 483, 484,
    };

    public static readonly int[] ImportantTrainers_USUM =
    {
        012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 131, 132, 138, 144, 146, 149, 153, 154, 156, 159, 160, 215, 216, 217, 218, 219, 220, 221, 222, 235, 236, 238, 239, 240,
        241, 350, 351, 352, 356, 358, 359, 396, 398, 401, 405, 409, 410, 412, 415, 416, 417, 418, 419, 438, 439, 440, 441, 447, 448, 449, 450, 451, 452, 477, 478, 479, 480, 489, 490, 494, 495,
        496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 541, 542, 543, 555, 556, 557, 558, 559, 560, 561, 562, 572, 573, 578, 580, 582, 583, 623, 630, 644, 645, 647, 648, 649,
        650, 651, 652,
    };

    public static readonly int[] ImportantTrainers_GG =
    {
        005, 007, 008, 009, 010, 011, 013, 014, 015, 016, 017, 018, 020, 021, 022, 023, 024, 025, 027, 028, 030, 031, 032, 033, 034, 035, 036, 037, 038, 039, 040, 041, 042, 043, 044, 045, 046,
        048, 049, 050, 051, 052, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 437, 439, 597, 601,
    };

    public static readonly int[] ImportantTrainers_SWSH =
    {
        032, 036, 037, 077, 078, 107, 108, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 138, 143, 144, 145, 149, 153, 154, 155, 156, 157, 158, 175, 189, 190,
        191, 192, 193, 195, 196, 197, 198, 199, 202, 203, 204, 210, 211, 212, 213, 214, 215, 216, 221, 222, 225, 226, 227, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 248, 249, 250, 251,
        252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 264, 265, 266, 267, 268, 269, 289, 315, 316, 317, 318, 319, 320, 321, 324, 325, 326, 327, 328, 329, 330, 374, 376, 414, 415, 416,
        417, 418, 419, 420, 431, 432, 433, 434,
    };

    public static readonly int[] ImportantTrainers_SV =
    {
    };
}
