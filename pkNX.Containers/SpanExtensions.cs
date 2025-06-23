using System;
using System.Runtime.CompilerServices;

namespace pkNX.Containers;

public static class SpanExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int offset)
        where T : IEquatable<T>?
    {
        return offset + span[offset..].IndexOf(value);
    }
}
