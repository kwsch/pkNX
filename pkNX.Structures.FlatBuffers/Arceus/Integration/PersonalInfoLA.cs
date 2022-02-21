using System;

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
public sealed class PersonalInfoLA : PersonalInfo
{
    public PersonalInfoLAfb FB { get; }
    public PersonalInfoLA(PersonalInfoLAfb fb) => FB = fb;

    public override byte[] Write() => Array.Empty<byte>();

    public override int HP  { get => FB.Stat_HP ; set => FB.Stat_HP  = (byte)value; }
    public override int ATK { get => FB.Stat_ATK; set => FB.Stat_ATK = (byte)value; }
    public override int DEF { get => FB.Stat_DEF; set => FB.Stat_DEF = (byte)value; }
    public override int SPE { get => FB.Stat_SPE; set => FB.Stat_SPE = (byte)value; }
    public override int SPA { get => FB.Stat_SPA; set => FB.Stat_SPA = (byte)value; }
    public override int SPD { get => FB.Stat_SPD; set => FB.Stat_SPD = (byte)value; }
    public override int Type1 { get => FB.Type1; set => FB.Type1 = (byte)value; }
    public override int Type2 { get => FB.Type2; set => FB.Type2 = (byte)value; }
    public override int CatchRate { get => FB.CatchRate; set => FB.CatchRate = (byte)value; }
    public override int EvoStage { get => FB.EvoStage; set => FB.EvoStage = (byte)value; }
    public override int EV_HP  { get => FB.EV_HP ; set => FB.EV_HP  = (byte)value; }
    public override int EV_ATK { get => FB.EV_ATK; set => FB.EV_ATK = (byte)value; }
    public override int EV_DEF { get => FB.EV_DEF; set => FB.EV_DEF = (byte)value; }
    public override int EV_SPE { get => FB.EV_SPE; set => FB.EV_SPE = (byte)value; }
    public override int EV_SPA { get => FB.EV_SPA; set => FB.EV_SPA = (byte)value; }
    public override int EV_SPD { get => FB.EV_SPD; set => FB.EV_SPD = (byte)value; }
    public int Item1 { get => FB.Item1; set => FB.Item1 = (ushort)value; }
    public int Item2 { get => FB.Item2; set => FB.Item2 = (ushort)value; }
    public int Item3 { get => FB.Item3; set => FB.Item3 = (ushort)value; }
    public override int Gender { get => FB.Gender; set => FB.Gender = (byte)value; }
    public override int HatchCycles { get => 0; set { } }
    public override int BaseFriendship { get => FB.BaseFriendship; set => FB.BaseFriendship = (byte)value; }
    public override int EXPGrowth { get => FB.EXPGrowth; set => FB.EXPGrowth = (byte)value; }
    public override int EggGroup1 { get => FB.EggGroup1; set => FB.EggGroup1 = (byte)value; }
    public override int EggGroup2 { get => FB.EggGroup2; set => FB.EggGroup2 = (byte)value; }
    public int Ability1 { get => FB.Ability1; set => FB.Ability1 = (ushort)value; }
    public int Ability2 { get => FB.Ability2; set => FB.Ability2 = (ushort)value; }
    public int AbilityH { get => FB.AbilityH; set => FB.AbilityH = (ushort)value; }
    public override int EscapeRate { get => 0; set { } } // moved?
    protected override int FormStatsIndex { get => 0; set { } }
    public override int Color { get => FB.Color; set => FB.Color = (byte)value; }
    public bool IsPresentInGame { get => FB.IsPresentInGame; set => FB.IsPresentInGame = value; }
    public override int BaseEXP { get => FB.BaseEXP; set => FB.BaseEXP = (ushort)value; }
    public override int Height  { get => FB.Height ; set => FB.Height = (ushort)value; }
    public override int Weight  { get => FB.Weight ; set => FB.Weight = (ushort)value; }

    public override int[] Items
    {
        get => new[] { Item1, Item2, Item3 };
        set
        {
            if (value.Length != 3) return;
            Item1 = value[0];
            Item2 = value[1];
            Item3 = value[2];
        }
    }

    public override int[] Abilities
    {
        get => new[] { Ability1, Ability2, AbilityH };
        set
        {
            if (value.Length != 3) return;
            Ability1 = value[0];
            Ability2 = value[1];
            AbilityH = value[2];
        }
    }

    public int HatchSpecies     { get => FB.HatchSpecies;  set => FB.HatchSpecies = (ushort)value; }
    public int LocalFormIndex   { get => FB.LocalFormIndex; set => FB.LocalFormIndex = (ushort)value; } // local region base form
    public int Species          { get => FB.Species; set => FB.Species = (ushort)value; }
    public int Form             { get => FB.Form; set => FB.Form = (ushort)value; }
    public int DexIndexHisui    { get => FB.DexIndexHisui ; set => FB.DexIndexHisui  = (ushort)value; }
    public int DexIndexLocal1   { get => FB.DexIndexLocal1; set => FB.DexIndexLocal1 = (ushort)value; }
    public int DexIndexLocal2   { get => FB.DexIndexLocal2; set => FB.DexIndexLocal2 = (ushort)value; }
    public int DexIndexLocal3   { get => FB.DexIndexLocal3; set => FB.DexIndexLocal3 = (ushort)value; }
    public int DexIndexLocal4   { get => FB.DexIndexLocal4; set => FB.DexIndexLocal4 = (ushort)value; }
    public int DexIndexLocal5   { get => FB.DexIndexLocal5; set => FB.DexIndexLocal5 = (ushort)value; }
}
