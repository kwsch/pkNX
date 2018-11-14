using System.Collections.Generic;

namespace pkNX.Structures
{
    public class VsTrainer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public TrainerData Trainer { get; set; }
        public readonly List<TrainerPoke> Team = new List<TrainerPoke>(6);

        public TrainerClass GetClass(IList<TrainerClass> list) => list[Trainer.Class];
    }
}
