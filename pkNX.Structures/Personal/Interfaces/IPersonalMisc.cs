namespace pkNX.Structures;

/// <summary>
/// Exposes miscellaneous metadata about an entity species/form.
/// </summary>
public interface IPersonalMisc
{
    /// <summary>
    /// Evolution Stage value (or equivalent for un-evolved).
    /// </summary>
    int EvoStage { get; set; }
}

/// <summary>
/// Added since SWSH
/// </summary>
public interface IPersonalMisc_SWSH : IPersonalMisc
{
    ushort ModelID { get; set; }
    ushort Form { get; set; }
    bool IsPresentInGame { get; set; }
    ushort LocalFormIndex { get; set; }

    ushort DexIndexNational { get; set; } // ushort
    ushort DexIndexRegional { get; set; } // ushort
}

public interface IPersonalMisc_PLA : IPersonalMisc_SWSH
{
    ushort DexIndexLocal1 { get; set; } // uint
    ushort DexIndexLocal2 { get; set; } // uint
    ushort DexIndexLocal3 { get; set; } // uint
    ushort DexIndexLocal4 { get; set; } // uint
    ushort DexIndexLocal5 { get; set; } // uint
}

public static class IPersonalMiscExtensions
{
    public static void ImportIPersonalMisc(this IPersonalMisc self, IPersonalMisc other)
    {
        self.EvoStage = other.EvoStage;

        if (self is IPersonalMisc_PLA self_1 && other is IPersonalMisc_SWSH other_1)
        {
            // PLA already has most properties filled in. Only these are missing.
            self_1.DexIndexNational = other_1.DexIndexNational;
            self_1.Form = other_1.Form;
        }
    }
}
