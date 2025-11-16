namespace pkNX.Structures.FlatBuffers.ZA;

public partial class ParamSet
{
    public string SlashSeparated() => $"{HP}/{ATK}/{DEF}/{SPA}/{SPD}/{SPE}";

    public int[] ToArray() => [HP, ATK, DEF, SPA, SPD, SPE];
}
