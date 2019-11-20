using System.Collections.Generic;

namespace pkNX.Structures
{
    public class EncounterTrade8Archive
    {
        public EncounterTrade8[] Table { get; set; }
    }

    public class EncounterTrade8
    {
        public byte Field_00 { get; set; }
        public byte DynamaxLevel { get; set; }
        public uint Field_02 { get; set; }
        public byte Field_03 { get; set; }
        public ulong Hash0 { get; set; }
        public byte AltForm { get; set; } // ???
        public uint HeldItem { get; set; }
        public byte Level { get; set; }
        public uint Species { get; set; }
        public ulong Hash1 { get; set; }
        public uint TrainerID { get; set; }
        public byte Field_0B { get; set; }
        public byte Field_0C { get; set; }
        public byte Field_0D { get; set; }
        public byte Field_0E { get; set; }
        public ulong Hash2 { get; set; }
        public byte OTGender { get; set; }
        public uint RequiredForm { get; set; }
        public uint RequiredSpecies { get; set; }
        public uint Nature { get; set; }
        public byte Memory { get; set; }
        public uint Argument { get; set; }
        public uint Feeling { get; set; }
        public uint Intensity { get; set; }
        public byte IV_Hp { get; set; }
        public byte IV_Atk { get; set; }
        public byte IV_Def { get; set; }
        public byte IV_SpAtk { get; set; }
        public byte IV_SpDef { get; set; }
        public byte IV_Spe { get; set; }
        public byte Ability { get; set; }
        public ushort Relearn1 { get; set; }
        public ushort Relearn2 { get; set; }
        public ushort Relearn3 { get; set; }
        public ushort Relearn4 { get; set; }

        private ushort[] Relearn => new[] {Relearn1, Relearn2, Relearn3, Relearn4};

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            var ability = $"Ability = {Ability}";
            var gender = $", OTGender = {OTGender}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm}";
            var tid = $", TID7 = {TrainerID}";
            var dyna = $", DynamaxLevel = {DynamaxLevel}";
            var relearn = Relearn1 == 0 ? "                                   " : $", Relearn = new[] {{{Relearn1},{Relearn2},{Relearn3},{Relearn4}}}";
            const string iv = ", IVs = TradeIVs";

            return
                $"            new EncounterTrade8({Species:000},{Level:00}) {{ {ability}{tid}{iv}{dyna}{gender}{altform}{relearn} }},{comment}";
        }
    }
}
