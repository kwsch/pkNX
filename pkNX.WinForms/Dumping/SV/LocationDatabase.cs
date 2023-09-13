using System.Collections.Generic;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public class LocationDatabase
{
    public readonly Dictionary<int, LocationStorage> Locations = new();

    public LocationStorage Get(int location, PaldeaFieldIndex fieldIndex, string areaName, AreaInfo areaInfo)
    {
        if (!Locations.TryGetValue(location, out var loc))
            Locations[location] = loc = new LocationStorage(location, fieldIndex, areaName, areaInfo);
        return loc;
    }
}
