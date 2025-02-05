namespace pkNX.Structures.FlatBuffers;

public partial class Vec4i : IEquatable<Vec4i>
{
    public static readonly Vec4i Zero = new();
    public static readonly Vec4i One = new(1, 1, 1, 1);

    public Vec4i(int x = 0, int y = 0, int z = 0, int w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public bool Equals(Vec4i? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vec4i)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public override string ToString() => $"{{ X: {X}, Y: {Y}, Z: {Z}, W: {W} }}";
}
