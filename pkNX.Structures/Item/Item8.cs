using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class Item8(int id, Memory<byte> Raw)
{
    public Span<byte> Data => Raw.Span;

    private const int SIZE = 0x30;
    public readonly int ItemID = id;

    public uint Price { get => ReadUInt32LittleEndian(Data); set => WriteUInt32LittleEndian(Data, value); }
    public uint PriceWatts { get => ReadUInt16LittleEndian(Data[0x04..]); set => WriteUInt32LittleEndian(Data[0x04..], value); }
    public uint PriceAlternate // BP, Dynite Ore
    {
        get => ReadUInt16LittleEndian(Data[(0x08)..]);
        set => WriteUInt32LittleEndian(Data[(0x08)..], value);
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
        get => BitConverter.ToUInt16(Data.Slice(0x1A, 2));
        set => BitConverter.GetBytes((ushort)value).CopyTo(Data.Slice(0x1A, 2));
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

    public static Item8[] GetArray(ReadOnlySpan<byte> bin)
    {
        int numEntries = ReadUInt16LittleEndian(bin);
        int maxEntryIndex = ReadUInt16LittleEndian(bin[4..]);
        int entriesStart = ReadInt32LittleEndian(bin[0x40..]);
        var result = new Item8[numEntries];
        for (var i = 0; i < result.Length; i++)
        {
            var entryIndex = ReadUInt16LittleEndian(bin[(0x44 + (2 * i))..]);
            if (entryIndex >= maxEntryIndex)
                throw new IndexOutOfRangeException();

            var ofs = entriesStart + (entryIndex * SIZE);
            result[i] = new Item8(i, bin.Slice(ofs, SIZE).ToArray());
        }

        return result;
    }

    public static byte[] SetArray(ReadOnlySpan<Item8> array, ReadOnlySpan<byte> bin)
    {
        int numEntries = ReadUInt16LittleEndian(bin);
        if (array.Length != numEntries)
            throw new ArgumentException("Incompatible sizes");

        var result = bin.ToArray();
        int maxEntryIndex = ReadUInt16LittleEndian(bin[4..]);
        int entriesStart = ReadInt32LittleEndian(bin[0x40..]);
        for (int i = 0; i < array.Length; i++)
        {
            var entryIndex = ReadUInt16LittleEndian(bin[(0x44 + (2 * i))..]);
            if (entryIndex >= maxEntryIndex)
                throw new IndexOutOfRangeException();

            var data = array[i].Data;
            var ofs = entriesStart + (entryIndex * SIZE);
            var span = result.AsSpan(ofs, SIZE);
            data.CopyTo(span);
        }

        return result;
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
