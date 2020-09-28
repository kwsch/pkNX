using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public class EncounterGift8
    {
        public int IsEgg { get; set; }
        public int AltForm { get; set; }
        public int DynamaxLevel { get; set; }
        public Ball Ball { get; set; }
        public int Field_04 { get; set; }
        public ulong Hash1 { get; set; }
        public bool CanGigantamax { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public Species Species { get; set; }
        public int Field_0A { get; set; }
        public int Field_0B { get; set; }
        public int Field_0C { get; set; }
        public int Field_0D { get; set; }
        public int Field_0E { get; set; }
        public ulong Hash2 { get; set; }
        public int Field_10 { get; set; }
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
        public int SpecialMove { get; set; }

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            int[] IVs = new[] { IV_Hp, IV_Atk, IV_Def, IV_Spe, IV_SpAtk, IV_SpDef };

            var ability = Ability switch
            {
                0 => string.Empty,
                3 => ", Ability = 4",
                _ => $", Ability = {Ability}",
            };

            var ivs = IVs[0] switch
            {
                31 when IVs.All(z => z == 31) => ", FlawlessIVCount = 6",
                -1 when IVs.All(z => z == -1) => string.Empty,
                -4 => ", FlawlessIVCount = 3",
                _ => $", IVs = new[]{{{string.Join(",", IVs)}}}",
            };

            var gender = Gender == FixedGender.Random ? string.Empty : $", Gender = {(int)Gender - 1}";
            var nature = Nature == Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm:00}";
            var shiny = ShinyLock == Shiny.Never ? ", Shiny = Never" : string.Empty;
            var giga = !CanGigantamax ? string.Empty : $", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";
            var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {(int)Ball}";

            return
                $"            new EncounterStatic8 {{ Gift = true, Species = {(int)Species:000}, Level = {Level:00}, Location = -01{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna}{ball} }},{comment}";
        }
    }
}