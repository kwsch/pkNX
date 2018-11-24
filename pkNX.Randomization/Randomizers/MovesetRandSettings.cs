using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace pkNX.Randomization
{
    public class MovesetRandSettings
    {
        private const string Damage = nameof(Damage);
        private const string SameType = nameof(SameType);
        private const string Misc = nameof(Misc);

        [Category(Damage), Description("Forces the moveset to have a minimum amount of damaging moves.")]
        public bool DMG { get; set; } = true;

        [Category(Damage), Description("Minimum amount of damaging moves in generated movesets.")]
        public int DMGCount { get; set; } = 2;

        [Category(SameType), Description("Forces the moveset to have a minimum amount of STAB moves.")]
        public bool STAB { get; set; } = true;

        [Category(SameType), Description("Minimum amount of STAB moves in generated 4-move movesets.")]
        public int STABCount { get; set; } = 2;

        [Category(SameType), Description("Minimum percent of STAB moves in generated learnsets.")]
        public decimal STABPercent { get; set; } = 100;

        [Category(Misc), Description("Banned move IDs.")]
        internal IList<int> BannedMoves { get; set; } = Array.Empty<int>();
    }
}