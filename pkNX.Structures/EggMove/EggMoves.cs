using System.Linq;

namespace pkNX.Structures;

public abstract class EggMoves
{
    public readonly int[] Moves;
    protected EggMoves(int[] moves) => Moves = moves;
    public bool GetHasEggMove(int move) => Moves.Contains(move);
}
