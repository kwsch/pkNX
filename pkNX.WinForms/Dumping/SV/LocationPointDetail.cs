using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

public class LocationPointDetail
{
    public readonly PointData Point;

    public LocationPointDetail(PointData point) => Point = point;
    public float X => Point.Position.X;
    public float Y => Point.Position.Y;
    public float Z => Point.Position.Z;
    public int Location { get; init; }

    public bool IsWithinLevelRange(int areaMin, int areaMax)
    {
        return Point.LevelRange.X <= areaMax && areaMin <= Point.LevelRange.Y;
    }

    public readonly List<PaldeaEncounter> Slots = new();

    public void Add(PaldeaEncounter slot) => Slots.Add(slot);

    public string GetString()
    {
        return $"{Location:0000} ({X:0.0000},{Y:0.0000},{Z:0.0000}) {Point.Biome} ({Point.LevelRange.X}-{Point.LevelRange.Y}) {Slots.Count:00}";
    }
}
