using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game
{
    public class TextManager
    {
        private readonly TextConfig Config;
        private readonly IReadOnlyCollection<TextReference> References;

        private readonly Dictionary<TextName, string[]> Cache = new Dictionary<TextName, string[]>();

        public TextManager(GameVersion game, TextConfig config = null)
        {
            References = TextMapping.GetMapping(game);
            Config = config ?? new TextConfig(game);
        }

        internal string[] GetStrings(byte[] data, bool remap = false)
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

            byte[] data;
            string path = info.FileName;
            if (!string.IsNullOrWhiteSpace(path) && textFile is FolderContainer c)
                data = c.GetFileData(info.FileName);
            else
                data = textFile[info.Index];

            var lines = GetStrings(data, remap);
            Cache.Add(file, lines);
            return lines;
        }
    }
}
