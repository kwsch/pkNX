using System.Collections.Generic;

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
        public int Ball { get; set; }
        public int Field_03 { get; set; }
        public ulong Hash0 { get; set; }
        public bool CanGigantamax { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public int Species { get; set; }
        public ulong Hash1 { get; set; }
        public int TrainerID { get; set; }
        public int Memory { get; set; }
        public int TextVar { get; set; }
        public int Feeling { get; set; }
        public int Intensity { get; set; }
        public ulong Hash2 { get; set; }
        public int OTGender { get; set; }
        public int RequiredForm { get; set; }
        public int RequiredSpecies { get; set; }
        public int RequiredNature { get; set; }
        public int UnknownRequirement { get; set; } // all 0; we know this field is a trade requirement, but unsure what exactly
        public int ShinyLock { get; set; }
        public int Nature { get; set; }
        public int Gender { get; set; }
        public int IV_Hp { get; set; }
        public int IV_Atk { get; set; }
        public int IV_Def { get; set; }
        public int IV_SpAtk { get; set; }
        public int IV_SpDef { get; set; }
        public int IV_Spe { get; set; }
        public int Ability { get; set; }
        public int Relearn1 { get; set; }
        public int Relearn2 { get; set; }
        public int Relearn3 { get; set; }
        public int Relearn4 { get; set; }

        private int[] Relearn => new[] {Relearn1, Relearn2, Relearn3, Relearn4};

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            var ability = $"Ability = {Ability}";
            var otgender = $", OTGender = {OTGender}";
            var gender = $", Gender = {Gender}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var tid = $", TID7 = {TrainerID}";
            var dyna = $", DynamaxLevel = {DynamaxLevel}";
            var relearn = Relearn1 == 0 ? "                                   " : $", Relearn = new[] {{{Relearn1},{Relearn2},{Relearn3},{Relearn4}}}";
            const string iv = ", IVs = TradeIVs";

            return
                $"            new EncounterTrade8({Species:000},{Level:00},{Memory:00},{TextVar:000},{Feeling:00},{Intensity}) {{ {ability}{tid}{iv}{dyna}{otgender}{gender}{altform}{giga}{relearn} }},{comment}";
        }
    }
}