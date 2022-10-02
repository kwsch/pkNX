using System.Collections.Generic;
using pkNX.Structures;

namespace pkNX.Game;

public class PokeEditor : IDataEditor
{
    public IPersonalTable Personal { get; set; }
    public DataCache<Learnset> Learn { get; set; }
    public DataCache<EvolutionSet> Evolve { get; set; }
    public DataCache<MegaEvolutionSet[]> Mega { get; set; }
    public IReadOnlyList<ushort> TMHM { get; set; }

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