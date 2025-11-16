using System;
using System.ComponentModel;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

/// <summary>
/// Span-backed lazy access Item definition. Properties read/write directly to underlying data.
/// </summary>
public class Item
{
    private readonly Memory<byte> Raw;
    private Span<byte> Data => Raw.Span;

    public const int SIZE = 0x24; // 36 bytes

    // Category names
    private const string Battle = "Battle";
    private const string Field = "Field";
    private const string Mart = "Mart";
    private const string Heal = "Heal";

    public Item() : this(new byte[SIZE]) { }
    private Item(Memory<byte> data)
    {
        if (data.Length < SIZE)
            throw new ArgumentException($"Item data must be >= {SIZE} bytes", nameof(data));
        Raw = data[..SIZE];
    }

    public static Item FromBytes(Memory<byte> data) => new(data);
    public byte[] Write() => Data.ToArray();

    #region Primitive Field Accessors (inline offsets)
    private ushort Price { get => ReadUInt16LittleEndian(Data[0x00..]); set => WriteUInt16LittleEndian(Data[0x00..], value); }
    private ushort Packed { get => ReadUInt16LittleEndian(Data[0x08..]); set => WriteUInt16LittleEndian(Data[0x08..], value); }
    private byte Boost0 { get => Data[0x11]; set => Data[0x11] = value; }
    private byte Boost1 { get => Data[0x12]; set => Data[0x12] = value; }
    private byte Boost2 { get => Data[0x13]; set => Data[0x13] = value; }
    private byte Boost3 { get => Data[0x14]; set => Data[0x14] = value; }
    private byte Consumable { get => Data[0x0E]; set => Data[0x0E] = value; }
    #endregion

    #region Structure
    [Category(Battle)] public byte HeldEffect { get => Data[0x02]; set => Data[0x02] = value; }
    public byte HeldArgument { get => Data[0x03]; set => Data[0x03] = value; }
    public byte NaturalGiftEffect { get => Data[0x04]; set => Data[0x04] = value; }
    public byte FlingEffect { get => Data[0x05]; set => Data[0x05] = value; }
    public byte FlingPower { get => Data[0x06]; set => Data[0x06] = value; }
    public byte NaturalGiftPower { get => Data[0x07]; set => Data[0x07] = value; }

    [Category(Field), Description("Routine # to call when used; 0=unusable.")]
    public byte EffectField { get => Data[0x0A]; set => Data[0x0A] = value; }

    [Category(Battle), Description("Routine # to call when used; 0=unusable.")]
    public byte EffectBattle { get => Data[0x0B]; set => Data[0x0B] = value; }

    public byte Unk_0xC { get => Data[0x0C]; set => Data[0x0C] = value; }
    public byte Unk_0xD { get => Data[0x0D]; set => Data[0x0D] = value; }
    public byte SortIndex { get => Data[0x0F]; set => Data[0x0F] = value; }
    public BattleStatusFlags CureInflict { get => (BattleStatusFlags)Data[0x10]; set => Data[0x10] = (byte)value; }
    public ItemFlags1 FunctionFlags0 { get => (ItemFlags1)Data[0x15]; set => Data[0x15] = (byte)value; }
    public ItemFlags2 FunctionFlags1 { get => (ItemFlags2)Data[0x16]; set => Data[0x16] = (byte)value; }

    [Category(Field), Description("Adds EVs to the HP stat.")]
    public sbyte EVHP { get => (sbyte)Data[0x17]; set => Data[0x17] = (byte)value; }
    [Category(Field), Description("Adds EVs to the Attack stat.")]
    public sbyte EVATK { get => (sbyte)Data[0x18]; set => Data[0x18] = (byte)value; }
    [Category(Field), Description("Adds EVs to the Defense stat.")]
    public sbyte EVDEF { get => (sbyte)Data[0x19]; set => Data[0x19] = (byte)value; }
    [Category(Field), Description("Adds EVs to the Speed stat.")]
    public sbyte EVSPE { get => (sbyte)Data[0x1A]; set => Data[0x1A] = (byte)value; }
    [Category(Field), Description("Adds EVs to the Sp. Attack stat.")]
    public sbyte EVSPA { get => (sbyte)Data[0x1B]; set => Data[0x1B] = (byte)value; }
    [Category(Field), Description("Adds EVs to the Sp. Defense stat.")]
    public sbyte EVSPD { get => (sbyte)Data[0x1C]; set => Data[0x1C] = (byte)value; }

    [Category(Heal), Description("Determines the healing percent, or if a flat value is used."), RefreshProperties(RefreshProperties.All)]
    public Heal HealAmount { get => (Heal)Data[0x1D]; set => Data[0x1D] = (byte)value; }

    [Category(Field), Description("PP to be added to the move's current PP if used.")]
    public byte PPGain { get => Data[0x1E]; set => Data[0x1E] = value; }

