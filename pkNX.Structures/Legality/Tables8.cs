using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public const int MaxSpeciesID_8 = 890; // Eternatus
        public const int MaxMoveID_8 = 796; // Steel Beam (jet fuel)
        public const int MaxItemID_8 = 1278; // Rotom Catalog, or 1578 for all catalog parts?
        public const int MaxAbilityID_8 = 258;
        public const int MaxBallID_8 = 0x1A; // 26 Beast
        public const int MaxGameID_8 = 45;

        #region Met Locations

        internal static readonly int[] Met_SWSH_0 =
        {
        };

        internal static readonly int[] Met_SWSH_3 =
        {
        };

        internal static readonly int[] Met_SWSH_4 =
        {
        };

        internal static readonly int[] Met_SWSH_6 =
        {
        };

        public const int StandardHatchLocation8 = 50; // todo

        #endregion

        internal static readonly ushort[] Pouch_Regular_SWSH = // 00
        {
        };

        internal static readonly ushort[] Pouch_Ball_SWSH = { // 08
        };

        internal static readonly ushort[] Pouch_Battle_SWSH = { // 16
        };

        internal static readonly ushort[] Pouch_Items_SWSH = Pouch_Regular_SWSH.Concat(Pouch_Ball_SWSH).Concat(Pouch_Battle_SWSH).ToArray();

        internal static readonly ushort[] Pouch_Key_SWSH = {
        };

        internal static readonly ushort[] Pouch_TMHM_SWSH = { // 02
        };

        internal static readonly ushort[] Pouch_Medicine_SWSH = { // 32
        };

        internal static readonly ushort[] Pouch_Berries_SWSH = {
        };

        internal static readonly ushort[] HeldItems_SWSH = new ushort[1].Concat(Pouch_Items_SWSH).Concat(Pouch_Berries_SWSH).Concat(Pouch_Medicine_SWSH).ToArray();

        internal static readonly HashSet<int> GalarOriginForms = new HashSet<int>
        {
        };

        internal static readonly HashSet<int> GalarVariantFormEvolutions = new HashSet<int>
        {
        };

        internal static readonly HashSet<int> EvolveToGalarForms = new HashSet<int>(GalarVariantFormEvolutions.Concat(GalarOriginForms));

        internal static readonly HashSet<int> ValidMet_SWSH = new HashSet<int>
        {
        };

        public static readonly int[] TMHM_SWSH =
        {
            // TM
            005, 025, 006, 007, 008, 009, 019, 042, 063, 416,
            345, 076, 669, 083, 086, 091, 103, 113, 115, 219,
            120, 156, 157, 168, 173, 182, 184, 196, 202, 204,
            211, 213, 201, 240, 241, 258, 250, 251, 261, 263,
            129, 270, 279, 280, 286, 291, 311, 313, 317, 328,
            331, 333, 340, 341, 350, 362, 369, 371, 372, 374,
            384, 385, 683, 409, 419, 421, 422, 423, 424, 427,
            433, 472, 478, 440, 474, 490, 496, 506, 512, 514,
            521, 523, 527, 534, 541, 555, 566, 577, 580, 581,
            604, 678, 595, 598, 206, 403, 684, 693, 707, 784,

            // TR
            014, 034, 053, 056, 057, 058, 059, 067, 085, 087,
            089, 094, 097, 116, 118, 126, 127, 133, 141, 161,
            164, 179, 188, 191, 200, 473, 203, 214, 224, 226,
            227, 231, 242, 247, 248, 253, 257, 269, 271, 276,
            285, 299, 304, 315, 322, 330, 334, 337, 339, 347,
            348, 349, 360, 370, 390, 394, 396, 398, 399, 402,
            404, 405, 406, 408, 411, 412, 413, 414, 417, 428,
            430, 437, 438, 441, 442, 444, 446, 447, 482, 484,
            486, 492, 500, 502, 503, 526, 528, 529, 535, 542,
            583, 599, 605, 663, 667, 675, 676, 706, 710, 776,
        };

        internal static readonly byte[] MovePP_SWSH =
        {
        };

        internal static readonly HashSet<int> Ban_NoHidden8 = new HashSet<int>
        {
        };

        internal static readonly HashSet<int> TransferrableGalar = new HashSet<int>()
        {
        };

        #region Unreleased Items
        internal static readonly HashSet<int> UnreleasedHeldItems_8 = new HashSet<int>
        {
        };
        #endregion
        internal static readonly bool[] ReleasedHeldItems_8 = Enumerable.Range(0, MaxItemID_8 + 1).Select(i => HeldItems_SWSH.Contains((ushort)i) && !UnreleasedHeldItems_8.Contains(i)).ToArray();
    }
}
