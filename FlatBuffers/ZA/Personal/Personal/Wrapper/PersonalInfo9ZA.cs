namespace pkNX.Structures.FlatBuffers.ZA;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.ZA"/> games.
/// </summary>
public sealed class PersonalInfo9ZA(PersonalInfo fb) : IPersonalInfo
{
    public PersonalInfo FB { get; } = fb;

    public int HP { get => FB.Base.HP; set => FB.Base.HP = (byte)value; }
    public int ATK { get => FB.Base.ATK; set => FB.Base.ATK = (byte)value; }
    public int DEF { get => FB.Base.DEF; set => FB.Base.DEF = (byte)value; }
    public int SPE { get => FB.Base.SPE; set => FB.Base.SPE = (byte)value; }
    public int SPA { get => FB.Base.SPA; set => FB.Base.SPA = (byte)value; }
    public int SPD { get => FB.Base.SPD; set => FB.Base.SPD = (byte)value; }
    public Types Type1 { get => (Types)FB.Type1; set => FB.Type1 = (byte)value; }
    public Types Type2 { get => (Types)FB.Type2; set => FB.Type2 = (byte)value; }
    public int Ability1 { get => FB.Ability1; set => FB.Ability1 = (ushort)value; }
    public int Ability2 { get => FB.Ability2; set => FB.Ability2 = (ushort)value; }
    public int AbilityH { get => FB.AbilityH; set => FB.AbilityH = (ushort)value; }
    public int CatchRate { get => FB.CatchRate; set => FB.CatchRate = (byte)value; }
    public int EvoStage { get => FB.EvoStage; set => FB.EvoStage = (byte)value; }
    public int EV_HP  { get => FB.EVYield.HP;  set => FB.EVYield.HP = (byte)value; }
    public int EV_ATK { get => FB.EVYield.ATK; set => FB.EVYield.ATK = (byte)value; }
    public int EV_DEF { get => FB.EVYield.DEF; set => FB.EVYield.DEF = (byte)value; }
    public int EV_SPE { get => FB.EVYield.SPE; set => FB.EVYield.SPE = (byte)value; }
    public int EV_SPA { get => FB.EVYield.SPA; set => FB.EVYield.SPA = (byte)value; }
    public int EV_SPD { get => FB.EVYield.SPD; set => FB.EVYield.SPD = (byte)value; }
    public byte GenderGroup { get => (byte)FB.Gender.Group; set => FB.Gender.Group = (SexGroup)value; }
    public byte GenderRatio { get => FB.Gender.Ratio; set => FB.Gender.Ratio = value; }
    public int Gender
    {
        get => FB.Gender.RatioMagicEquivalent();
        set { }
    }

    public int BaseFriendship { get => FB.BaseFriendship; set => FB.BaseFriendship = (byte)value; }
    public int EXPGrowth { get => FB.EXPGrowth; set => FB.EXPGrowth = (byte)value; }
    public int EggGroup1 { get => FB.EggGroup1; set => FB.EggGroup1 = (byte)value; }
    public int EggGroup2 { get => FB.EggGroup2; set => FB.EggGroup2 = (byte)value; }
    public int Color { get => FB.Info.Color; set => FB.Info.Color = (byte)value; }
    public bool IsPresentInGame { get => FB.IsPresentInGame; set => FB.IsPresentInGame = value; }
    public int Height  { get => FB.Info.Height; set => FB.Info.Height = (ushort)value; }
    public int Weight  { get => FB.Info.Weight; set => FB.Info.Weight = (ushort)value; }
    public byte DebutVersion { get => FB.Info.DebutVersion; set => FB.Info.DebutVersion = value; }
    public byte SpeciesClassMajor { get => FB.Info.SpeciesClassMajor; set => FB.Info.SpeciesClassMajor = value; }
    public uint SpeciesClassMinor { get => FB.Info.SpeciesClassMinor; set => FB.Info.SpeciesClassMinor = value; }

    public ushort HatchedSpecies { get => SpeciesConverterZA.GetNational9(FB.Hatch.SpeciesInternal); set => FB.Hatch.SpeciesInternal = SpeciesConverterZA.GetInternal9(value); }
    public ushort LocalFormIndex { get => FB.Hatch.Form; set => FB.Hatch.Form = value; }
    public bool RegionalFlags { get => FB.Hatch.RegionalFlags == 1; set => FB.Hatch.RegionalFlags = value ? (ushort)1 : (ushort)0; }
    public ushort EverstoneForm { get => FB.Hatch.EverstoneForm; set => FB.Hatch.EverstoneForm = value; }

