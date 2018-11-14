namespace pkNX.Structures
{
    /// <summary>
    /// Criteria for evolving to another species.
    /// </summary>
    public class EvolutionMethod
    {
        public EvolutionType Method;
        public int Species;
        public int Argument;
        public int Form = -1;
        public int Level;

        public EvolutionMethod Copy(int species = -1)
        {
            if (species < 0)
                species = Species;
            return new EvolutionMethod
            {
                Method = Method,
                Species = species,
                Argument = Argument,
                Form = Form,
                Level = Level
            };
        }
    }
}