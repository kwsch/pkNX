using System;
using static System.Buffers.Binary.BinaryPrimitives;
using static pkNX.Structures.IPersonalInfoBinExt;

namespace pkNX.Structures;

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the OR &amp; AS games.
/// </summary>
public sealed class PersonalInfo6ORAS : IPersonalInfoORAS
{
    public const int SIZE = 0x50;
    private readonly byte[] Data;

    public bool[] TMHM { get; set; }
    public bool[] TypeTutors { get; set; }
    public bool[][] SpecialTutors { get; set; }

    public PersonalInfo6ORAS(byte[] data)
    {
        Data = data;
        // Unpack TMHM & Tutors
        TMHM = GetBits(Data.AsSpan(0x28, 0x10));
        TypeTutors = GetBits(Data.AsSpan(0x38, 0x4));
        // 0x3C-0x40 unknown
        SpecialTutors = new[]
        {
            GetBits(Data.AsSpan(0x40, 0x04)),
            GetBits(Data.AsSpan(0x44, 0x04)),
            GetBits(Data.AsSpan(0x48, 0x04)),
            GetBits(Data.AsSpan(0x4C, 0x04)),
        };
    }

    public byte[] Write()
    {
        SetBits(TMHM, Data.AsSpan(0x28));
        SetBits(TypeTutors, Data.AsSpan(0x38));
        SetBits(SpecialTutors[0], Data.AsSpan(0x40));
        SetBits(SpecialTutors[1], Data.AsSpan(0x44));
        SetBits(SpecialTutors[2], Data.AsSpan(0x48));
        SetBits(SpecialTutors[3], Data.AsSpan(0x4C));
        return Data;
    }

    public int HP { get => Data[0x00]; set => Data[0x00] = (byte)value; }
    public int ATK { get => Data[0x01]; set => Data[0x01] = (byte)value; }
    public int DEF { get => Data[0x02]; set => Data[0x02] = (byte)value; }
    public int SPE { get => Data[0x03]; set => Data[0x03] = (byte)value; }
    public int SPA { get => Data[0x04]; set => Data[0x04] = (byte)value; }
    public int SPD { get => Data[0x05]; set => Data[0x05] = (byte)value; }
    public Types Type1 { get => (Types)Data[0x06]; set => Data[0x06] = (byte)value; }
    public Types Type2 { get => (Types)Data[0x07]; set => Data[0x07] = (byte)value; }
    public int CatchRate { get => Data[0x08]; set => Data[0x08] = (byte)value; }
    public int EvoStage { get => Data[0x09]; set => Data[0x09] = (byte)value; }
    private int EVYield { get => ReadUInt16LittleEndian(Data.AsSpan(0x0A)); set => WriteUInt16LittleEndian(Data.AsSpan(0x0A), (ushort)value); }
    public int EV_HP { get => (EVYield >> 0) & 0x3; set => EVYield = (EVYield & ~(0x3 << 0)) | ((value & 0x3) << 0); }
    public int EV_ATK { get => (EVYield >> 2) & 0x3; set => EVYield = (EVYield & ~(0x3 << 2)) | ((value & 0x3) << 2); }
    public int EV_DEF { get => (EVYield >> 4) & 0x3; set => EVYield = (EVYield & ~(0x3 << 4)) | ((value & 0x3) << 4); }
    public int EV_SPE { get => (EVYield >> 6) & 0x3; set => EVYield = (EVYield & ~(0x3 << 6)) | ((value & 0x3) << 6); }
    public int EV_SPA { get => (EVYield >> 8) & 0x3; set => EVYield = (EVYield & ~(0x3 << 8)) | ((value & 0x3) << 8); }
    public int EV_SPD { get => (EVYield >> 10) & 0x3; set => EVYield = (EVYield & ~(0x3 << 10)) | ((value & 0x3) << 10); }
    public bool Telekenesis { get => ((EVYield >> 12) & 1) == 1; set => EVYield = (EVYield & ~(0x1 << 12)) | ((value ? 1 : 0) << 12); }
    public int Item1 { get => ReadInt16LittleEndian(Data.AsSpan(0x0C)); set => WriteInt16LittleEndian(Data.AsSpan(0x0C), (short)value); }
    public int Item2 { get => ReadInt16LittleEndian(Data.AsSpan(0x0E)); set => WriteInt16LittleEndian(Data.AsSpan(0x0E), (short)value); }
    public int Item3 { get => ReadInt16LittleEndian(Data.AsSpan(0x10)); set => WriteInt16LittleEndian(Data.AsSpan(0x10), (short)value); }
    public int Gender { get => Data[0x12]; set => Data[0x12] = (byte)value; }
    public int HatchCycles { get => Data[0x13]; set => Data[0x13] = (byte)value; }
    public int BaseFriendship { get => Data[0x14]; set => Data[0x14] = (byte)value; }
    public int EXPGrowth { get => Data[0x15]; set => Data[0x15] = (byte)value; }
    public int EggGroup1 { get => Data[0x16]; set => Data[0x16] = (byte)value; }
    public int EggGroup2 { get => Data[0x17]; set => Data[0x17] = (byte)value; }
    public int Ability1 { get => Data[0x18]; set => Data[0x18] = (byte)value; }
    public int Ability2 { get => Data[0x19]; set => Data[0x19] = (byte)value; }
    public int AbilityH { get => Data[0x1A]; set => Data[0x1A] = (byte)value; }

    public int EscapeRate { get => Data[0x1B]; set => Data[0x1B] = (byte)value; }
    public int FormStatsIndex { get => ReadUInt16LittleEndian(Data.AsSpan(0x1C)); set => WriteUInt16LittleEndian(Data.AsSpan(0x1C), (ushort)value); }
    public int FormSprite { get => ReadUInt16LittleEndian(Data.AsSpan(0x1E)); set => WriteUInt16LittleEndian(Data.AsSpan(0x1E), (ushort)value); }
    public byte FormCount { get => Data[0x20]; set => Data[0x20] = value; }
    public int Color { get => Data[0x21] & 0x3F; set => Data[0x21] = (byte)((Data[0x21] & 0xC0) | (value & 0x3F)); }
    public bool SpriteFlip { get => ((Data[0x21] >> 6) & 1) == 1; set => Data[0x21] = (byte)((Data[0x21] & ~0x40) | (value ? 0x40 : 0)); }
    public bool SpriteForm { get => ((Data[0x21] >> 7) & 1) == 1; set => Data[0x21] = (byte)((Data[0x21] & ~0x80) | (value ? 0x80 : 0)); }

    public int BaseEXP { get => ReadUInt16LittleEndian(Data.AsSpan(0x22)); set => WriteUInt16LittleEndian(Data.AsSpan(0x22), (ushort)value); }
    public int Height { get => ReadUInt16LittleEndian(Data.AsSpan(0x24)); set => WriteUInt16LittleEndian(Data.AsSpan(0x24), (ushort)value); }
    public int Weight { get => ReadUInt16LittleEndian(Data.AsSpan(0x26)); set => WriteUInt16LittleEndian(Data.AsSpan(0x26), (ushort)value); }

    public int AbilityCount => 3;
    public int GetIndexOfAbility(int abilityID) => abilityID == Ability1 ? 0 : abilityID == Ability2 ? 1 : abilityID == AbilityH ? 2 : -1;
    public int GetAbilityAtIndex(int abilityIndex) => abilityIndex switch
    {
        0 => Ability1,
        1 => Ability2,
        2 => AbilityH,
        _ => throw new ArgumentOutOfRangeException(nameof(abilityIndex), abilityIndex, null),
    };
}
