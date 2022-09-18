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