    public ushort Form { get => FB.Info.Form; set => FB.Info.Form = value; }

    public ushort DexIndex
    {
        get => FB.Dex;
        set => FB.Dex = value;
    }

    public int Item1 { get => 0; set { } }
    public int Item2 { get => 0; set { } }
    public int Item3 { get => 0; set { } }
    public int BaseEXP { get => 0; set { } }
    public int EscapeRate { get => 0; set { } }
    public int FormSprite { get => 0; set { } }
    public int FormStatsIndex { get; set; }
    public byte FormCount { get; set; } = 1;

    public int BST => FB.Base.HP + FB.Base.ATK + FB.Base.DEF + FB.Base.SPE + FB.Base.SPA + FB.Base.SPD;
    public ushort AlphaMove { get; set; }

    public void Write(BinaryWriter bw)
    {
        bw.Write(FB.Base.HP);
        bw.Write(FB.Base.ATK);
        bw.Write(FB.Base.DEF);
        bw.Write(FB.Base.SPE);
        bw.Write(FB.Base.SPA);
        bw.Write(FB.Base.SPD);
        bw.Write(FB.Type1);
        bw.Write(FB.Type2);
        bw.Write(FB.CatchRate);
        bw.Write(FB.EvoStage);
        bw.Write(FB.EVYield.U16());
        bw.Write(FB.Gender.RatioMagicEquivalent());
        bw.Write(FB.HatchCycles);
        bw.Write(FB.BaseFriendship);
        bw.Write(FB.EXPGrowth);
        bw.Write(FB.EggGroup1);
        bw.Write(FB.EggGroup2);
        bw.Write(FB.Ability1);
        bw.Write(FB.Ability2);
        bw.Write(FB.AbilityH);
        bw.Write((ushort)FormStatsIndex);
        bw.Write(FormCount);
        bw.Write(FB.Info.Color);
        bw.Write(FB.IsPresentInGame);

        bw.Write((byte)0);
        bw.Write((ushort)DexIndex);
        bw.Write(FB.Info.Height);
        bw.Write(FB.Info.Weight);
        bw.Write(SpeciesConverterZA.GetNational9(FB.Hatch.SpeciesInternal));
        bw.Write(FB.Hatch.Form);
        bw.Write(FB.Hatch.RegionalFlags);
        bw.Write(FB.Hatch.EverstoneForm);
        // 0x2C

        // TMs
        byte[] tmFlags = new byte[0x1E];
        if (IsPresentInGame)
        {
            foreach (ushort tm in FB.TechnicalMachine)
            {
                // Get the index within TMIndexes, then set the bitflag within tmFlags
                var bitIndex = TMIndexes.IndexOf(tm);
                if (bitIndex < 0)
                    continue;
                tmFlags[bitIndex / 8] |= (byte)(1 << (bitIndex % 8));
            }
        }
        bw.Write(tmFlags);
        bw.Write((ushort)0); // align 0x50
        bw.Write(GetEXP(BST, FB.EvoStage, FB.BaseEXPAddend));
        bw.Write(AlphaMove); // align
    }

    private static ushort GetEXP(int bst, byte evoStage, short add)
        => (ushort)(Math.Ceiling(bst * (1 + (3 * evoStage)) / 20d) + add);

    public static ReadOnlySpan<ushort> TMIndexes =>
    [
        // Bit Index order
        029, 337, 473, 249, 046, 347, 092, 086, 812, 280,
        339, 157, 058, 424, 423, 113, 182, 612, 408, 583,
        422, 332, 009, 008, 242, 412, 129, 091, 007, 014,
        115, 104, 034, 400, 203, 317, 446, 126, 435, 331,
        352, 202, 019, 063, 282, 341, 097, 120, 196, 315,
        219, 414, 188, 434, 416, 038, 261, 442, 428, 248,
        421, 053, 094, 076, 444, 521, 085, 257, 089, 250,
        304, 083, 057, 247, 406, 710, 398, 523, 542, 334,
        404, 369, 417, 430, 164, 528, 231, 191, 390, 399,
        174, 605, 200, 018, 269, 056, 377, 127, 118, 441,
        527, 411, 526, 394, 059, 087, 370,
    ];
}
