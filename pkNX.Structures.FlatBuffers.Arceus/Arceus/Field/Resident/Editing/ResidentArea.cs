using System.Buffers;
using FlatSharp;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.Arceus;

// Not a FlatBuffer; wraps the fields into a single object.
public sealed class ResidentArea
{
    public readonly AreaSettings Settings;
    private readonly GFPack Resident;

    public ResidentArea(GFPack resident, AreaSettings settings)
    {
        Resident = resident;
        Settings = settings;
    }

    public string AreaName => Settings.Name;
    public string FriendlyAreaName => Settings.FriendlyAreaName;

    // Encount
    public EncounterDataArchive Encounters { get; private set; } = null!;

    // Placement
    public PlacementLocationArchive Locations { get; private set; } = null!;
    public PlacementSpawnerArchive Spawners { get; private set; } = null!;
    public PlacementSpawnerArchive Wormholes { get; private set; } = null!;
    public LandmarkItemSpawnTable LandItems { get; private set; } = null!;
    public LandmarkItemTable LandMarks { get; private set; } = null!;
    public PlacementUnnnTable Unown { get; private set; } = null!;
    public PlacementMkrgTable Mikaruge { get; private set; } = null!;
    public PlacementSearchItemTable SearchItem { get; private set; } = null!;

    private T TryRead<T>(string path) where T : class, IFlatBufferSerializable<T>
    {
        var index = Resident.GetIndexFull(path);
        if (index == -1)
            throw new ArgumentOutOfRangeException(nameof(path));

        var data = Resident[index];
        return T.GreedyMutableSerializer.Parse(data);
    }

    private static byte[] Write<T>(T obj) where T : class, IFlatBufferSerializable<T>
    {
        var pool = ArrayPool<byte>.Shared;
        var serializer = obj.Serializer;
        var data = pool.Rent(serializer.GetMaxSize(obj));
        var len = serializer.Write(data, obj);
        var result = data.AsSpan(0, len).ToArray();
        pool.Return(data);
        return result;
    }

    private void TryWrite<T>(string path, T obj) where T : class, IFlatBufferSerializable<T>
    {
        var index = Resident.GetIndexFull(path);
        if (index == -1)
            return;

        byte[] result = Write(obj);
        Resident[index] = result;
    }

    public void LoadInfo()
    {
        // Load encount
        Encounters = TryRead<EncounterDataArchive      >(Settings.Encounters);
        Locations  = TryRead<PlacementLocationArchive  >(Settings.Locations);
        Spawners   = TryRead<PlacementSpawnerArchive   >(Settings.Spawners);
        Wormholes  = TryRead<PlacementSpawnerArchive   >(Settings.WormholeSpawners);
        LandItems  = TryRead<LandmarkItemSpawnTable    >(Settings.LandmarkItemSpawns);
        LandMarks  = TryRead<LandmarkItemTable         >(Settings.LandmarkItems);
        Unown      = TryRead<PlacementUnnnTable        >(Settings.UnownSpawners);
        Mikaruge   = TryRead<PlacementMkrgTable        >(Settings.Mkrg);
        SearchItem = TryRead<PlacementSearchItemTable  >(Settings.SearchItem);
    }

    public void SaveInfo()
    {
        TryWrite(Settings.Encounters, Encounters);
        TryWrite(Settings.Locations, Locations);
        TryWrite(Settings.Spawners, Spawners);
        TryWrite(Settings.WormholeSpawners, Wormholes);
        TryWrite(Settings.LandmarkItemSpawns, LandItems);
        TryWrite(Settings.LandmarkItems, LandMarks);
        TryWrite(Settings.UnownSpawners, Unown);
        TryWrite(Settings.Mkrg, Mikaruge);
        TryWrite(Settings.SearchItem, SearchItem);
    }
}
