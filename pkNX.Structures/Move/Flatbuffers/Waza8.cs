using System;
// ReSharper disable UnusedMember.Local

namespace pkNX.Structures
{
    public class Waza8
    {
        public uint Version { get; set; }
        public uint MoveID { get; set; }
        public bool CanUseMove { get; set; }
        public byte Type { get; set; }
        public byte Quality { get; set; }
        public byte Category { get; set; }
        public byte Power { get; set; }
        public byte Accuracy { get; set; }
        public byte PP { get; set; }
        public byte Priority { get; set; }
        public byte HitMin { get; set; }
        public byte HitMax { get; set; }
        public ushort Inflict { get; set; }
        public byte InflictPercent { get; set; }
        public byte RawInflictCount { get; set; }
        public byte TurnMin { get; set; }
        public byte TurnMax { get; set; }
        public byte CritStage { get; set; }
        public byte Flinch { get; set; }
        public ushort EffectSequence { get; set; }
        public byte Recoil { get; set; }
        public byte RawHealing { get; set; }
        public byte RawTarget { get; set; }
        public byte Stat1 { get; set; }
        public byte Stat2 { get; set; }
        public byte Stat3 { get; set; }
        public byte Stat1Stage { get; set; }
        public byte Stat2Stage { get; set; }
        public byte Stat3Stage { get; set; }
        public byte Stat1Percent { get; set; }
        public byte Stat2Percent { get; set; }
        public byte Stat3Percent { get; set; }
        public byte GigantamaxPower { get; set; }
        public bool Flag_MakesContact { get; set; }
        public bool Flag_Charge { get; set; }
        public bool Flag_Recharge { get; set; }
        public bool Flag_Protect { get; set; }
        public bool Flag_Reflectable { get; set; }
        public bool Flag_Snatch { get; set; }
        public bool Flag_Mirror { get; set; }
        public bool Flag_Punch { get; set; }
        public bool Flag_Sound { get; set; }
        public bool Flag_Gravity { get; set; }
        public bool Flag_Defrost { get; set; }
        public bool Flag_DistanceTriple { get; set; }
        public bool Flag_Heal { get; set; }
        public bool Flag_IgnoreSubstitute { get; set; }
        public bool Flag_FailSkyBattle { get; set; }
        public bool Flag_AnimateAlly { get; set; }
        public bool Flag_Dance { get; set; }
        public bool Flag_Metronome { get; set; }

        public MoveInflictDuration InflictCount
        {
            get => (MoveInflictDuration)RawInflictCount;
            set => RawInflictCount = (byte)value;
        }

        public Heal Healing
        {
            get => (Heal)RawHealing;
            set => RawHealing = (byte)value;
        }

        public MoveTarget Target
        {
            get => (MoveTarget)RawTarget;
            set => RawTarget = (byte)value;
        }
    }

    /// <summary>
    /// Manual Flatbuffer reader for <seealso cref="Waza8"/>
    /// </summary>
#pragma warning disable IDE0051, RCS1213 // Remove unused member declaration.
    public static class Waza8Reader
    {
        private const int Version = 4;
        private const int MoveID = 6;
        private const int CanUseMove = 8;
        private const int Type = 10;
        private const int Quality = 12;
        private const int Category = 14;
        private const int Power = 16;
        private const int Accuracy = 18;
        private const int PP = 20;
        private const int Priority = 22;
        private const int HitMin = 24;
        private const int HitMax = 26;
        private const int Inflict = 28;
        private const int InflictPercent = 30;
        private const int RawInflictCount = 32;
        private const int TurnMin = 34;
        private const int TurnMax = 36;
        private const int CritStage = 38;
        private const int Flinch = 40;
        private const int EffectSequence = 42;
        private const int Recoil = 44;
        private const int RawHealing = 46;
        private const int RawTarget = 48;
        private const int Stat1 = 50;
        private const int Stat2 = 52;
        private const int Stat3 = 54;
        private const int Stat1Stage = 56;
        private const int Stat2Stage = 58;
        private const int Stat3Stage = 60;
        private const int Stat1Percent = 62;
        private const int Stat2Percent = 64;
        private const int Stat3Percent = 66;
        private const int GigantamaxPower = 68;
        private const int FlagMakesContact = 70;
        private const int FlagCharge = 72;
        private const int FlagRecharge = 74;
        private const int FlagProtect = 76;
        private const int FlagReflectable = 78;
        private const int FlagSnatch = 80;
        private const int FlagMirror = 82;
        private const int FlagPunch = 84;
        private const int FlagSound = 86;
        private const int FlagGravity = 88;
        private const int FlagDefrost = 90;
        private const int FlagDistanceTriple = 92;
        private const int FlagHeal = 94;
        private const int FlagIgnoreSubstitute = 96;
        private const int FlagFailSkyBattle = 98;
        private const int FlagAnimateAlly = 100;
        private const int FlagDance = 102;
        private const int FlagMetronome = 104;
#pragma warning restore IDE0051, RCS1213 // Remove unused member declaration.

        public static Move8Fake ReadPlaceholder(byte[] data)
        {
            var result = new Move8Fake();
            var root = BitConverter.ToInt32(data, 0);
            var vtable = BitConverter.ToInt32(data, root);
            var vOfs = root - vtable;
            var vLength = BitConverter.ToUInt16(data, vOfs);

            result.Version = ReadU16(vOfs, Version); // u32 meh
            result.MoveID = ReadU16(vOfs, MoveID); // u32 meh
            result.CanUseMove = ReadB8(vOfs, CanUseMove);
            result.Type = ReadU8(vOfs, Type);
            result.Quality = ReadU8(vOfs, Quality);
            result.Category = ReadU8(vOfs, Category);
            result.Power = ReadU8(vOfs, Power);

            // that's enough for reading, gets us all the properties we need for randomization
            return result;

            byte ReadU8(int v, int o)
            {
                var vptr = v + o;
                if (vptr > vLength)
                    return default;
                var rofs = BitConverter.ToUInt16(data, vptr);
                if (rofs == 0)
                    return default;
                return data[root + rofs];
            }

            ushort ReadU16(int v, int o)
            {
                var vptr = v + o;
                if (vptr > vLength)
                    return default;
                var rofs = BitConverter.ToUInt16(data, vptr);
                if (rofs == 0)
                    return default;
                return BitConverter.ToUInt16(data, root + rofs);
            }

            bool ReadB8(int v, int o)
            {
                var vptr = v + o;
                if (vptr > vLength)
                    return default;
                var rofs = BitConverter.ToUInt16(data, vptr);
                if (rofs == 0)
                    return default;
                return data[root + rofs] == 1;
            }
        }
    }
}
