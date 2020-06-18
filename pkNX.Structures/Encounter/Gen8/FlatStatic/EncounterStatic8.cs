using System.Collections.Generic;

namespace pkNX.Structures
{
    public class EncounterStatic8
    {
        public ulong Hash1 { get; set; }
        public ulong Hash2 { get; set; }
        public int EV_Spe { get; set; }
        public int EV_Atk { get; set; }
        public int EV_Def { get; set; }
        public int EV_Hp { get; set; }
        public int EV_SpAtk { get; set; }
        public int EV_SpDef { get; set; }
        public int AltForm { get; set; }
        public int DynamaxLevel { get; set; }
        public int Field_0A { get; set; }
        public ulong Hash3 { get; set; }
        public bool CanGigantamax { get; set; }
        public int Field_0D { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public int Field_10 { get; set; }
        public Species Species { get; set; }
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
        public int Move0 { get; set; }
        public int Move1 { get; set; }
        public int Move2 { get; set; }
        public int Move3 { get; set; }

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            var ability = Ability == 0 ? string.Empty : $", Ability = {Ability}";
            var gender = Gender == FixedGender.Random ? string.Empty : $", Gender = {(int)Gender - 1}";
            var nature = Nature == Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm:00}";
            var moves = Move0 == 0 ? string.Empty : $", Moves = new[] {{{Move0:000},{Move1:000},{Move2:000},{Move3:000}}}";
            var ivs = IV_Hp == -4 ? ", FlawlessIVCount = 3" : string.Empty;
            var shiny = ShinyLock == Shiny.Never ? ", Shiny = Never" : string.Empty;
            var giga = !CanGigantamax ? string.Empty : $", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";

            return
                $"            new EncounterStatic8 {{ Species = {(int)Species:000}, Level = {Level:00}, Location = -01{moves}{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna} }},{comment}";
        }
    }
}