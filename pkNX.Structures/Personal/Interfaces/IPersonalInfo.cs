using System;

namespace pkNX.Structures;

/// <summary>
/// Base interface that can be used for any version. This should not contain variables that are not present in every game
/// </summary>
public interface IPersonalInfo : IBaseStat, IEffortValueYield, IPersonalType, IPersonalEgg, IPersonalTraits, IPersonalAbility, IPersonalMisc, IPersonalItems, IPersonalFormInfo;

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
public interface IPersonalInfo_v1 : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_v1, IMovesInfo_v1;

/// <summary>
/// Version 2 adds `SpecialTutors` to moves
/// </summary>
public interface IPersonalInfo_v2 : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_v1, IMovesInfo_B2W2;

// Game specific PersonalInfo interfaces

public interface IPersonalInfoBW : IPersonalInfo_v1;
public interface IPersonalInfoXY : IPersonalInfo_v1;
public interface IPersonalInfoB2W2 : IPersonalInfo_v2;
public interface IPersonalInfoORAS : IPersonalInfo_v2;
public interface IPersonalInfoSM : IPersonalInfo_v2
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
public interface IPersonalInfoSWSH : IPersonalInfoBin, IPersonalInfo, IPersonalEgg_SWSH, IMovesInfo_SWSH, IPersonalMisc_SWSH
{
    bool SpriteForm { get; set; }
    bool IsRegionalForm { get; set; }
    ushort RegionalFlags { get; set; }
    bool CanNotDynamax { get; set; }
    ushort ArmorDexIndex { get; set; }
    ushort CrownDexIndex { get; set; }
}
public interface IPersonalInfoPLA : IPersonalInfo, IPersonalEgg_PLA, IMovesInfo_PLA, IPersonalMisc_PLA
{
    byte Field_18 { get; set; } // Always Default (0)
    bool IsRegionalForm { get; set; } // byte
    ushort RegionalFormIndex { get; set; } // ushort
}
public interface IPersonalInfoSV : IPersonalInfo, IPersonalEgg_SWSH;

public static class IPersonalInfoExt
{
    /// <summary>
    /// Import the missing properties from an older generation
    /// </summary>
    /// <param name="self">The data to fill</param>
    /// <param name="other">The older generation that is used to fill in the missing pieces</param>
    public static void ImportPersonalInfo(this IPersonalInfo self, IPersonalInfo other)
    {
        self.ImportIBaseStats(other);
        self.ImportIEffortValueYield(other);
        self.ImportIPersonalAbility(other);
        self.ImportIPersonalItems(other);
        self.ImportIPersonalType(other);
        self.ImportIPersonalEgg(other);
        self.ImportIPersonalTraits(other);
        self.ImportIPersonalMisc(other);

        if (self is IMovesInfo self_1 && other is IMovesInfo other_1)
        {
            self_1.ImportIMovesInfo(other_1);
        }
    }
}
