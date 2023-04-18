using System.Collections.Generic;
using pkNX.Structures;

namespace pkNX.Game;

public class PokeEditor : IDataEditor
{
    public IPersonalTable Personal { get; init; } = null!;
    public DataCache<Learnset> Learn { get; init; } = null!;
    public DataCache<EvolutionSet> Evolve { get; init; } = null!;
    public DataCache<MegaEvolutionSet[]> Mega { get; init; } = null!;
    public IReadOnlyList<ushort> TMHM { get; init; } = null!;
    public IReadOnlyList<ushort> TR { get; init; } = null!;

    public void CancelEdits()
    {
        Learn.CancelEdits();
        Evolve.CancelEdits();
        Mega?.CancelEdits();
    }

    public void Initialize() { }

    public void Save()
    {
        Learn.Save();
        Evolve.Save();
        Mega?.Save();
    }
}
