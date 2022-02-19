using System;
using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// Manually entered metadata about the interesting areas.
/// </summary>
public sealed class ResidentAreaSet
{
    public readonly string ParentName;
    public readonly IReadOnlyList<string> SubAreas;

    private ResidentAreaSet(string parentName, IReadOnlyList<string> subAreas)
    {
        ParentName = parentName;
        SubAreas = subAreas;
    }

    public static readonly ResidentAreaSet[] AreaNames =
    {
        new("ha_area00", new[] {
            "ha_area00_s01",
            "ha_area00_s02",
            "ha_area00_s03_a",
            "ha_area00_s03_b",
            "ha_area00_s03_c",
            "ha_area00_s03_d",
            "ha_area00_s03_e",
            "ha_area00_s03_f",
            "ha_area00_s03_g",
            "ha_area00_s03_h",
            "ha_area00_s03_i",
            "ha_area00_s03_j",
            "ha_area00_s03_k",
            "ha_area00_s03_l",
            "ha_area00_s03_m",
            "ha_area00_s03_n",
            "ha_area00_s03_o",
            "ha_area00_s03_p",
            "ha_area00_s03_q",
            "ha_area00_s03_r",
            "ha_area00_s03_s",
            "ha_area00_s03_t",
            "ha_area00_s06",
            "ha_area00_s07",
            "ha_area00_s09",
            "ha_area00_s12_a",
            "ha_area00_s12_b",
            "ha_area00_s12_c",
            "ha_area00_s12_d",
            "ha_area00_s16_a",
            "ha_area00_s16_b",
        }),
        new("ha_area01", new[] {
            "ha_area01_s01",
        }),
        new("ha_area02", new[] {
            "ha_area02_s01",
            "ha_area02_s01_a",
            "ha_area02_s01_b",
            "ha_area02_s01_c",
            "ha_area02_s01_d",
            "ha_area02_s02",
        }),
        new("ha_area03", new[] {
            "ha_area03_s01",
            "ha_area03_s02",
            "ha_area03_s03",
            "ha_area03_s04",
        }),
        new("ha_area04", new[] {
            "ha_area04_s01",
            "ha_area04_s02",
            "ha_area04_s03",
            "ha_area04_s04",
        }),
        new("ha_area05", new[] {
            "ha_area05_s01",
            "ha_area05_s02",
            "ha_area05_s03",
            "ha_area05_s04",
            "ha_area05_s04_a",
            "ha_area05_s04_b",
            "ha_area05_s04_c",
            "ha_area05_s04_d",
        }),
        new("ha_area06", new[] {
            "ha_area06_s01",
        }),
        new("ha_area11",  Array.Empty<string>()),
    };
}
