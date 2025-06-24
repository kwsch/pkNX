using System;
using System.Collections.Generic;
using System.IO;
using pkNX.Structures.FlatBuffers.SV;
using static pkNX.Structures.FlatBuffers.AreaWeather9;

namespace pkNX.Structures.FlatBuffers;

[Flags]
public enum AreaWeather9 : byte
{
    None,
    Normal = 1,
    Overcast = 1 << 1,
    Raining = 1 << 2,
    Thunderstorm = 1 << 3,
    Mist = 1 << 4,
    Snowing = 1 << 5,
    Snowstorm = 1 << 6,
    Sandstorm = 1 << 7,

    Standard = Normal | Overcast | Raining | Thunderstorm,
    Sand = Normal | Overcast | Raining | Sandstorm,
    Snow = Normal | Overcast | Snowing | Snowstorm,
    Inside = Normal | Overcast,
}

public static class AreaWeather9Extensions
{
    /// <summary>
    /// Location IDs matched with possible weather types. Unlisted locations may only have Normal weather.
    /// </summary>
    public static AreaWeather9 GetWeather(byte location) => location switch
    {
        006 => Standard,                       // South Province (Area One)
        010 => Standard,                       // Pokémon League
        012 => Standard,                       // South Province (Area Two)
        014 => Standard,                       // South Province (Area Four)
        016 => Standard,                       // South Province (Area Six)
        018 => Standard,                       // South Province (Area Five)
        020 => Standard,                       // South Province (Area Three)
        022 => Standard,                       // West Province (Area One)
        024 => Sand,                           // Asado Desert
        026 => Standard,                       // West Province (Area Two)
        028 => Standard,                       // West Province (Area Three)
        030 => Standard,                       // Tagtree Thicket
        032 => Standard,                       // East Province (Area Three)
        034 => Standard,                       // East Province (Area One)
        036 => Standard,                       // East Province (Area Two)
        038 => Snow,                           // Glaseado Mountain (1)
        040 => Standard,                       // Casseroya Lake
        044 => Standard,                       // North Province (Area Three)
        046 => Standard,                       // North Province (Area One)
        048 => Standard,                       // North Province (Area Two)
        050 => Standard,                       // Great Crater of Paldea
        056 => Standard,                       // South Paldean Sea
        058 => Standard,                       // West Paldean Sea
        060 => Standard,                       // East Paldean Sea
        062 => Standard,                       // North Paldean Sea
        064 => Inside,                         // Inlet Grotto
        067 => Inside,                         // Alfornada Cavern
        069 => Standard | Inside | Snow | Snow,// Dalizapa Passage (Near Medali, Tunnels, Near Pokémon Center, Near Zapico)
        070 => Standard,                       // Poco Path
        080 => Standard,                       // Cabo Poco
        109 => Standard,                       // Socarrat Trail
        124 => Inside,                         // Area Zero (5)

        132 => Standard, // Kitakami Road
        134 => Standard, // Mossui Town
        136 => Standard, // Apple Hills
        138 => Standard, // Loyalty Plaza
        140 => Standard, // Reveler’s Road
        142 => Standard, // Kitakami Hall
        144 => Standard, // Oni Mountain
        146 => Standard, // Dreaded Den
        148 => Standard, // Oni’s Maw
        150 => Standard, // Oni Mountain
        152 => Standard, // Crystal Pool
        154 => Standard, // Crystal Pool
        156 => Standard, // Wistful Fields
        158 => Standard, // Mossfell Confluence
        160 => Standard, // Fellhorn Gorge
        162 => Standard, // Paradise Barrens
        164 => Standard, // Kitakami Wilds
        166 => Standard, // Timeless Woods
        168 => Standard, // Infernal Pass
        170 => Standard, // Chilling Waterhead

        174 => Standard, // Savanna Biome
        176 => Standard, // Coastal Biome
        178 => Standard, // Canyon Biome
        180 => Snow,     // Polar Biome
        182 => Standard, // Central Plaza
        184 => Standard, // Savanna Plaza
        186 => Standard, // Coastal Plaza
        188 => Standard, // Canyon Plaza
        190 => Standard, // Polar Plaza
        192 => Inside,   // Chargestone Cavern
        194 => Inside,   // Torchlit Labyrinth

        _ => None,
    };

    /// <summary>
    /// Checks if the point is a Misty Point, which is a special case in Kitakami.
    /// </summary>
    public static bool IsMistyPoint(PointData point)
    {
        var coordinate = (point.Position.X, point.Position.Z);
        // Check if the point is within the predefined misty points
        return MistyPoints.Contains(coordinate);
    }

    private static readonly HashSet<(float X, float Y)> MistyPoints = GetMistyPoints();

    private static HashSet<(float X, float Y)> GetMistyPoints()
    {
        const string file = "all_fog.txt"; // Path to the file containing misty points
        var points = new HashSet<(float X, float Y)>();
        if (!File.Exists(file))
        {
            Console.WriteLine("No misty points to consider!");
            return points;
        }

        foreach (var line in File.ReadLines(file))
        {
            var parts = line.Split(',');
            if (parts.Length != 2 || !float.TryParse(parts[0], out var x) || !float.TryParse(parts[1], out var y))
                continue;
            points.Add((x, y));
        }
        return points;
    }
}
