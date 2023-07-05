using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmsh

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class BoneWeight
{
    public override string ToString() => $"{{ [{RigIndex}]: {Weight} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class InputLayoutElement
{
    public override string ToString() => $"{{ {SemanticName}{SemanticIndex:#} ({Format}) @ {Offset:00} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class VertexSize
{
    public override string ToString() => $"{{ {Size} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class VertexAttributeLayout
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SubMesh
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshShape
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Mesh
{
}
