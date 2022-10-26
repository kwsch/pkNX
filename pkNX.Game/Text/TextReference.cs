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
        FileName = $"{index:0000} - {name}";
    }

    internal TextReference(string fileName, TextName name)
    {
        FileName = fileName;
        Name = name;
    }
}
