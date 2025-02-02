namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmmt

public partial class MultiMaterialTable
{
    public static MultiMaterialTable Empty => new()
    {
        Material = [],
    };
}
