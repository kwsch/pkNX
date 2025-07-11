namespace pkNX.Structures.FlatBuffers;

public class PaldeaFixedSymbolPoint(string key, PackedVec3f pos)
{
    public string TableKey = key;
    public PackedVec3f Position = new()
    {
        X = pos.X,
        Y = pos.Y,
        Z = pos.Z,
    };
}
