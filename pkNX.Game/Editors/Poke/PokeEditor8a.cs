using pkNX.Structures;
using pkNX.Structures.FlatBuffers.Arceus;
using Learnset = pkNX.Structures.FlatBuffers.Arceus.Learnset;
using EvolutionSet = pkNX.Structures.FlatBuffers.Arceus.EvolutionSet;

namespace pkNX.Game;

public class PokeEditor8a : IDataEditor
{
    public IPersonalTable Personal { get; init; } = null!;
    public TableCache<PokeMiscTable, PokeMisc> PokeMisc { get; init; } = null!;
    public TableCache<PokeAIArchive, PokeAI> SymbolBehave { get; init; } = null!;
    public TableCache<EvolutionTable, EvolutionSet> Evolve { get; init; } = null!;
    public TableCache<Learnset, LearnsetMeta> Learn { get; init; } = null!;
    public TableCache<PokeDropItemArchive, PokeDropItem> FieldDropTables { get; init; } = null!;
    public TableCache<PokeDropItemBattleArchive, PokeDropItemBattle> BattleDropTabels { get; init; } = null!;
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; init; } = null!;
    public TableCache<PokeInfoList, PokeInfo> PokeResourceList { get; init; } = null!;
    public TableCache<PokeResourceTable, PokeModelConfig> PokeResourceTable { get; init; } = null!;
    public TableCache<EncounterMultiplierArchive, EncounterMultiplier> EncounterRateTable { get; init; } = null!;
    public TableCache<PokeCaptureCollisionArchive, PokeCaptureCollision> CaptureCollisionTable { get; init; } = null!;

    public void CancelEdits() { }

    public void Initialize() { }

    public void Save()
    {
        Personal.Save();
        PokeMisc.Save();
        SymbolBehave.Save();
        Learn.Save();
        Evolve.Save();
        FieldDropTables.Save();
        BattleDropTabels.Save();
        DexResearch.Save();
        PokeResourceList.Save();
        PokeResourceTable.Save();
        EncounterRateTable.Save();
        CaptureCollisionTable.Save();
    }
}
