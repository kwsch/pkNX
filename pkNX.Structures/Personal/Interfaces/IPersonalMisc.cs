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
public interface IPersonalMisc_1 : IPersonalMisc
{
    ushort Species { get; set; }
    bool IsPresentInGame { get; set; }
    ushort LocalFormIndex { get; set; }
}

public interface IPersonalMisc_2 : IPersonalMisc_1
{
    ushort Form { get; set; }
    ushort DexIndexNational { get; set; } // ushort
    ushort DexIndexRegional { get; set; } // ushort
    ushort DexIndexLocal1 { get; set; } // uint
    ushort DexIndexLocal2 { get; set; } // uint
    ushort DexIndexLocal3 { get; set; } // uint
    ushort DexIndexLocal4 { get; set; } // uint
    ushort DexIndexLocal5 { get; set; } // uint
}