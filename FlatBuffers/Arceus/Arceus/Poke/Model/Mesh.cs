namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmsh

public partial class BoneWeight
{
    public override string ToString() => $"{{ [{RigIndex}]: {Weight} }}";
}

public partial class InputLayoutElement
{
    public override string ToString() => $"{{ {SemanticName}{SemanticIndex:#} ({Format}) @ {Offset:00} }}";
}

public partial class VertexSize
{
    public override string ToString() => $"{{ {Size} }}";
}
