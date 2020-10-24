using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public class EncounterStatic8
    {
        public ulong BackgroundFarTypeID { get; set; }
        public ulong BackgroundNearTypeID { get; set; }
        public int EV_Spe { get; set; }
        public int EV_Atk { get; set; }
        public int EV_Def { get; set; }
        public int EV_Hp { get; set; }
        public int EV_SpAtk { get; set; }
        public int EV_SpDef { get; set; }
        public int AltForm { get; set; }
        public int DynamaxLevel { get; set; }
        public int Field_0A { get; set; }
        public ulong EncounterID { get; set; }
        public int Field_0C { get; set; }
        public bool CanGigantamax { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public Scenario EncounterScenario { get; set; }
        public Species Species { get; set; }
        public Shiny ShinyLock { get; set; }
        public Nature Nature { get; set; }
        public FixedGender Gender { get; set; }
        public int IV_Spe { get; set; }
        public int IV_Atk { get; set; }
        public int IV_Def { get; set; }
        public int IV_Hp { get; set; }
        public int IV_SpAtk { get; set; }
        public int IV_SpDef { get; set; }
        public int Ability { get; set; }
        public int Move0 { get; set; }
        public int Move1 { get; set; }
        public int Move2 { get; set; }
        public int Move3 { get; set; }

        [JsonIgnore]
        public int[] IVs
        {
            get => new[] { IV_Hp, IV_Atk, IV_Def, IV_Spe, IV_SpAtk, IV_SpDef };
            set
            {
                if (value?.Length != 6) return;
                IV_Hp =    value[0];
                IV_Atk =   value[1];
                IV_Def =   value[2];
                IV_Spe =   value[3];
                IV_SpAtk = value[4];
                IV_SpDef = value[5];
            }
        }

        [JsonIgnore]
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

        // scenarios that are set for specific story encounters, most don't work on encounters that are not meant to have them
        public enum Scenario
        {
            None = 0,
            Zacian_Zamazenta_1 = 1,
            _2 = 2,
            _3 = 3,
            Eternatus_1 = 4,
            Eternatus_2 = 5,
            Eternatus_3 = 6,
            Zacian_Zamazenta_2 = 7,
            Motostoke_Gym_Challenge = 8,
            Max_Raid_Battle_1 = 9,
            Max_Raid_Battle_2 = 10,
            Max_Raid_Battle_3 = 11,
            Max_Raid_Battle_4 = 12,
            Zacian_Zamazenta_3 = 13,
            Fast_Slowpoke = 14,
            Regigigas_Raid_Battle = 15,
            Special_Raid_Battle = 16,
            Calyrex = 17,
            Glastrier_Spectrier = 18,
            Calyrex_Fusion = 19,
        }

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[(int)Species]}{(AltForm == 0 ? string.Empty : "-" + AltForm)}";
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

            var gender = Gender == FixedGender.Random ? string.Empty : $", Gender = {(int)Gender - 1}";
            var nature = Nature == Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = AltForm == 0 ? string.Empty : $", Form = {AltForm:00}";
            var moves = Move0 == 0 ? string.Empty : $", Moves = new[] {{{Move0:000},{Move1:000},{Move2:000},{Move3:000}}}";
            var shiny = ShinyLock == Shiny.Random ? string.Empty : $", Shiny = {ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : $", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";

            return
                $"            new EncounterStatic8 {{ Species = {(int)Species:000}, Level = {Level:00}, Location = -01{moves}{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna} }},{comment}";
        }
    }
}