namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class PlacementZoneSpeciesHolder
{
    public override string ToString() => $"{(Species)Species}{(Form != 0 ? $"-{Form}" : "")}";
}
