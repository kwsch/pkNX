using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalTableLA : IFlatBufferArchive<PersonalInfoLAfb>
{
    [FlatBufferItem(0)] public PersonalInfoLAfb[] Table { get; set; } = Array.Empty<PersonalInfoLAfb>();
}

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfoLAfb
{
    [FlatBufferItem(00)] public ushort Species { get; set; } // ushort
    [FlatBufferItem(01)] public ushort Form { get; set; } // ushort
    [FlatBufferItem(02)] public bool IsPresentInGame { get; set; } // byte
    [FlatBufferItem(03)] public byte Type1 { get; set; } // byte
    [FlatBufferItem(04)] public byte Type2 { get; set; } // byte
    [FlatBufferItem(05)] public ushort Ability1 { get; set; } // ushort
    [FlatBufferItem(06)] public ushort Ability2 { get; set; } // ushort
    [FlatBufferItem(07)] public ushort AbilityH { get; set; } // ushort
    [FlatBufferItem(08)] public byte Stat_HP  { get; set; } // byte
    [FlatBufferItem(09)] public byte Stat_ATK { get; set; } // byte
    [FlatBufferItem(10)] public byte Stat_DEF { get; set; } // byte
    [FlatBufferItem(11)] public byte Stat_SPA { get; set; } // byte
    [FlatBufferItem(12)] public byte Stat_SPD { get; set; } // byte
    [FlatBufferItem(13)] public byte Stat_SPE { get; set; } // byte
    [FlatBufferItem(14)] public byte Gender   { get; set; } // byte
    [FlatBufferItem(15)] public byte EXPGrowth { get; set; } // byte
    [FlatBufferItem(16)] public byte EvoStage { get; set; } // byte
    [FlatBufferItem(17)] public byte CatchRate{ get; set; } // byte
    [FlatBufferItem(18)] public byte Field_18 { get; set; } // Always Default (0)
    [FlatBufferItem(19)] public byte Color { get; set; } // byte
    [FlatBufferItem(20)] public ushort Height { get; set; } // ushort
    [FlatBufferItem(21)] public ushort Weight { get; set; } // ushort
    [FlatBufferItem(22)] public uint TM_A { get; set; } // uint, not used by game
    [FlatBufferItem(23)] public uint TM_B { get; set; } // uint, not used by game
    [FlatBufferItem(24)] public uint TM_C { get; set; } // uint, not used by game
    [FlatBufferItem(25)] public uint TM_D { get; set; } // uint, not used by game
    [FlatBufferItem(26)] public uint TR_A { get; set; } // uint, not used by game
    [FlatBufferItem(27)] public uint TR_B { get; set; } // uint, not used by game
    [FlatBufferItem(28)] public uint TR_C { get; set; } // uint, not used by game
    [FlatBufferItem(29)] public uint TR_D { get; set; } // uint, not used by game
    [FlatBufferItem(30)] public uint TypeTutor { get; set; } // uint, not used by game
    [FlatBufferItem(31)] public ushort BaseEXP { get; set; } // ushort
    [FlatBufferItem(32)] public byte EV_HP  { get; set; } // byte
    [FlatBufferItem(33)] public byte EV_ATK { get; set; } // byte
    [FlatBufferItem(34)] public byte EV_DEF { get; set; } // byte
    [FlatBufferItem(35)] public byte EV_SPA { get; set; } // byte
    [FlatBufferItem(36)] public byte EV_SPD { get; set; } // byte
    [FlatBufferItem(37)] public byte EV_SPE { get; set; } // byte
    [FlatBufferItem(38)] public ushort Item1 { get; set; } // ushort
    [FlatBufferItem(39)] public ushort Item2 { get; set; } // ushort
    [FlatBufferItem(40)] public ushort Item3 { get; set; } // Always Default (0)
    [FlatBufferItem(41)] public byte EggGroup1 { get; set; } // byte
    [FlatBufferItem(42)] public byte EggGroup2 { get; set; } // byte
    [FlatBufferItem(43)] public ushort HatchSpecies { get; set; } // ushort
    [FlatBufferItem(44)] public ushort LocalFormIndex { get; set; } // ushort
    [FlatBufferItem(45)] public bool Field_45 { get; set; } // byte
    [FlatBufferItem(46)] public ushort Field_46 { get; set; } // ushort
    [FlatBufferItem(47)] public byte Field_47 { get; set; } // byte
    [FlatBufferItem(48)] public byte BaseFriendship { get; set; } // byte
    [FlatBufferItem(49)] public ushort DexIndexHisui { get; set; } // ushort
    [FlatBufferItem(50)] public ushort DexIndexOther { get; set; } // ushort
    [FlatBufferItem(51)] public int DexIndexLocal1 { get; set; } // uint
    [FlatBufferItem(52)] public int DexIndexLocal2 { get; set; } // uint
    [FlatBufferItem(53)] public int DexIndexLocal3 { get; set; } // uint
    [FlatBufferItem(54)] public int DexIndexLocal4 { get; set; } // uint
    [FlatBufferItem(55)] public int DexIndexLocal5 { get; set; } // uint
    [FlatBufferItem(56)] public uint MoveShop1 { get; set; } // uint
    [FlatBufferItem(57)] public uint MoveShop2 { get; set; } // uint
}

