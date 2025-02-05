namespace pkNX.Structures.FlatBuffers;

public partial struct Sphere(AABB bounds)
{
    public override string ToString() => $"{{ Radius: {Radius}, Center: {Center} }}";
}
