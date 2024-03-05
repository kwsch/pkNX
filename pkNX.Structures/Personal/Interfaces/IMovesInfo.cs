namespace pkNX.Structures;

public interface IMovesInfo;

public interface IMovesInfo_v1 : IMovesInfo
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
/// SpecialTutors added in B2W2
/// </summary>
public interface IMovesInfo_B2W2 : IMovesInfo_v1
{
    /// <summary>
    /// Special tutor learn compatibility flags for individual moves.
    /// </summary>
    bool[] SpecialTutors { get; set; }
}

/// <summary>
/// TRs added in SWSH
/// </summary>
public interface IMovesInfo_SWSH : IMovesInfo_B2W2
{
    /// <summary>
    /// TR learn compatibility flags for individual moves.
    /// </summary>
    bool[] TR { get; set; }
}

/// <summary>
/// Moves layout seems to have changed completely from the old verion
/// </summary>
public interface IMovesInfo_PLA : IMovesInfo_SWSH
{
    /* This data is converted to bool[] in the constructor so we don't need it here
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
    */
}

public static class IPersonalMovesExtensions
{
    public static void ImportIMovesInfo(this IMovesInfo self, IMovesInfo other)
    {
        if (self is IMovesInfo_v1 self_1 && other is IMovesInfo_v1 other_1)
        {
            self_1.TMHM = other_1.TMHM;
            self_1.TypeTutors = other_1.TypeTutors;
        }

        if (self is IMovesInfo_SWSH self_SWSH && other is IMovesInfo_SWSH other_SWSH)
        {
            self_SWSH.TR = other_SWSH.TR;
        }
    }
}
