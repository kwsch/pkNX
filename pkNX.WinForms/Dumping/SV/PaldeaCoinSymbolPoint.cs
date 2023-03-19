using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.WinForms;

public class PaldeaCoinSymbolPoint
{
    public string Name;
    public ulong FirstNum;
    public string BoxLabel;
    public PackedVec3f Position;

    public PaldeaCoinSymbolPoint(string name, ulong num, string boxLabel, PackedVec3f pos)
    {
        Name = name;
        FirstNum = num;
        BoxLabel = boxLabel;
        Position = new PackedVec3f
        {
            X = pos.X,
            Y = pos.Y,
            Z = pos.Z,
        };
    }

    public bool IsBox => !string.IsNullOrEmpty(BoxLabel);
}
