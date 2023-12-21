using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary>
/// 16 bit unsigned normalized float type. All 0's means 0.0f, and all 1's means 1.0f.
/// A sequence of evenly spaced floating point values from 0.0f to 1.0f are represented.
/// e.g. a 2-bit UNORM represents 0.0f, 1/3, 2/3, and 1.0f.
/// </summary>
public struct Unorm16 : IEquatable<Unorm16>, IFormattable
{
    private ushort _value;
    private const int ONE = ushort.MaxValue;

    public static float MaxValue => 1.0f;
    public static float MinValue => 0.0f;

    public static readonly Unorm16 Zero = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm16(float v)
    {
        _value = FromFloat(v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm16(int fixedValue)
    {
        _value = (ushort)fixedValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm16(ushort fixedValue)
    {
        _value = fixedValue;
    }

    /// <summary>Explicitly converts a float value to a Unorm16 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Unorm16(float v)
    {
        return new Unorm16(v);
    }

    /// <summary>Implicitly converts a Unorm16 value to a float value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Unorm16 d)
    {
        return ToFloat(d._value);
    }

    /// <summary>Explicitly converts a ushort value to a Unorm16 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Unorm16(ushort v)
    {
        return new Unorm16(v);
    }

    /// <summary>Returns whether two Unorm16 values are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Unorm16 lhs, Unorm16 rhs)
    {
        return lhs._value == rhs._value;
    }

    /// <summary>Returns whether two Unorm16 values are different.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Unorm16 lhs, Unorm16 rhs)
    {
        return lhs._value != rhs._value;
    }

    /// <summary>Returns true if the Unorm16 is equal to a given Unorm16, false otherwise.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unorm16 rhs)
    {
        return _value == rhs._value;
    }

    /// <summary>Returns true if the Unorm16 is equal to a given Unorm16, false otherwise.</summary>
    public override bool Equals(object? o)
    {
        return o != null && Equals((Unorm16)o);
    }

    /// <summary>Returns a hash code for the Unorm16.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return _value;
    }

    /// <summary>Returns a string representation of the Unorm16.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return ToFloat(_value).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>Returns a string representation of the Unorm16 using a specified format and culture-specific format information.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToFloat(_value).ToString(format, formatProvider);
    }

    private static float ToFloat(ushort val)
    {
        return ((float)val) / ONE;
    }

    public static ushort FromFloat(float val)
    {
        return (ushort)(val * ONE);
    }
}
