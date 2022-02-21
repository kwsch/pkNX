using System;
using System.ComponentModel;

namespace pkNX.Randomization
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LearnSettings
    {
        private const string General = nameof(General);
        private const string Misc = nameof(Misc);

        [Category(General), Description("Expands the learnset to the specified count.")]
        public bool Expand { get; set; } = true;

        [Category(General), Description("Count to expand the learnset to.")]
        public int ExpandTo { get; set; } = 25;

        [Category(General), Description("Evenly spreads learned moves out from level 1 to the specified end level.")]
        public bool Spread { get; set; } = true;

        [Category(General), Description("Level to end learning level up moves.")]
        public int SpreadTo { get; set; } = 75;

        [Category(Misc), Description("Reorders moves so that moves are learned with increasing power.")]
        public bool OrderByPower { get; set; } = true;

        [Category(Misc), Description("Requires the first move learned to be STAB.")]
        public bool STABFirst { get; set; } = true;

        [Category(Misc), Description("Requires 4 moves to be available at level 1.")]
        public bool Learn4Level1 { get; set; } = false;
    }
}