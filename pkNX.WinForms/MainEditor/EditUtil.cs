using pkNX.Randomization;

namespace pkNX.WinForms
{
    public static class EditUtil
    {
        public static PersonalRandSettings Personal { get; set; } = new PersonalRandSettings();
        public static SpeciesSettings Species { get; set; } = new SpeciesSettings();
        public static TrainerRandSettings Trainer { get; set; } = new TrainerRandSettings();
        public static MovesetRandSettings Move { get; set; } = new MovesetRandSettings();
        public static LearnSettings Learn { get; set; } = new LearnSettings();
    }
}
