using System;

namespace pkNX.Structures;

/// <summary>
/// Base interface that can be used for any version. This should not contain variables that are not present in every game
/// </summary>
public interface IPersonalInfo : IBaseStat, IEffortValueYield, IPersonalType, IPersonalEgg, IPersonalTraits, IPersonalAbility, IPersonalMisc, IPersonalItems, IPersonalFormInfo { }

public interface IPersonalInfoBin
{
    byte[] Write();
}

public static class IPersonalInfoBinExt
{
    public static bool[] GetBits(ReadOnlySpan<byte> data)
    {
        bool[] result = new bool[data.Length << 3];
        for (int i = result.Length - 1; i >= 0; i--)
            result[i] = ((data[i >> 3] >> (i & 7)) & 0x1) == 1;
        return result;
    }

    public static void SetBits(ReadOnlySpan<bool> bits, Span<byte> data)
    {
        for (int i = bits.Length - 1; i >= 0; i--)
            data[i >> 3] |= (byte)(bits[i] ? 1 << (i & 0x7) : 0);
    }
}

/// <summary>
/// Version one
/// </summary>
public interface IPersonalInfo_1 : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_1, IMovesInfo_1 { }

/// <summary>
/// Version 2 adds `SpecialTutors` to moves
/// </summary>
public interface IPersonalInfo_2 : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_1, IMovesInfo_2 { }

// Game specific PersonalInfo interfaces

public interface IPersonalInfoBW : IPersonalInfo_1 { }
public interface IPersonalInfoXY : IPersonalInfo_1 { }
public interface IPersonalInfoB2W2 : IPersonalInfo_2 { }
public interface IPersonalInfoORAS : IPersonalInfo_2 { }
public interface IPersonalInfoSM : IPersonalInfo_2
{
    int SpecialZ_Item { get; set; }
    int SpecialZ_BaseMove { get; set; }
    int SpecialZ_ZMove { get; set; }
    bool IsRegionalForm { get; set; }
}
public interface IPersonalInfoGG : IPersonalInfoSM
{
    int GoSpecies { get; set; }
}
public interface IPersonalInfoSWSH : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_2, IMovesInfo_SWSH, IPersonalMisc_1
{
    bool SpriteForm { get; set; }
    bool IsRegionalForm { get; set; }
    ushort RegionalFlags { get; set; }
    bool CanNotDynamax { get; set; }
    ushort ArmorDexIndex { get; set; }
    ushort CrownDexIndex { get; set; }
}
public interface IPersonalInfoPLA : IPersonalInfo, IPersonalEgg_3, IMovesInfo_3, IPersonalMisc_2
{
    byte Field_18 { get; set; } // Always Default (0)
    bool Field_45 { get; set; } // byte
    ushort Field_46 { get; set; } // ushort
    byte Field_47 { get; set; } // byte
}
public interface IPersonalInfoSV : IPersonalInfo, IPersonalEgg_3
{
}

public static class IPersonalInfoExt
{
    public static void SetPersonalInfo(this IPersonalInfo self, IPersonalInfo other)
    {
        self.SetIBaseStats(other);
        self.SetIEffortValueYield(other);
        self.SetIPersonalAbility(other);
        self.SetIPersonalItems(other);
        self.SetIPersonalType(other);
        self.SetIPersonalEgg(other);
        self.SetIPersonalTraits(other);
        self.SetIPersonalMisc(other);

        if (self is IPersonalInfo_1 self_1 && other is IPersonalInfo_1 other_1)
        {
            self_1.SetIMovesInfo(other_1);
        }

        if (self is IPersonalInfo_2 self_2 && other is IPersonalInfo_2 other_2)
        {
            self_2.SetIMovesInfo(other_2);
        }
    }
}
