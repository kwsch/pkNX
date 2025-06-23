using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public class PaldeaFieldModel
{
	private const int MapCount = 3;
    public IList<FieldMainArea>[] MainAreas { get; } = new IList<FieldMainArea>[MapCount];
    public IList<FieldSubArea>[] SubAreas { get; } = new IList<FieldSubArea>[MapCount];
    public IList<FieldInsideArea>[] InsideAreas { get; } = new IList<FieldInsideArea>[MapCount];
    public IList<FieldDungeonArea>[] DungeonAreas { get; } = new IList<FieldDungeonArea>[MapCount];
    public IList<FieldLocation>[] FieldLocations { get; } = new IList<FieldLocation>[MapCount];

    public PaldeaFieldModel(IFileInternal ROM)
    {
        MainAreas[(int)PaldeaFieldIndex.Paldea] = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
        SubAreas[(int)PaldeaFieldIndex.Paldea] = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
        InsideAreas[(int)PaldeaFieldIndex.Paldea] = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
        DungeonAreas[(int)PaldeaFieldIndex.Paldea] = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
        FieldLocations[(int)PaldeaFieldIndex.Paldea] = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;

        MainAreas[(int)PaldeaFieldIndex.Kitakami] = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_main_area_su1/field_main_area_su1_array.bin")).Table;
        SubAreas[(int)PaldeaFieldIndex.Kitakami] = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_sub_area_su1/field_sub_area_su1_array.bin")).Table;
        InsideAreas[(int)PaldeaFieldIndex.Kitakami] = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_inside_area_su1/field_inside_area_su1_array.bin")).Table;
        DungeonAreas[(int)PaldeaFieldIndex.Kitakami] = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_dungeon_area_su1/field_dungeon_area_su1_array.bin")).Table;
        FieldLocations[(int)PaldeaFieldIndex.Kitakami] = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area_su1/field_location_su1/field_location_su1_array.bin")).Table;

        MainAreas[(int)PaldeaFieldIndex.Terarium] = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area_su2/field_main_area_su2/field_main_area_su2_array.bin")).Table;
        SubAreas[(int)PaldeaFieldIndex.Terarium] = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area_su2/field_sub_area_su2/field_sub_area_su2_array.bin")).Table;
        InsideAreas[(int)PaldeaFieldIndex.Terarium] = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area_su2/field_inside_area_su2/field_inside_area_su2_array.bin")).Table;
        DungeonAreas[(int)PaldeaFieldIndex.Terarium] = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area_su2/field_dungeon_area_su2/field_dungeon_area_su2_array.bin")).Table;
        FieldLocations[(int)PaldeaFieldIndex.Terarium] = []; //FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area_su2/field_location_su2/field_location_su2_array.bin")).Table;
    }

    public AreaInfo FindAreaInfo(PaldeaFieldIndex index, string name)
    {
        foreach (var area in MainAreas[(int)index])
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in SubAreas[(int)index])
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in InsideAreas[(int)index])
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in DungeonAreas[(int)index])
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in FieldLocations[(int)index])
        {
            if (area.Name == name)
                return area.Info;
        }
        throw new ArgumentException($"Unknown area {name}");
    }
}

public enum PaldeaFieldIndex
{
    Paldea = 0,
    Kitakami = 1,
    Terarium = 2,
}
