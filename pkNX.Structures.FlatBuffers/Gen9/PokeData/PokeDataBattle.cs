using System;
using System.ComponentModel;
using System.IO;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDataBattle
{
    [FlatBufferItem(00)] public DevID DevId { get; set; }
    [FlatBufferItem(01)] public short FormId { get; set; }
    [FlatBufferItem(02)] public SexType Sex { get; set; }
    [FlatBufferItem(03)] public ItemID Item { get; set; }
    [FlatBufferItem(04)] public int Level { get; set; }
    [FlatBufferItem(05)] public BallType BallId { get; set; }
    [FlatBufferItem(06)] public WazaType WazaType { get; set; }
    [FlatBufferItem(07)] public WazaSet Waza1 { get; set; }
    [FlatBufferItem(08)] public WazaSet Waza2 { get; set; }
    [FlatBufferItem(09)] public WazaSet Waza3 { get; set; }
    [FlatBufferItem(10)] public WazaSet Waza4 { get; set; }
    [FlatBufferItem(11)] public GemType GemType { get; set; }
    [FlatBufferItem(12)] public SeikakuType Seikaku { get; set; }
    [FlatBufferItem(13)] public TokuseiType Tokusei { get; set; }
    [FlatBufferItem(14)] public TalentType TalentType { get; set; }
    [FlatBufferItem(15)] public ParamSet TalentValue { get; set; }
    [FlatBufferItem(16)] public sbyte TalentVnum { get; set; }
    [FlatBufferItem(17)] public ParamSet EffortValue { get; set; }
    [FlatBufferItem(18)] public RareType RareType { get; set; }
    [FlatBufferItem(19)] public SizeType ScaleType { get; set; }
    [FlatBufferItem(20)] public short ScaleValue { get; set; }

    public void SerializePKHeX(BinaryWriter bw, sbyte captureLv, RaidSerializationFormat format)
    {
        if (format != RaidSerializationFormat.Type3)
            AssertRegularFormat();

        // If any PointUp for a move is nonzero, throw an exception.
        if (Waza1.PointUp != 0 || Waza2.PointUp != 0 || Waza3.PointUp != 0 || Waza4.PointUp != 0)
            throw new ArgumentOutOfRangeException(nameof(WazaSet.PointUp), $"No {nameof(WazaSet.PointUp)} allowed!");

        // flag BallId if not none
        if (BallId != BallType.NONE)
            throw new ArgumentOutOfRangeException(nameof(BallId), BallId, $"No {nameof(BallId)} allowed!");

        bw.Write((ushort)DevId);
        bw.Write((byte)FormId);
        bw.Write((byte)Sex);

        bw.Write((byte)Tokusei);
        bw.Write((byte)TalentVnum);
        bw.Write((byte)RareType);
        bw.Write((byte)captureLv);

        // Write moves
        bw.Write((ushort)Waza1.WazaId);
        bw.Write((ushort)Waza2.WazaId);
        bw.Write((ushort)Waza3.WazaId);
        bw.Write((ushort)Waza4.WazaId);

        // ROM raids with 5 stars have a few entries that are defined as DEFAULT
        // If the type is not {specified}, the game will assume it is RANDOM.
        // Thus, DEFAULT behaves like RANDOM.
        // Let's clean up this mistake and make it explicit so we don't have to program this workaround in other tools.
        var gem = GemType is GemType.DEFAULT ? GemType.RANDOM : GemType;
        bw.Write((byte)gem);
    }

    private void AssertRegularFormat()
    {
        if (TalentType != TalentType.V_NUM)
            throw new ArgumentOutOfRangeException(nameof(TalentType), TalentType, "No min flawless IVs?");
        if (TalentVnum == 0 && DevId != DevID.DEV_PATIRISU &&
            Level != 35) // nice mistake gamefreak -- 3star Pachirisu is 0 IVs.
            throw new ArgumentOutOfRangeException(nameof(TalentVnum), TalentVnum, "No min flawless IVs?");

        if (Seikaku != SeikakuType.DEFAULT)
            throw new ArgumentOutOfRangeException(nameof(Seikaku), Seikaku, $"No {nameof(Seikaku)} allowed!");
    }
}

public enum RaidSerializationFormat
{
    /// <summary>
    /// Base ROM Raids
    /// </summary>
    BaseROM,

    /// <summary>
    /// Regular Distribution Raids
    /// </summary>
    Type2,

    /// <summary>
    /// 7 Star Distribution Raids
    /// </summary>
    Type3,
}
