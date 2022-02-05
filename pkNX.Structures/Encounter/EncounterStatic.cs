using System;
using System.Linq;

namespace pkNX.Structures
{
    public abstract class EncounterStatic
    {
        protected readonly byte[] Data;
        protected EncounterStatic(byte[] data) => Data = data;
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

        public virtual int IV_HP { get; set; } = -1;
        public virtual int IV_ATK { get; set; } = -1;
        public virtual int IV_DEF { get; set; } = -1;
        public virtual int IV_SPE { get; set; } = -1;
        public virtual int IV_SPA { get; set; } = -1;
        public virtual int IV_SPD { get; set; } = -1;
        public virtual int EV_HP { get; set; }
        public virtual int EV_ATK { get; set; }
        public virtual int EV_DEF { get; set; }
        public virtual int EV_SPE { get; set; }
        public virtual int EV_SPA { get; set; }
        public virtual int EV_SPD { get; set; }

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

        public string GetSummary()
        {
            var str = $"new EncounterStatic {{ Species = {Species:000}, Level = {Level:00}, Location = -01, ";
            if (Ability != 0)
                str += $"Ability = {1 << (Ability - 1)}, ";
            if (ShinyLock)
                str += "Shiny = false, ";

            if (IV3)
            {
                str += "IV3 = true, ";
            }
            else if (IVs.Any(z => z >= 0))
            {
                var iv = IVs.Select(z => z >= 0 ? $"{z:00}" : "-1");
                str += $"IVs = new[] {{{string.Join(",", iv)}}}, ";
            }
            if (RelearnMoves.Any(z => z != 0))
            {
                var mv = RelearnMoves.Select(z => $"{z:000}");
                str += $"Relearn = new[] {{{string.Join(",", mv)}}}, ";
            }
            if (Form != 0)
                str += $"Form = {Form}, ";
            if (Gender != 0)
                str += $"Gender = {Gender - 1}, ";
            if (HeldItem > 0)
                str += $"HeldItem = {HeldItem}, ";
            if (Nature is >= 0 and < Nature.Random25)
                str += $"Nature = {Nature - 1}, ";

            str += " },";
            return str;
        }
    }
}
