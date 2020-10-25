using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public class EncounterTrade8Archive : IFlatBufferArchive<EncounterTrade8>
    {
        public EncounterTrade8[] Table { get; set; }
    }

    public class EncounterTrade8
    {
        public int AltForm { get; set; }
        public int DynamaxLevel { get; set; }
        public Ball Ball { get; set; }
        public int Field_03 { get; set; }
        public ulong Hash0 { get; set; }
        public bool CanGigantamax { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public Species Species { get; set; }
        public ulong Hash1 { get; set; }
        public int TrainerID { get; set; }
        public int Memory { get; set; }
        public int TextVar { get; set; }
        public int Feeling { get; set; }
        public int Intensity { get; set; }
        public ulong Hash2 { get; set; }
        public int OTGender { get; set; }
        public int RequiredForm { get; set; }
        public Species RequiredSpecies { get; set; }
        public Nature RequiredNature { get; set; }
        public int UnknownRequirement { get; set; } // all 0; we know this field is a trade requirement, but unsure what exactly
        public Shiny ShinyLock { get; set; }
        public Nature Nature { get; set; }
        public FixedGender Gender { get; set; }
        public int IV_Spe { get; set; }
        public int IV_Atk { get; set; }
        public int IV_Def { get; set; }
        public int IV_Hp { get; set; }
        public int IV_SpAtk { get; set; }
        public int IV_SpDef { get; set; }
        public int Ability { get; set; }
        public int Relearn1 { get; set; }
        public int Relearn2 { get; set; }
        public int Relearn3 { get; set; }
        public int Relearn4 { get; set; }

        [JsonIgnore]
        public int[] IVs
        {
            get => new[] { IV_Hp, IV_Atk, IV_Def, IV_Spe, IV_SpAtk, IV_SpDef };
            set
            {
                if (value?.Length != 6) return;
                IV_Hp =    value[0];
                IV_Atk =   value[1];
                IV_Def =   value[2];
                IV_Spe =   value[3];
                IV_SpAtk = value[4];
                IV_SpDef = value[5];
            }
        }

        [JsonIgnore]
        private int[] Relearn => new[] {Relearn1, Relearn2, Relearn3, Relearn4};

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            const string iv = ", IVs = TradeIVs";

            var ability = Ability switch
            {
                0 => "             ",
                3 => "Ability = 4, ",
                _ => $"Ability = {Ability}, ",
            };

            var ivs = IVs[0] switch
            {
                31 when IVs.All(z => z == 31) => ", FlawlessIVCount = 6",
                -1 when IVs.All(z => z == -1) => string.Empty,
                -4 => ", FlawlessIVCount = 3",
                _ => iv,
            };

            var otgender = $", OTGender = {OTGender}";
            var gender = Gender == FixedGender.Random ? string.Empty : $", Gender = {(int)Gender - 1}";
            var nature = Nature == Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm}";
            var shiny = ShinyLock == Shiny.Never ? string.Empty : $", Shiny = Shiny.{ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var tid = $"TID7 = {TrainerID}";
            var dyna = $", DynamaxLevel = {DynamaxLevel}";
            var relearn = Relearn1 == 0 ? "                                   " : $", Relearn = new[] {{{Relearn1:000},{Relearn2:000},{Relearn3:000},{Relearn4:000}}}";
            var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {Ball}";

            return
                $"            new EncounterTrade8({(int)Species:000},{Level:00},{Memory:00},{TextVar:000},{Feeling:00},{Intensity}) {{ {ability}{tid}{ivs}{dyna}{otgender}{gender}{shiny}{nature}{giga}{relearn}{altform}{ball} }},{comment}";
        }
    }
}