using System.IO;
using pkNX.Containers;

namespace pkNX.Game;

/// <summary>
/// File reference pointing to the location of the <see cref="GameFile"/> data.
/// </summary>
public class GameFileReference
{
    /// <summary>
    /// Type of data the file contains.
    /// </summary>
    public GameFile File { get; }

    /// <summary>
    /// Location of the file in the user's environment.
    /// </summary>
    public string RelativePath { get; }

    /// <summary>
    /// Type of container the data is within.
    /// </summary>
    public ContainerType Type { get; }

    /// <summary>
    /// Toggle to indicate that the <see cref="File"/> data is localized and should be shifted.
    /// </summary>
    public bool LanguageVariant { get; }

    public int Language { get; }

    /// <summary>
    /// Indicates the parent of the data.
    /// </summary>
    public ContainerParent Parent { get; set; }

    internal GameFileReference(int FileNumber, GameFile ident, ContainerType t = ContainerType.GARC)
    {
        File = ident;
        Type = t;

        int A = FileNumber / 100 % 10;
        int B = FileNumber / 10 % 10;
        int C = FileNumber / 1 % 10;
        RelativePath = Path.Combine("a", A.ToString(), B.ToString(), C.ToString());
    }

    internal GameFileReference(string relPath, ContainerType t, GameFile ident, bool variant = false)
    {
        File = ident;
        Type = t;
        LanguageVariant = variant;

        RelativePath = relPath;
    }

    internal GameFileReference(GameFile ident, int lang, params string[] relPath)
    {
        File = ident;
        Type = ContainerType.Folder;
        LanguageVariant = true;
        Language = lang;

        RelativePath = Path.Combine(relPath);
    }

    internal GameFileReference(GameFile ident, params string[] relPath)
    {
        File = ident;
        Type = ContainerType.Folder;

        RelativePath = Path.Combine(relPath);
    }

    internal GameFileReference(GameFile ident, ContainerType t, params string[] relPath)
    {
        File = ident;
        Type = t;

        RelativePath = Path.Combine(relPath);
    }

    public IFileContainer Get(string basePath)
    {
        var path = Path.Combine(basePath, RelativePath);
        var container = Container.GetContainer(path, Type);
        container.FilePath = path;
        return container;
    }
}