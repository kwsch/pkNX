using System;
using System.Collections.Generic;
using System.ComponentModel;
using pkNX.Structures;

namespace pkNX.Randomization
{
    /// <summary>
    /// Randomization settings when randomizing a <see cref="PersonalInfo"/>.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PersonalRandSettings : RandSettings
    {
        private const string Moves = nameof(Moves);
        private const string Types = nameof(Types);
        private const string Stats = nameof(Stats);
        private const string Abilities = nameof(Abilities);
        private const string Misc = nameof(Misc);
        private const string Evolutions = nameof(Evolutions);

        /// <summary>Toggle to use Evolution data for inherited <see cref="PersonalInfo"/> properties.</summary>
        [Category("A1"), Description("Evolution Chain Species Randomization instead of Pure Random. Refer to the Evolutions settings when using this mode.")]
        public bool ModifyByEvolutions { get; set; } = true;

        #region Moves
        /// <summary>Permits randomizing the <see cref="PersonalInfo.TMHM"/> values.</summary>
        [Category(Moves), Description("Enables randomizing the TM (Technical Machine) compatibility flags.")]
        public bool ModifyLearnsetTM { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.TMHM"/> values.</summary>
        [Category(Moves), Description("Enables randomizing the HM (Hidden Machine) compatibility flags.")]
        public bool ModifyLearnsetHM { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.TMHM"/> values.</summary>
        [Category(Moves), Description("Enables randomizing the move tutor compatibility flags.")]
        public bool ModifyLearnsetMoveTutors { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.TMHM"/> values.</summary>
        [Category(Moves), Description("Enables randomizing the type tutor compatibility flags.")]
        public bool ModifyLearnsetTypeTutors { get; set; } = false;

        /// <summary>Percent chance to learn a TMHM move (0-100).</summary>
        /// <remarks>Average Learnable TMs is 35.260.</remarks>
        [Category(Moves), Description("Percentage chance to learn a given TM move.")]
        public float LearnTMPercent { get; set; } = 35;

        /// <summary>Percent chance to learn a type tutor move (0-100).</summary>
        /// <remarks>136 special tutor moves learnable by species in Untouched ORAS.</remarks>
        [Category(Moves), Description("Percentage chance to learn a given special Type Tutor move.")]
        public float LearnTypeTutorPercent { get; set; } = 2;

        /// <summary>Percent chance to learn a tutor move (0-100).</summary>
        /// <remarks>10001 tutor moves learnable by 826 species in Untouched ORAS.</remarks>
        [Category(Moves), Description("Percentage chance to learn a given Move Tutor move.")]
        public float LearnMoveTutorPercent { get; set; } = 30;
        #endregion

        #region Types
        /// <summary>Permits modification of <see cref="PersonalInfo.Types"/>.</summary>
        [Category(Types), Description("Enables a PKM's Type to be modified.")]
        public bool ModifyTypes { get; set; } = true;

        /// <summary>Option to modify the <see cref="PersonalInfo.Types"/>.</summary>
        [Category(Types), Description("Option to modify the Types depending on the specified setting.")]
        public ModifyState Type { get; set; } = ModifyState.All;

        /// <summary>Chance that both types are the same.</summary>
        [Category(Types), Description("Chance that both types are the same.")]
        public float SameTypeChance { get; set; } = 50;
        #endregion

        #region Ability
        /// <summary>Toggle to permit modification of <see cref="PersonalInfo.Abilities"/>.</summary>
        [Category(Abilities), Description("Enables a PKM's Abilities to be modified.")]
        public bool ModifyAbility { get; set; } = true;

        /// <summary>Permits Wonder Guard as a random ability.</summary>
        [Category(Abilities), Description("Permits Wonder Guard as a random ability.")]
        public Permissive WonderGuard { get; set; } = Permissive.No;

        /// <summary>Option to modify the <see cref="PersonalInfo.Abilities"/>.</summary>
        [Category(Abilities), Description("Option to modify the Abilities depending on the specified setting.")]
        public ModifyState Ability { get; set; } = ModifyState.All;

        /// <summary>Chance that both abilities are the same.</summary>
        [Category(Abilities), Description("Chance that both abilities are the same.")]
        public float SameAbilityChance { get; set; } = 100;
        #endregion

        #region Stats
        /// <summary>Permits modification of <see cref="PersonalInfo.Stats"/>.</summary>
        [Category(Stats), Description("Enables a PKM's base stats to be modified.")]
        public bool ModifyStats { get; set; } = true;

        /// <summary>Amount a Base Stat is amplified as a low bound.</summary>
        [Category(Stats), Description("Minimum Percentage bound a Base Stat is after randomizing. 100 corresponds to an unchanged minimum.")]
        public int StatDeviationMin { get; set; } = 75;

        /// <summary>Amount a Base Stat is amplified as a high bound.</summary>
        [Category(Stats), Description("Maximum Percentage bound a Base Stat is after randomizing. 100 corresponds to an unchanged maximum.")]
        public int StatDeviationMax { get; set; } = 125;

        /// <summary>Toggle to permit shuffling of <see cref="PersonalInfo.Stats"/>.</summary>
        [Category(Stats), Description("Shuffles the PKM's base stats after any modifications have been made.")]
        public bool ShuffleStats { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.HP"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the HP base stat.")]
        public bool HP { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.ATK"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the Attack base stat.")]
        public bool ATK { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.DEF"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the Defense base stat.")]
        public bool DEF { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.SPA"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the Special Attack base stat.")]
        public bool SPA { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.SPD"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the Special Defense base stat.")]
        public bool SPD { get; set; } = true;

        /// <summary>Permits randomizing the <see cref="PersonalInfo.SPE"/> stat.</summary>
        [Category(Stats), Description("Permits randomizing the Speed base stat.")]
        public bool SPE { get; set; } = true;

        /// <summary>
        /// Flags to edit the stats when randomizing.
        /// </summary>
        public IReadOnlyList<bool> StatsToRandomize => new[] {HP, ATK, DEF, SPE, SPA, SPD};
        #endregion

        #region Misc
        /// <summary>Option permitting modification of <see cref="PersonalInfo.CatchRate"/>.</summary>
        [Category(Misc), Description("Enables a PKM's catch rate to be modified. Can inversely scale off BST.")]
        public CatchRate CatchRate { get; set; } = CatchRate.Unchanged;

        /// <summary>Permits modification of Held Items.</summary>
        [Category(Misc), Description("Enables a PKM's held items to be modified.")]
        public bool ModifyHeldItems { get; set; } = true;

        /// <summary>Chance all held items are the same.</summary>
        [Category(Misc), Description("Percentage chance that all Held Items are the same, resulting in a 100% chance of having the held item.")]
        public float AlwaysHeldItemChance { get; set; } = 20;

        /// <summary>Permits modification of <see cref="PersonalInfo.EggGroups"/>.</summary>
        [Category(Misc), Description("Enables a PKM's egg groups to be modified.")]
        public bool ModifyEgg { get; set; } = false;

        /// <summary>Chance both egg groups are the same.</summary>
        [Category(Misc), Description("Percentage chance that both egg groups will be the same.")]
        public float SameEggGroupChance { get; set; } = 50;
        #endregion

        #region Evolutions
        /// <summary>Toggles inheriting types from the pre-evolution that evolves into this species/form.</summary>
        [Category(Evolutions), Description("Toggles inheriting types from the pre-evolution that evolves into this species/form.")]
        public bool InheritType { get; set; } = true;

        /// <summary>Maximum amount of Types that can be different from the pre-evolution.</summary>
        [Category(Evolutions), Description("Maximum amount of Types that can be different from the pre-evolution.")]
        public ModifyState InheritTypeSetting { get; set; } = ModifyState.One;

        /// <summary>Percentage chance that only one type will be inherited, and a new random one will replace the other.</summary>
        [Category(Evolutions), Description("Percentage chance that only one type will be inherited, and a new random one will replace the other.")]
        public float InheritTypeOnlyOneChance { get; set; } = 65;

        /// <summary>Percentage chance that neither one type will be inherited, and new random ones will replace the others.</summary>
        [Category(Evolutions), Description("Percentage chance that neither type will be inherited, and new random ones will replace the others.")]
        public float InheritTypeNeitherChance { get; set; } = 30;

        /// <summary>Toggles  chance that neither one type will be inherited, and new random ones will replace the others.</summary>
        [Category(Evolutions), Description("Amount of abilities that will be inherited, and new random ones will replace the others.")]
        public ModifyState InheritAbilitySetting { get; set; } = ModifyState.One;

        /// <summary>Toggles inheriting abilities from the pre-evolution that evolves into this species/form.</summary>
        [Category(Evolutions), Description("Toggles inheriting abilities from the pre-evolution that evolves into this species/form.")]
        public bool InheritAbility { get; set; } = true;

        /// <summary>Percentage chance that only one ability will be inherited, and a new random one will replace the other.</summary>
        [Category(Evolutions), Description("Percentage chance that only one ability will be inherited, and a new random one will replace the other.")]
        public float InheritAbilityOnlyOneChance { get; set; } = 45;

        /// <summary>Percentage chance that neither one ability will be inherited, and new random ones will replace the others.</summary>
        [Category(Evolutions), Description("Percentage chance that neither ability will be inherited, and new random ones will replace the others.")]
        public float InheritAbilityNeitherChance { get; set; } = 20;

        /// <summary>Inherit the held item values from the pre-evolution.</summary>
        [Category(Evolutions), Description("Inherit the held item values from the pre-evolution.")]
        public bool InheritHeldItem { get; set; } = true;

        /// <summary>Inherit the TM/HM compatibility from the pre-evolution.</summary>
        [Category(Evolutions), Description("Inherit the TM/HM compatibility values from the pre-evolution.")]
        public bool InheritChildTM { get; set; } = true;

        /// <summary>Inherit the Tutor compatibility from the pre-evolution.</summary>
        [Category(Evolutions), Description("Inherit the Tutor compatibility values from the pre-evolution.")]
        public bool InheritChildTutor { get; set; } = true;

        /// <summary>Inherit the Type Tutor values from the pre-evolution.</summary>
        [Category(Evolutions), Description("Inherit the Type Tutor compatibility values from the pre-evolution.")]
        public bool InheritChildSpecial { get; set; } = true;
        #endregion
    }
}