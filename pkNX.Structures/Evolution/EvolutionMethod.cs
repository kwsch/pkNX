namespace pkNX.Structures
{
    /// <summary>
    /// Criteria for evolving to this branch in the <see cref="EvolutionTree"/>
    /// </summary>
    /// <param name="Method">Evolution Method</param>
    /// <param name="Species">Evolve to Species</param>
    /// <param name="Form">Destination Form</param>
    /// <param name="Argument">Conditional Argument (different from <see cref="Level"/>)</param>
    /// <param name="Level">Conditional Argument (different from <see cref="Argument"/>)</param>
    /// <param name="LevelUp">Indicates if a level up is required to trigger evolution.</param>
    public class EvolutionMethod
    {
        public EvolutionMethod Copy(int species = -1)
        {
            if (species < 0)
                species = Species;

            return new EvolutionMethod
            {
                Method = Method,
                Species = (ushort)species,
                Form = Form,
                Argument = Argument,
                Level = Level
            };
        }

        public bool HasData => Species != 0;

        /// <summary>Evolve to Species</summary>
        public ushort Species { get; set; }

        /// <summary>Conditional Argument (different from <see cref="Level"/>)</summary>
        public ushort Argument { get; set; }

        /// <summary>Evolution Method</summary>
        public EvolutionType Method { get; set; }

        /// <summary>Destination Form</summary>
        public byte Form { get; set; }

        /// <summary>Conditional Argument (different from <see cref="Argument"/>)</summary>
        public byte Level { get; set; }

        public override string ToString() => $"{(Species)Species}-{Form} [{Argument}] @ {Level}{(RequiresLevelUp ? "X" : "")}";

        /// <summary>Is <see cref="AnyForm"/> if the evolved form isn't modified. Special consideration for <see cref="LevelUpFormFemale1"/>, which forces 1.</summary>
        private const byte AnyForm = byte.MaxValue;

        public bool RequiresLevelUp => Method.IsLevelUpRequired();

        /// <summary>
        /// Returns the form that the Pokémon will have after evolution.
        /// </summary>
        /// <param name="form">Un-evolved Form ID</param>
        public byte GetDestinationForm(byte form)
        {
            if (Method == EvolutionType.LevelUpFormFemale1)
                return 1;
            if (Form == AnyForm)
                return form;
            return Form;
        }
    }
}