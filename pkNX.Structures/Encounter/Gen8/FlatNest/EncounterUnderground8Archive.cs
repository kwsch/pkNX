using System.Collections.Generic;
using Newtonsoft.Json;

namespace pkNX.Structures
{
    public class EncounterUnderground8Archive
    {
        public EncounterUnderground8[] PokemonTables;
    }

    public class EncounterUnderground8
    {
        public bool HasFlagRequirement { get; set; }
        public ulong FlagRequirementID { get; set; }
        public byte Field_02 { get; set; } // all zero, Gender?
        public byte AltForm { get; set; }
        public uint GigantamaxState { get; set; }
        public uint Ball { get; set; }
        public uint IndexNum { get; set; }
        public uint Level { get; set; }
        public uint Species { get; set; }
        public ulong UiMessageID { get; set; }
        public uint OT_Gender { get; set; }
        public byte Version { get; set; }
        public uint Shiny { get; set; }
        public sbyte IV_SPE { get; set; }
        public sbyte IV_ATK { get; set; }
        public sbyte IV_DEF { get; set; }
        public sbyte IV_HP { get; set; }
        public sbyte IV_SPA { get; set; }
        public sbyte IV_SPD { get; set; }
        public uint Ability { get; set; }
        public byte Field_14 { get; set; } // ultra beasts only, selectability
        public uint Move0 { get; set; }
        public uint Move1 { get; set; }
        public uint Move2 { get; set; }
        public uint Move3 { get; set; }

        [JsonIgnore] public int Gender => 0; // Random
        [JsonIgnore] public bool IsGigantamax => GigantamaxState == 2;

        public override string ToString() => $"{IndexNum:00} - {Species:000}";

        public string GetSummary(IReadOnlyList<string> species)
        {
            var gender = Gender == 0 ? string.Empty : $", Gender = {Gender - 1}";
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            var moves = $", Moves = new[] {{{Move0:000},{Move1:000},{Move2:000},{Move3:000}}}";
            var game = Version != 0 ? Version == 1 ? ", Version = GameVersion.SW" : ", Version = GameVersion.SH" : "";
            var g = IsGigantamax ? ", CanGigantamax = true" : "";
            return $"            new EncounterStatic8U({Species:000},{AltForm},{Level:00}) {{ Ability = A{Ability}{gender}{moves}{g}{game} }},{comment}";
        }
    }
}