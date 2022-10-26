using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class PokeEditor8a : IDataEditor
{
    public IPersonalTable Personal { get; init; } = null!;
    public TableCache<PokeMiscTable8a, PokeMisc8a> PokeMisc { get; init; } = null!;
    public TableCache<PokeAIArchive8a, PokeAI8a> SymbolBehave { get; init; } = null!;
    public TableCache<EvolutionTable8, EvolutionSet8a> Evolve { get; init; } = null!;
    public TableCache<Learnset8a, Learnset8aMeta> Learn { get; init; } = null!;
    public TableCache<PokeDropItemArchive8a, PokeDropItem8a> FieldDropTables { get; init; } = null!;
    public TableCache<PokeDropItemBattleArchive8a, PokeDropItemBattle8a> BattleDropTabels { get; init; } = null!;
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; init; } = null!;
    public TableCache<PokeInfoList8a, PokeInfo8a> PokeResourceList { get; init; } = null!;
    public TableCache<PokeResourceTable8a, PokeModelConfig8a> PokeResourceTable { get; init; } = null!;
    public TableCache<EncounterMultiplierArchive8a, EncounterMultiplier8a> EncounterRateTable { get; init; } = null!;

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
    }
}