public static class PersonalConverter
{
    public static byte[] GetBin(PersonalTableLA table)
    {
        var all = FromArceus(table);
        var data = all.SelectMany(z => z.Write()).ToArray();
        return data;
    }

    public static PersonalInfoLA_Bin[] FromArceus(PersonalTableLA table)
    {
        var max = table.Table.Max(z => z.Species);
        var baseForms = new PersonalInfoLA_Bin[max + 1];
        var formTable = new List<PersonalInfoLA_Bin>();

        for (int i = 0; i <= max; i++)
        {
            var forms = table.Table.Where(z => z.Species == (ushort)i).OrderBy(z => z.Form).ToList();

            var e = forms[0];
            baseForms[i] = GetObj(e, forms, max, formTable);
            for (int f = 1; f < forms.Count; f++)
                formTable.Add(GetObj(forms[f], forms, max, formTable, f));
        }

        return baseForms.Concat(formTable).ToArray();
    }

    // ugly converter to be a data-backed object
    private static PersonalInfoLA_Bin GetObj(PersonalInfoLAfb e, List<PersonalInfoLAfb> forms, ushort max, List<PersonalInfoLA_Bin> formTable, int f = 0)
    {
        var result = new PersonalInfoLA_Bin(new byte[PersonalInfoLA_Bin.SIZE])
        {
            HP = e.Stat_HP,
            ATK = e.Stat_ATK,
            DEF = e.Stat_DEF,
            SPA = e.Stat_SPA,
            SPD = e.Stat_SPD,
            SPE = e.Stat_SPE,
            FormeCount = forms.Count,
            Type1 = e.Type1,
            Type2 = e.Type2,
            Gender = e.Gender,
            Ability1 = e.Ability1,
            Ability2 = e.Ability2,
            AbilityH = e.AbilityH,
            IsPresentInGame = e.IsPresentInGame,
            Item1 = e.Item1,
            Item2 = e.Item2,
            Height = e.Height,
            Weight = e.Weight,
            EggGroup1 = e.EggGroup1,
            EggGroup2 = e.EggGroup2,
            EvoStage = e.EvoStage,
            BaseFriendship = e.BaseFriendship,
            EXPGrowth = e.EXPGrowth,
            HatchSpecies = e.HatchSpecies,
            LocalFormIndex = e.LocalFormIndex,
            EV_HP = e.EV_HP,
            EV_ATK = e.EV_ATK,
            EV_DEF = e.EV_DEF,
            EV_SPA = e.EV_SPA,
            EV_SPD = e.EV_SPD,
            EV_SPE = e.EV_SPE,
            CatchRate = e.CatchRate,
            DexIndexHisui = e.DexIndexHisui,
            DexIndexLocal1 = e.DexIndexLocal1,
            DexIndexLocal2 = e.DexIndexLocal2,
            DexIndexLocal3 = e.DexIndexLocal3,
            DexIndexLocal4 = e.DexIndexLocal4,
            DexIndexLocal5 = e.DexIndexLocal5,
            Color = e.Color,
            BaseEXP = e.BaseEXP,

            Species = e.Species,
            Form = e.Form,
        };
        result.SetFormStat(f != 0 ? 0 : forms.Count == 1 ? 0 : max + formTable.Count + 1);

        var shop = e.MoveShop1 | ((ulong)e.MoveShop2 << 32);
        bool[] flags = new bool[64];
        for (int i = 0; i < flags.Length; i++)
            flags[i] = (shop & (1ul << i)) != 0;
        result.SpecialTutors = new[] { flags };

        BitConverter.GetBytes(e.TM_A).CopyTo(result.Data, 0x28);
        BitConverter.GetBytes(e.TM_B).CopyTo(result.Data, 0x2C);
        BitConverter.GetBytes(e.TM_C).CopyTo(result.Data, 0x30);
        BitConverter.GetBytes(e.TM_D).CopyTo(result.Data, 0x34);
        BitConverter.GetBytes(e.TypeTutor).CopyTo(result.Data, 0x38);
        BitConverter.GetBytes(e.TR_A).CopyTo(result.Data, 0x3C);
        BitConverter.GetBytes(e.TR_B).CopyTo(result.Data, 0x40);
        BitConverter.GetBytes(e.TR_C).CopyTo(result.Data, 0x44);
        BitConverter.GetBytes(e.TR_D).CopyTo(result.Data, 0x48);
        result.LoadTMHM();
        result.LoadTutors();

        return result;
    }
}
