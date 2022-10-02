using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterArchive7b
{
    [FlatBufferItem(0)] public EncounterTable7b[] EncounterTables { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterTable7b
{
    [FlatBufferItem(00)] public ulong ZoneID { get; set; }
    [FlatBufferItem(01)] public int TrainerRankMin { get; set; }
    [FlatBufferItem(02)] public int TrainerRankMax { get; set; }

    [FlatBufferItem(03)] public bool GroundSpawnAllowed { get; set; }
    [FlatBufferItem(04)] public int GroundSpawnCountMax { get; set; }
    [FlatBufferItem(05)] public int GroundSpawnDuration { get; set; }
    [FlatBufferItem(06)] public int GroundTableEncounterRate { get; set; }
    [FlatBufferItem(07)] public int GroundTableLevelMin { get; set; }
    [FlatBufferItem(08)] public int GroundTableLevelMax { get; set; }
    [FlatBufferItem(09)] public int GroundTableRandChanceTotal { get; set; }
    [FlatBufferItem(10)] public EncounterSlot7b[] GroundTable { get; set; }

    [FlatBufferItem(11)] public bool WaterSpawnAllowed { get; set; }
    [FlatBufferItem(12)] public int WaterSpawnCountMax { get; set; }
    [FlatBufferItem(13)] public int WaterSpawnDuration { get; set; }
    [FlatBufferItem(14)] public int WaterTableEncounterRate { get; set; }
    [FlatBufferItem(15)] public int WaterTableLevelMin { get; set; }
    [FlatBufferItem(16)] public int WaterTableLevelMax { get; set; }
    [FlatBufferItem(17)] public int WaterTableRandChanceTotal { get; set; }
    [FlatBufferItem(18)] public EncounterSlot7b[] WaterTable { get; set; }

    [FlatBufferItem(19)] public int OldRodTableEncounterRate { get; set; }
    [FlatBufferItem(20)] public int OldRodTableLevelMin { get; set; }
    [FlatBufferItem(21)] public int OldRodTableLevelMax { get; set; }
    [FlatBufferItem(22)] public int OldRodTableRandChanceTotal { get; set; }
    [FlatBufferItem(23)] public EncounterSlot7b[] OldRodTable { get; set; }

    [FlatBufferItem(24)] public int GoodRodTableEncounterRate { get; set; }
    [FlatBufferItem(25)] public int GoodRodTableLevelMin { get; set; }
    [FlatBufferItem(26)] public int GoodRodTableLevelMax { get; set; }
    [FlatBufferItem(27)] public int GoodRodTableRandChanceTotal { get; set; }
    [FlatBufferItem(28)] public EncounterSlot7b[] GoodRodTable { get; set; }

    [FlatBufferItem(29)] public int SuperRodTableEncounterRate { get; set; }
    [FlatBufferItem(30)] public int SuperRodTableLevelMin { get; set; }
    [FlatBufferItem(31)] public int SuperRodTableLevelMax { get; set; }
    [FlatBufferItem(32)] public int SuperRodTableRandChanceTotal { get; set; }
    [FlatBufferItem(33)] public EncounterSlot7b[] SuperRodTable { get; set; }

    [FlatBufferItem(34)] public bool SkySpawnAllowed { get; set; }
    [FlatBufferItem(35)] public int SkySpawnCountMax { get; set; }
    [FlatBufferItem(36)] public int SkySpawnDuration { get; set; }
    [FlatBufferItem(37)] public int SkyTableEncounterRate { get; set; }
    [FlatBufferItem(38)] public int SkyTableLevelMin { get; set; }
    [FlatBufferItem(39)] public int SkyTableLevelMax { get; set; }
    [FlatBufferItem(40)] public int SkyTableRandChanceTotal { get; set; }
    [FlatBufferItem(41)] public EncounterSlot7b[] SkyTable { get; set; }
}

[FlatBufferTable]
public class EncounterSlot7b
{
    [FlatBufferItem(0)] public int Probability { get; set; }
    [FlatBufferItem(1)] public int Species { get; set; }
    [FlatBufferItem(2)] public short Form { get; set; }
}
