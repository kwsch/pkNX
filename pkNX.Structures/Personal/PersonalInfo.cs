﻿using System;

namespace pkNX.Structures
{
    /// <summary>
    /// Stat/misc data for individual species or their associated alternate forme data.
    /// </summary>
    public abstract class PersonalInfo
    {
        public byte[] Data;
        public abstract byte[] Write();
        public abstract int HP { get; set; }
        public abstract int ATK { get; set; }
        public abstract int DEF { get; set; }
        public abstract int SPE { get; set; }
        public abstract int SPA { get; set; }
        public abstract int SPD { get; set; }

        public int[] Stats
        {
            get => new[] { HP, ATK, DEF, SPE, SPA, SPD };
            set
            {
                HP = value[0];
                ATK = value[1];
                DEF = value[2];
                SPE = value[3];
                SPA = value[4];
                SPD = value[5];
            }
        }

        public abstract int EV_HP { get; set; }
        public abstract int EV_ATK { get; set; }
        public abstract int EV_DEF { get; set; }
        public abstract int EV_SPE { get; set; }
        public abstract int EV_SPA { get; set; }
        public abstract int EV_SPD { get; set; }
        public abstract Types Type1 { get; set; }
        public abstract Types Type2 { get; set; }
        public abstract int EggGroup1 { get; set; }
        public abstract int EggGroup2 { get; set; }
        public abstract int CatchRate { get; set; }
        public virtual int EvoStage { get; set; }
        public abstract int[] Items { get; set; }
        public abstract int Gender { get; set; }
        public abstract int HatchCycles { get; set; }
        public abstract int BaseFriendship { get; set; }
        public abstract int EXPGrowth { get; set; }
        public abstract int[] Abilities { get; set; }
        public abstract int EscapeRate { get; set; }
        public virtual int FormeCount { get; set; } = 1;
        protected internal virtual int FormStatsIndex { get; set; }
        public virtual int FormeSprite { get; set; }
        public abstract int BaseEXP { get; set; }
        public abstract int Color { get; set; }

        public virtual int Height { get; set; } = 0;
        public virtual int Weight { get; set; } = 0;

        public int[] Types
        {
            get => new[] { (int)Type1, (int)Type2 };
            set
            {
                if (value?.Length != 2) return;
                Type1 = (Types)value[0];
                Type2 = (Types)value[1];
            }
        }

        public int[] EggGroups
        {
            get => new[] { EggGroup1, EggGroup2 };
            set
            {
                if (value?.Length != 2) return;
                EggGroup1 = (byte)value[0];
                EggGroup2 = (byte)value[1];
            }
        }

        /// <summary>
        /// TM/HM learn compatibility flags for individual moves.
        /// </summary>
        public bool[] TMHM { get; set; }

        /// <summary>
        /// Grass-Fire-Water-Etc typed learn compatibility flags for individual moves.
        /// </summary>
        public bool[] TypeTutors { get; set; }

        /// <summary>
        /// Special tutor learn compatibility flags for individual moves.
        /// </summary>
        public bool[][] SpecialTutors { get; set; } = Array.Empty<bool[]>();

        protected static bool[] GetBits(byte[] data, int start = 0, int length = -1)
        {
            if (length < 0)
                length = data.Length;
            bool[] r = new bool[length << 3];
            for (int i = 0; i < r.Length; i++)
                r[i] = (data[start + (i >> 3)] >> (i & 7) & 0x1) == 1;
            return r;
        }

        protected static byte[] SetBits(bool[] bits)
        {
            byte[] data = new byte[bits.Length >> 3];
            for (int i = 0; i < bits.Length; i++)
                data[i >> 3] |= (byte)(bits[i] ? 1 << (i & 0x7) : 0);
            return data;
        }

        /// <summary>
        /// Injects supplementary TM/HM compatibility which is not present in the generation specific <see cref="PersonalInfo"/> format.
        /// </summary>
        /// <param name="data">Data to read from</param>
        /// <param name="start">Starting offset to read at</param>
        /// <param name="length">Amount of bytes to decompose into bits</param>
        internal void AddTMHM(byte[] data, int start = 0, int length = -1) => TMHM = GetBits(data, start, length);

        /// <summary>
        /// Injects supplementary Type Tutor compatibility which is not present in the generation specific <see cref="PersonalInfo"/> format.
        /// </summary>
        /// <param name="data">Data to read from</param>
        /// <param name="start">Starting offset to read at</param>
        /// <param name="length">Amount of bytes to decompose into bits</param>
        internal void AddTypeTutors(byte[] data, int start = 0, int length = -1) => TypeTutors = GetBits(data, start, length);

        /// <summary>
        /// Gets the <see cref="PersonalTable"/> <see cref="FormStatsIndex"/> entry index for the input criteria, with fallback for the original species entry.
        /// </summary>
        /// <param name="species">Species to retrieve for</param>
        /// <param name="forme">Alternate Forme to retrieve for</param>
        /// <returns>Index the Alternate Forme exists as in the <see cref="PersonalTable"/>.</returns>
        public int FormeIndex(int species, int forme)
        {
            if (forme <= 0) // no forme requested
                return species;
            if (FormStatsIndex <= 0) // no formes present
                return species;
            if (forme >= FormeCount) // beyond range of species' formes
                return species;

            return FormStatsIndex + forme - 1;
        }

        /// <summary>
        /// Gets a random valid gender for the entry.
        /// </summary>
        public int RandomGender()
        {
            var fix = FixedGender;
            return fix >= 0 ? fix : Util.Rand.Next(2);
        }

        public bool IsDualGender => FixedGender < 0;

        public int FixedGender
        {
            get
            {
                if (Genderless)
                    return 2;
                if (OnlyFemale)
                    return 1;
                if (OnlyMale)
                    return 0;
                return -1;
            }
        }

        public bool Genderless => Gender == 255;
        public bool OnlyFemale => Gender == 254;
        public bool OnlyMale => Gender == 0;
        public bool HasFormes => FormeCount > 1;
        public int BST => HP + ATK + DEF + SPE + SPA + SPD;

        public bool IsFormeWithinRange(int forme)
        {
            if (forme == 0)
                return true;
            return forme < FormeCount;
        }

        public bool IsValidTypeCombination(Types type1, Types type2) => Type1 == type1 && Type2 == type2;
        public bool IsType(Types type1) => Type1 == type1 || Type2 == type1;
        public bool IsType(Types type1, Types type2) => (Type1 == type1 || Type2 == type1) && (Type1 == type2 || Type2 == type2);
        public bool IsEggGroup(int group) => EggGroup1 == group || EggGroup2 == group;
    }
}
