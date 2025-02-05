namespace pkNX.Structures.FlatBuffers;

public partial struct PackedVec3f : IEquatable<PackedVec3f>
{
    public static readonly PackedVec3f Zero = new();
    public static readonly PackedVec3f One = new() { X = 1, Y = 1, Z = 1 };

    public static explicit operator PackedVec3f(Vec3f v) => new() { X = v.X, Y = v.Y, Z = v.Z };

    // ReSharper disable once ConvertToPrimaryConstructor
#pragma warning disable IDE0290 // Use primary constructor
    public PackedVec3f(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }
#pragma warning restore IDE0290 // Use primary constructor

    public readonly bool IsOne() => X is 1 && Y is 1 && Z is 1;
    public readonly bool IsZero() => X is 0 && Y is 0 && Z is 0;
    public readonly float Magnitude() => MathF.Sqrt(MagnitudeSqr());
    public readonly float MagnitudeSqr() => Dot(this);
    public readonly PackedVec3f Normalized() => this * (1 / Magnitude());

    public readonly float Dot(PackedVec3f other) => (X * other.X) + (Y * other.Y) + (Z * other.Z);
    public readonly PackedVec3f Cross(PackedVec3f other) => new((Y * other.Z) - (Z * other.Y), (Z * other.X) - (X * other.Z), (X * other.Y) - (Y * other.X));
    public readonly float DistanceTo(PackedVec3f other) => (this - other).Magnitude();
    public readonly float DistanceToSqr(PackedVec3f other) => (this - other).MagnitudeSqr();
    public readonly PackedVec3f Lerp(PackedVec3f other, float t) => this + ((other - this) * t);

    public readonly PackedVec3f Clone() => new()
    {
        X = X,
        Y = Y,
        Z = Z,
    };

    public readonly override string ToString() => $"V3f({X}, {Y}, {Z})";
    public readonly string ToTriple() => $"({X}, {Y}, {Z})";

    public readonly bool Equals(PackedVec3f other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PackedVec3f other && Equals(other);
    }

    public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static PackedVec3f operator -(PackedVec3f v) => new(-v.X, -v.Y, -v.Z);

    public static PackedVec3f operator +(PackedVec3f l, PackedVec3f r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static PackedVec3f operator -(PackedVec3f l, PackedVec3f r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
    public static PackedVec3f operator *(PackedVec3f l, PackedVec3f r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
    public static PackedVec3f operator /(PackedVec3f l, PackedVec3f r) => new(l.X / r.X, l.Y / r.Y, l.Z / r.Z);

    public static PackedVec3f operator +(PackedVec3f l, float r) => new(l.X + r, l.Y + r, l.Z + r);
    public static PackedVec3f operator -(PackedVec3f l, float r) => new(l.X - r, l.Y - r, l.Z - r);
    public static PackedVec3f operator *(PackedVec3f l, float r) => new(l.X * r, l.Y * r, l.Z * r);
    public static PackedVec3f operator /(PackedVec3f l, float r) => new(l.X / r, l.Y / r, l.Z / r);

    public static bool operator ==(PackedVec3f? left, PackedVec3f? right) => Equals(left, right);
    public static bool operator !=(PackedVec3f? left, PackedVec3f? right) => !Equals(left, right);
}
