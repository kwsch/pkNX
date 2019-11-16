namespace pkNX.Structures
{
    public class BattleTowerPoke8Archive
    {
        public BattleTowerPoke8[] Entries { get; set; }
    }

    public class BattleTowerPoke8
    {
        public bool Field_00 { get; set; }
        public bool Field_01 { get; set; }
        public bool Field_02 { get; set; }
        public uint Field_03 { get; set; }
        public bool Field_04 { get; set; }
        public bool Field_05 { get; set; }
        public bool Field_06 { get; set; }
        public uint AltForm { get; set; }
        public uint Field_08 { get; set; }
        public uint HeldItem { get; set; }
        public uint Species { get; set; }
        public uint EntryID { get; set; }
        public uint Field_0C { get; set; }
        public uint Nature { get; set; }
        public uint Field_0E { get; set; }
        public uint IV_Hp { get; set; }
        public uint IV_Atk { get; set; }
        public uint IV_Def { get; set; }
        public uint IV_SpAtk { get; set; }
        public uint IV_SpDef { get; set; }
        public uint IV_Spe { get; set; }
        public uint Field_15 { get; set; }
        public uint Move0 { get; set; }
        public uint Move1 { get; set; }
        public uint Move2 { get; set; }
        public uint Move3 { get; set; }
    }
}
