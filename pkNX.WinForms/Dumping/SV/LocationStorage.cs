using System;
using System.Collections.Generic;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public class LocationStorage
{
    public readonly int Location;
    public readonly AreaInfo AreaInfo;
    public readonly string AreaName;

    public readonly HashSet<ulong> Added = new();
    public readonly List<PaldeaEncounter> Slots = new();
    public readonly Dictionary<int, LocationStorage> SlotsCrossover = new();
    public readonly List<LocationPointDetail> Local = new();
    public readonly List<LocationPointDetail> Nearby = new();

    public LocationStorage(int loc, string areaName, AreaInfo info)
    {
        Location = loc;
        AreaName = areaName;
        AreaInfo = info;
    }

    private void Add(PaldeaEncounter slot)
    {
        var key = slot.GetHash();
        if (Added.Add(key))
            Slots.Add(slot);
    }

    private void AddCrossover(PaldeaEncounter slot, int loc)
    {
        if (!SlotsCrossover.TryGetValue(loc, out var s))
            SlotsCrossover[loc] = s = new LocationStorage(loc, AreaName, AreaInfo);
        s.Add(slot with { CrossFromLocation = (ushort)Location });
    }

    public void Integrate()
    {
        foreach (var point in Local)
        {
            foreach (var slot in point.Slots)
                Add(slot);
        }
        foreach (var point in Nearby)
        {
            foreach (var slot in point.Slots)
                AddCrossover(slot, point.Location);
        }
    }

    public void Consolidate()
    {
        ConsolidateEncounters(Slots);
        foreach (var loc in SlotsCrossover.Values)
            loc.Consolidate();
    }

    private static void ConsolidateEncounters(List<PaldeaEncounter> encounters)
    {
        // Merge and remove future indexes if they can be combined with current
        // Pre-sort so we can just iterate.
        encounters.Sort();

        for (int i = 0; i < encounters.Count;)
        {
            // For i to i+count, merge within
            var enc = encounters[i];
            int count = GetPotentialConsolidationCount(encounters, i, enc);

            // Remove all indexes that have Absorb return true with a later element
            while (TryAbsorbFromRange(encounters, i, count))
                count--;
            i += count;
        }
    }

    private static int GetPotentialConsolidationCount(List<PaldeaEncounter> encounters, int start, PaldeaEncounter enc)
    {
        int count = 1;
        for (int end = start + 1; end < encounters.Count; end++)
        {
            if (!encounters[end].IsSameSpecFormGender(enc))
                break;
            count++;
        }
        return count;
    }

    private static bool TryAbsorbFromRange(List<PaldeaEncounter> encounters, int start, int count)
    {
        // Need to compare all elements with each-other; return true if any absorb.
        var end = start + count - 1;
        for (int i = start; i < end; i++)
        {
            for (int j = i + 1; j <= end; j++)
            {
                if (!encounters[i].Absorb(encounters[j]))
                    continue;
                encounters.RemoveAt(j);
                return true;
            }
        }
        return false;
    }

    public void LoadPoints(IEnumerable<LocationPointDetail> points, IContainsV3f collider, int areaMin, int areaMax)
    {
        foreach (var w in points)
        {
            if (!w.IsWithinLevelRange(areaMin, areaMax))
                continue;
            if (collider.ContainsPoint(w.X, w.Y, w.Z))
            {
                var composite = GetCompositePoint(w.Point, areaMin, areaMax, Location);
                Local.Add(composite); // native
            }
            // Don't check here for cross-area, as our areaMin/areaMax is not valid for that crossover case.
        }
    }

    private static LocationPointDetail GetCompositePoint(PointData ep, int areaMin, int areaMax, int location)
    {
        var newX = Math.Max(ep.LevelRange.X, areaMin);
        var newY = Math.Min(ep.LevelRange.Y, areaMax);
        var newPoint = new PointData
        {
            Position = ep.Position,
            LevelRange = new SV.PackedVec2f { X = newX, Y = newY },
            Biome = ep.Biome,
            Substance = ep.Substance,
            AreaNo = ep.AreaNo,
        };
        return new LocationPointDetail(newPoint) { Location = location };
    }

    public void GetEncounters(EncountPokeDataArray pokeData, PaldeaSceneModel scene)
    {
        foreach (var spawner in Local)
        {
            foreach (var pd in pokeData.Table)
            {
                if (!IsAbleToSpawnAt(pd, spawner, AreaName, scene))
                    continue;

                // Add encount
                spawner.Add(PaldeaEncounter.GetNew(pd, spawner.Point));
                if (pd.BandPoke != 0) // Add band encount
                    spawner.Add(PaldeaEncounter.GetBand(pd, spawner.Point));
            }
        }
    }

    private static bool IsAbleToSpawnAt(EncountPokeData pd, LocationPointDetail ep, string areaName, PaldeaSceneModel scene)
    {
        // Check area
        if (!string.IsNullOrEmpty(pd.Area) && !IsInArea(pd.Area, ep.Point.AreaNo))
            return false;

        // Check loc
        if (!string.IsNullOrEmpty(pd.LocationName) && !IsInArea(pd.LocationName, areaName, scene, ep.Point))
            return false;

        // Check biome
        if (!HasBiome(pd, (Biome)(int)ep.Point.Biome))
            return false;

        // check level range overlap
        // check area level range overlap -- already done via point
        if (!LevelWithinRange(pd, ep.Point))
            return false;

        // Assume flag, enable table, timetable are fine
        // Assume version is fine -- union wireless sessions can share encounters.

        return true;
    }

    private static bool LevelWithinRange(EncountPokeData pd, PointData clamp)
    {
        return clamp.LevelRange.X <= pd.MaxLevel && pd.MinLevel <= clamp.LevelRange.Y;
    }

    private static bool HasBiome(EncountPokeData pd, Biome biome)
    {
        if (biome == pd.Biome1)
            return true;
        if (biome == pd.Biome2)
            return true;
        if (biome == pd.Biome3)
            return true;
        if (biome == pd.Biome4)
            return true;
        return false;
    }

    private static bool IsInArea(ReadOnlySpan<char> areaName, int areaNo)
    {
        int start = 0;
        for (int i = 0; i < areaName.Length; i++)
        {
            if (areaName[i] != ',')
                continue;
            var name = areaName[start..i];
            if (int.TryParse(name, out var tmp) && areaNo == tmp)
                return true;
            start = i + 1;
        }
        return int.TryParse(areaName[start..], out var x) && areaNo == x;
    }

    private static bool IsInArea(string locName, string areaName, PaldeaSceneModel scene, PointData ep)
    {
        var split = locName.Split(",");
        foreach (string a in split)
        {
            if (a == areaName)
                return true;

            if (!scene.IsPointContained(a, ep.Position.X, ep.Position.Y, ep.Position.Z))
                continue;

            return true;
        }
        return false;
    }
}
