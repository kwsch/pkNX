
namespace pkNX.Game
{
    public class TextReference
    {
        public readonly int Index;
        public readonly TextName Name;

        internal TextReference(int index, TextName name)
        {
            Index = index;
            Name = name;
        }
    }
}
