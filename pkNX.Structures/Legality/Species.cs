using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public static readonly int[] FinalEvolutions_1 =
        {
            003, 006, 009, 012, 015, 018, 020, 022, 024, 026, 028, 031, 034, 036, 038, 040, 045, 047, 049, 051, 053, 055, 057, 059, 062, 065, 068, 071, 073, 076, 078, 080, 083, 085, 087, 089, 091,
            094, 097, 099, 101, 103, 105, 106, 107, 110, 115, 119, 121, 122, 124, 127, 128, 130, 131, 132, 134, 135, 136, 139, 141, 142, 143, 149,
        };

        public static readonly int[] FinalEvolutions_6 = FinalEvolutions_1.Concat(new[]
        {
            154, 157, 160, 162, 164, 166, 168, 169, 171, 178, 181, 182, 184, 185, 186, 189, 192, 195, 196, 197, 199, 201, 202, 203, 205, 206, 208, 210, 211, 212, 213, 214, 217, 219, 222, 224, 225,
            226, 227, 229, 230, 232, 234, 235, 237, 241, 242, 248, 254, 257, 260, 262, 264, 267, 269, 272, 275, 277, 279, 282, 284, 286, 289, 291, 292, 295, 297, 301, 302, 303, 306, 308, 310, 311,
            312, 313, 314, 317, 319, 321, 323, 324, 326, 327, 330, 332, 334, 335, 336, 337, 338, 340, 342, 344, 346, 348, 350, 351, 352, 354, 357, 358, 359, 362, 365, 367, 368, 369, 370, 373, 376,
            389, 392, 395, 398, 400, 402, 405, 407, 409, 411, 413, 414, 416, 417, 419, 421, 423, 424, 426, 428, 429, 430, 432, 435, 437, 441, 442, 445, 448, 450, 452, 454, 455, 457, 460, 461, 462,
            463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 497, 500, 503, 505, 508, 510, 512, 514, 516, 518, 521, 523, 526, 528, 530, 531, 534, 537, 538, 539,
            542, 545, 547, 549, 550, 553, 555, 556, 558, 560, 561, 563, 565, 567, 569, 571, 573, 576, 579, 581, 584, 586, 587, 589, 591, 593, 594, 596, 598, 601, 604, 606, 609, 612, 614, 615, 617,
            618, 620, 621, 623, 625, 626, 628, 630, 631, 632, 635, 637, 652, 655, 658, 660, 663, 666, 668, 671, 673, 675, 676, 678, 681, 683, 685, 687, 689, 691, 693, 695, 697, 699, 700, 701, 702,
            703, 706, 707, 709, 711, 713, 715,
        }).ToArray();

        public static readonly int[] FinalEvolutions_7 = FinalEvolutions_6.Concat(new[]
        {
            724, 727, 730, 733, 735, 738, 740, 741, 743, 745, 746, 748, 750, 752, 754, 756, 758, 760, 763, 764, 765, 766, 768, 770, 771, 774, 775, 776, 777, 779, 780, 781, 784,
        }).ToArray();

        public static readonly int[] FinalEvolutions_8 = FinalEvolutions_7.Concat(new[]
        {
            812, 815, 818, 820, 823, 826, 828, 830, 832, 834, 836, 839, 841, 842, 844, 845, 847, 849, 851, 853, 855, 858, 861, 862, 863, 864, 865, 866, 867, 869, 870, 871, 873, 874, 875, 876, 877,
            879, 880, 881, 882, 883, 884, 887,
        }).ToArray();

        public static readonly int[] Legendary_1 =
        {
            #region Legendary
            144, // Articuno
            145, // Zapdos
            146, // Moltres
            150, // Mewtwo
            #endregion
        };

        public static readonly int[] Legendary_6 = Legendary_1.Concat(new[]
        {
            #region Legendary
            243, // Raikou
            244, // Entei
            245, // Suicune
            249, // Lugia
            250, // Ho-Oh
            377, // Regirock
            378, // Regice
            379, // Registeel
            380, // Latias
            381, // Latios
            382, // Kyogre
            383, // Groudon
            384, // Rayquaza
            480, // Uxie
            481, // Mesprit
            482, // Azelf
            483, // Dialga
            484, // Palkia
            485, // Heatran
            486, // Regigigas
            487, // Giratina
            488, // Cresselia
            638, // Cobalion
            639, // Terrakion
            640, // Virizion
            641, // Tornadus
            642, // Thundurus
            643, // Reshiram
            644, // Zekrom
            645, // Landorus
            646, // Kyurem
            716, // Xerneas
            717, // Yveltal
            718, // Zygarde
            #endregion
        }).ToArray();

        public static readonly int[] Legendary_SM = Legendary_6.Concat(new[]
        {
            #region Legendary
            773, // Silvally
            785, // Tapu Koko
            786, // Tapu Lele
            787, // Tapu Bulu
            788, // Tapu Fini
            791, // Solgaleo
            792, // Lunala
            793, // Nihilego
            794, // Buzzwole
            795, // Pheromosa
            796, // Xurkitree
            797, // Celesteela
            798, // Kartana
            799, // Guzzlord
            800, // Necrozma
            #endregion
        }).ToArray();

        public static readonly int[] Legendary_USUM = Legendary_SM.Concat(new[] { 804, 805, 806 }).ToArray(); // Poipole, Blacephalon, Stakataka

        public static readonly int[] Legendary_8 = Legendary_USUM.Concat(new[]
        {
            #region Legendary
            888, // Zacian
            889, // Zamazenta
            890, // Eternatus
            891, // Kubfu
            892, // Urshifu
            894, // Regieleki
            895, // Regidrago
            896, // Glastrier
            897, // Spectrier
            898, // Calyrex
            #endregion
        }).ToArray();

        public static readonly int[] Legendary_8a = Legendary_8.Concat(new[] { 905 }).ToArray();

        public static readonly int[] Mythical_1 = { 151 }; // Mew

        public static readonly int[] Mythical_6 = Mythical_1.Concat(new[]
        {
            #region Mythical
            251, // Celebi
            385, // Jirachi
            386, // Deoxys
            489, // Phione
            490, // Manaphy
            491, // Darkrai
            492, // Shaymin
            493, // Arceus
            494, // Victini
            647, // Keldeo
            648, // Meloetta
            649, // Genesect
            719, // Diancie
            720, // Hoopa
            721, // Volcanion
            #endregion
        }).ToArray();

        public static readonly int[] Mythical_SM = Mythical_6.Concat(new[] { 801, 802 }).ToArray(); // Magearna, Marshadow

        public static readonly int[] Mythical_USUM = Mythical_SM.Concat(new[] { 807 }).ToArray(); // Zeraora

        public static readonly int[] Mythical_GG = Mythical_1.Concat(new[] { 809 }).ToArray(); // Melmetal

        public static readonly int[] Mythical_8 = Mythical_USUM.Concat(new[] { 809, 893 }).ToArray(); // Melmetal, Zarude
    }
}
