using System.Diagnostics;

namespace pkNX.Structures
{
    public class Trainer7b : VsTrainer
    {
        public Trainer7b(byte[] tr = null, byte[] tp = null)
        {
            Trainer = new TrainerData7b(tr);
            LoadTeam(tp);
        }

        private void LoadTeam(byte[] tp)
        {
            var pokes = tp.GetArray((data, offset) => new TrainerPoke7b(offset, data), TrainerPoke7b.SIZE);
            Debug.Assert(pokes.Length == Trainer.NumPokemon);
            Team.AddRange(pokes);
        }
    }
}
