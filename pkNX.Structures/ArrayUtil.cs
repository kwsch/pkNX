using System;
using System.Collections.Generic;

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

    public static bool WithinRange(int value, int min, int max) => min <= value && value < max;

    public static T[][] Split<T>(this T[] data, int size)
    {
        var result = new T[data.Length / size][];
        for (int i = 0; i < data.Length; i += size)
            result[i / size] = data.Slice(i, size);
        return result;
    }

    public static int FindNextValidIndex<T>(IList<T> dest, Func<T, bool> skip, int ctr)
    {
        while (true)
        {
            if ((uint)ctr >= dest.Count)
                return -1;
            var exist = dest[ctr];
            if (exist == null || !skip(exist))
                return ctr;
            ctr++;
        }
    }

    internal static T[] ConcatAll<T>(params T[][] arr)
    {
        int len = 0;
        foreach (var a in arr)
            len += a.Length;

        var result = new T[len];

        int ctr = 0;
        foreach (var a in arr)
        {
            a.CopyTo(result, ctr);
            ctr += a.Length;
        }

        return result;
    }

    internal static T[] ConcatAll<T>(T[] arr1, T[] arr2)
    {
        int len = arr1.Length + arr2.Length;
        var result = new T[len];
        arr1.CopyTo(result, 0);
        arr2.CopyTo(result, arr1.Length);
        return result;
    }

    internal static T[] ConcatAll<T>(T[] arr1, T[] arr2, T[] arr3)
    {
        int len = arr1.Length + arr2.Length + arr3.Length;
        var result = new T[len];
        arr1.CopyTo(result, 0);
        arr2.CopyTo(result, arr1.Length);
        arr3.CopyTo(result, arr1.Length + arr2.Length);
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
