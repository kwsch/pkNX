namespace pkNX.Structures
{
    public abstract class TrainerPoke
    {
        protected byte[] Data;

        public abstract int Species { get; set; }
        public abstract int Form { get; set; }
        public abstract int Level { get; set; }
        public abstract int HeldItem { get; set; }
        public abstract int Nature { get; set; }
        public abstract int Gender { get; set; }
        public abstract int Ability { get; set; }
        public abstract int Friendship { get; set; }

        public abstract bool Shiny { get; set; }
        public abstract bool CanMegaEvolve { get; set; }

        public abstract int Move1 { get; set; }
        public abstract int Move2 { get; set; }
        public abstract int Move3 { get; set; }
        public abstract int Move4 { get; set; }

        public abstract uint IV32 { get; set; }
        public abstract int IV_HP { get; set; }
        public abstract int IV_ATK { get; set; }
        public abstract int IV_DEF { get; set; }
        public abstract int IV_SPE { get; set; }
        public abstract int IV_SPA { get; set; }
        public abstract int IV_SPD { get; set; }

        public abstract int EV_HP { get; set; }
        public abstract int EV_ATK { get; set; }
        public abstract int EV_DEF { get; set; }
        public abstract int EV_SPA { get; set; }
        public abstract int EV_SPD { get; set; }
        public abstract int EV_SPE { get; set; }

        public abstract int AV_HP { get; set; }
        public abstract int AV_ATK { get; set; }
        public abstract int AV_DEF { get; set; }
        public abstract int AV_SPA { get; set; }
        public abstract int AV_SPD { get; set; }
        public abstract int AV_SPE { get; set; }

        public abstract int Rank { get; set; }

        public byte[] Write() => (byte[])Data.Clone();
        public abstract TrainerPoke Clone();

        #region Derived

        public int[] Moves
        {
            get => new[] { Move1, Move2, Move3, Move4 };
            set { if (value?.Length != 4) return; Move1 = value[0]; Move2 = value[1]; Move3 = value[2]; Move4 = value[3]; }
        }

        public int[] IVs
        {
            get => new[] { IV_HP, IV_ATK, IV_DEF, IV_SPE, IV_SPA, IV_SPD };
            set
            {
                if (value?.Length != 6) return;
                IV_HP = value[0]; IV_ATK = value[1]; IV_DEF = value[2];
                IV_SPE = value[3]; IV_SPA = value[4]; IV_SPD = value[5];
            }
        }

        public int[] EVs
        {
            get => new[] { EV_HP, EV_ATK, EV_DEF, EV_SPE, EV_SPA, EV_SPD };
            set
            {
                if (value?.Length != 6) return;
                EV_HP = value[0]; EV_ATK = value[1]; EV_DEF = value[2];
                EV_SPE = value[3]; EV_SPA = value[4]; EV_SPD = value[5];
            }
        }

        public int[] AVs
        {
            get => new[] { AV_HP, AV_ATK, AV_DEF, AV_SPE, AV_SPA, AV_SPD };
            set
            {
                if (value?.Length != 6) return;
                AV_HP = value[0]; AV_ATK = value[1]; AV_DEF = value[2];
                AV_SPE = value[3]; AV_SPA = value[4]; AV_SPD = value[5];
            }
        }

        #endregion
    }
}
