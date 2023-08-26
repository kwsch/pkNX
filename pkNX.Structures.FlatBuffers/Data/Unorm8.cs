using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary>
/// 8 bit unsigned normalized float type. All 0's means 0.0f, and all 1's means 1.0f.
/// A sequence of evenly spaced floating point values from 0.0f to 1.0f are represented.
/// e.g. a 2-bit UNORM represents 0.0f, 1/3, 2/3, and 1.0f.
/// </summary>
public struct Unorm8 : IEquatable<Unorm8>, IFormattable
{
    private byte _value;
    private const int ONE = byte.MaxValue;

    public static float MaxValue => 1.0f;
    public static float MinValue => 0.0f;

    public static readonly Unorm8 Zero = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm8(float v)
    {
        _value = FromFloat(v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm8(int fixedValue)
    {
        _value = (byte)fixedValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Unorm8(byte fixedValue)
    {
        _value = fixedValue;
    }

    /// <summary>Explicitly converts a float value to a Unorm8 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Unorm8(float v)
    {
        return new Unorm8(v);
    }

    /// <summary>Implicitly converts a Unorm8 value to a float value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Unorm8 d)
    {
        return ToFloat(d._value);
    }

    /// <summary>Explicitly converts a byte value to a Unorm8 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Unorm8(byte v)
    {
        return new Unorm8(v);
    }

    /// <summary>Returns whether two Unorm8 values are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Unorm8 lhs, Unorm8 rhs)
    {
        return lhs._value == rhs._value;
    }

    /// <summary>Returns whether two Unorm8 values are different.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Unorm8 lhs, Unorm8 rhs)
    {
        return lhs._value != rhs._value;
    }


    /// <summary>Returns true if the Unorm8 is equal to a given Unorm8, false otherwise.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unorm8 rhs)
    {
        return _value == rhs._value;
    }

    /// <summary>Returns true if the Unorm8 is equal to a given Unorm8, false otherwise.</summary>
    public override bool Equals(object? o)
    {
        return o != null && Equals((Unorm8)o);
    }

    /// <summary>Returns a hash code for the Unorm8.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return _value;
    }

    /// <summary>Returns a string representation of the Unorm8.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return ToFloat(_value).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>Returns a string representation of the Unorm8 using a specified format and culture-specific format information.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToFloat(_value).ToString(format, formatProvider);
    }

    private static float ToFloat(byte val)
    {
        return ((float)val) / ONE;
    }

    public static byte FromFloat(float val)
    {
        return (byte)(val * ONE);
    }
}
