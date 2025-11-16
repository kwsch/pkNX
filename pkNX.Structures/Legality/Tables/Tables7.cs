using System.Collections.Generic;
using static pkNX.Structures.Species;

namespace pkNX.Structures;

public static partial class Legal
{
    public const int MaxSpeciesID_7_USUM = 807;
    public const int MaxAbilityID_7_USUM = 233;

    #region Inventory Pouch
    public static readonly ushort[] Pouch_TMHM_SM = [ // 02
            328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345,
            346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363,
            364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381,
            382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399,
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417,
            418, 419, 618, 619, 620, 690, 691, 692, 693, 694,
        ];

    #endregion
    public static readonly HashSet<ushort> Totem_Alolan =
    [
        (int)Raticate, // (Normal, Alolan, Totem)
        (int)Marowak, // (Normal, Alolan, Totem)
        (int)Mimikyu, // (Normal, Busted, Totem, Totem_Busted)
    ];

    public static readonly HashSet<ushort> Totem_USUM =
    [
        (int)Raticate,
        (int)Gumshoos,
        //(int)Wishiwashi,
        (int)Salazzle,
        (int)Lurantis,
        (int)Vikavolt,
        (int)Mimikyu,
        (int)Kommoo,
        (int)Marowak,
        (int)Araquanid,
        (int)Togedemaru,
        (int)Ribombee,
    ];
}
