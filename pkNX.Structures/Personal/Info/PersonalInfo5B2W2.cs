using System;
using static System.Buffers.Binary.BinaryPrimitives;
using static pkNX.Structures.IPersonalInfoBinExt;

namespace pkNX.Structures;

/// <summary>
/// Personal Info class with values from the Black 2 &amp; White 2 games.
/// </summary>
public sealed class PersonalInfo5B2W2(byte[] data) : IPersonalInfoB2W2
{
    public const int SIZE = 0x4C;

    public bool[] TMHM { get; set; } = GetBits(data.AsSpan(0x28, 0x10));
    public bool[] TypeTutors { get; set; } = GetBits(data.AsSpan(0x38, 0x4));
    public bool[] SpecialTutors { get; set; } = GetBits(data.AsSpan(0x3C, 0x16)); // 0x3C - 0x43

    // Unpack TMHM & Tutors

    public byte[] Write()
    {
        SetBits(TMHM, data.AsSpan(0x28));
        SetBits(TypeTutors, data.AsSpan(0x38));
        SetBits(SpecialTutors, data.AsSpan(0x3C));
        return data;
    }

    public int HP { get => data[0x00]; set => data[0x00] = (byte)value; }
    public int ATK { get => data[0x01]; set => data[0x01] = (byte)value; }
    public int DEF { get => data[0x02]; set => data[0x02] = (byte)value; }
    public int SPE { get => data[0x03]; set => data[0x03] = (byte)value; }
    public int SPA { get => data[0x04]; set => data[0x04] = (byte)value; }
    public int SPD { get => data[0x05]; set => data[0x05] = (byte)value; }
    public Types Type1 { get => (Types)data[0x06]; set => data[0x06] = (byte)value; }
    public Types Type2 { get => (Types)data[0x07]; set => data[0x07] = (byte)value; }
    public int CatchRate { get => data[0x08]; set => data[0x08] = (byte)value; }
    public int EvoStage { get => data[0x09]; set => data[0x09] = (byte)value; }
    private int EVYield { get => ReadUInt16LittleEndian(data.AsSpan(0x0A)); set => WriteUInt16LittleEndian(data.AsSpan(0x0A), (ushort)value); }
    public int EV_HP { get => (EVYield >> 0) & 0x3; set => EVYield = (EVYield & ~(0x3 << 0)) | ((value & 0x3) << 0); }
    public int EV_ATK { get => (EVYield >> 2) & 0x3; set => EVYield = (EVYield & ~(0x3 << 2)) | ((value & 0x3) << 2); }
    public int EV_DEF { get => (EVYield >> 4) & 0x3; set => EVYield = (EVYield & ~(0x3 << 4)) | ((value & 0x3) << 4); }
    public int EV_SPE { get => (EVYield >> 6) & 0x3; set => EVYield = (EVYield & ~(0x3 << 6)) | ((value & 0x3) << 6); }
    public int EV_SPA { get => (EVYield >> 8) & 0x3; set => EVYield = (EVYield & ~(0x3 << 8)) | ((value & 0x3) << 8); }
    public int EV_SPD { get => (EVYield >> 10) & 0x3; set => EVYield = (EVYield & ~(0x3 << 10)) | ((value & 0x3) << 10); }
    public bool Telekenesis { get => ((EVYield >> 12) & 1) == 1; set => EVYield = (EVYield & ~(0x1 << 12)) | ((value ? 1 : 0) << 12); }
    public int Item1 { get => ReadInt16LittleEndian(data.AsSpan(0x0C)); set => WriteInt16LittleEndian(data.AsSpan(0x0C), (short)value); }
    public int Item2 { get => ReadInt16LittleEndian(data.AsSpan(0x0E)); set => WriteInt16LittleEndian(data.AsSpan(0x0E), (short)value); }
    public int Item3 { get => ReadInt16LittleEndian(data.AsSpan(0x10)); set => WriteInt16LittleEndian(data.AsSpan(0x10), (short)value); }
    public int Gender { get => data[0x12]; set => data[0x12] = (byte)value; }
    public byte HatchCycles { get => data[0x13]; set => data[0x13] = value; }
    public int BaseFriendship { get => data[0x14]; set => data[0x14] = (byte)value; }
    public int EXPGrowth { get => data[0x15]; set => data[0x15] = (byte)value; }
    public int EggGroup1 { get => data[0x16]; set => data[0x16] = (byte)value; }
    public int EggGroup2 { get => data[0x17]; set => data[0x17] = (byte)value; }
    public int Ability1 { get => data[0x18]; set => data[0x18] = (byte)value; }
    public int Ability2 { get => data[0x19]; set => data[0x19] = (byte)value; }
    public int AbilityH { get => data[0x1A]; set => data[0x1A] = (byte)value; }

    public int EscapeRate { get => data[0x1B]; set => data[0x1B] = (byte)value; }
    public int FormStatsIndex { get => ReadUInt16LittleEndian(data.AsSpan(0x1C)); set => WriteUInt16LittleEndian(data.AsSpan(0x1C), (ushort)value); }
    public int FormSprite { get => ReadUInt16LittleEndian(data.AsSpan(0x1E)); set => WriteUInt16LittleEndian(data.AsSpan(0x1E), (ushort)value); }
    public byte FormCount { get => data[0x20]; set => data[0x20] = value; }
    public int Color { get => data[0x21] & 0x3F; set => data[0x21] = (byte)((data[0x21] & 0xC0) | (value & 0x3F)); }
    public bool SpriteFlip { get => ((data[0x21] >> 6) & 1) == 1; set => data[0x21] = (byte)((data[0x21] & ~0x40) | (value ? 0x40 : 0)); }
    public bool SpriteForm { get => ((data[0x21] >> 7) & 1) == 1; set => data[0x21] = (byte)((data[0x21] & ~0x80) | (value ? 0x80 : 0)); }

    public int BaseEXP { get => ReadUInt16LittleEndian(data.AsSpan(0x22)); set => WriteUInt16LittleEndian(data.AsSpan(0x22), (ushort)value); }
    public int Height { get => ReadUInt16LittleEndian(data.AsSpan(0x24)); set => WriteUInt16LittleEndian(data.AsSpan(0x24), (ushort)value); }
    public int Weight { get => ReadUInt16LittleEndian(data.AsSpan(0x26)); set => WriteUInt16LittleEndian(data.AsSpan(0x26), (ushort)value); }

    public int AbilityCount => 3;
    public int GetIndexOfAbility(int abilityID) => abilityID == Ability1 ? 0 : abilityID == Ability2 ? 1 : abilityID == AbilityH ? 2 : -1;
    public int GetAbilityAtIndex(int abilityIndex) => abilityIndex switch
    {
        0 => Ability1,
        1 => Ability2,
        2 => AbilityH,
        _ => throw new ArgumentOutOfRangeException(nameof(abilityIndex), abilityIndex, null),
    };

    public bool HasHiddenAbility => AbilityH != Ability1;
}
