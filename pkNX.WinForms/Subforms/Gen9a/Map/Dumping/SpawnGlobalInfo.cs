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
    public readonly Dictionary<string, SpawnerTransformData> SpawnerTransforms;

    public readonly PokemonSpawnerDataDBArray SpawnerDatabase;

    public SpawnGlobalInfo(GameManager9a rom)
    {
        _rom = rom;

        const string emd = "world/ik_data/field/pokemon/encount_data/encount_data/encount_data_array.bin";
        var encT = Get<EncountDataDBArray>(emd);
        Encounters = encT.Table.SelectMany(z => z.Table).ToDictionary(z => z.Id, z => z);

        const string pst = "world/ik_data/field/spawner_transform_data/pokemon_spawner_transform/pokemon_spawner_transform/pokemon_spawner_transform_array.bin";
        var spawnerT = Get<SpawnerTransformDataDBArray>(pst);
        SpawnerTransforms = spawnerT.Table.SelectMany(z => z.Table).ToDictionary(z => z.Name ?? "", z => z);

        const string psp = "world/ik_data/field/pokemon_spawner/pokemon_spawner_data/pokemon_spawner_data_array.bin";
        SpawnerDatabase = Get<PokemonSpawnerDataDBArray>(psp);
    }

    private T Get<T>(string path) where T : class, IFlatBufferSerializable<T>
        => FlatBufferConverter.DeserializeFrom<T>(_rom.GetPackedFile(path));
}
