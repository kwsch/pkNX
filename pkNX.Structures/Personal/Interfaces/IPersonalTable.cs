namespace pkNX.Structures;

/// <summary>
/// Exposes info about all <see cref="IPersonalInfo"/> contained in the object.
/// </summary>
public interface IPersonalTable
{
    /// <summary>
    /// Max Species ID (National Dex) that is stored in the table.
    /// </summary>
    int MaxSpeciesID { get; }

    IPersonalInfo[] Table { get; }

    /// <summary>
    /// Gets an index from the inner array.
    /// </summary>
    /// <remarks>Has built in length checks; returns empty (0) entry if out of range.</remarks>
    /// <param name="index">Index to retrieve</param>
    /// <returns>Requested index entry</returns>
    IPersonalInfo this[int index] { get; }

    /// <summary>
    /// Alternate way of fetching <see cref="GetFormEntry"/>.
    /// </summary>
    IPersonalInfo this[ushort species, byte form] { get; }

    /// <summary>
    /// Gets the <see cref="PersonalInfo"/> entry index for a given <see cref="PKM.Species"/> and <see cref="PKM.Form"/>.
    /// </summary>
    /// <param name="species"><see cref="PKM.Species"/></param>
    /// <param name="form"><see cref="PKM.Form"/></param>
    /// <returns>Entry index for the input criteria</returns>
    int GetFormIndex(ushort species, byte form);

    /// <summary>
    /// Gets the <see cref="PersonalInfo"/> entry for a given <see cref="PKM.Species"/> and <see cref="PKM.Form"/>.
    /// </summary>
    /// <param name="species"><see cref="PKM.Species"/></param>
    /// <param name="form"><see cref="PKM.Form"/></param>
    /// <returns>Entry for the input criteria</returns>
    IPersonalInfo GetFormEntry(ushort species, byte form);

    /// <summary>
    /// Checks if the <see cref="PKM.Species"/> is within the bounds of the table.
    /// </summary>
    /// <param name="species"><see cref="PKM.Species"/></param>
    /// <returns>True if present in game</returns>
    bool IsSpeciesInGame(ushort species);

    /// <summary>
    /// Checks if the <see cref="PKM.Species"/> and <see cref="PKM.Form"/> is within the bounds of the table.
    /// </summary>
    /// <param name="species"><see cref="PKM.Species"/></param>
    /// <param name="form"><see cref="PKM.Form"/></param>
    /// <returns>True if present in game</returns>
    bool IsPresentInGame(ushort species, byte form);

    /// <summary>
    /// Save any changes made to the table back to the original file
    /// </summary>
    void Save();
}

/// <summary>
/// Generic interface for exposing specific <see cref="IPersonalInfo"/> retrieval methods.
/// </summary>
/// <typeparam name="T">Specific type of <see cref="IPersonalInfo"/> the table contains.</typeparam>
public interface IPersonalTable<out T> where T : IPersonalInfo
{
    T[] Table { get; }
    T this[int index] { get; }
    T this[ushort species, byte form] { get; }
    T GetFormEntry(ushort species, byte form);
}


public static class IPersonalTableExt
{
    /// <summary>
    /// Gets form names for every species.
    /// </summary>
    /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
    /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
    /// <returns>Array of species containing an array of form names for that species.</returns>
    public static string[][] GetFormList(this IPersonalTable pt, string[] species)
    {
        string[][] FormList = new string[pt.MaxSpeciesID + 1][];
        for (int i = 0; i < FormList.Length; i++)
        {
            int FormCount = pt[i].FormCount;
            FormList[i] = new string[FormCount];
            if (FormCount <= 0) continue;
            FormList[i][0] = species[i];
            for (int j = 1; j < FormCount; j++)
                FormList[i][j] = $"{species[i]} {j}";
        }

        return FormList;
    }

    /// <summary>
    /// Gets an arranged list of Form names and indexes for use with the individual <see cref="PersonalInfo"/> AltForm ID values.
    /// </summary>
    /// <param name="AltForms">Raw string resource (Forms) for the corresponding table.</param>
    /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
    /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
    /// <param name="baseForm">Pointers for base form IDs</param>
    /// <param name="formVal">Pointers for table indexes for each form</param>
    /// <returns>Sanitized list of species names, and outputs indexes for various lookup purposes.</returns>
    public static string[] GetPersonalEntryList(this IPersonalTable pt, string[][] AltForms, string[] species, out int[] baseForm, out int[] formVal)
    {
        string[] result = new string[pt.Table.Length];
        baseForm = new int[result.Length];
        formVal = new int[result.Length];
        for (int i = 0; i <= pt.MaxSpeciesID; i++)
        {
            result[i] = species[i];
            if (AltForms[i].Length == 0) continue;
            int altformpointer = pt[i].FormStatsIndex;
            if (altformpointer <= 0) continue;
            for (int j = 1; j < AltForms[i].Length; j++)
            {
                int ptr = altformpointer + j - 1;
                baseForm[ptr] = i;
                formVal[ptr] = j;
                result[ptr] = AltForms[i][j];
            }
        }
        return result;
    }
}