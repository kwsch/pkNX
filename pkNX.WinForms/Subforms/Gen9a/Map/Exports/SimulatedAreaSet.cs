using pkNX.Structures.FlatBuffers.ZA;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.WinForms;

public sealed class SimulatedAreaSet(SpawnRipper9a self)
{
    // Synthetic data to aggregate results in an easier to handle format.
    public readonly Dictionary<int, List<EncounterSlot9a>> Simple = []; // pkl for PKHeX
    public readonly Dictionary<int, List<FakeSpawner9a>> Detailed = []; // json for detailed import elsewhere

    public void Populate(SpawnSceneSimulation simulation)
    {
        var spawnerPositions = simulation.SpawnerPositions;
        foreach (var (name, pos) in spawnerPositions)
        {
            if (!self.TryGetSpawner(name, out var spawnerData))
                continue;

            var locationIndex = simulation.LocationNameFetch(pos.Position);
            foreach (var appearInfo in spawnerData.AppearanceSpawnerObjectInfoList)
            {
                var count = appearInfo.Appearance;
                if (count.MaxCount == 0)
                    continue; // shouldn't ever be true

                var encounters = spawnerData.EncountDataInfoList;
                if (encounters.Count == 0)
                    continue; // no encounters?

                AddSpawnsToLocation(locationIndex, encounters);
                AddSpawnsToMap(locationIndex, encounters, pos, spawnerData, count);
            }
        }
    }

    private void AddSpawnsToLocation(int locationIndex, IEnumerable<EncountDataInfo> encounters)
    {
        if (!Simple.TryGetValue(locationIndex, out var list))
            Simple[locationIndex] = list = [];

        foreach (var enc in encounters)
        {
            if (enc.EncountDataId is not { } id)
                continue;
            if (!self.TryGetEncounter(id, out var slot))
                continue;

            EncounterSlot9a.AddSlots(list, slot, enc);
        }
    }

    private void AddSpawnsToMap(int locationIndex, IList<EncountDataInfo> encounters, SceneSpawner link, PokemonSpawnerData spawnerData, AppearanceInfo count)
    {
        if (!Detailed.TryGetValue(locationIndex, out var list))
            Detailed[locationIndex] = list = [];

        var point = new FakeSpawner9a
        {
            Link = link,
            Location = self.GetLocationName(locationIndex),
            SpawnCountMin = count.MinCount,
            SpawnCountMax = count.MaxCount,
            Cooldown = spawnerData.CoolTime.Time,
            CooldownCondition = spawnerData.CoolTime.Condition,

            Conditions = spawnerData.ActivationConditionList.Select(z => z.Element).GetSummary(),
        };

        // not a perfect approximate (time of day Pumpkaboo, weather) but close enough for rough rips.
        var totalWeight = encounters.Sum(z => z.Weight);

        foreach (var enc in encounters)
        {
            if (enc.EncountDataId is not { } id)
                continue;
            if (!self.TryGetEncounter(id, out var slot))
                continue;

            var detailed = DetailedSpawn9a.Create(enc, slot, totalWeight);
            point.Spawns.Add(detailed);
        }

        if (point.Spawns.Count == 0)
        {

        }

        list.Add(point);
    }

    public IEnumerable<EncounterArea9a> ToAreas()
    {
        foreach (var (loc, slots) in Simple)
        {
            yield return new EncounterArea9a
            {
                Location = loc,
                Slots = slots,
            };
        }
    }
}
