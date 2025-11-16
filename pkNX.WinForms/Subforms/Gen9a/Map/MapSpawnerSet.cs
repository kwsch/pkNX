using System.Collections.Generic;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public sealed class MapSpawnerSet
{
    public required Dictionary<string, PokemonSpawnerData> SpawnerInfo { get; init; }
}
