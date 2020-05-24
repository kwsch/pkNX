namespace pkNX.Structures
{
#pragma warning disable CA1819 // Properties should not return arrays
    public class EncounterArchive7b
    {
        public EncounterTable7b[] EncounterTables { get; set; }
    }

    public class EncounterTable7b
    {
        public ulong ZoneID { get; set; }
        public int TrainerRankMin { get; set; }
        public int TrainerRankMax { get; set; }

        public bool GroundSpawnAllowed { get; set; }
        public int GroundSpawnCountMax { get; set; }
        public int GroundSpawnDuration { get; set; }
        public int GroundTableEncounterRate { get; set; }
        public int GroundTableLevelMin { get; set; }
        public int GroundTableLevelMax { get; set; }
        public int GroundTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] GroundTable { get; set; }

        public bool WaterSpawnAllowed { get; set; }
        public int WaterSpawnCountMax { get; set; }
        public int WaterSpawnDuration { get; set; }
        public int WaterTableEncounterRate { get; set; }
        public int WaterTableLevelMin { get; set; }
        public int WaterTableLevelMax { get; set; }
        public int WaterTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] WaterTable { get; set; }

        public int OldRodTableEncounterRate { get; set; }
        public int OldRodTableLevelMin { get; set; }
        public int OldRodTableLevelMax { get; set; }
        public int OldRodTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] OldRodTable { get; set; }

        public int GoodRodTableEncounterRate { get; set; }
        public int GoodRodTableLevelMin { get; set; }
        public int GoodRodTableLevelMax { get; set; }
        public int GoodRodTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] GoodRodTable { get; set; }

        public int SuperRodTableEncounterRate { get; set; }
        public int SuperRodTableLevelMin { get; set; }
        public int SuperRodTableLevelMax { get; set; }
        public int SuperRodTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] SuperRodTable { get; set; }

        public bool SkySpawnAllowed { get; set; }
        public int SkySpawnCountMax { get; set; }
        public int SkySpawnDuration { get; set; }
        public int SkyTableEncounterRate { get; set; }
        public int SkyTableLevelMin { get; set; }
        public int SkyTableLevelMax { get; set; }
        public int SkyTableRandChanceTotal { get; set; }
        public EncounterSlot7b[] SkyTable { get; set; }
    }

    public class EncounterSlot7b
    {
        public int Probability { get; set; }
        public int Species { get; set; }
        public int Form { get; set; }
    }
}
