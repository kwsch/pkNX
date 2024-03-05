using System;

namespace pkNX.Structures;

/// <summary>
/// Array reusable logic
/// </summary>
public static class ArrayUtil
{
    public static byte[] Slice(this byte[] src, int offset, int length)
    {
        byte[] data = new byte[length];
        Buffer.BlockCopy(src, offset, data, 0, data.Length);
        return data;
    }

    public static T[] Slice<T>(this T[] src, int offset, int length)
    {
        var data = new T[length];
        Array.Copy(src, offset, data, 0, data.Length);
        return data;
    }

    public static T[][] Split<T>(this T[] data, int size)
    {
        var result = new T[data.Length / size][];
        for (int i = 0; i < data.Length; i += size)
            result[i / size] = data.Slice(i, size);
        return result;
    }
}

public static class FlagUtil
{
    public static bool GetFlag(ReadOnlySpan<byte> arr, int offset, int bitIndex)
    {
        bitIndex &= 7; // ensure bit access is 0-7
        return (arr[offset] >> bitIndex & 1) != 0;
    }

    public static void SetFlag(Span<byte> arr, int offset, int bitIndex, bool value)
    {
        bitIndex &= 7; // ensure bit access is 0-7
        arr[offset] &= (byte)~(1 << bitIndex);
        arr[offset] |= (byte)((value ? 1 : 0) << bitIndex);
    }

    public static void GetFlagArray(ReadOnlySpan<byte> data, Span<bool> result)
    {
        for (int i = 0; i < result.Length; i++)
            result[i] = GetFlag(data, i >> 3, i);
    }

    public static void SetFlagArray(Span<byte> data, ReadOnlySpan<bool> result)
    {
        for (int i = 0; i < result.Length; i++)
            SetFlag(data, i >> 3, i, result[i]);
    }
}
