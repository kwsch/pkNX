using System.Collections.Generic;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// Spawn location point that can originate encounter slots.
/// </summary>
public record LocationPointDetail(PointData Point)
{
    /// <summary>
    /// All slots that can originate from this point spawner.
    /// </summary>
    public readonly List<PaldeaEncounter> Slots = [];

    /// <summary>
    /// All area IDs that the player can be standing in to activate this point spawner.
    /// </summary>
    public readonly List<int> ActiveViaAdjacentLocations = new(0);

    /// <summary>
    /// Base location ID of the point spawner.
    /// </summary>
    public int Location { get; init; }

    /// <summary>
    /// Area info AdjustEncLv bias to shift the level range of the point spawner up.
    /// </summary>
    public int LevelAdjust { get; init; }

    public float X => Point.Position.X;
    public float Y => Point.Position.Y;
    public float Z => Point.Position.Z;

    public bool IsWithinLevelRange(int areaMin, int areaMax)
    {
        var range = Point.LevelRange;
        return range.X <= areaMax && areaMin <= range.Y;
    }

    public void Add(PaldeaEncounter slot) => Slots.Add(slot);

    /// <summary>
    /// Flag as being able to be activated from an adjacent location.
    /// </summary>
    public void Activate(int locationID) => ActiveViaAdjacentLocations.Add(locationID);

    public void AssignWeatherPermissions(PaldeaFieldIndex fieldIndex)
    {
        var weather = AreaWeather9Extensions.GetWeather((byte)Location);
        foreach (var other in ActiveViaAdjacentLocations) // duplicates are OK, same result.
        {
            var test = AreaWeather9Extensions.GetWeather((byte)other);
            weather |= test;
        }

        if (fieldIndex is PaldeaFieldIndex.Kitakami && AreaWeather9Extensions.IsMistyPoint(Point.Position))
            weather |= AreaWeather9.Mist;

        foreach (var slot in Slots)
            slot.Weather |= weather;
    }

    public string GetString()
    {
        var range = Point.LevelRange;
        var adjustStr = LevelAdjust != 0 ? $", {range.X + LevelAdjust}-{range.Y + LevelAdjust}" : "";
        return $"{Location:0000} ({X:0.0000},{Y:0.0000},{Z:0.0000}) {Point.Biome} ({range.X}-{range.Y}{adjustStr}) {Slots.Count:00}";
    }
}
