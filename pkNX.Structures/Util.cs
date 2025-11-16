using System;
using System.Diagnostics;

namespace pkNX.Structures;

public static class Util
{
    public static Random Rand { get; set; } = new();

    public static T[] GetArray<T>(this byte[] entries, Func<byte[], int, T> del, int size)
    {
        if (entries.Length < size)
            return [];

        var data = new T[entries.Length / size];
        for (int i = 0; i < entries.Length; i += size)
            data[i / size] = del(entries, i);
        return data;
    }

    public static T[] GetArray<T>(this byte[] entries, Func<byte[], T> del, int size)
    {
        if (entries.Length < size)
            return [];

        var data = new T[entries.Length / size];
        for (int i = 0; i < entries.Length; i += size)
        {
            byte[] arr = new byte[size];
            Array.Copy(entries, i, arr, 0, size);
            data[i / size] = del(arr);
        }
        return data;
    }

    public delegate TResult FromBytesConstructor<out TResult>(ReadOnlySpan<byte> arg);
    public static T[] GetArray<T>(this ReadOnlySpan<byte> entries, FromBytesConstructor<T> constructor, int size)
    {
        if (entries.Length < size)
            return [];

        Debug.Assert(entries.Length % size == 0, $"This data of size {entries.Length} can't be split into equally sized entries with the provided slice size {size}");

        var array = new T[entries.Length / size];
        for (int i = 0; i < entries.Length; i += size)
        {
            var entry = entries.Slice(i, size);
            array[i / size] = constructor(entry);
        }
        return array;
    }
}
