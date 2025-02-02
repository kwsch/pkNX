namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class AreaSettingsTable
{
    public AreaSettings Find(string name) => Table.First(z => z.Name == name);
}

public partial class AreaSettings
{
    // Get the in game name of the area
    public string FriendlyAreaName
    {
        get => Name switch
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
