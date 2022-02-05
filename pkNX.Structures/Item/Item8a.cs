using System;
using System.ComponentModel;

namespace pkNX.Structures
{
    public class Item8a
    {
        private const int SIZE = 0x3C;
        public readonly int ItemID;
        public readonly byte[] Data;

        private const string Battle = "Battle";
        private const string Field = "Field";
        private const string Mart = "Mart";
        private const string Heal = "Heal";

        public Item8a(int id, byte[] data) => (ItemID, Data) = (id, data);

        public uint Price { get => BitConverter.ToUInt32(Data, 0x00); set => BitConverter.GetBytes(value).CopyTo(Data, 0x00); }
        public uint PriceWatts { get => BitConverter.ToUInt32(Data, 0x04); set => BitConverter.GetBytes(value).CopyTo(Data, 0x04); }
        public uint MeritPrice { get => BitConverter.ToUInt32(Data, 0x08); set => BitConverter.GetBytes(value).CopyTo(Data, 0x08); }
        public byte BattleEffect { get => Data[0x0C]; set => Data[0x0C] = value; }
        public byte BattleArg { get => Data[0x0D]; set => Data[0x0D] = value; }
        public byte BerryValue { get => Data[0x0F]; set => Data[0x0F] = value; }
        public byte Unk_0x10 { get => Data[0x10]; set => Data[0x10] = value; }
        public PouchID8a Pouch
        {
            get => (PouchID8a)(Data[0x11] & 0xF);
            set => Data[0x11] = (byte)((Data[0x11] & 0xF0) | ((byte)value & 0xF));
        }

        public ItemFlags8a Unknown
        {
            get => (ItemFlags8a)(Data[0x11] >> 4);
            set => Data[0x11] = (byte)((Data[0x11] & 0x0F) | (((byte)value & 0xF) << 4));
        }

        public byte FlingPower  { get => Data[0x12]; set => Data[0x12] = value; }
        public FieldItemType Unk_0x13    { get => (FieldItemType)Data[0x13]; set => Data[0x13] = (byte)value; }
        public BattlePouch8a BattlePouch { get => (BattlePouch8a)Data[0x14]; set => Data[0x14] = (byte)value; }
        public bool CanUse { get => Data[0x15] != 0; set => Data[0x15] = (byte)(value ? 1 : 0); }
        public ItemType8a ItemType { get => (ItemType8a)Data[0x16]; set => Data[0x16] = (byte)value; }
        public byte Unk_0x17 { get => Data[0x17]; set => Data[0x17] = value; }
        public byte SortIndex { get => Data[0x18]; set => Data[0x18] = value; }
        // 0x19 align
        public short ItemSprite { get => BitConverter.ToInt16(Data, 0x1A); set => BitConverter.GetBytes(value).CopyTo(Data, 0x1A); }
        public ushort MaxQuantity { get => BitConverter.ToUInt16(Data, 0x1C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x1C); }
        public ushort Percentage { get => BitConverter.ToUInt16(Data, 0x1E); set => BitConverter.GetBytes(value).CopyTo(Data, 0x1E); }
        public ItemClass8a ItemGroup { get => (ItemClass8a)Data[0x20]; set => Data[0x20] = (byte)value; }
        public byte Variant   { get => Data[0x21]; set => Data[0x21] = value; }
        // 22 unused
        // 23 unused
        public BattleStatusFlags CureInflict { get => (BattleStatusFlags)Data[0x24]; set => Data[0x24] = (byte)value; }
        public byte BallID { get => Data[0x24]; set => Data[0x24] = value; } // same offset as above
        public byte Boost0 { get => Data[0x25]; set => Data[0x25] = value; }
        public byte Boost1 { get => Data[0x26]; set => Data[0x26] = value; }
        public byte Boost2 { get => Data[0x27]; set => Data[0x27] = value; }
        public byte Boost3 { get => Data[0x28]; set => Data[0x28] = value; }
        public ItemFlags1 FunctionFlags0 { get => (ItemFlags1)Data[0x29]; set => Data[0x29] = (byte)value; }
        public ItemFlags2 FunctionFlags1 { get => (ItemFlags2)Data[0x2A]; set => Data[0x2A] = (byte)value; }
        public sbyte EVHP  { get => (sbyte)Data[0x2B]; set => Data[0x2B] = (byte)value; }
        public sbyte EVATK { get => (sbyte)Data[0x2C]; set => Data[0x2C] = (byte)value; }
        public sbyte EVDEF { get => (sbyte)Data[0x2D]; set => Data[0x2D] = (byte)value; }
        public sbyte EVSPE { get => (sbyte)Data[0x2E]; set => Data[0x2E] = (byte)value; }
        public sbyte EVSPA { get => (sbyte)Data[0x2F]; set => Data[0x2F] = (byte)value; }
        public sbyte EVSPD { get => (sbyte)Data[0x30]; set => Data[0x30] = (byte)value; }
        public Heal HealAmount { get => (Heal)Data[0x31]; set => Data[0x31] = (byte)value; }
        public byte PPGain { get => Data[0x32]; set => Data[0x32] = value; }
        public sbyte FriendshipGain1 { get => (sbyte)Data[0x33]; set => Data[0x33] = (byte)value; }
        public sbyte FriendshipGain2 { get => (sbyte)Data[0x34]; set => Data[0x34] = (byte)value; }
        public sbyte FriendshipGain3 { get => (sbyte)Data[0x35]; set => Data[0x35] = (byte)value; }
        public byte Flags_36 { get => Data[0x36]; set => Data[0x36] = value; }
        public byte EffectTurns1 { get => Data[0x37]; set => Data[0x37] = value; } // duration?
        public byte EffectTurns2 { get => Data[0x38]; set => Data[0x38] = value; } // duration?
        public byte EffectTurns3 { get => Data[0x39]; set => Data[0x39] = value; } // duration?
        public sbyte StatChangeAmount { get => (sbyte)Data[0x3A]; set => Data[0x3A] = (byte)value; }
        // 0x1B unused

