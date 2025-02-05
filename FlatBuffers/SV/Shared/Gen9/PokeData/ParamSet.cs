namespace pkNX.Structures.FlatBuffers.SV;

public partial class ParamSet
{
    public string SlashSeparated() => $"{HP}/{ATK}/{DEF}/{SPA}/{SPD}/{SPE}";

    public int[] ToArray() => [HP, ATK, DEF, SPA, SPD, SPE];
}
