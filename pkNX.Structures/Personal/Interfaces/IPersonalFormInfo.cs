namespace pkNX.Structures;

/// <summary>
/// Exposes info about the entry's form(s).
/// </summary>
public interface IPersonalFormInfo
{
    /// <summary>
    /// Count of form values the entry can have.
    /// </summary>
    byte FormCount { get; set; }

    /// <summary>
    /// Pointer to the first form <see cref="IPersonalInfo"/> index
    /// </summary>
    int FormStatsIndex { get; set; }

    /// <summary>
    /// ? TODO: Remove from later interface versions
    /// </summary>
    int FormSprite { get; set; }
}

public static class IPersonalFormInfoExtensions
{
    /// <summary>
    /// Checks if the <see cref="IPersonalFormInfo"/> has any form entry available.
    /// </summary>
    public static bool HasAnyForms(this IPersonalFormInfo info) => info.FormCount > 1;

    /// <summary>
    /// Checks if the <see cref="IPersonalFormInfo"/> has the requested form entry index available.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="form">Form to retrieve for</param>
    public static bool HasForm(this IPersonalFormInfo info, byte form)
    {
        if (form == 0) // no form requested
            return false;
        if (info.FormStatsIndex <= 0) // no forms present
            return false;
        if (form >= info.FormCount) // beyond range of species' forms
            return false;
        return true;
    }

    /// <summary>
    /// Gets the <see cref="IPersonalFormInfo"/> form entry index for the input criteria, with fallback for the original species entry.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="species"><see cref="Species"/> to retrieve for</param>
    /// <param name="form">Form to retrieve for</param>
    /// <returns>Index the form exists as in the table.</returns>
    public static int FormIndex(this IPersonalFormInfo info, ushort species, byte form)
    {
        if (!info.HasForm(form))
            return species;
        return info.FormStatsIndex + form - 1;
    }

    /// <summary>
    /// Checks to see if the form is valid within the Personal Info
    /// </summary>
    /// <param name="info"></param>
    /// <param name="form"></param>
    public static bool IsFormWithinRange(this IPersonalFormInfo info, byte form)
    {
        if (form == 0)
            return true;
        return form < info.FormCount;
    }
}
