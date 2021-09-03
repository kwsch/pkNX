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
    public class EncounterTrade8
    {
        [FlatBufferItem(00)] public byte Form { get; set; }
        [FlatBufferItem(01)] public byte DynamaxLevel { get; set; }
        [FlatBufferItem(02)] public int BallItemID { get; set; }
        [FlatBufferItem(03)] public int Field_03 { get; set; }
        [FlatBufferItem(04)] public ulong Hash0 { get; set; }
        [FlatBufferItem(05)] public bool CanGigantamax { get; set; }
        [FlatBufferItem(06)] public int HeldItem { get; set; }
        [FlatBufferItem(07)] public byte Level { get; set; }
        [FlatBufferItem(08)] public int Species { get; set; }
        [FlatBufferItem(09)] public ulong Hash1 { get; set; }
        [FlatBufferItem(10)] public int TrainerID { get; set; }
        [FlatBufferItem(11)] public byte Memory { get; set; }
        [FlatBufferItem(12)] public ushort TextVar { get; set; }
        [FlatBufferItem(13)] public byte Feeling { get; set; }
        [FlatBufferItem(14)] public byte Intensity { get; set; }
        [FlatBufferItem(15)] public ulong Hash2 { get; set; }
        [FlatBufferItem(16)] public byte OTGender { get; set; }
        [FlatBufferItem(17)] public byte RequiredForm { get; set; }
        [FlatBufferItem(18)] public int RequiredSpecies { get; set; }
        [FlatBufferItem(19)] public int RequiredNature { get; set; }
        [FlatBufferItem(20)] public byte UnknownRequirement { get; set; } // all 0; we know this field is a trade requirement, but unsure what exactly
        [FlatBufferItem(21)] public int ShinyLock { get; set; }
        [FlatBufferItem(22)] public int Nature { get; set; }
        [FlatBufferItem(23)] public byte Gender { get; set; }
        [FlatBufferItem(24)] public sbyte IV_SPE { get; set; }
        [FlatBufferItem(25)] public sbyte IV_ATK { get; set; }
        [FlatBufferItem(26)] public sbyte IV_DEF { get; set; }
        [FlatBufferItem(27)] public sbyte IV_HP { get; set; }
        [FlatBufferItem(28)] public sbyte IV_SPA { get; set; }
        [FlatBufferItem(29)] public sbyte IV_SPD { get; set; }
        [FlatBufferItem(30)] public byte AbilityNumber { get; set; }
        [FlatBufferItem(31)] public ushort Relearn1 { get; set; }
        [FlatBufferItem(32)] public ushort Relearn2 { get; set; }
        [FlatBufferItem(33)] public ushort Relearn3 { get; set; }
        [FlatBufferItem(34)] public ushort Relearn4 { get; set; }

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
            const string iv = ", IVs = TradeIVs";

            var ability = AbilityNumber switch
            {
                0 => "             ",
                3 => "Ability = 4, ",
                _ => $"Ability = {AbilityNumber}, ",
            };

            var ivs = IVs[0] switch
            {
                31 when IVs.All(z => z == 31) => ", FlawlessIVCount = 6",
                -1 when IVs.All(z => z == -1) => string.Empty,
                -4 => ", FlawlessIVCount = 3",
                _ => iv,
            };

            var otgender = $", OTGender = {OTGender}";
            var gender = Gender == (int)FixedGender.Random ? string.Empty : $", Gender = {Gender - 1}";
            var nature = Nature == (int)Structures.Nature.Random25 ? string.Empty : $", Nature = Nature.{Nature}";
            var altform = Form == 0 ? string.Empty : $", Form = {Form}";
            var shiny = ShinyLock == (int)Shiny.Never ? string.Empty : $", Shiny = Shiny.{ShinyLock}";
            var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
            var tid = $"TID7 = {TrainerID}";
            var dyna = $", DynamaxLevel = {DynamaxLevel}";
            var relearn = Relearn1 == 0 ? "                                   " : $", Relearn = new[] {{{Relearn1:000},{Relearn2:000},{Relearn3:000},{Relearn4:000}}}";
            var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {Ball}";

            return
                $"            new({Species:000},{Level:00},{Memory:00},{TextVar:000},{Feeling:00},{Intensity}) {{ {ability}{tid}{ivs}{dyna}{otgender}{gender}{shiny}{nature}{giga}{relearn}{altform}{ball} }},{comment}";
        }
    }
}
