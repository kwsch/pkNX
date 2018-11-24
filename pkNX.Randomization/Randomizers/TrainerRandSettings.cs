using System.ComponentModel;

namespace pkNX.Randomization
{
    public class TrainerRandSettings
    {
        private const string General = nameof(General);
        private const string Classes = nameof(Classes);
        private const string Misc = nameof(Misc);
        private const string Pokémon = nameof(Pokémon);
        private const string Stats = nameof(Stats);
        private const string Moves = nameof(Moves);

        [Category(General), Description("")]
        public int TeamCountMin { get; set; }
        [Category(General), Description("")]
        public int TeamCountMax { get; set; }
        [Category(General), Description("")]
        public bool TeamTypeThemed { get; set; }
        [Category(General), Description("")]
        public bool TrainerMaxAI { get; set; }
        [Category(General), Description("")]
        public bool ForceSpecialTeamCount6 { get; set; }

        [Category(Classes), Description("")]
        public bool SkipSpecialClasses { get; set; }
        [Category(Classes), Description("")]
        public bool RandomTrainerClass { get; set; }

        [Category(Pokémon), Description("")]
        public bool RandomizeTeam { get; set; }
        [Category(Pokémon), Description("")]
        public bool ForceFullyEvolved { get; set; }
        [Category(Pokémon), Description("")]
        public int ForceFullyEvolvedAtLevel { get; set; }

        [Category(Pokémon), Description("")]
        public bool BoostLevel { get; set; }
        [Category(Pokémon), Description("")]
        public decimal LevelBoostRatio { get; set; }

        [Category(Stats), Description("")]
        public bool RandomShinies { get; set; }
        [Category(Stats), Description("")]
        public decimal ShinyChance { get; set; }
        [Category(Stats), Description("")]
        public bool MaxIVs { get; set; }
        [Category(Stats), Description("")]
        public bool RandomAbilities { get; set; }
        [Category(Misc), Description("")]
        public bool AllowRandomMegaForms { get; set; }

        [Category(Moves), Description("")]
        public bool BanFixedDamageMoves { get; set; }
        [Category(Moves), Description("")]
        public MoveRandType MoveRandType { get; set; }
    }
}