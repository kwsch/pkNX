using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class PaldeaCoinSymbolPoint(string name, ulong num, string boxLabel, PackedVec3f pos)
{
    public string Name = name;
    public ulong FirstNum = num;
    public string BoxLabel = boxLabel;
    public PackedVec3f Position = new()
    {
        X = pos.X,
        Y = pos.Y,
        Z = pos.Z,
    };

    public bool IsBox => !string.IsNullOrEmpty(BoxLabel);
}
