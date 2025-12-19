using System.Collections.Generic;
using System.Linq;
using FlatSharp;
using pkNX.Game;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public sealed class SpawnGlobalInfo
{
    private readonly GameManager9a _rom;

    public readonly Dictionary<string, EncountData> Encounters;
    public readonly Dictionary<string, List<SpawnerTransformData>> SpawnerTransforms;
    public readonly List<string> HyperspaceSpawnSets;

    public readonly PokemonSpawnerDataDBArray SpawnerDatabase;

    public SpawnGlobalInfo(GameManager9a rom)
    {
        _rom = rom;

        const string emd = "world/ik_data/field/pokemon/encount_data/encount_data/encount_data_array.bin";
        var encT = Get<EncountDataDBArray>(emd);
        Encounters = encT.Table.SelectMany(z => z.Table).ToDictionary(z => z.Id, z => z);

        const string pst = "world/ik_data/field/spawner_transform_data/pokemon_spawner_transform/pokemon_spawner_transform/pokemon_spawner_transform_array.bin";
        var spawnerT = Get<SpawnerTransformDataDBArray>(pst);
        // Build multimap of Name -> List<SpawnerTransformData>; skip null/empty names to avoid collisions.
        SpawnerTransforms = spawnerT.Table
            .SelectMany(z => z.Table)
            .Where(x => !string.IsNullOrEmpty(x.Name))
            .GroupBy(x => x.Name)
            .ToDictionary(g => g.Key!, g => g.ToList());

        const string psp = "world/ik_data/field/pokemon_spawner/pokemon_spawner_data/pokemon_spawner_data_array.bin";
        SpawnerDatabase = Get<PokemonSpawnerDataDBArray>(psp);

        const string dimWild = "world/ik_data/field/dimension/dimension_wild_pokemon/dimension_wild_pokemon/dimension_wild_pokemon_array.bin";
        var dim = Get<DimensionWildPokemonDBArray>(dimWild);

        // Since we don't care about the star count requirements (since distortions can be 1-5 regardless of game progress), we can lump them all together.
        HyperspaceSpawnSets = GetHyperspaceSpawnSets(dim);
    }

    private static List<string> GetHyperspaceSpawnSets(DimensionWildPokemonDBArray dim)
    {
        List<string> result = [];
        foreach (var a in dim.Table)
        {
            foreach (var b in a.Table)
            {
                foreach (var c in b.RankItems)
                {
                    if (c.PatternItems is null) // no encounters for this type-set
                        continue;
                    foreach (var set in c.PatternItems)
                    {
                        if (set.SpawnerItems is not { } si)
                            continue;
                        result.AddRange(si.Select(x => x.SpawnerDataId));
                    }
                }
            }
        }

        return result;
    }

    private T Get<T>(string path) where T : class, IFlatBufferSerializable<T>
        => FlatBufferConverter.DeserializeFrom<T>(_rom.GetPackedFile(path));
}
