using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.Arceus;

// Not a FlatBuffer; wraps the fields into a single object.
public sealed class AreaInstance
{
    // Area Info
    private readonly ResidentArea Area;
    public readonly AreaInstance? ParentArea;
    private readonly List<AreaInstance> Children = [];

    public bool IsSubArea => ParentArea != null;
    public IReadOnlyList<AreaInstance> SubAreas => Children;
    public string AreaName => Area.AreaName;

    public string FriendlyAreaName => Area.FriendlyAreaName;

    // Encount
    public IList<EncounterTable> Encounters => Area.Encounters.Table;
    public IList<PlacementLocation> Locations => Area.Locations.Table;
    public IList<PlacementSpawner> Spawners => Area.Spawners.Table;
    public IList<PlacementSpawner> Wormholes => Area.Wormholes.Table;
    public IList<LandmarkItemSpawn> LandItems => Area.LandItems.Table;
    public IList<LandmarkItem> LandMarks => Area.LandMarks.Table;
    public IList<PlacementUnnnEntry> Unown => Area.Unown.Table;
    public IList<PlacementMkrgEntry> Mikaruge => Area.Mikaruge.Table;
    public IList<PlacementSearchItem> SearchItem => Area.SearchItem.Table;

    private AreaInstance(GFPack resident, AreaSettings settings, AreaInstance? parentArea = null)
    {
        // General area properties
        Area = new ResidentArea(resident, settings);
        ParentArea = parentArea;
        Area.LoadInfo();
    }
    public static AreaInstance Create(GFPack resident, ResidentAreaSet areaNameList, AreaSettingsTable settingsTable)
    {
        var parentSettings = settingsTable.Find(areaNameList.ParentName);
        var parent = new AreaInstance(resident, parentSettings);
        foreach (var child in areaNameList.SubAreas)
        {
            var childSettings = settingsTable.Find(child);
            parent.Children.Add(new AreaInstance(resident, childSettings, parent));
        }
        return parent;
    }
}
