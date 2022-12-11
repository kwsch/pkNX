using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

public class LocationDatabase
{
    public readonly Dictionary<int, LocationStorage> Locations = new();

    public LocationStorage Get(int location, string areaName, AreaInfo areaInfo)
    {
        if (!Locations.TryGetValue(location, out var loc))
            Locations[location] = loc = new LocationStorage(location, areaName, areaInfo);
        return loc;
    }
}
