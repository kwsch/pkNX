using System.Diagnostics;

namespace pkNX.Structures;

public class Trainer7b : VsTrainer
{
    public Trainer7b(byte[] tr = null, byte[] tp = null)
    {
        Self = new TrainerData7b(tr);
        LoadTeam(tp);
    }

    private void LoadTeam(byte[] tp)
    {
        var pokes = TrainerPoke7b.ReadTeam(tp, Self);
        Debug.Assert(pokes.Length == Self.NumPokemon);
        Team.AddRange(pokes);
    }
}
