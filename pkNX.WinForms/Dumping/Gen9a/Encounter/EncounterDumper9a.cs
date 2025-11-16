using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV; // TODO ZA
using System.Collections.Generic;
using System.IO;

namespace pkNX.Structures.FlatBuffers;

public static class EncounterDumper9a
{
    public static void Dump(IFileInternal rom, EncounterDumpConfig9a config)
    {
        if (!Directory.Exists(config.Path))
            Directory.CreateDirectory(config.Path);

        // TODO ZA
    }
}

public record EncounterDumpConfig9a
{
    public required string Path { get; init; }
    public required string[] SpecNamesInternal { get; init; }
    public required string[] MoveNames { get; init; }
    public required Dictionary<string, (string Name, int Index)> PlaceNameMap { get; init; }

    public bool WriteText { get; init; } = true;
    public bool WritePickle { get; init; } = true;

    public string this[WazaSet move] => MoveNames[(int)move.WazaId];
    public string this[DevID species] => SpecNamesInternal[(int)species];
}
