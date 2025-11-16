using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game;

public class TextManager(GameVersion game, TextConfig? config = null)
{
    private readonly TextConfig Config = config ?? new TextConfig(game);
    private readonly IReadOnlyCollection<TextReference> References = TextMapping.GetMapping(game);

    private readonly Dictionary<TextName, string[]> Cache = [];

    public void ClearCache() => Cache.Clear();

    internal string[] GetStrings(ReadOnlySpan<byte> data, bool remap = false)
    {
        var txt = new TextFile(data, Config, remap);
        return txt.Lines;
    }

    internal string[] GetStrings(TextName file, IFileContainer textFile, bool remap = false)
    {
        if (Cache.TryGetValue(file, out var container))
            return container;

        var info = References.FirstOrDefault(f => f.Name == file);
        if (info == null)
            throw new ArgumentException($"Unknown {nameof(TextName)} provided.", file.ToString());

        ReadOnlySpan<byte> data;
        if (textFile is FolderContainer c)
            data = c.GetFileData(info.FileName);
        else
            data = textFile[info.Index];

        var lines = GetStrings(data, remap);
        Cache.Add(file, lines);
        return lines;
    }
}
