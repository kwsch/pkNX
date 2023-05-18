using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Transform
{
    public override string ToString() => $"{{ S: {Scale}, R: {Rotate}, T: {Translate} }}";
}
