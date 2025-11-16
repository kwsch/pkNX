using System;
using FlatSharp;
using pkNX.Game;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public class World9a
{
    // New: explicit map instances for 4 maps (0..3)
    public readonly LumioseMap Overworld;
    public readonly LumioseMap Labs;
    public readonly LumioseMap Sewer1;
    public readonly LumioseMap Sewer2;

    public World9a(GameManager9a rom)
    {
        // Map 0: load data
        Overworld = new LumioseMap(
            Get<FieldMainAreaArray>(rom, "world/ik_data/field/area/field_main_area/field_main_area_array.bin").Table,
            Get<FieldSubAreaArray>(rom, "world/ik_data/field/area/field_sub_area/field_sub_area_array.bin").Table,
            Get<FieldBattleZoneArray>(rom, "world/ik_data/field/area/field_battle_zone/field_battle_zone_array.bin").Table,
            Get<FieldWildZoneArray>(rom, "world/ik_data/field/area/field_wild_zone/field_wild_zone_array.bin").Table,
            Get<FieldSceneAreaArray>(rom, "world/ik_data/field/area/field_scene_area/field_scene_area_array.bin").Table,
            Get<FieldLocationArray>(rom, "world/ik_data/field/area/field_location/field_location_array.bin").Table
        );
        Overworld.LoadScenes(rom, ScenesT1);

        Labs = new LumioseMap([], [], [], [], [], []);
        Sewer1 = new LumioseMap([], [], [], [], [], []);
        Sewer2 = new LumioseMap([], [], [], [], [], []);
    }

    public LumioseMap GetMap(int index) => index switch
    {
        0 => Overworld,
        1 => Labs,
        2 => Sewer1,
        3 => Sewer2,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    private static T Get<T>(GameManager9a rom, string path) where T : class, IFlatBufferSerializable<T>
        => FlatBufferConverter.DeserializeFrom<T>(rom.GetPackedFile(path));

    private static readonly string[] ScenesT1 =
    [
        // Order scenes so that the collision check finds the inner ones first -- might have some overlaps in main map?
        "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision_/area_collision_0.trscn", // zones (wild, battle)
        "world/ik_scene/field/area/t1/sub_scene/t1_field_area_/t1_field_area_0.trscn", // general map

        // Others, implicitly visited via recursion from the main scene.
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0102_b01_/area_col_a0102_b01_0.trscn",
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0102_b02_/area_col_a0102_b02_0.trscn",
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0202_w01_01_/area_col_a0202_w01_01_0.trscn",
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0202_w01_02_/area_col_a0202_w01_02_0.trscn",
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0502_w01_01_/area_col_a0502_w01_01_0.trscn",
        // "world/ik_scene/field/area/t1/sub_scene/t1_zone/area_collision/area_col_a0502_w01_02_/area_col_a0502_w01_02_0.trscn",
    ];
}

// Concrete implementation for current ZA map handling
