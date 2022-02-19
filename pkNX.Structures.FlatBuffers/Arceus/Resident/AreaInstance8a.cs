using System;
using pkNX.Containers;
using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

// Not a flatbuffer; wraps the fields into a single object.
public sealed class AreaInstance8a
{
    // Area Info
    public readonly AreaInstance8a? ParentArea;
    private readonly List<AreaInstance8a> Children = new();
    private readonly AreaSettings8a Settings;
    private readonly GFPack Resident;

    public string AreaName => Settings.Name;
    public bool IsSubArea => ParentArea != null;
    public IReadOnlyList<AreaInstance8a> SubAreas => Children;

    // Encount
    public EncounterTable8a[] Encounters { get; private set; } = Array.Empty<EncounterTable8a>();

    // Placement
    public PlacementLocation8a[] Locations { get; private set; } = Array.Empty<PlacementLocation8a>();
    public PlacementSpawner8a[] Spawners { get; private set; } = Array.Empty<PlacementSpawner8a>();
    public PlacementSpawner8a[] Wormholes { get; private set; } = Array.Empty<PlacementSpawner8a>();
    public LandmarkItemSpawn8a[] LandItems { get; private set; } = Array.Empty<LandmarkItemSpawn8a>();
    public LandmarkItem8a[] LandMarks { get; private set; } = Array.Empty<LandmarkItem8a>();
    public PlacementUnnnEntry[] Unown { get; private set; } = Array.Empty<PlacementUnnnEntry>();
    public PlacementMkrgEntry[] Mikaruge { get; private set; } = Array.Empty<PlacementMkrgEntry>();
    public PlacementSearchItem[] SearchItem { get; private set; } = Array.Empty<PlacementSearchItem>();

    private AreaInstance8a(GFPack resident, AreaSettings8a settings, AreaInstance8a? parentArea = null)
    {
        // General area properties
        Resident = resident;
        Settings = settings;
        ParentArea = parentArea;

        LoadInfo();
    }

    private void LoadInfo()
    {
        // Load encount
        var enc = Resident.GetDataFullPath(Settings.Encounters);
        Encounters = FlatBufferConverter.DeserializeFrom<EncounterDataArchive8a>(enc).Table;

        // Load placement
        var locationf = Resident.GetDataFullPath(Settings.Locations);
        var spawnerf = Resident.GetDataFullPath(Settings.Spawners);
        var wh_spawnerf = Resident.GetDataFullPath(Settings.WormholeSpawners);
        var l_spawnerf = Resident.GetDataFullPath(Settings.LandmarkItemSpawns);
        var l_markf = Resident.GetDataFullPath(Settings.LandmarkItems);
        var unnf = Resident.GetDataFullPath(Settings.UnownSpawners);
        var mkrgf = Resident.GetDataFullPath(Settings.Mkrg);
        var psif = Resident.GetDataFullPath(Settings.SearchItem);

        Locations = FlatBufferConverter.DeserializeFrom<PlacementLocationArchive8a>(locationf).Table;
        Spawners = FlatBufferConverter.DeserializeFrom<PlacementSpawnerArchive8a>(spawnerf).Table;
        Wormholes = FlatBufferConverter.DeserializeFrom<PlacementSpawnerArchive8a>(wh_spawnerf).Table;
        LandItems = FlatBufferConverter.DeserializeFrom<LandmarkItemSpawnTable8a>(l_spawnerf).Table;
        LandMarks = FlatBufferConverter.DeserializeFrom<LandmarkItemTable8a>(l_markf).Table;
        Unown = FlatBufferConverter.DeserializeFrom<PlacementUnnnTable>(unnf).Table;
        Mikaruge = FlatBufferConverter.DeserializeFrom<PlacementMkrgTable>(mkrgf).Table;
        SearchItem = FlatBufferConverter.DeserializeFrom<PlacementSearchItemTable>(psif).Table;
    }

    private void SaveInfo()
    {
        Resident.SetDataFullPath(Settings.Encounters, new EncounterDataArchive8a { Table = Encounters }.Write());
        Resident.SetDataFullPath(Settings.Locations, new PlacementLocationArchive8a { Table = Locations }.Write());
        Resident.SetDataFullPath(Settings.Spawners, new PlacementSpawnerArchive8a { Table = Spawners }.Write());
        Resident.SetDataFullPath(Settings.WormholeSpawners, new PlacementSpawnerArchive8a { Table = Wormholes }.Write());
        Resident.SetDataFullPath(Settings.LandmarkItemSpawns, new LandmarkItemSpawnTable8a { Table = LandItems }.Write());
        Resident.SetDataFullPath(Settings.LandmarkItems, new LandmarkItemTable8a { Table = LandMarks }.Write());
        Resident.SetDataFullPath(Settings.UnownSpawners, new PlacementUnnnTable { Table = Unown }.Write());
        Resident.SetDataFullPath(Settings.Mkrg, new PlacementMkrgTable { Table = Mikaruge }.Write());
        Resident.SetDataFullPath(Settings.SearchItem, new PlacementSearchItemTable { Table = SearchItem }.Write());
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
