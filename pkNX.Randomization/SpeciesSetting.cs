using pkNX.Structures;

namespace pkNX.Randomization
{
    public class SpeciesSetting
    {
        public bool Gen1 { get; set; }
        public bool Gen2 { get; set; }
        public bool Gen3 { get; set; }
        public bool Gen4 { get; set; }
        public bool Gen5 { get; set; }
        public bool Gen6 { get; set; }
        public bool Gen7 { get; set; }

        public bool L { get; set; } = false;
        public bool E { get; set; } = false;
        public bool Shedinja = false;
        public bool EXP { get; set; } = false;
        public bool BST { get; set; } = true;
        public bool Type { get; set; } = false;

        private readonly GameVersion Game;
        private readonly int Generation;

        public SpeciesSetting(GameVersion game)
        {
            Game = game;
            Generation = Game.GetGeneration();
            Gen1 = Generation <= 1;
            Gen2 = Generation <= 2;
            Gen3 = Generation <= 3;
            Gen4 = Generation <= 4;
            Gen5 = Generation <= 5;
            Gen6 = Generation <= 6;
            Gen7 = Generation <= 7;
        }
    }
}
