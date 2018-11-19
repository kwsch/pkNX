using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public class TrainerPoke7b : TrainerPoke, IAwakened
    {
        public const int SIZE = 0x28;
        public override TrainerPoke Clone() => new TrainerPoke7b((byte[])Write().Clone());
        public TrainerPoke7b(byte[] data = null) => Data = data ?? new byte[SIZE];

        public static TrainerPoke7b[] ReadTeam(byte[] data, TrainerData _) => data.GetArray((x, offset) => new TrainerPoke7b(offset, x), SIZE);
        public static byte[] WriteTeam(IList<TrainerPoke7b> team, TrainerData _) => team.SelectMany(z => z.Write()).ToArray();

        public TrainerPoke7b(int offset, byte[] data = null)
        {
            Data = new byte[SIZE];
            if (data == null || offset + SIZE > data.Length)
                return;
            Array.Copy(data, offset, Data, 0, SIZE);
        }

        public override int Gender
        {
            get => Data[0] & 0x3;
            set => Data[0] = (byte)((Data[0] & 0xFC) | (value & 0x3));
        }

        public override int Ability
        {
            get => (Data[0] >> 4) & 0x3;
            set => Data[0] = (byte)((Data[0] & 0xCF) | ((value & 0x3) << 4));
        }

        public override int Nature { get => Data[0x01]; set => Data[0x01] = (byte)value; }

        public override int EV_HP  { get => Data[0x02]; set => Data[0x02] = (byte)value; }
        public override int EV_ATK { get => Data[0x03]; set => Data[0x03] = (byte)value; }
        public override int EV_DEF { get => Data[0x04]; set => Data[0x04] = (byte)value; }
        public override int EV_SPA { get => Data[0x05]; set => Data[0x05] = (byte)value; }
        public override int EV_SPD { get => Data[0x06]; set => Data[0x06] = (byte)value; }
        public override int EV_SPE { get => Data[0x07]; set => Data[0x07] = (byte)value; }

        public int AV_HP  { get => Data[0x08]; set => Data[0x08] = (byte)value; }
        public int AV_ATK { get => Data[0x09]; set => Data[0x09] = (byte)value; }
        public int AV_DEF { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public int AV_SPA { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }
        public int AV_SPD { get => Data[0x0C]; set => Data[0x0C] = (byte)value; }
        public int AV_SPE { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }

        public override int Friendship { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
        public override int Rank { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }

        public override uint IV32  { get => BitConverter.ToUInt32(Data, 0x10); set => BitConverter.GetBytes(value).CopyTo(Data, 0x10); }
        public override int IV_HP  { get => (int)(IV32 >> 00) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 00)) | (uint)((value > 31 ? 31 : value) << 00)); }
        public override int IV_ATK { get => (int)(IV32 >> 05) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 05)) | (uint)((value > 31 ? 31 : value) << 05)); }
        public override int IV_DEF { get => (int)(IV32 >> 10) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 10)) | (uint)((value > 31 ? 31 : value) << 10)); }
        public override int IV_SPE { get => (int)(IV32 >> 15) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 15)) | (uint)((value > 31 ? 31 : value) << 15)); }
        public override int IV_SPA { get => (int)(IV32 >> 20) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 20)) | (uint)((value > 31 ? 31 : value) << 20)); }
        public override int IV_SPD { get => (int)(IV32 >> 25) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 25)) | (uint)((value > 31 ? 31 : value) << 25)); }
        public override bool Shiny { get => ((IV32 >> 30) & 1) == 1; set => IV32 = (uint)((IV32 & ~0x40000000) | (uint)(value ? 0x40000000 : 0)); }

        public override bool CanMegaEvolve
        {
            get => ((IV32 >> 31) & 1) == 1;
            set => IV32 = (IV32 & ~(1 << 31)) | (uint)((value ? 1 : 0) << 31);
        }

        public int MegaFormChoice { get => BitConverter.ToUInt16(Data, 0x14); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x14); }
        public override int Level { get => BitConverter.ToUInt16(Data, 0x16); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x16); }
        public override int Species { get => BitConverter.ToUInt16(Data, 0x18); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x18); }
        public override int Form { get => BitConverter.ToUInt16(Data, 0x1A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1A); }
        public override int HeldItem { get => BitConverter.ToUInt16(Data, 0x1C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1C); }

        // 1E-1F unused

        public override int Move1 { get => BitConverter.ToUInt16(Data, 0x20); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x20); }
        public override int Move2 { get => BitConverter.ToUInt16(Data, 0x22); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x22); }
        public override int Move3 { get => BitConverter.ToUInt16(Data, 0x24); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x24); }
        public override int Move4 { get => BitConverter.ToUInt16(Data, 0x26); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x26); }


        public override ushort[] GetStats(PersonalInfo p)
        {
            return CalculateStatsBeluga(p);
        }

        public ushort[] CalculateStatsBeluga(PersonalInfo p)
        {
            int level = Level;
            int nature = Nature;
            int friend = Friendship; // stats +10% depending on friendship!
            int scalar = (int)(((friend / 255.0f / 10.0f) + 1.0f) * 100.0f);
            ushort[] Stats =
            {
                (ushort)(AV_HP  + GetStat(p.HP, IV_HP,  level) + 10 + level),
                (ushort)(AV_ATK + (scalar * GetStat(p.ATK, IV_ATK, level, nature, 0) / 100)),
                (ushort)(AV_DEF + (scalar * GetStat(p.DEF, IV_DEF, level, nature, 1) / 100)),
                (ushort)(AV_SPE + (scalar * GetStat(p.SPE, IV_SPE, level, nature, 4) / 100)),
                (ushort)(AV_SPA + (scalar * GetStat(p.SPA, IV_SPA, level, nature, 2) / 100)),
                (ushort)(AV_SPD + (scalar * GetStat(p.SPD, IV_SPD, level, nature, 3) / 100)),
            };
            if (Species == 292)
                Stats[0] = 1;
            return Stats;
        }

        /// <summary>
        /// Gets the initial stat value based on the base stat value, IV, and current level.
        /// </summary>
        /// <param name="baseStat"><see cref="PersonalInfo"/> stat.</param>
        /// <param name="iv">Current IV, already accounted for Hyper Training</param>
        /// <param name="level">Current Level</param>
        /// <returns>Initial Stat</returns>
        private static int GetStat(int baseStat, int iv, int level) => (iv + (2 * baseStat)) * level / 100;

        /// <summary>
        /// Gets the initial stat value with nature amplification applied. Used for all stats except HP.
        /// </summary>
        /// <param name="baseStat"><see cref="PersonalInfo"/> stat.</param>
        /// <param name="iv">Current IV, already accounted for Hyper Training</param>
        /// <param name="level">Current Level</param>
        /// <param name="nature">Current Nature</param>
        /// <param name="statIndex">Stat amp index in the nature amp table</param>
        /// <returns>Initial Stat with nature amplification applied.</returns>
        private static int GetStat(int baseStat, int iv, int level, int nature, int statIndex)
        {
            int initial = GetStat(baseStat, iv, level) + 5;
            return AmplifyStat(nature, statIndex, initial);
        }

        private static int AmplifyStat(int nature, int index, int initial)
        {
            switch (AbilityAmpTable[(5 * nature) + index])
            {
                case 1: return 110 * initial / 100; // 110%
                case -1: return 90 * initial / 100; // 90%
                default: return initial;            // 100%
            }
        }

        private static readonly sbyte[] AbilityAmpTable =
        {
            0, 0, 0, 0, 0, // Hardy
            1,-1, 0, 0, 0, // Lonely
            1, 0, 0, 0,-1, // Brave
            1, 0,-1, 0, 0, // Adamant
            1, 0, 0,-1, 0, // Naughty
           -1, 1, 0, 0, 0, // Bold
            0, 0, 0, 0, 0, // Docile
            0, 1, 0, 0,-1, // Relaxed
            0, 1,-1, 0, 0, // Impish
            0, 1, 0,-1, 0, // Lax
           -1, 0, 0, 0, 1, // Timid
            0,-1, 0, 0, 1, // Hasty
            0, 0, 0, 0, 0, // Serious
            0, 0,-1, 0, 1, // Jolly
            0, 0, 0,-1, 1, // Naive
           -1, 0, 1, 0, 0, // Modest
            0,-1, 1, 0, 0, // Mild
            0, 0, 1, 0,-1, // Quiet
            0, 0, 0, 0, 0, // Bashful
            0, 0, 1,-1, 0, // Rash
           -1, 0, 0, 1, 0, // Calm
            0,-1, 0, 1, 0, // Gentle
            0, 0, 0, 1,-1, // Sassy
            0, 0,-1, 1, 0, // Careful
            0, 0, 0, 0, 0, // Quirky
        };
    }
}