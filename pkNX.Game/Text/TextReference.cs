
namespace pkNX.Game;

public class TextReference
{
    public readonly int Index;
    public readonly TextName Name;
    public readonly string FileName;

    internal TextReference(int index, TextName name)
    {
        Index = index;
        Name = name;
    }

    internal TextReference(string fileName, TextName name)
    {
        FileName = fileName;
        Name = name;
    }
}