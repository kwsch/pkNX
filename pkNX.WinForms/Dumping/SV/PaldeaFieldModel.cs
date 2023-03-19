using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public class PaldeaFieldModel
{
    private readonly IList<FieldMainArea> mainAreas;
    private readonly IList<FieldSubArea> subAreas;
    private readonly IList<FieldInsideArea> insideAreas;
    private readonly IList<FieldDungeonArea> dungeonAreas;
    private readonly IList<FieldLocation> fieldLocations;

    public PaldeaFieldModel(IFileInternal ROM)
    {
        mainAreas = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
        subAreas = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
        insideAreas = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
        dungeonAreas = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
        fieldLocations = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;
    }

    public AreaInfo FindAreaInfo(string name)
    {
        foreach (var area in mainAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in subAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in insideAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in dungeonAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in fieldLocations)
        {
            if (area.Name == name)
                return area.Info;
        }
        throw new ArgumentException($"Unknown area {name}");
    }
}
