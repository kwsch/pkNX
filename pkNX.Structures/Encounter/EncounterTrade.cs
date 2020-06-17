using System;

namespace pkNX.Structures
{
    public abstract class EncounterTrade
    {
        protected readonly byte[] Data;

        protected EncounterTrade(byte[] data) => Data = data;

        public virtual byte[] Write() => (byte[])Data.Clone();

        public abstract Species Species { get; set; }
        public virtual int HeldItem { get; set; }
        public abstract int Level { get; set; }
        public abstract int Form { get; set; }
        public abstract FixedGender Gender { get; set; }

        public virtual Nature Nature { get; set; } = Nature.Random;
        public virtual int Ability { get; set; }
        public virtual bool ShinyLock { get; set; }

        public virtual bool IV3 { get; set; }
        public virtual int[] RelearnMoves { get; set; } = Array.Empty<int>();
        public abstract Shiny Shiny { get; set; }

        public abstract int IV_HP { get; set; }
        public abstract int IV_ATK { get; set; }
        public abstract int IV_DEF { get; set; }
        public abstract int IV_SPE { get; set; }
        public abstract int IV_SPA { get; set; }
        public abstract int IV_SPD { get; set; }

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
    }
}
