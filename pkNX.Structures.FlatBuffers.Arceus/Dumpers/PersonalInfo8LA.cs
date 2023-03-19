using System.Collections;

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
public sealed class PersonalInfo8LA : IPersonalInfoPLA
{
    public PersonalInfo FB { get; }

    private bool[][] _SpecialTutors = Array.Empty<bool[]>();
    public bool[][] SpecialTutors
    {
        get => _SpecialTutors;
        set
        {
            _SpecialTutors = value;

            BitArray bits = new(SpecialTutors[0]);
            byte[] bytes = new byte[bits.Length / 8];
            bits.CopyTo(bytes, 0);
            MoveShop = BitConverter.ToUInt64(bytes, 0);
        }
    }

    public PersonalInfo8LA(PersonalInfo fb)
    {
        FB = fb;

        var shop = MoveShop;
        bool[] flags = new bool[64];
        for (int i = 0; i < flags.Length; i++)
            flags[i] = (shop & (1ul << i)) != 0;
        SpecialTutors = new[] { flags };
    }

    public int HP { get => FB.StatHP; set => FB.StatHP = (byte)value; }
    public int ATK { get => FB.StatATK; set => FB.StatATK = (byte)value; }
    public int DEF { get => FB.StatDEF; set => FB.StatDEF = (byte)value; }
    public int SPE { get => FB.StatSPE; set => FB.StatSPE = (byte)value; }
    public int SPA { get => FB.StatSPA; set => FB.StatSPA = (byte)value; }
    public int SPD { get => FB.StatSPD; set => FB.StatSPD = (byte)value; }
    public Types Type1 { get => (Types)FB.Type1; set => FB.Type1 = (byte)value; }
    public Types Type2 { get => (Types)FB.Type2; set => FB.Type2 = (byte)value; }
    public int CatchRate { get => FB.CatchRate; set => FB.CatchRate = (byte)value; }
    public int EvoStage { get => FB.EvoStage; set => FB.EvoStage = (byte)value; }
    public byte Field_18 { get => FB.Field18; set => FB.Field18 = value; }

    public int EV_HP  { get => FB.EVHP;  set => FB.EVHP = (byte)value; }
    public int EV_ATK { get => FB.EVATK; set => FB.EVATK = (byte)value; }
    public int EV_DEF { get => FB.EVDEF; set => FB.EVDEF = (byte)value; }
    public int EV_SPE { get => FB.EVSPE; set => FB.EVSPE = (byte)value; }
    public int EV_SPA { get => FB.EVSPA; set => FB.EVSPA = (byte)value; }
    public int EV_SPD { get => FB.EVSPD; set => FB.EVSPD = (byte)value; }
    public int Item1 { get => FB.Item1; set => FB.Item1 = (ushort)value; }
    public int Item2 { get => FB.Item2; set => FB.Item2 = (ushort)value; }
    public int Item3 { get => FB.Item3; set => FB.Item3 = (ushort)value; }
    public int Gender { get => FB.Gender; set => FB.Gender = (byte)value; }
    public int BaseFriendship { get => FB.BaseFriendship; set => FB.BaseFriendship = (byte)value; }
    public int EXPGrowth { get => FB.EXPGrowth; set => FB.EXPGrowth = (byte)value; }
    public int EggGroup1 { get => FB.EggGroup1; set => FB.EggGroup1 = (byte)value; }
    public int EggGroup2 { get => FB.EggGroup2; set => FB.EggGroup2 = (byte)value; }
    public int Ability1 { get => FB.Ability1; set => FB.Ability1 = (ushort)value; }
    public int Ability2 { get => FB.Ability2; set => FB.Ability2 = (ushort)value; }
    public int AbilityH { get => FB.AbilityH; set => FB.AbilityH = (ushort)value; }
    public int Color { get => FB.Color; set => FB.Color = (byte)value; }
    public bool IsPresentInGame { get => FB.IsPresentInGame; set => FB.IsPresentInGame = value; }
    public int BaseEXP { get => FB.BaseEXP; set => FB.BaseEXP = (ushort)value; }
    public int Height { get => FB.Height; set => FB.Height = (ushort)value; }
    public int Weight { get => FB.Weight; set => FB.Weight = (ushort)value; }

