namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class PlacementZone_V3f
{
    public string Location3f => $"({LocationX}, {LocationY}, {LocationZ})";

    public override string ToString() => Location3f;
}
