namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class ConfigEntry
{
    public string ConfiguredValue
    {
        get => Parameters[0];
        set => Parameters[0] = value;
    }
}
