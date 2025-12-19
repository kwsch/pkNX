using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public sealed class MapSpawnerSet
{
    public required Dictionary<string, PokemonSpawnerData> SpawnerInfo { get; init; }

    public bool TryGetSpawnerFromEncounter(string encId, [NotNullWhen(true)] out PokemonSpawnerData? spawnerData)
    {
        foreach (var (key, value) in SpawnerInfo)
        {
            if (value.EncountDataInfoList.All(x => x.EncountDataId != encId))
                continue;

            spawnerData = value;
            return true;
        }
        spawnerData = null;
        return false;
    }
}
