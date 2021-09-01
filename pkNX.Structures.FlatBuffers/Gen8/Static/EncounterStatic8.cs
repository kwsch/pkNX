using System.Collections.Generic;
using System.Linq;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable]
    public class EncounterStatic8
    {
        [FlatBufferItem(0)] public ulong BackgroundFarTypeID { get; set; }
        [FlatBufferItem(1)] public ulong BackgroundNearTypeID { get; set; }
        [FlatBufferItem(2)] public byte EV_Spe { get; set; }
        [FlatBufferItem(3)] public byte EV_Atk { get; set; }
        [FlatBufferItem(4)] public byte EV_Def { get; set; }
        [FlatBufferItem(5)] public byte EV_Hp { get; set; }
        [FlatBufferItem(6)] public byte EV_SpAtk { get; set; }
        [FlatBufferItem(7)] public byte EV_SpDef { get; set; }
        [FlatBufferItem(8)] public byte AltForm { get; set; }
        [FlatBufferItem(9)] public byte DynamaxLevel { get; set; }
        [FlatBufferItem(10)] public int Field_0A { get; set; }
        [FlatBufferItem(11)] public ulong EncounterID { get; set; }
        [FlatBufferItem(12)] public byte Field_0C { get; set; }
        [FlatBufferItem(13)] public bool CanGigantamax { get; set; }
        [FlatBufferItem(14)] public int HeldItem { get; set; }
        [FlatBufferItem(15)] public int Level { get; set; }
        [FlatBufferItem(16)] public Scenario EncounterScenario { get; set; }
        [FlatBufferItem(17)] public int Species { get; set; }
        [FlatBufferItem(18)] public uint ShinyLock { get; set; }
        [FlatBufferItem(19)] public uint Nature { get; set; }
        [FlatBufferItem(20)] public byte Gender { get; set; }
        [FlatBufferItem(21)] public sbyte IV_Spe { get; set; }
        [FlatBufferItem(22)] public sbyte IV_Atk { get; set; }
        [FlatBufferItem(23)] public sbyte IV_Def { get; set; }
        [FlatBufferItem(24)] public sbyte IV_Hp { get; set; }
        [FlatBufferItem(25)] public sbyte IV_SpAtk { get; set; }
        [FlatBufferItem(26)] public sbyte IV_SpDef { get; set; }
        [FlatBufferItem(27)] public int Ability { get; set; }
        [FlatBufferItem(28)] public int Move0 { get; set; }
        [FlatBufferItem(29)] public int Move1 { get; set; }
        [FlatBufferItem(30)] public int Move2 { get; set; }
        [FlatBufferItem(31)] public int Move3 { get; set; }

        public int[] IVs
        {
            get => new int[] { IV_Hp, IV_Atk, IV_Def, IV_Spe, IV_SpAtk, IV_SpDef };
            set
            {
                if (value?.Length != 6) return;
                IV_Hp =    (sbyte)value[0];
                IV_Atk =   (sbyte)value[1];
                IV_Def =   (sbyte)value[2];
                IV_Spe =   (sbyte)value[3];
                IV_SpAtk = (sbyte)value[4];
                IV_SpDef = (sbyte)value[5];
            }
        }

        public int[] Moves
        {
            get => new[] { Move0, Move1, Move2, Move3 };
            set
            {
                if (value?.Length != 4) return;
                Move0 = value[0];
                Move1 = value[1];
                Move2 = value[2];
                Move3 = value[3];
            }
        }

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
        [FlatBufferEnum(typeof(byte))]
        public enum FixedGender
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
        {
            Random = 0,
            Male = 1,
            Female = 2,
            Genderless = Random,
        }

        // scenarios that are set for specific story encounters, most don't work on encounters that are not meant to have them
        [FlatBufferEnum(typeof(int))]
        public enum Scenario
        {
            None,
            Legendary_Pokemon,
            _2,
            _3,
            Eternatus,
            Eternamax_Eternatus_1,
            Eternamax_Eternatus_2,
            Zacian_Zamazenta_Fog,
            Motostoke_Gym_Challenge,
            Max_Raid_Battle_1,
            Max_Raid_Battle_2,
            Max_Raid_Battle_3,
            Max_Raid_Battle_4,
            Zacian_Zamazenta_Boss,
            Fast_Slowpoke,
            Regigigas_Raid_Battle,
            Special_Raid_Battle,
            Calyrex,
            Glastrier_Spectrier,
            Calyrex_Fusion,
        }

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
            var ability = Ability switch
            {
                0 => string.Empty,
                3 => ", Ability = 4",
                _ => $", Ability = {Ability}",
            };

            var ivs = IVs[0] switch
            {
                31 when IVs.All(z => z == 31) => ", FlawlessIVCount = 6",
                -1 when IVs.All(z => z == -1) => string.Empty,
                -4 => ", FlawlessIVCount = 3",
                _ => $", IVs = new[]{{{string.Join(",", IVs)}}}",
            };

            var gender = Gender == (int)FixedGender.Random ? string.Empty : $", Gender = {Gender - 1}";
            var nature = (Nature)Nature == Structures.Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm:00}";
            var moves = Move0 == 0 ? string.Empty : $", Moves = new[] {{{Move0:000},{Move1:000},{Move2:000},{Move3:000}}}";
            var shiny = (Shiny)ShinyLock == Shiny.Random ? string.Empty : $", Shiny = {ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";

            return
                $"            new(SWSH) {{ Species = {Species:000}, Level = {Level:00}, Location = -01{moves}{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna} }},{comment}";
        }
    }
}
