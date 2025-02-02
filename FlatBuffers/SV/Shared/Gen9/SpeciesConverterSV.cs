using System;

namespace pkNX.Structures.FlatBuffers.SV;

/// <summary>
/// <see cref="DevID"/> does not match National Dex ID.
/// </summary>
public static class SpeciesConverterSV
{
    public static T[] GetRearrangedAsNational<T>(T[] specNames)
    {
        var result = new T[specNames.Length];
        for (ushort indexInternal = 0; indexInternal < specNames.Length; indexInternal++)
        {
            var indexNational = GetNational9(indexInternal);
            if (indexNational >= specNames.Length) continue;
            result[indexNational] = specNames[indexInternal];
        }
        return result;
    }

    /// <summary>
    /// Converts a National Dex ID to Generation 9 internal species ID.
    /// </summary>
    /// <param name="species">National Dex ID</param>
    /// <returns>Generation 9 species ID.</returns>
    public static ushort GetInternal9(ushort species)
    {
        var shift = species - FirstUnalignedNational9;
        var table = Table9NationalToInternal;
        if ((uint)shift >= table.Length)
            return species;
        return (ushort)(species + table[shift]);
    }

    /// <summary>
    /// Converts a Generation 9 internal species ID to National Dex ID.
    /// </summary>
    /// <param name="raw">Generation 9 species ID.</param>
    /// <returns>National Dex ID.</returns>
    public static ushort GetNational9(ushort raw)
    {
        var table = Table9InternalToNational;
        var shift = raw - FirstUnalignedInternal9;
        if ((uint)shift >= table.Length)
            return raw;
        return (ushort)(raw + table[shift]);
    }

    private const int FirstUnalignedNational9 = 917;
    private const int FirstUnalignedInternal9 = FirstUnalignedNational9;

    /// <summary>
    /// Difference of National Dex IDs (index) and the associated Gen9 Species IDs (value)
    /// </summary>
    private static ReadOnlySpan<sbyte> Table9NationalToInternal =>
    [
                                           001, 001, 001,
        001, 033, 033, 033, 021, 021, 044, 044, 007, 007,
        007, 029, 031, 031, 031, 068, 068, 068, 002, 002,
        017, 017, 030, 030, 024, 024, 028, 028, 058, 058,
        012, -13, -13, -31, -31, -29, -29, 043, 043, 043,
        -31, -31, -03, -30, -30, -23, -23, -14, -24, -03,
        -03, -47, -47, -12, -27, -27, -44, -46, -26, 031,
        029, -53, -65, 025, -06, -03, -07, -04, -04, -08,
        -04, 001, -03, -03, -06, -04, -47, -47, -47, -23,
        -23, -05, -07, -09, -07, -20, -13, -09, -09, -29,
        -23, 001, 012, 012, 000, 000, 000, -06, 005, -06,
        -03, -03, -02, -04, -03, -03,
    ];

    /// <summary>
    /// Difference of Gen9 Species IDs (index) and the associated National Dex IDs (value)
    /// </summary>
    private static ReadOnlySpan<sbyte> Table9InternalToNational =>
    [
                                           065, -01, -01,
        -01, -01, 031, 031, 047, 047, 029, 029, 053, 031,
        031, 046, 044, 030, 030, -07, -07, -07, 013, 013,
        -02, -02, 023, 023, 024, -21, -21, 027, 027, 047,
        047, 047, 026, 014, -33, -33, -33, -17, -17, 003,
        -29, 012, -12, -31, -31, -31, 003, 003, -24, -24,
        -44, -44, -30, -30, -28, -28, 023, 023, 006, 007,
        029, 008, 003, 004, 004, 020, 004, 023, 006, 003,
        003, 004, -01, 013, 009, 007, 005, 007, 009, 009,
        -43, -43, -43, -68, -68, -68, -58, -58, -25, -29,
        -31, 006, -01, 006, 000, 000, 000, 003, 003, 004,
        002, 003, 003, -05, -12, -12,
    ];
}
