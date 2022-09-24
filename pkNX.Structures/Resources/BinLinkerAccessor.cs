﻿using System;
using System.Diagnostics;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

/// <summary>
/// Unpacks a BinLinkerAccessor generated file container into individual arrays.
/// </summary>
[DebuggerDisplay($"{{{nameof(Identifier)},nq}}[{{{nameof(Length)},nq}}]")]
public readonly ref struct BinLinkerAccessor
{
    /// <summary> Backing data object </summary>
    private readonly ReadOnlySpan<byte> Data;

    /// <summary> Total count of files available for accessing. </summary>
    public int Length => ReadUInt16LittleEndian(Data[2..]);

    /// <summary> Magic identifier for the file. </summary>
    public string Identifier => new(new[] { (char)Data[0], (char)Data[1] });

    /// <summary>
    /// Retrieves a view of the entry at the requested <see cref="index"/>.
    /// </summary>
    /// <param name="index">Entry to retrieve.</param>
    public ReadOnlySpan<byte> this[int index] => GetEntry(index);

    private BinLinkerAccessor(ReadOnlySpan<byte> data) => Data = data;

    private ReadOnlySpan<byte> GetEntry(int index)
    {
        int offset = 4 + (index * sizeof(int));
        int end = ReadInt32LittleEndian(Data[(offset + 4)..]);
        int start = ReadInt32LittleEndian(Data[offset..]);
        return Data[start..end];
    }

    /// <summary>
    /// Sanity checks the input <see cref="data"/> only in DEBUG builds, and returns a new wrapper.
    /// </summary>
    /// <param name="data">Data reference</param>
    /// <param name="identifier">Expected identifier (debug verification only)</param>
    public static BinLinkerAccessor Get(ReadOnlySpan<byte> data, string identifier)
    {
        var result = new BinLinkerAccessor(data);
#if DEBUG
        Debug.Assert(data.Length > 4);
        Debug.Assert(identifier[0] == data[0] && identifier[1] == data[1]);
#endif
        return result;
    }
}
