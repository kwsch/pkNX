namespace pkNX.Structures;

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
/// SpecialTutors added in B2W2
/// </summary>
public interface IMovesInfo_2 : IMovesInfo_1
{
    /// <summary>
    /// Special tutor learn compatibility flags for individual moves.
    /// </summary>
    bool[][] SpecialTutors { get; set; }
}

/// <summary>
/// TRs added in SWSH
/// </summary>
public interface IMovesInfo_SWSH : IMovesInfo_2
{
    /// <summary>
    /// TR learn compatibility flags for individual moves.
    /// </summary>
    bool[] TR { get; set; }
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

public static class IPersonalMovesExtensions
{
    public static void SetIMovesInfo(this IMovesInfo self, IMovesInfo other)
    {
        if (self is IMovesInfo_1 self_1 && other is IMovesInfo_1 other_1)
        {
            self_1.TMHM = other_1.TMHM;
            self_1.TypeTutors = other_1.TypeTutors;
        }

        if (self is IMovesInfo_2 self_2 && other is IMovesInfo_2 other_2)
        {
            self_2.SpecialTutors = other_2.SpecialTutors;
        }

        if (self is IMovesInfo_3 self_3)
        {
            if (other is IMovesInfo_2 other_2b)
            {
                self_3.SpecialTutors = other_2b.SpecialTutors;
            }
            else if (other is IMovesInfo_3 other_3)
            {
                self_3.TM_A = other_3.TM_A;
                self_3.TM_B = other_3.TM_B;
                self_3.TM_C = other_3.TM_C;
                self_3.TM_D = other_3.TM_D;
                self_3.TR_A = other_3.TR_A;
                self_3.TR_B = other_3.TR_B;
                self_3.TR_C = other_3.TR_C;
                self_3.TR_D = other_3.TR_D;
                self_3.TypeTutor = other_3.TypeTutor;
                self_3.MoveShop1 = other_3.MoveShop1;
                self_3.MoveShop2 = other_3.MoveShop2;
                self_3.SpecialTutors = other_3.SpecialTutors;
            }
        }
    }
}
