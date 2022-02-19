using pkNX.Containers;
using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

// Not a flatbuffer; wraps the fields into a single object.
public sealed class AreaInstance8a
{
    // Area Info
    private readonly ResidentArea8a Area;
    public readonly AreaInstance8a? ParentArea;
    private readonly List<AreaInstance8a> Children = new();

    public bool IsSubArea => ParentArea != null;
    public IReadOnlyList<AreaInstance8a> SubAreas => Children;
    public string AreaName => Area.AreaName;

    // Encount
    public EncounterTable8a[] Encounters => Area.Encounters;

    // Placement
    public PlacementLocation8a[] Locations => Area.Locations;
    public PlacementSpawner8a[] Spawners => Area.Spawners;
    public PlacementSpawner8a[] Wormholes => Area.Wormholes;
    public LandmarkItemSpawn8a[] LandItems => Area.LandItems;
    public LandmarkItem8a[] LandMarks => Area.LandMarks;
    public PlacementUnnnEntry[] Unown => Area.Unown;
    public PlacementMkrgEntry[] Mikaruge => Area.Mikaruge;
    public PlacementSearchItem[] SearchItem => Area.SearchItem;

    private AreaInstance8a(GFPack resident, AreaSettings8a settings, AreaInstance8a? parentArea = null)
    {
        // General area properties
        Area = new ResidentArea8a(resident, settings);
        ParentArea = parentArea;
        Area.LoadInfo();
    }
    public static AreaInstance8a Create(GFPack resident, ResidentAreaSet areaNameList, AreaSettingsTable8a settingsTable)
    {
        var parentSettings = settingsTable.Find(areaNameList.ParentName);
        var parent = new AreaInstance8a(resident, parentSettings);
        foreach (var child in areaNameList.SubAreas)
        {
            var childSettings = settingsTable.Find(child);
            parent.Children.Add(new AreaInstance8a(resident, childSettings, parent));
        }
        return parent;
    }
}
