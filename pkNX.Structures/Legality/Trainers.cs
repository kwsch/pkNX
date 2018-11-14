﻿using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public static readonly ushort[] Mega_XY =
        {
            003, 006, 009, 065, 080, 115, 127, 130, 142, 150,
            181, 212, 214, 229, 248,
            257, 282, 303, 306, 308, 310, 354, 359, 380, 381,
            445, 448, 460
        };

        public static readonly ushort[] Mega_ORAS = Mega_XY.Concat(new ushort[]
        {
            015, 018, 094,
            208,
            254, 260, 302, 319, 323, 334, 362, 373, 376, 384,
            428, 475,
            531,
            719
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
            000, // Pokémon Trainer
            001, // Gym Leader
            002, // Gym Leader
            003, // Gym Leader
            004, // Gym Leader
            005, // Gym Leader
            006, // Gym Leader
            007, // Gym Leader
            008, // Pokémon Trainer
            009, // Pokémon Trainer
            010, // Pokémon Trainer
            011, // Pokémon Trainer
            012, // Team Rocket Boss
            013, // Team Rocket Admin
            014, // Team Rocket
            017, // Elite Four
            018, // Elite Four
            019, // Elite Four
            020, // Elite Four
            027, // Team Rocket
            032, // Pokémon Trainer
            033, // Pokémon Trainer
            053, // Coach Trainer
            054, // Coach Trainer
            057, // Gym Leader
            058, // Pokémon Trainer
            059, // Coach Trainer
            060, // Coach Trainer
            061, // Champion
            383, // Pokémon Trainer
            #endregion
        };

        public static readonly int[] Model_XY =
        {
            018, 019, 020, 021, 022, 055, 056, 057, 077, 078, 079, 080, 081, 102, 103, 104, 105, 107, 108, 173, 174, 175
        };

        public static readonly int[] Model_AO =
        {
            127, 128, 174, 178, 192, 198, 219, 221, 267, 272, 277, 278, 279
        };

        public static readonly int[] Z_Moves =
        {
            622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658,
            695, 696, 697, 698, 699, 700, 701, 702, 703, 719, 723, 724, 725, 726, 727, 728
        };

        public static readonly int[] ImportantTrainers_SM =
        {
            012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 129, 131, 132, 138, 144, 146, 149, 152, 153, 154, 155, 156, 158, 159, 160, 164, 167, 215, 216, 217, 218, 219, 220, 221,
            222, 235, 236, 238, 239, 240, 241, 349, 350, 351, 352, 356, 357, 358, 359, 360, 392, 396, 398, 400, 401, 403, 405, 409, 410, 412, 413, 414, 415, 416, 417, 418, 419, 435, 438, 439, 440,
            441, 447, 448, 449, 450, 451, 452, 467, 477, 478, 479, 480, 481, 482, 483, 484
        };

        public static readonly int[] ImportantTrainers_USUM =
        {
            012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 131, 132, 138, 144, 146, 149, 153, 154, 156, 159, 160, 215, 216, 217, 218, 219, 220, 221, 222, 235, 236, 238, 239, 240,
            241, 350, 351, 352, 356, 358, 359, 396, 398, 401, 405, 409, 410, 412, 415, 416, 417, 418, 419, 438, 439, 440, 441, 447, 448, 449, 450, 451, 452, 477, 478, 479, 480, 489, 490, 494, 495,
            496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 541, 542, 543, 555, 556, 557, 558, 559, 560, 561, 562, 572, 573, 578, 580, 582, 583, 623, 630, 644, 645, 647, 648, 649,
            650, 651, 652
        };

        public static readonly int[] ImportantTrainers_GG =
        {
            005, 007, 008, 009, 010, 011, 013, 014, 015, 016, 017, 018, 020, 021, 022, 023, 024, 025, 027, 028, 030, 031, 032, 033, 034, 035, 036, 037, 038, 039, 040, 041, 042, 043, 044, 045, 046,
            048, 049, 050, 051, 052, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447,
            448, 597, 601
        };
    }
}
