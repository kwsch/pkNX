using System.ComponentModel;
using System.IO;
using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidEnemyTableArray : IFlatBufferArchive<DeliveryRaidEnemyTable>
{
    [FlatBufferItem(0)] public DeliveryRaidEnemyTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidEnemyTable
{
    [FlatBufferItem(0)] public RaidEnemyInfo RaidEnemyInfo { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidTimeData
{
    [FlatBufferItem(0)] public bool IsActive { get; set; }
    [FlatBufferItem(1)] public int GameLimit { get; set; }
    [FlatBufferItem(2)] public int ClientLimit { get; set; }
    [FlatBufferItem(3)] public int CommandLimit { get; set; }
    [FlatBufferItem(4)] public int PokeReviveTime { get; set; }
    [FlatBufferItem(5)] public int AiIntervalTime { get; set; }
    [FlatBufferItem(6)] public int AiIntervalRand { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidEnemyInfo
{
    [FlatBufferItem(00)] public RaidRomType RomVer { get; set; }
    [FlatBufferItem(01)] public int No { get; set; }
    [FlatBufferItem(02)] public sbyte DeliveryGroupID { get; set; }
    [FlatBufferItem(03)] public int Difficulty { get; set; }
    [FlatBufferItem(04)] public sbyte Rate { get; set; }
    [FlatBufferItem(05)] public ulong DropTableFix { get; set; }
    [FlatBufferItem(06)] public ulong DropTableRandom { get; set; }
    [FlatBufferItem(07)] public sbyte CaptureRate { get; set; }
    [FlatBufferItem(08)] public sbyte CaptureLv { get; set; }
    [FlatBufferItem(09)] public PokeDataBattle BossPokePara { get; set; }
    [FlatBufferItem(10)] public RaidBossSizeData BossPokeSize { get; set; }
    [FlatBufferItem(11)] public RaidBossData BossDesc { get; set; }
    [FlatBufferItem(12)] public RaidTimeData RaidTimeData { get; set; }

    public void SerializePKHeX(BinaryWriter bw, byte star, sbyte rate, RaidSerializationFormat format)
    {
        BossPokePara.SerializePKHeX(bw, CaptureLv, format);
        BossPokeSize.SerializePKHeX();
        bw.Write(DeliveryGroupID);

        // Append RNG details.
        bw.Write(star);
        bw.Write(rate);
    }

    public void SerializeType3(BinaryWriter bw)
    {
        // Fixed Nature, fixed IVs, fixed Scale
        var b = BossPokePara;
        if (b.TalentType > TalentType.VALUE)
            throw new InvalidDataException("Invalid talent type for Type 3 serialization.");

        bw.Write(b.Seikaku == SeikakuType.DEFAULT ? (byte)25 : (byte)(b.Seikaku - 1));
        bw.Write((byte)b.TalentValue.HP);
        bw.Write((byte)b.TalentValue.ATK);
        bw.Write((byte)b.TalentValue.DEF);
        bw.Write((byte)b.TalentValue.SPE);
        bw.Write((byte)b.TalentValue.SPA);
        bw.Write((byte)b.TalentValue.SPD);
        bw.Write((byte)(b.TalentType == 0 ? 0 : 1));
        bw.Write((byte)b.ScaleValue);
        bw.Write((byte)0);
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidBossSizeData
{
    [FlatBufferItem(0)] public SizeType HeightType { get; set; }
    [FlatBufferItem(1)] public short HeightValue { get; set; }
    [FlatBufferItem(2)] public SizeType WeightType { get; set; }
    [FlatBufferItem(3)] public short WeightValue { get; set; }
    [FlatBufferItem(4)] public SizeType ScaleType { get; set; }
    [FlatBufferItem(5)] public short ScaleValue { get; set; }

    public void SerializePKHeX()
    {
        // If any property is not zero, throw an exception.
        // if (HeightType != 0 || HeightValue != 0 || WeightType != 0 || WeightValue != 0 || ScaleType != 0 || ScaleValue != 0)
        //     throw new ArgumentException("Expected default sizes.");
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidBossExtraData
{
    [FlatBufferItem(0)] public RaidBossExtraTimingType Timming { get; set; }
    [FlatBufferItem(1)] public RaidBossExtraActType Action { get; set; }
    [FlatBufferItem(2)] public short Value { get; set; }
    [FlatBufferItem(3)] public WazaID Wazano { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidBossData
{
    [FlatBufferItem(00)] public short HpCoef { get; set; }
    [FlatBufferItem(01)] public sbyte PowerChargeTrigerHp { get; set; }
    [FlatBufferItem(02)] public sbyte PowerChargeTrigerTime { get; set; }
    [FlatBufferItem(03)] public short PowerChargeLimitTime { get; set; }
    [FlatBufferItem(04)] public sbyte PowerChargeCancelDamage { get; set; }
    [FlatBufferItem(05)] public short PowerChargePenaltyTime { get; set; }
    [FlatBufferItem(06)] public WazaID PowerChargePenaltyAction { get; set; }
    [FlatBufferItem(07)] public sbyte PowerChargeDamageRate { get; set; }
    [FlatBufferItem(08)] public sbyte PowerChargeGemDamageRate { get; set; }
    [FlatBufferItem(09)] public sbyte PowerChargeChangeGemDamageRate { get; set; }
    [FlatBufferItem(10)] public RaidBossExtraData ExtraAction1 { get; set; }
    [FlatBufferItem(11)] public RaidBossExtraData ExtraAction2 { get; set; }
    [FlatBufferItem(12)] public RaidBossExtraData ExtraAction3 { get; set; }
    [FlatBufferItem(13)] public RaidBossExtraData ExtraAction4 { get; set; }
    [FlatBufferItem(14)] public RaidBossExtraData ExtraAction5 { get; set; }
    [FlatBufferItem(15)] public RaidBossExtraData ExtraAction6 { get; set; }
    [FlatBufferItem(16)] public sbyte DoubleActionTriggerHp { get; set; }
    [FlatBufferItem(17)] public sbyte DoubleActionTriggerTime { get; set; }
    [FlatBufferItem(18)] public sbyte DoubleActionRate { get; set; }
}
