using System.Linq;

namespace pkNX.Structures;

public abstract class EggMoves(int[] moves)
{
    public readonly int[] Moves = moves;
    public bool GetHasEggMove(int move) => Moves.Contains(move);
}
