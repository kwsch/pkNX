using System.Collections.Generic;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public record EncounterDumpConfigSV
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
