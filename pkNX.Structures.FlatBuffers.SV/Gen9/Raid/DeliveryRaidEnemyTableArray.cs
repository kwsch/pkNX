using System.ComponentModel;
using System.IO;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class RaidEnemyInfo
{
    public void SerializePKHeX(BinaryWriter bw, byte star, sbyte rate, RaidSerializationFormat format)
    {
        BossPokePara.SerializePKHeX(bw, CaptureLv, format);
        BossPokeSize.SerializePKHeX();
        bw.Write(DeliveryGroupID);

        // Append RNG details.
        bw.Write(star);
        bw.Write(rate);
    }

    public void SerializeType2(BinaryWriter bw)
    {
        var b = BossPokePara;
        bw.Write((byte)b.ScaleType);
        bw.Write((byte)b.ScaleValue);
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
        bw.Write((byte)b.ScaleType);
        bw.Write((byte)b.ScaleValue);
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class RaidBossSizeData
{
    public void SerializePKHeX()
    {
        // If any property is not zero, throw an exception.
        // if (HeightType != 0 || HeightValue != 0 || WeightType != 0 || WeightValue != 0 || ScaleType != 0 || ScaleValue != 0)
        //     throw new ArgumentException("Expected default sizes.");
    }
}
