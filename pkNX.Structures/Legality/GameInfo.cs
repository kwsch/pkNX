using System;

namespace pkNX.Structures
{
    /// <summary>
    /// Misc information pertaining to the game.
    /// </summary>
    public class GameInfo
    {
        public readonly GameVersion Game;
        public readonly int Generation;

        public int MaxSpeciesID { get; private set; }
        public int MaxItemID { get; private set; }
        public int MaxMoveID { get; private set; }
        public ushort[] HeldItems { get; private set; }
        public int MaxAbilityID { get; private set; }

        public bool XY { get; private set; }
        public bool AO { get; private set; }
        public bool SM { get; private set; }
        public bool USUM { get; private set; }
        public bool GG { get; private set; }

        public GameInfo(GameVersion game)
        {
            Game = game;
            Generation = game.GetGeneration();
            GetInitMethod(game)();
        }

        private Action GetInitMethod(GameVersion game)
        {
            return game switch
            {
                GameVersion.XY => (Action) LoadXY,
                GameVersion.ORASDEMO => LoadAO,
                GameVersion.ORAS => LoadAO,
                GameVersion.SMDEMO => LoadSM,
                GameVersion.SM => LoadSM,
                GameVersion.USUM => LoadUSUM,
                GameVersion.GG => LoadGG,
                _ => throw new ArgumentException(nameof(game))
            };
        }

        private void LoadXY()
        {
            XY = true;
            MaxSpeciesID = Legal.MaxSpeciesID_6;
            MaxMoveID = Legal.MaxMoveID_6_XY;
            MaxItemID = Legal.MaxItemID_6_XY;
            HeldItems = Legal.HeldItem_XY;
            MaxAbilityID = Legal.MaxAbilityID_6_XY;
        }

        private void LoadAO()
        {
            AO = true;
            MaxSpeciesID = Legal.MaxSpeciesID_6;
            MaxMoveID = Legal.MaxMoveID_6_AO;
            MaxItemID = Legal.MaxItemID_6_AO;
            HeldItems = Legal.HeldItem_AO;
            MaxAbilityID = Legal.MaxAbilityID_6_AO;
        }

        private void LoadSM()
        {
            SM = true;
            MaxSpeciesID = Legal.MaxSpeciesID_7_SM;
            MaxMoveID = Legal.MaxMoveID_7_SM;
            MaxItemID = Legal.MaxItemID_7_SM;
            HeldItems = Legal.HeldItems_SM;
            MaxAbilityID = Legal.MaxAbilityID_7_SM;
        }

        private void LoadUSUM()
        {
            USUM = true;
            MaxSpeciesID = Legal.MaxSpeciesID_7_USUM;
            MaxMoveID = Legal.MaxMoveID_7_USUM;
            MaxItemID = Legal.MaxItemID_7_USUM;
            HeldItems = Legal.HeldItems_USUM;
            MaxAbilityID = Legal.MaxAbilityID_7_USUM;
        }

        private void LoadGG()
        {
            GG = true;
            MaxSpeciesID = Legal.MaxSpeciesID_7_GG;
            MaxMoveID = Legal.MaxMoveID_7_GG;
            MaxItemID = Legal.MaxItemID_7_GG;
            HeldItems = new ushort[1];
            MaxAbilityID = Legal.MaxAbilityID_7_GG;
        }
    }
}