        public static Item8a[] GetArray(byte[] bin)
        {
            int numEntries = BitConverter.ToUInt16(bin, 0);
            int maxEntryIndex = BitConverter.ToUInt16(bin, 4);
            int entriesStart = (int)BitConverter.ToUInt32(bin, 0x48);
            var result = new Item8a[numEntries];
            for (var i = 0; i < result.Length; i++)
            {
                var entryIndex = BitConverter.ToUInt16(bin, 0x4C + (2 * i));
                if (entryIndex >= maxEntryIndex) { throw new ArgumentException(); }
                result[i] = new Item8a(i, bin.Slice(entriesStart + (entryIndex * SIZE), SIZE));
            }

            return result;
        }

        public static byte[] SetArray(Item8a[] array, byte[] bin)
        {
            bin = (byte[])bin.Clone();
            if (array.Length != BitConverter.ToInt16(bin, 0))
                throw new ArgumentException("Incompatible sizes");

            int maxEntryIndex = BitConverter.ToUInt16(bin, 4);
            int entriesStart = (int)BitConverter.ToUInt32(bin, 0x48);
            for (int i = 0; i < array.Length; i++)
            {
                var entryIndex = BitConverter.ToUInt16(bin, 0x4C + (2 * i));
                if (entryIndex >= maxEntryIndex) { throw new ArgumentException(); }

                var data = array[i].Data;
                data.CopyTo(bin, entriesStart + (entryIndex * SIZE));
            }

            return bin;
        }

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
    }

    public enum ItemClass8a
    {
        None,
        Ball = 1,
        Berry = 3,
        TM = 4,
        Gem = 6,
        Charm = 14,
        Plate = 15,
    }

    public enum ItemType8a : byte
    {
        Pocket = 0,
        Medicinal = 1,
        Equip = 2,
        Treasure = 3,
        BattleItem = 4,
        Ball = 5,
        Mail = 6,
        TM = 7,
        Berry = 8,
        Key = 9,
        LegendItemUseBattle = 10,
        LegendItemToss = 11,
        Rare = 12,
        Dummy = 255,
    }

    public enum BattlePouch8a : byte
    {
        None = 0,
        Balls = 1,
        Use = 2,
    }

    public enum PouchID8a : byte
    {
        Regular,
        Key,
        Recipe,
    }

    [Flags]
    public enum ItemFlags8a : byte
    {
        None = 0,
        Flag1 = 1,
        Flag2 = 2,
        Flag4 = 4,
        Flag8 = 8,
    }

    public enum FieldItemType : byte
    {
        Inert,
        Medicine = 1,
        TM = 2,
        Spray = 5,
        Evolution = 6,
        EscapeRope = 7,
        Berry = 12,
        FormChange = 15,
    }
}
