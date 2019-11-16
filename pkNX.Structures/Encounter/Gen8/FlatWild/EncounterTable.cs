namespace pkNX.Structures
{
    public class EncounterArchive8
    {
        public uint Field_00 { get; set; }
        public EncounterTable8[] EncounterTables { get; set; }
    }

    public class EncounterTable8
    {
        public ulong ZoneID { get; set; }
        public EncounterSubTable8[] SubTables { get; set; }
    }

    public class EncounterSubTable8
    {
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public EncounterSlot8[] Slots { get; set; }
    }

    public class EncounterSlot8
    {
        public int Probability { get; set; }
        public int Species { get; set; }
        public int Form { get; set; }
    }
}
