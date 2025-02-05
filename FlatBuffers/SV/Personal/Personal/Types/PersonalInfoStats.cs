namespace pkNX.Structures.FlatBuffers.SV;

public partial class PersonalInfoStats
{
    public ushort U16() => (ushort)((HP & 0b11) | ((ATK & 0b11) << 2) | ((DEF & 0b11) << 4) | ((SPE & 0b11) << 6) | ((SPA & 0b11) << 8) | ((SPD & 0b11) << 10));
}
