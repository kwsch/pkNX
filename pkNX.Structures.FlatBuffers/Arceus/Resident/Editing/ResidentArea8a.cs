using System;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers;

// Not a flatbuffer; wraps the fields into a single object.
public sealed class ResidentArea8a
{
    public readonly AreaSettings8a Settings;
    private readonly GFPack Resident;

    public ResidentArea8a(GFPack resident, AreaSettings8a settings)
    {
        Resident = resident;
        Settings = settings;
    }

    public string AreaName => Settings.Name;

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

    private T2[] TryRead<T1, T2>(string path) where T1 : class, IFlatBufferArchive<T2> where T2 : class
    {
        var index = Resident.GetIndexFull(path);
        if (index == -1)
            return Array.Empty<T2>();

        var data = Resident[index];
        return FlatBufferConverter.DeserializeFrom<T1>(data).Table;
    }

    private void TryWrite(string path, byte[] obj)
    {
        var index = Resident.GetIndexFull(path);
        if (index == -1)
            return;

        Resident[index] = obj;
    }

    private void TryWrite<T1>(string path, T1 obj) where T1 : class =>
        TryWrite(path, FlatBufferConverter.SerializeFrom(obj));

    public void LoadInfo()
    {
        // Load encount
        Encounters = TryRead<EncounterDataArchive8a    , EncounterTable8a     >(Settings.Encounters);
        Locations  = TryRead<PlacementLocationArchive8a, PlacementLocation8a  >(Settings.Locations);
        Spawners   = TryRead<PlacementSpawnerArchive8a , PlacementSpawner8a   >(Settings.Spawners);
        Wormholes  = TryRead<PlacementSpawnerArchive8a , PlacementSpawner8a   >(Settings.WormholeSpawners);
        LandItems  = TryRead<LandmarkItemSpawnTable8a  , LandmarkItemSpawn8a  >(Settings.LandmarkItemSpawns);
        LandMarks  = TryRead<LandmarkItemTable8a       , LandmarkItem8a       >(Settings.LandmarkItems);
        Unown      = TryRead<PlacementUnnnTable        , PlacementUnnnEntry   >(Settings.UnownSpawners);
        Mikaruge   = TryRead<PlacementMkrgTable        , PlacementMkrgEntry   >(Settings.Mkrg);
        SearchItem = TryRead<PlacementSearchItemTable  , PlacementSearchItem  >(Settings.SearchItem);
    }

    public void SaveInfo()
    {
        TryWrite(Settings.Encounters, new EncounterDataArchive8a { Table = Encounters }.Write());
        TryWrite(Settings.Locations, new PlacementLocationArchive8a { Table = Locations }.Write());
        TryWrite(Settings.Spawners, new PlacementSpawnerArchive8a { Table = Spawners }.Write());
        TryWrite(Settings.WormholeSpawners, new PlacementSpawnerArchive8a { Table = Wormholes }.Write());
        TryWrite(Settings.LandmarkItemSpawns, new LandmarkItemSpawnTable8a { Table = LandItems }.Write());
        TryWrite(Settings.LandmarkItems, new LandmarkItemTable8a { Table = LandMarks }.Write());
        TryWrite(Settings.UnownSpawners, new PlacementUnnnTable { Table = Unown }.Write());
        TryWrite(Settings.Mkrg, new PlacementMkrgTable { Table = Mikaruge }.Write());
        TryWrite(Settings.SearchItem, new PlacementSearchItemTable { Table = SearchItem }.Write());
    }
}
