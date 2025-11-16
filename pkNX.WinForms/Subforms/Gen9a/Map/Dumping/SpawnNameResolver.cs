using System;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms;

public sealed class SpawnNameResolver
{
    private readonly GameManager9a _rom;

    // From Text File
    public readonly string[] PlaceNames; // internal
    public readonly string[] LocationNames; // display
    public readonly AHTB Lookup;

    public SpawnNameResolver(GameManager9a rom, string language)
    {
        _rom = rom;
        var cfg = new TextConfig(rom.Game);
        LocationNames = GetCommonText("place_name", language, cfg);
        Lookup = GetCommonAHTB("place_name", language);
        PlaceNames = Lookup.Entries.Select(z => z.Name).ToArray();
    }

    public int GetLocationIndex(string place) => Array.IndexOf(PlaceNames, place);

    private const string message = "ik_message";

    private string[] GetCommonText(string name, string lang, TextConfig cfg)
    {
        var path = $"{message}/dat/{lang}/common/{name}.dat";
        var data = _rom.GetPackedFile(path);
        return new TextFile(data, cfg).Lines;
    }

    private AHTB GetCommonAHTB(string name, string lang)
    {
        var path = $"{message}/dat/{lang}/common/{name}.tbl";
        var data = _rom.GetPackedFile(path);
        return new AHTB(data);
    }
}
