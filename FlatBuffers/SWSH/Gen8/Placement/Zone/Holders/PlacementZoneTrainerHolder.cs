namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class PlacementZoneTrainerHolder
{
    public override string ToString()
    {
        var hashModel = Field00.Field00.HashModel;
        var name = PlacementZoneOtherNPCHolder.Models.TryGetValue(hashModel, out var model) ? model : hashModel.ToString("X16");
        return name;
    }
}
