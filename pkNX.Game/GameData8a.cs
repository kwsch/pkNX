using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class GameData8a
{
    public IPersonalTable PersonalData { get; internal set; }

    public DataCache<Waza8a> MoveData { get; internal set; }
    public TableCache<EvolutionTable8, EvolutionSet8a> EvolutionData { get; internal set; }
    public TableCache<Learnset8a, Learnset8aMeta> LevelUpData { get; internal set; }
}
