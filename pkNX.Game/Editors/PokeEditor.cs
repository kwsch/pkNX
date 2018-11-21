using System;
using pkNX.Structures;

namespace pkNX.Game
{
    public class PokeEditor : IDataEditor
    {
        public PersonalTable Personal { get; set; }
        public DataCache<Learnset> Learn { get; set; }
        public DataCache<EvolutionSet> Evolve { get; set; }
        public DataCache<MegaEvolutionSet[]> Mega { get; set; }

        public void CancelEdits()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
