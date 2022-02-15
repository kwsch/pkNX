using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class GameData8a
{
    public PersonalTable PersonalData { get; internal set; }

    public DataCache<Waza8a> MoveData { get; internal set; }
    public DataCache<EvolutionSet8a> EvolutionData { get; internal set; }
    public DataCache<Learnset8aMeta> LevelUpData { get; internal set; }
}
