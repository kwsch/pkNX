using System;
using System.IO;

namespace pkNX.Structures.FlatBuffers.SV;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
public sealed class PersonalInfo9SV(PersonalInfo fb) : IPersonalInfo
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

    public ushort HatchedSpecies { get => SpeciesConverterSV.GetNational9(FB.Hatch.SpeciesInternal); set => FB.Hatch.SpeciesInternal = SpeciesConverterSV.GetInternal9(value); }
    public ushort LocalFormIndex { get => FB.Hatch.Form; set => FB.Hatch.Form = value; }
    public bool RegionalFlags { get => FB.Hatch.RegionalFlags == 1; set => FB.Hatch.RegionalFlags = value ? (ushort)1 : (ushort)0; }
    public ushort EverstoneForm { get => FB.Hatch.EverstoneForm; set => FB.Hatch.EverstoneForm = value; }

    public ushort Form { get => FB.Info.Form; set => FB.Info.Form = value; }

    public int DexGroup
    {
        get => FB.Dex?.Group ?? 0;
        set
        {
            if (value == 0)
                FB.Dex = null;
            else
                (FB.Dex ??= new PersonalInfoDex()).Group = (byte)value;
        }
    }
    public int DexIndex
    {
        get => FB.Dex?.Index ?? 0;
        set
        {
            if (value == 0)
                FB.Dex = null;
            else
                (FB.Dex ??= new PersonalInfoDex()).Index = (byte)value;
        }
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

        bw.Write((byte)(DexGroup == 1 ? 1 : FB.KitakamiDex != 0 ? 2 : FB.BlueberryDex != 0 ? 2 : 0));
        bw.Write((ushort)DexIndex);
        bw.Write(FB.Info.Height);
        bw.Write(FB.Info.Weight);
        bw.Write(SpeciesConverterSV.GetNational9(FB.Hatch.SpeciesInternal));
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
                var bitIndex = Array.IndexOf(TMIndexes, tm);
                if (bitIndex < 0)
                    continue;
                tmFlags[bitIndex / 8] |= (byte)(1 << (bitIndex % 8));
            }
        }
        bw.Write(tmFlags);
        bw.Write((byte)FB.KitakamiDex); // always <= 255
        bw.Write((byte)FB.BlueberryDex); // always <= 255
        bw.Write(GetEXP(BST, FB.EvoStage, FB.BaseEXPAddend));
        bw.Write((ushort)0); // align 0x50
    }

    private static ushort GetEXP(int bst, byte evoStage, short add)
        => (ushort)(Math.Ceiling(bst * (1 + (3 * evoStage)) / 20d) + add);

    public static readonly ushort[] TMIndexes =
    [
        005, 036, 204, 313, 097, 189, 184, 182, 424, 422,
        423, 352, 067, 491, 512, 522, 060, 109, 168, 574,
        885, 884, 886, 451, 083, 263, 342, 332, 523, 506,
        555, 232, 129, 345, 196, 341, 317, 577, 488, 490,
        314, 500, 101, 374, 525, 474, 419, 203, 521, 241,
        240, 201, 883, 684, 473, 091, 331, 206, 280, 428,
        369, 421, 492, 706, 339, 403, 034, 007, 009, 008,
        214, 402, 486, 409, 115, 113, 350, 127, 337, 605,
        118, 447, 086, 398, 707, 156, 157, 269, 014, 776,
        191, 390, 286, 430, 399, 141, 598, 019, 285, 442,
        349, 408, 441, 164, 334, 404, 529, 261, 242, 271,
        710, 202, 396, 366, 247, 406, 446, 304, 257, 412,
        094, 484, 227, 057, 861, 053, 085, 583, 133, 347,
        270, 676, 226, 414, 179, 058, 604, 580, 678, 581,
        417, 126, 056, 059, 519, 518, 520, 528, 188, 089,
        444, 566, 416, 307, 308, 338, 200, 315, 411, 437,
        542, 433, 405, 063, 413, 394, 087, 370, 076, 434,
        796, 851, 046, 268, 114, 092, 328, 180, 356, 479,
        360, 282, 450, 162, 410, 679, 667, 333, 503, 535,
        669, 253, 264, 311, 803, 807, 812, 814, 809, 808,
        799, 802, 220, 244, 038, 283, 572, 915, 250, 330,
        916, 527, 813, 811, 482, 815, 297, 248, 797, 806,
        800, 675, 784, 319, 174, 912, 913, 914, 917, 918,
    ];
}
