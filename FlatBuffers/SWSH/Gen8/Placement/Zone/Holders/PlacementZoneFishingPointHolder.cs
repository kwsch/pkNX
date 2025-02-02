namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class PlacementZoneFishingPointHolder
{
    public override string ToString() => $"{Object.Identifier}" + (Object.IterateForSlotsExceptLastN == 0 ? "" : $" SkipLast{Object.IterateForSlotsExceptLastN}");
}
