namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmdl

public partial class FileReference
{
    public override string ToString() => $"{{ {Filename} }}";

    public static FileReference Empty => new() { Filename = string.Empty };
}

public partial class Model
{
    public static Model Empty => new()
    {
        Meshes = [],
        LODs = [],
        Skeleton = FileReference.Empty,
        Materials = [],
    };
}
