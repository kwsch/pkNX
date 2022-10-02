using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures;

public class TrainerPoke8 : TrainerPoke
{
    //sub_7101452DB0
    public const int SIZE = 0x20;
    public override TrainerPoke Clone() => new TrainerPoke8((byte[])Write().Clone());
    public TrainerPoke8(byte[] data = null) => Data = data ?? new byte[SIZE];

    public static TrainerPoke8[] ReadTeam(byte[] data, TrainerData _) => data.GetArray((x, offset) => new TrainerPoke8(offset, x), SIZE);
    public static byte[] WriteTeam(IList<TrainerPoke> team, TrainerData _) => team.SelectMany(z => z.Write()).ToArray();

    public TrainerPoke8(int offset, byte[] data = null)
    {
        Data = new byte[SIZE];
        if (data == null || offset + SIZE > data.Length)
            return;
        Array.Copy(data, offset, Data, 0, SIZE);
    }

    public override int Gender
    {
        get => Data[0] & 0x3;
        set => Data[0] = (byte)((Data[0] & 0xFC) | (value & 0x3));
    }

    public bool Flag
    {
        get => ((Data[0] >> 4) & 1) == 1;
        set => Data[0] = (byte)((Data[0] & 0xF7) | ((value ? 1 : 0) << 4));
    }

    public override int Ability
    {
        get => (Data[0] >> 4) & 0x3;
        set => Data[0] = (byte)((Data[0] & 0xCF) | ((value & 0x3) << 4));
    }

    public override int Nature { get => Data[0x01]; set => Data[0x01] = (byte)value; }

    public override int EV_HP { get => Data[0x02]; set => Data[0x02] = (byte)value; }
    public override int EV_ATK { get => Data[0x03]; set => Data[0x03] = (byte)value; }
    public override int EV_DEF { get => Data[0x04]; set => Data[0x04] = (byte)value; }
    public override int EV_SPA { get => Data[0x05]; set => Data[0x05] = (byte)value; }
    public override int EV_SPD { get => Data[0x06]; set => Data[0x06] = (byte)value; }
    public override int EV_SPE { get => Data[0x07]; set => Data[0x07] = (byte)value; }

    public byte DynamaxLevel { get => Data[0x08]; set => Data[0x08] = Math.Min((byte)10, value); }
    public bool CanGigantamax { get => Data[0x09] != 0; set => Data[0x09] = Convert.ToByte(value); }

    public override int Friendship { get => 0; set { } }
    public override int Rank { get => 0; set { } }
    public override bool CanMegaEvolve { get => false; set { } }

    public override int Level { get => BitConverter.ToUInt16(Data, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A); }
    public override int Species { get => BitConverter.ToUInt16(Data, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C); }
    public override int Form { get => BitConverter.ToUInt16(Data, 0x0E); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0E); }
    public override int HeldItem { get => BitConverter.ToUInt16(Data, 0x10); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x10); }

    public override int Move1 { get => BitConverter.ToUInt16(Data, 0x12); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x12); }
    public override int Move2 { get => BitConverter.ToUInt16(Data, 0x14); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x14); }
    public override int Move3 { get => BitConverter.ToUInt16(Data, 0x16); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x16); }
    public override int Move4 { get => BitConverter.ToUInt16(Data, 0x18); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x18); }

    // 1A-1B unused padding

    public override uint IV32 { get => BitConverter.ToUInt32(Data, 0x1C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x1C); }
    public override int IV_HP { get => (int)(IV32 >> 00) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 00)) | (uint)((value > 31 ? 31 : value) << 00)); }
    public override int IV_ATK { get => (int)(IV32 >> 05) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 05)) | (uint)((value > 31 ? 31 : value) << 05)); }
    public override int IV_DEF { get => (int)(IV32 >> 10) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 10)) | (uint)((value > 31 ? 31 : value) << 10)); }
    public override int IV_SPE { get => (int)(IV32 >> 15) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 15)) | (uint)((value > 31 ? 31 : value) << 15)); }
    public override int IV_SPA { get => (int)(IV32 >> 20) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 20)) | (uint)((value > 31 ? 31 : value) << 20)); }
    public override int IV_SPD { get => (int)(IV32 >> 25) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 25)) | (uint)((value > 31 ? 31 : value) << 25)); }
    public override bool Shiny { get => ((IV32 >> 30) & 1) == 1; set => IV32 = (IV32 & ~0x40000000u) | (value ? 0x40000000u : 0); }

    public override bool CanDynamax
    {
        get => ((IV32 >> 31) & 1) == 1;
        set => IV32 = (IV32 & ~(1 << 31)) | (uint)((value ? 1 : 0) << 31);
    }
}