    public sbyte Friendship1 { get => (sbyte)Data[0x1F]; set => Data[0x1F] = (byte)value; }
    public sbyte Friendship2 { get => (sbyte)Data[0x20]; set => Data[0x20] = (byte)value; }
    public sbyte Friendship3 { get => (sbyte)Data[0x21]; set => Data[0x21] = (byte)value; }
    public byte _0x22 { get => Data[0x22]; set => Data[0x22] = value; }
    public byte _0x23 { get => Data[0x23]; set => Data[0x23] = value; }
    #endregion

    #region Derived Properties
    [Category(Mart), RefreshProperties(RefreshProperties.All)]
    public int BuyPrice { get => Price * 10; set => Price = (ushort)(value / 10); }

    [Category(Mart), ReadOnly(true)]
    public int SellPrice { get => Price * 5; set => Price = (ushort)(value / 5); }

    [Category(Battle)] public int NaturalGiftType { get => Packed & 0x1F; set => Packed = (ushort)((NaturalGiftEffect & ~0x1F) | value); }
    [Category(Battle)] public bool Flag1 { get => ((Packed >> 5) & 1) == 1; set => Packed = (ushort)((Packed & ~(1 << 5)) | ((value ? 1 : 0) << 5)); }
    [Category(Battle)] public bool Flag2 { get => ((Packed >> 6) & 1) == 1; set => Packed = (ushort)((Packed & ~(1 << 6)) | ((value ? 1 : 0) << 6)); }
    [Category(Field)] public int PocketField { get => (Packed >> 7) & 0xF; set => Packed = (ushort)((Packed & 0xF87F) | ((value & 0xF) << 7)); }
    [Category(Battle)] public BattlePocket PocketBattle { get => (BattlePocket)(Packed >> 11); set => Packed = (ushort)((Packed & 0x077F) | (((byte)value & 0x1F) << 11)); }

    [Category(Field)] public bool Revive { get => ((Boost0 >> 0) & 1) == 0; set => Boost0 = (byte)((Boost0 & ~(1 << 0)) | ((value ? 1 : 0) << 0)); }
    [Category(Field)] public bool ReviveAll { get => ((Boost0 >> 1) & 1) == 1; set => Boost0 = (byte)((Boost0 & ~(1 << 1)) | ((value ? 1 : 0) << 1)); }
    [Category(Field)] public bool LevelUp { get => ((Boost0 >> 2) & 1) == 1; set => Boost0 = (byte)((Boost0 & ~(1 << 2)) | ((value ? 1 : 0) << 2)); }
    [Category(Field)] public bool EvoStone { get => ((Boost0 >> 3) & 1) == 1; set => Boost0 = (byte)((Boost0 & ~(1 << 3)) | ((value ? 1 : 0) << 3)); }

    [Category(Battle)] public int BoostATK { get => Boost0 >> 4; set => Boost0 = (byte)((Boost0 & 0xF) | (value << 4)); }
    [Category(Battle)] public int BoostDEF { get => Boost1 & 0xF; set => Boost1 = (byte)((Boost1 & ~0xF) | (value & 0xF)); }
    [Category(Battle)] public int BoostSPA { get => Boost1 >> 4; set => Boost1 = (byte)((Boost1 & 0xF) | (value << 4)); }
    [Category(Battle)] public int BoostSPD { get => Boost2 & 0xF; set => Boost2 = (byte)((Boost2 & ~0xF) | (value & 0xF)); }
    [Category(Battle)] public int BoostSPE { get => Boost2 >> 4; set => Boost2 = (byte)((Boost2 & 0xF) | (value << 4)); }
    [Category(Battle)] public int BoostACC { get => Boost3 & 0xF; set => Boost3 = (byte)((Boost3 & ~0xF) | (value & 0xF)); }
    [Category(Battle)] public int BoostCRIT { get => (Boost3 >> 4) & 3; set => Boost3 = (byte)((Boost3 & ~0x30) | ((value & 3) << 4)); }
    [Category(Battle)] public int BoostPP1 { get => (Boost3 >> 6) & 1; set => Boost3 = (byte)((Boost3 & 0xBF) | ((value & 1) << 6)); }
    [Category(Battle)] public int BoostPPMax { get => (Boost3 >> 7) & 1; set => Boost3 = (byte)((Boost3 & 0x7F) | ((value & 1) << 7)); }

    [Category(Heal), Description("Raw value of the Heal enum."), RefreshProperties(RefreshProperties.All)]
    public int HealValue { get => (int)HealAmount; set => HealAmount = (Heal)value; }

    [Category(Heal), Description("Item is consumed when used."), RefreshProperties(RefreshProperties.All)]
    public bool UseConsume { get => (Consumable & 0xF) != 0; set => Consumable = (byte)((Consumable & 0xF0) | (value ? 1 : 0)); }

    [Category(Heal), Description("Item is not consumed when used."), RefreshProperties(RefreshProperties.All)]
    public bool UseKeep { get => (Consumable & 0xF0) != 0; set => Consumable = (byte)((Consumable & 0x0F) | (value ? 0x10 : 0)); }
    #endregion
}
