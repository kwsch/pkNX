using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct PackedVec4f
{
    public PackedVec4f() { }

    public PackedVec4f(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public override string ToString() => $"{{ X: {X}, Y: {Y}, Z: {Z}, W: {W} }}";
}
