using System.Collections.Generic;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game
{
    public class PokeEditor8a : IDataEditor
    {
        public IPersonalTable Personal { get; set; }
        public TableCache<EvolutionTable8, EvolutionSet8a> Evolve { get; set; }
        public TableCache<Learnset8a, Learnset8aMeta> Learn { get; set; }

        public void CancelEdits() { }

        public void Initialize() { }

        public void Save()
        {
            Personal.Save();
            Learn.Save();
            Evolve.Save();
        }
    }
}
