using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterTradeArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterTrade
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
            if (value?.Length != 6) return;
            IVHP = (sbyte)value[0];
            IVATK = (sbyte)value[1];
            IVDEF = (sbyte)value[2];
            IVSPE = (sbyte)value[3];
            IVSPA = (sbyte)value[4];
            IVSPD = (sbyte)value[5];
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
        var nature = Nature == (int)Structures.Nature.Random25 ? string.Empty : $", Nature = Nature.{(Nature)Nature}";
        var altform = Form == 0 ? string.Empty : $", Form = {Form:00}";
        var shiny = ShinyLock == (int)Shiny.Never ? string.Empty : $", Shiny = {(Shiny)ShinyLock}";
        var giga = !CanGigantamax ? string.Empty : ", CanGigantamax = true";
        var tid = $"TID7 = {TrainerID}";
        var dyna = $", DynamaxLevel = {DynamaxLevel}";
        var relearn = Relearn1 == 0 ? "                                   " : $", Relearn = new[] {{{Relearn1:000},{Relearn2:000},{Relearn3:000},{Relearn4:000}}}";
        var ball = Ball == Ball.Poke ? string.Empty : $", Ball = {Ball}";

        return
            $"            new({Species:000},{Level:00},{Memory:00},{TextVar:000},{Feeling:00},{Intensity}) {{ {ability}{tid}{ivs}{dyna}{otgender}{gender}{shiny}{nature}{giga}{relearn}{altform}{ball} }},{comment}";
    }
}
