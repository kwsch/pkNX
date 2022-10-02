using System.Diagnostics;

namespace pkNX.Structures;

public class Trainer8 : VsTrainer
{
    public Trainer8(byte[] tr = null, byte[] tp = null)
    {
        Self = new TrainerData8(tr);
        LoadTeam(tp);
    }

    private void LoadTeam(byte[] tp)
    {
        var pokes = TrainerPoke8.ReadTeam(tp, Self);
        Debug.Assert(pokes.Length == Self.NumPokemon);
        Team.AddRange(pokes);
    }
}
