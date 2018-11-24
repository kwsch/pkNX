using System.ComponentModel;

namespace pkNX.Randomization
{
    public class TrainerRandSettings
    {
        private const string General = nameof(General);
        private const string Classes = nameof(Classes);
        private const string Pokémon = nameof(Pokémon);
        private const string Stats = nameof(Stats);
        private const string Moves = nameof(Moves);

        #region General
        [Category(General), Description("Minimum count of Pokémon the Trainer has. New Pokémon will be added to the team if less are currently present.")]
        public int TeamCountMin { get; set; }

        [Category(General), Description("Maximum count of Pokémon the Trainer has. Pokémon will be removed from the team if more are currently present.")]
        public int TeamCountMax { get; set; }

        [Category(General), Description("Chooses a random type for the Trainer, and requires each Pokémon to have that type.")]
        public bool TeamTypeThemed { get; set; }

        [Category(General), Description("Maxes out the Trainer AI value to use its team and moves most effectively.")]
        public bool TrainerMaxAI { get; set; }

        [Category(General), Description("Force special strong battles to have a full team of 6 Pokémon.")]
        public bool ForceSpecialTeamCount6 { get; set; }
        #endregion

        #region General
        [Category(Classes), Description("Change Trainer Class to another random Trainer Class.")]
        public bool RandomTrainerClass { get; set; }

        [Category(Classes), Description("Skip changing Trainer Classes that are considered special (avoiding crashes).")]
        public bool SkipSpecialClasses { get; set; }
        #endregion

        #region Pokémon
        [Category(Pokémon), Description("Randomizes the Species and basic stat details of all Team members.")]
        public bool RandomizeTeam { get; set; }

        [Category(Pokémon), Description("Allows random Mega Forms when randomizing species.")]
        public bool AllowRandomMegaForms { get; set; }

        [Category(Pokémon), Description("Forces all Pokémon above the specified level setting to be fully evolved.")]
        public bool ForceFullyEvolved { get; set; }

        [Category(Pokémon), Description("Forces all Pokémon above this level to be fully evolved if the " + nameof(ForceFullyEvolved) + " setting is set.")]
        public int ForceFullyEvolvedAtLevel { get; set; }

        [Category(Pokémon), Description("Causes all Pokémon levels to be boosted by the specified ratio multiplier.")]
        public bool BoostLevel { get; set; }

        [Category(Pokémon), Description("Boosts levels of all Pokémon by this ratio if the " + nameof(BoostLevel) + "setting is set.")]
        public decimal LevelBoostRatio { get; set; }
        #endregion

        #region Stats
        [Category(Stats), Description("Makes random Trainer Pokémon shiny.")]
        public bool RandomShinies { get; set; }

        [Category(Stats), Description("Makes random Trainer Pokémon shiny at this rate.")]
        public decimal ShinyChance { get; set; }

        [Category(Stats), Description("Maximizes all IVs.")]
        public bool MaxIVs { get; set; }

        [Category(Stats), Description("Picks a random valid ability for each Pokémon.")]
        public bool RandomAbilities { get; set; }
        #endregion

        #region Moves
        [Category(Moves), Description("Prevents Pokémon movesets from containing fixed damage moves.")]
        public bool BanFixedDamageMoves { get; set; }

        [Category(Moves), Description("How movesets are randomized/chosen for each Pokémon.")]
        public MoveRandType MoveRandType { get; set; }
        #endregion
    }
}