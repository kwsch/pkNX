using System;
using System.ComponentModel;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaSettingsTable
{
    public AreaSettings Find(string name) => Table.First(z => z.Name == name);
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaSettings
{
    // Get the in game name of the area
    public string FriendlyAreaName
    {
        get
        {
            return Name switch
            {
                "ha_area00" => "Jubilife Village",
                "ha_area01" => "Obsidian Fieldlands",
                "ha_area02" => "Crimson Mirelands",
                "ha_area03" => "Cobalt Coastlands",
                "ha_area04" => "Coronet Highlands",
                "ha_area05" => "Alabaster Icelands",
                "ha_area06" => "Ancient Retreat",
                _ => $"Unknown Area ({Name})",
            };
        }
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaSettings_F50 { }
