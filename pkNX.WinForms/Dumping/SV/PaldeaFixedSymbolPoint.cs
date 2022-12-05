using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class PaldeaFixedSymbolPoint
{
    public string TableKey;
    public PackedVec3f Position;

    public PaldeaFixedSymbolPoint(string key, PackedVec3f pos)
    {
        TableKey = key;
        Position = new PackedVec3f
        {
            X = pos.X,
            Y = pos.Y,
            Z = pos.Z,
        };
    }
}
