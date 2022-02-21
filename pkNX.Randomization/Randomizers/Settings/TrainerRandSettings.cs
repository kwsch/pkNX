using System;
using System.ComponentModel;

namespace pkNX.Randomization
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TrainerRandSettings
    {
        private const string General = nameof(General);
        private const string Classes = nameof(Classes);
        private const string PKM = nameof(PKM);
        private const string Stats = nameof(Stats);
        private const string Moves = nameof(Moves);

        #region General
        [Category(General), Description("Modifies the team count per specifications, and forces fixed counts for some trainers.")]
        public bool ModifyTeamCount { get; set; } = true;

        [Category(General), Description("Minimum count of PKM the Trainer has. New PKM will be added to the team if less are currently present.")]
        public int TeamCountMin { get; set; } = 1;

        [Category(General), Description("Maximum count of PKM the Trainer has. PKM will be removed from the team if more are currently present.")]
        public int TeamCountMax { get; set; } = 6;

        [Category(General), Description("Chooses a random type for the Trainer, and requires each PKM to have that type.")]
        public bool TeamTypeThemed { get; set; } = false;

        [Category(General), Description("Maxes out the Trainer AI value to use its team and moves most effectively.")]
        public bool TrainerMaxAI { get; set; } = true;

        [Category(General), Description("Force special strong battles to have a full team of 6 PKM.")]
        public bool ForceSpecialTeamCount6 { get; set; } = true;

        [Category(General), Description("Force all battles to be a Double Battle with an even (not odd) amount of PKM.")]
        public bool ForceDoubles { get; set; }
        #endregion

        #region Classes
        [Category(Classes), Description("Change Trainer Class to another random Trainer Class.")]
        public bool RandomTrainerClass { get; set; } = false;

        [Category(Classes), Description("Skip changing Trainer Classes that are considered special (avoiding crashes).")]
        public bool SkipSpecialClasses { get; set; } = true;
        #endregion

        #region PKM
        [Category(PKM), Description("Randomizes the Species and basic stat details of all Team members.")]
        public bool RandomizeTeam { get; set; } = true;

        [Category(PKM), Description("Allows random Mega Forms when randomizing species.")]
        public bool AllowRandomMegaForms { get; set; } = false;

        [Category(PKM), Description("Allows random Fused PKM when randomizing species.")]
        public bool AllowRandomFusions { get; set; } = false;

        [Category(PKM), Description("Allows random Held Items when randomizing species.")]
        public bool AllowRandomHeldItems { get; set; } = false;

        [Category(PKM), Description("Forces all PKM above the specified level setting to be fully evolved.")]
        public bool ForceFullyEvolved { get; set; } = true;

        [Category(PKM), Description("Forces all PKM above this level to be fully evolved if the " + nameof(ForceFullyEvolved) + " setting is set.")]
        public int ForceFullyEvolvedAtLevel { get; set; } = 36;

        [Category(PKM), Description("Swaps Gigantamaxed species with other Gigantamaxed species.")]
        public bool GigantamaxSwap { get; set; } = false;

        [Category(PKM), Description("Causes all PKM levels to be boosted by the specified ratio multiplier.")]
        public bool BoostLevel { get; set; } = true;

        [Category(PKM), Description("Boosts levels of all PKM by this ratio if the " + nameof(BoostLevel) + " setting is set.")]
        public float LevelBoostRatio { get; set; } = 1.1f;
        #endregion

        #region Stats
        [Category(Stats), Description("Makes random Trainer PKM shiny.")]
        public bool RandomShinies { get; set; } = true;

        [Category(Stats), Description("Makes random Trainer PKM shiny at this rate (percent).")]
        public float ShinyChance { get; set; } = 2.5f;

        [Category(Stats), Description("Maximizes all IVs.")]
        public bool MaxIVs { get; set; } = true;

        [Category(Stats), Description("Picks a random valid ability for each PKM.")]
        public bool RandomAbilities { get; set; } = true;

        [Category(Stats), Description("Makes all Dynamaxed PKM have a Dynamax Level of 10.")]
        public bool MaxDynamaxLevel { get; set; } = true;
        #endregion

        #region Moves
        [Category(Moves), Description("How movesets are randomized/chosen for each PKM.")]
        public MoveRandType MoveRandType { get; set; } = MoveRandType.RandomMoves;
        #endregion
    }
}