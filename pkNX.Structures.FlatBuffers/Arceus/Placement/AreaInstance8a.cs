using pkNX.Containers;
using System;
using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers;

// Not a flatbuffer; wraps the fields into a single object.
public sealed class AreaInstance8a
{
    public readonly string AreaName;

    public readonly PlacementLocation8a[] Locations;
    public readonly PlacementSpawner8a[] Spawners;
    public readonly PlacementSpawner8a[] Wormholes;
    public readonly LandmarkItemSpawn8a[] LandItems;
    public readonly LandmarkItem8a[] LandMarks;
    public readonly PlacementUnnnEntry[] Unown;
    public readonly PlacementMkrgEntry[] Mikaruge;

    public readonly EncounterDataArchive8a Encounters;

    public readonly AreaInstance8a? ParentArea;
    public readonly List<AreaInstance8a> SubAreas;

    public bool IsSubArea => ParentArea != null;

    private AreaInstance8a(GFPack resident, string areaName, AreaInstance8a? parentArea)
    {
        var f = resident.GetDataFullPath($"bin/encount/{areaName}/encount_data.bin");
        Encounters = FlatBufferConverter.DeserializeFrom<EncounterDataArchive8a>(f);

        var locationf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/location/location.bin");
        var spawnerf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/spawner/spawner_data.bin");
        var wh_spawnerf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/wh_spawner/spawner_data.bin");
        var l_spawnerf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/landmark_item/expansion/landmark_item_spawn_table.bin");
        var l_markf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/landmark_item/landmark_item.bin");
        var unnf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/unnn/unnn.bin");
        var mkrgf = resident.GetDataFullPath($"bin/field/param/placement/{areaName}/mkrg/mkrg.bin");

        Locations = FlatBufferConverter.DeserializeFrom<PlacementLocationArchive8a>(locationf).Table;
        Spawners = FlatBufferConverter.DeserializeFrom<PlacementSpawnerArchive8a>(spawnerf).Table;
        Wormholes = FlatBufferConverter.DeserializeFrom<PlacementSpawnerArchive8a>(wh_spawnerf).Table;
        LandItems = FlatBufferConverter.DeserializeFrom<LandmarkItemSpawnTable8a>(l_spawnerf).Table;
        LandMarks = FlatBufferConverter.DeserializeFrom<LandmarkItemTable8a>(l_markf).Table;
        Unown = FlatBufferConverter.DeserializeFrom<PlacementUnnnTable>(unnf).Table;
        Mikaruge = FlatBufferConverter.DeserializeFrom<PlacementMkrgTable>(mkrgf).Table;

        AreaName = areaName;

        ParentArea = parentArea;
        SubAreas = new List<AreaInstance8a>();
    }

    public static AreaInstance8a Create(GFPack resident, string[] areaNameList)
    {
        if (areaNameList.Length < 1)
            throw new ArgumentException("Invalid area name list!");

        var parentArea = new AreaInstance8a(resident, areaNameList[0], null);

        for (var i = 1; i < areaNameList.Length; i++)
            parentArea.SubAreas.Add(new AreaInstance8a(resident, areaNameList[i], parentArea));

        return parentArea;
    }
}
