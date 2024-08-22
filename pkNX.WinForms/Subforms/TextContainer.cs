using System.IO;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.WinForms;

public class TextContainer
{
    public readonly IFileContainer Container;
    public readonly TextConfig? Config;
    public bool Remap { get; set; }

    private readonly string[]?[] Cache;

    public TextContainer(IFileContainer c, TextConfig? t = null, bool remap = false)
    {
        Remap = remap;
        Config = t;
        Container = c;
        Cache = new string[Container.Count][];
    }

    public int Length => Cache.Length;

    public string[] this[int index]
    {
        get => Cache[index] ??= GetLines(index);
        set => Cache[index] = value;
    }

    private string[] GetLines(int index) => new TextFile(Container[index], Config, remapChars: Remap).Lines;

    public string GetFileName(int i)
    {
        if (Container is FolderContainer f)
            return Path.GetFileNameWithoutExtension(f.GetFileName(i));
        return i.ToString();
    }

    public void Save()
    {
        for (int i = 0; i < Length; i++)
        {
            if (Cache[i] is not { } x)
                continue;
            var flags = new ushort[x.Length]; // TODO: handle properly, for now just zero-out flags
            Container[i] = TextFile.GetBytes(x, flags, Config, Remap);
        }
    }
}
