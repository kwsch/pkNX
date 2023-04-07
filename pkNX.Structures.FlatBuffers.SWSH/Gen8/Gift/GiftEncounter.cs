using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterGift
{
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
        get => new int[] { IVHP, IVATK, IVDEF, IVSPE, IVSPA, IVSPD };
        set
        {
            if (value.Length != 6) return;
            IVHP =    (sbyte)value[0];
            IVATK =   (sbyte)value[1];
            IVDEF =   (sbyte)value[2];
            IVSPE =   (sbyte)value[3];
            IVSPA = (sbyte)value[4];
            IVSPD = (sbyte)value[5];
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
