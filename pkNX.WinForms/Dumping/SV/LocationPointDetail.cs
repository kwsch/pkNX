using System.Collections.Generic;
using PKHeX.Core;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public class LocationPointDetail
{
    public readonly PointData Point;

    public LocationPointDetail(PointData point) => Point = point;
    public float X => Point.Position.X;
    public float Y => Point.Position.Y;
    public float Z => Point.Position.Z;
    public int Location { get; init; }
    public int LevelAdjust { get; init; }

    public bool IsWithinLevelRange(int areaMin, int areaMax)
    {
        return Point.LevelRange.X <= areaMax && areaMin <= Point.LevelRange.Y;
    }

    public readonly List<PaldeaEncounter> Slots = new();

    public void Add(PaldeaEncounter slot) => Slots.Add(slot);

    public string GetString()
    {
        var adjustStr = LevelAdjust != 0 ? $", {Point.LevelRange.X + LevelAdjust}-{Point.LevelRange.Y + LevelAdjust}" : "";
        return $"{Location:0000} ({X:0.0000},{Y:0.0000},{Z:0.0000}) {Point.Biome} ({Point.LevelRange.X}-{Point.LevelRange.Y}{adjustStr}) {Slots.Count:00}";
    }
}
