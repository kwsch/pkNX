using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public class EncounterGift8
    {
        public int IsEgg { get; set; }
        public int AltForm { get; set; }
        public int DynamaxLevel { get; set; }
        public int BallItemID { get; set; }
        public int Field_04 { get; set; }
        public ulong Hash1 { get; set; }
        public bool CanGigantamax { get; set; }
        public int HeldItem { get; set; }
        public int Level { get; set; }
        public Species Species { get; set; }
        public int Field_0A { get; set; }
        public int MemoryCode { get; set; }
        public int MemoryData { get; set; }
        public int MemoryFeel { get; set; }
        public int MemoryLevel { get; set; }
        public ulong OtNameID { get; set; }
        public int OtGender { get; set; }
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
        public int SpecialMove { get; set; }

        [JsonIgnore]
        internal static readonly int[] BallToItem = {
            000, // None
            001, // Master
            002, // Ultra
            003, // Great
            004, // Poke
            005, // Safari
            006, // Net
            007, // Dive
            008, // Nest
            009, // Repeat
            010, // Timer
            011, // Luxury
            012, // Premier
            013, // Dusk
            014, // Heal
            015, // Quick
            016, // Cherish
            492, // Fast
            493, // Level
            494, // Lure
            495, // Heavy
            496, // Love
            497, // Friend
            498, // Moon
            499, // Sport
            576, // Dream
            851, // Beast
        };

        [JsonIgnore]
        public Ball Ball
        {
            get => (Ball)Array.IndexOf(BallToItem, BallItemID);
            set => BallItemID = BallToItem[(int)value];
        }

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
            var shiny = ShinyLock == Shiny.Random ? string.Empty : $", Shiny = {ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";
            var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {(int)Ball}";

            return
                $"            new EncounterStatic8 {{ Gift = true, Species = {(int)Species:000}, Level = {Level:00}, Location = -01{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna}{ball} }},{comment}";
        }
    }
}