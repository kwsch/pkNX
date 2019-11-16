namespace pkNX.Structures
{
    public class BattleTowerTrainer8Archive
    {
        public BattleTowerTrainer8[] Entries { get; set; }
    }

    public class BattleTowerTrainer8
    {
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public ushort EntryID { get; set; }
        public ushort Field_03 { get; set; }
        public ushort Field_04 { get; set; }
        public ushort[] Choices { get; set; }
    }
}
