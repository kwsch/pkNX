using System;

namespace pkNX.Structures
{
    public interface IMovesInfo
    {
    }

    public interface IMovesInfo_1 : IMovesInfo
    {
        /// <summary>
        /// TM/HM learn compatibility flags for individual moves.
        /// </summary>
        bool[] TMHM { get; set; }

        /// <summary>
        /// Grass-Fire-Water-Etc typed learn compatibility flags for individual moves.
        /// </summary>
        bool[] TypeTutors { get; set; }
    }

    /// <summary>
    /// SpecialTutors added in BW2 
    /// </summary>
    public interface IMovesInfo_2 : IMovesInfo_1
    {
        /// <summary>
        /// Special tutor learn compatibility flags for individual moves.
        /// </summary>
        bool[][] SpecialTutors { get; set; }
    }

    /// <summary>
    /// Moves layout seems to have changed completely from the old verion
    /// </summary>
    public interface IMovesInfo_3 : IMovesInfo
    {
        uint TM_A { get; set; }
        uint TM_B { get; set; }
        uint TM_C { get; set; }
        uint TM_D { get; set; }
        uint TR_A { get; set; }
        uint TR_B { get; set; }
        uint TR_C { get; set; }
        uint TR_D { get; set; }
        uint TypeTutor { get; set; }
        uint MoveShop1 { get; set; } // uint
        uint MoveShop2 { get; set; } // uint

        /// <summary>
        /// Special tutor learn compatibility flags for individual moves.
        /// </summary>
        bool[][] SpecialTutors { get; set; }
    }

    public interface IFormInfo : IPersonalFormInfo
    {
        int FormSprite { get; set; }
    }

    public interface IPersonalMetaInfo
    {
        ushort Species { get; set; }
        ushort Form { get; set; }
        bool IsPresentInGame { get; set; }
        ushort LocalFormIndex { get; set; } // ushort
        ushort DexIndexNational { get; set; } // ushort
        ushort DexIndexRegional { get; set; } // ushort
        ushort DexIndexLocal1 { get; set; } // uint
        ushort DexIndexLocal2 { get; set; } // uint
        ushort DexIndexLocal3 { get; set; } // uint
        ushort DexIndexLocal4 { get; set; } // uint
        ushort DexIndexLocal5 { get; set; } // uint
    }

    // Version based interfaces

    /// <summary>
    /// Base interface that can be used for any version. This should not contain variables that are not present in every game
    /// </summary>
    public interface IPersonalInfo : IBaseStat, IEffortValueYield, IPersonalType, IPersonalEgg, IPersonalTraits, IPersonalAbility, IPersonalMisc, IPersonalItems, IFormInfo { }

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

    /// <summary>
    /// Version 3 changes a lot, starting with the use of flat buffers
    /// </summary>
    public interface IPersonalInfo_3 : IPersonalInfo, IPersonalEgg_2, IPersonalMetaInfo, IMovesInfo_3 { }

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

        bool LocalVariant { get; set; }
    }
    public interface IPersonalInfoGG : IPersonalInfoSM
    {
        int GoSpecies { get; set; }
    }
    public interface IPersonalInfoSWSH : IPersonalInfo_2
    {
        public bool IsPresentInGame { get; set; }
        public bool SpriteForm { get; set; }
        public ushort Species { get; set; }
        public ushort HatchSpecies { get; set; }
        public byte LocalFormIndex { get; set; }
        public ushort RegionalFlags { get; set; }
        public bool IsRegionalForm { get; set; }
        public bool CanNotDynamax { get; set; }
        public ushort PokeDexIndex { get; set; }
        public byte RegionalFormIndex { get; set; }
        public ushort ArmorDexIndex { get; set; }
        public ushort CrownDexIndex { get; set; }
    }
    public interface IPersonalInfoPLA : IPersonalInfo_3
    {
        byte Field_18 { get; set; } // Always Default (0)
        bool Field_45 { get; set; } // byte
        ushort Field_46 { get; set; } // ushort
        byte Field_47 { get; set; } // byte
    }
}