    public uint TM_A { get => FB.TMA; set => FB.TMA = value; }
    public uint TM_B { get => FB.TMB; set => FB.TMB = value; }
    public uint TM_C { get => FB.TMC; set => FB.TMC = value; }
    public uint TM_D { get => FB.TMD; set => FB.TMD = value; }
    public uint TR_A { get => FB.TRA; set => FB.TRA = value; }
    public uint TR_B { get => FB.TRB; set => FB.TRB = value; }
    public uint TR_C { get => FB.TRC; set => FB.TRC = value; }
    public uint TR_D { get => FB.TRD; set => FB.TRD = value; }
    public uint TypeTutor { get => FB.TypeTutor; set => FB.TypeTutor = value; }

    public ushort HatchedSpecies { get => FB.HatchedSpecies; set => FB.HatchedSpecies = value; }
    public ushort LocalFormIndex { get => FB.LocalFormIndex; set => FB.LocalFormIndex = value; }
    public bool Field_45   { get => FB.Field45; set => FB.Field45 = value; }
    public ushort Field_46 { get => FB.Field46; set => FB.Field46 = value; }
    public byte Field_47   { get => FB.Field47; set => FB.Field47 = value; }

    public ushort ModelID { get => FB.ModelID; set => FB.ModelID = value; }
    public ushort Form { get => FB.Form; set => FB.Form = value; }
    public ushort DexIndexNational { get => FB.DexIndexNational; set => FB.DexIndexNational = value; }
    public ushort DexIndexRegional { get => FB.DexIndexHisui; set => FB.DexIndexHisui = value; }
    public ushort DexIndexLocal1 { get => (ushort)FB.DexIndexLocal1; set => FB.DexIndexLocal1 = value; }
    public ushort DexIndexLocal2 { get => (ushort)FB.DexIndexLocal2; set => FB.DexIndexLocal2 = value; }
    public ushort DexIndexLocal3 { get => (ushort)FB.DexIndexLocal3; set => FB.DexIndexLocal3 = value; }
    public ushort DexIndexLocal4 { get => (ushort)FB.DexIndexLocal4; set => FB.DexIndexLocal4 = value; }
    public ushort DexIndexLocal5 { get => (ushort)FB.DexIndexLocal5; set => FB.DexIndexLocal5 = value; }

    public uint MoveShop1 { get => FB.MoveShop1; set => FB.MoveShop1 = value; }
    public uint MoveShop2 { get => FB.MoveShop2; set => FB.MoveShop2 = value; }
    public ulong MoveShop { get => MoveShop1 | ((ulong)MoveShop2 << 32); set { MoveShop1 = (uint)value; MoveShop2 = (uint)(value >> 32); } }

    public int EscapeRate { get => 0; set { } }
    public int FormSprite { get => 0; set { } }
    public int FormStatsIndex { get; set; }
    public byte FormCount { get; set; } = 1;

    public int GetMoveShopCount()
    {
        // Return a count of true indexes from Tutors
        var arr = SpecialTutors[0];
        int count = 0;
        foreach (var index in arr)
        {
            if (index)
                count++;
        }
        return count;
    }

    public int GetMoveShopIndex(int randIndexFromCount)
    {
        // Return a count of true indexes from Tutors
        var arr = SpecialTutors[0];
        for (var i = 0; i < arr.Length; i++)
        {
            var index = arr[i];
            if (!index)
                continue;
            if (randIndexFromCount-- == 0)
                return i;
        }
        throw new ArgumentOutOfRangeException(nameof(randIndexFromCount));
    }
}
