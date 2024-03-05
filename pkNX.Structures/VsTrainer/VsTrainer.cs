using System.Collections.Generic;

namespace pkNX.Structures;

public class VsTrainer(TrainerData self)
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public TrainerData Self { get; set; } = self;
    public readonly List<TrainerPoke> Team = new(6);

    public TrainerClass GetClass(IList<TrainerClass> list) => list[Self.Class];
}
