using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
