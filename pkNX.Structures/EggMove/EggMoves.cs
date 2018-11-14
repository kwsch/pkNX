namespace pkNX.Structures
{
    public abstract class EggMoves
    {
        public int Count;
        public int[] Moves;
        public int FormTableIndex;

        public abstract byte[] Write();
    }
}
