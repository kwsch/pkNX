using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct PackedVec2f
{
    public override string ToString() => $"{{ X: {X}, Y: {Y} }}";
}
