using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterGift8
    {
        [FlatBufferItem(00)] public int IsEgg { get; set; }
        [FlatBufferItem(01)] public byte Form { get; set; }
        [FlatBufferItem(02)] public byte DynamaxLevel { get; set; }
        [FlatBufferItem(03)] public int BallItemID { get; set; }
        [FlatBufferItem(04)] public byte Field_04 { get; set; }
        [FlatBufferItem(05)] public ulong Hash1 { get; set; }
        [FlatBufferItem(06)] public bool CanGigantamax { get; set; }
        [FlatBufferItem(07)] public int HeldItem { get; set; }
        [FlatBufferItem(08)] public byte Level { get; set; }
        [FlatBufferItem(09)] public int Species { get; set; }
        [FlatBufferItem(10)] public byte Field_0A { get; set; }
        [FlatBufferItem(11)] public byte MemoryCode { get; set; }
        [FlatBufferItem(12)] public ushort MemoryData { get; set; }
        [FlatBufferItem(13)] public byte MemoryFeel { get; set; }
        [FlatBufferItem(14)] public byte MemoryLevel { get; set; }
        [FlatBufferItem(15)] public ulong OtNameID { get; set; }
        [FlatBufferItem(16)] public int OtGender { get; set; }
        [FlatBufferItem(17)] public int ShinyLock { get; set; }
        [FlatBufferItem(18)] public int Nature { get; set; }
        [FlatBufferItem(19)] public byte Gender { get; set; }
        [FlatBufferItem(20)] public sbyte IV_SPE { get; set; }
        [FlatBufferItem(21)] public sbyte IV_ATK { get; set; }
        [FlatBufferItem(22)] public sbyte IV_DEF { get; set; }
        [FlatBufferItem(23)] public sbyte IV_HP { get; set; }
        [FlatBufferItem(24)] public sbyte IV_SPA { get; set; }
        [FlatBufferItem(25)] public sbyte IV_SPD { get; set; }
        [FlatBufferItem(26)] public int Ability { get; set; }
        [FlatBufferItem(27)] public int SpecialMove { get; set; }

        public Species SpeciesID => (Species)Species;

        public static readonly int[] BallToItem =
        {
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

        public Ball Ball
        {
            get => (Ball)Array.IndexOf(BallToItem, BallItemID);
            set => BallItemID = BallToItem[(int)value];
        }

        public int[] IVs
        {
            get => new int[] { IV_HP, IV_ATK, IV_DEF, IV_SPE, IV_SPA, IV_SPD };
            set
            {
                if (value?.Length != 6) return;
                IV_HP =    (sbyte)value[0];
                IV_ATK =   (sbyte)value[1];
                IV_DEF =   (sbyte)value[2];
                IV_SPE =   (sbyte)value[3];
                IV_SPA = (sbyte)value[4];
                IV_SPD = (sbyte)value[5];
            }
        }

        public string GetSummary(IReadOnlyList<string> species)
        {
            var comment = $" // {species[Species]}{(Form == 0 ? string.Empty : "-" + Form)}";
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

            var gender = (FixedGender)Gender == FixedGender.Random ? string.Empty : $", Gender = {Gender - 1}";
            var nature = Nature == (int)Structures.Nature.Random25 ? string.Empty : $", Nature = Nature.{(Nature)Nature}";
            var altform = Form == 0 ? string.Empty : $", Form = {Form:00}";
            var shiny = (Shiny)ShinyLock == Shiny.Random ? string.Empty : $", Shiny = {(Shiny)ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var dyna = DynamaxLevel == 0 ? string.Empty : $", DynamaxLevel = {DynamaxLevel}";
            var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {(int)Ball}";

            return
                $"            new(SWSH) {{ Gift = true, Species = {Species:000}, Level = {Level:00}, Location = -01{ivs}{shiny}{gender}{ability}{nature}{altform}{giga}{dyna}{ball} }},{comment}";
        }
    }
}
