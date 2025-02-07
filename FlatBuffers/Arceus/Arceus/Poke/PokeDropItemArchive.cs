namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PokeDropItem
{
    public string Dump(string[] itemNames) => $"{Hash:X16}\t{itemNames[RegularItem]}\t{RegularItemProbability}\t{itemNames[RareItem]}\t{RareItemProbability}";

    public override string ToString() =>
        $"Hash: {Hash:X16}, " +
        $"RegularItem: {RegularItem}, " +
        $"RegularItemProbability: {RegularItemProbability}, " +
        $"RareItem: {RareItem}, " +
        $"RareItemProbability: {RareItemProbability}";
}
