using System;

namespace pkNX.Structures
{
    public class Item8
    {
        private const int SIZE = 0x30;
        public readonly int ItemID;
        public readonly byte[] Data;

        public Item8(int id, byte[] data)
        {
            ItemID = id;
            Data = data;
        }

        public uint Price
        {
            get => BitConverter.ToUInt32(Data, 0x00);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x00);
        }

        public uint PriceWatts
        {
            get => BitConverter.ToUInt32(Data, 0x04);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x04);
        }

        public uint PriceAlternate // BP, Dynite Ore
        {
            get => BitConverter.ToUInt32(Data, 0x08);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x08);
        }

        public PouchID Pouch
        {
            get => (PouchID)(Data[0x11] & 0xF);
            set => Data[0x11] = (byte)((Data[0x11] & 0xF0) | ((byte)value & 0xF));
        }

        public byte EffectField
        {
            get => Data[0x13];
            set => Data[0x13] = value;
        }

        public int ItemSprite
        {
            get => BitConverter.ToInt16(Data, 0x1A);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1A);
        }

        public GroupIndexType GroupType
        {
            get => (GroupIndexType)Data[0x1C];
            set => Data[0x1C] = (byte)value;
        }

        public bool CanUseOnPokemon
        {
            get => Data[0x15] == 1;
            set => Data[0x15] = (byte)(value ? 1 : 0);
        }

        public byte GroupIndex
        {
            get => Data[0x1D];
            set => Data[0x1D] = value;
        }

        public byte Boost0
        {
            get => Data[0x1F];
            set => Data[0x1F] = value;
        }

        public byte Boost1
        {
            get => Data[0x20];
            set => Data[0x20] = value;
        }

        public byte Boost2
        {
            get => Data[0x21];
            set => Data[0x21] = value;
        }

        public byte Boost3
        {
            get => Data[0x22];
            set => Data[0x22] = value;
        }

        public static Item8[] GetArray(byte[] bin)
        {
            int numEntries = BitConverter.ToUInt16(bin, 0);
            int maxEntryIndex = BitConverter.ToUInt16(bin, 4);
            int entriesStart = (int)BitConverter.ToUInt32(bin, 0x40);
            var result = new Item8[numEntries];
            for (var i = 0; i < result.Length; i++)
            {
                var entryIndex = BitConverter.ToUInt16(bin, 0x44 + (2 * i));
                if (entryIndex >= maxEntryIndex) { throw new ArgumentException();  }
                result[i] = new Item8(i, bin.Slice(entriesStart + (entryIndex * SIZE), SIZE));
            }

            return result;
        }

        public static byte[] SetArray(Item8[] array, byte[] bin)
        {
            bin = (byte[])bin.Clone();
            if (array.Length != BitConverter.ToInt16(bin, 0))
                throw new ArgumentException("Incompatible sizes");

            int maxEntryIndex = BitConverter.ToUInt16(bin, 4);
            int entriesStart = (int)BitConverter.ToUInt32(bin, 0x40);
            for (int i = 0; i < array.Length; i++)
            {
                var entryIndex = BitConverter.ToUInt16(bin, 0x44 + (2 * i));
                if (entryIndex >= maxEntryIndex) { throw new ArgumentException(); }

                var data = array[i].Data;
                data.CopyTo(bin, entriesStart + (entryIndex * SIZE));
            }

            return bin;
        }

        public enum PouchID : byte
        {
            Medicine,
            Balls,
            Battle,
            Berries,
            Items,
            TMs,
            Treasures,
            Ingredients,
            Key,
        }

        public enum GroupIndexType : byte
        {
            None = 0,
            Ball = 1,
            _2 = 2, // unused?
            Berries = 3,
            TM = 4,
            Gems = 5, // only for Normal Gem, rest are unused items
        }
    }
}