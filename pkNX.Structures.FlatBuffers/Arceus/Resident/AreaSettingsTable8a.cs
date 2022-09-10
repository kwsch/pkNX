﻿using System;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaSettingsTable8a : IFlatBufferArchive<AreaSettings8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public AreaSettings8a[] Table { get; set; } = Array.Empty<AreaSettings8a>();

    public AreaSettings8a Find(string name) => Table.First(z => z.Name == name);
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaSettings8a
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string NameParent { get; set; } = string.Empty;
    [FlatBufferItem(02)] public PlacementV3f8a Position { get; set; } = new();
    [FlatBufferItem(03)] public bool Flag_03 { get; set; }
    [FlatBufferItem(04)] public bool Flag_04 { get; set; }
    [FlatBufferItem(05)] public bool Flag_05 { get; set; }
    [FlatBufferItem(06)] public bool Flag_06 { get; set; }
    [FlatBufferItem(07)] public bool Flag_07 { get; set; }
    [FlatBufferItem(08)] public bool Flag_08 { get; set; }
    [FlatBufferItem(09)] public string NPC { get; set; } = string.Empty;
    [FlatBufferItem(10)] public string NPCPokemon { get; set; } = string.Empty;
    [FlatBufferItem(11)] public string BgParts { get; set; } = string.Empty;
    [FlatBufferItem(12)] public string Emitter { get; set; } = string.Empty;
    [FlatBufferItem(13)] public string Item { get; set; } = string.Empty;
    [FlatBufferItem(14)] public string SearchItem { get; set; } = string.Empty;
    [FlatBufferItem(15)] public string Spawners { get; set; } = string.Empty;
    [FlatBufferItem(16)] public string Pos { get; set; } = string.Empty;
    [FlatBufferItem(17)] public string BgEvent { get; set; } = string.Empty;
    [FlatBufferItem(18)] public string Encounters { get; set; } = string.Empty;
    [FlatBufferItem(19)] public string NavMesh { get; set; } = string.Empty;
    [FlatBufferItem(20)] public string TerrainTable { get; set; } = string.Empty;
    [FlatBufferItem(21)] public string NameOther { get; set; } = string.Empty;
    [FlatBufferItem(22)] public bool Flag_22 { get; set; }
    [FlatBufferItem(23)] public bool Flag_23 { get; set; }
    [FlatBufferItem(24)] public string LandmarkItems { get; set; } = string.Empty;
    [FlatBufferItem(25)] public string LandmarkItemSpawns { get; set; } = string.Empty;
    [FlatBufferItem(26)] public string AttributeInfo { get; set; } = string.Empty;
    [FlatBufferItem(27)] public string Locations { get; set; } = string.Empty;
    [FlatBufferItem(28)] public string BattleStartPoint { get; set; } = string.Empty;
    [FlatBufferItem(29)] public string SoundEvent { get; set; } = string.Empty;
    [FlatBufferItem(30)] public string AreaCamera { get; set; } = string.Empty;
    [FlatBufferItem(31)] public string TargetAI { get; set; } = string.Empty;
    [FlatBufferItem(32)] public string Slot0 { get; set; } = string.Empty;
    [FlatBufferItem(33)] public AreaBGMTable8a BGM { get; set; } = new();
    [FlatBufferItem(34)] public string Wormholes { get; set; } = string.Empty;
    [FlatBufferItem(35)] public string WormholeSpawners { get; set; } = string.Empty;
    [FlatBufferItem(36)] public string WormholeItems { get; set; } = string.Empty;
    [FlatBufferItem(37)] public string OverViewDepth { get; set; } = string.Empty;
    [FlatBufferItem(38)] public string RealTimeEventData { get; set; } = string.Empty;
    [FlatBufferItem(39)] public string Mkrg { get; set; } = string.Empty;
    [FlatBufferItem(40)] public string AreaWall { get; set; } = string.Empty;
    [FlatBufferItem(41)] public string Balloon { get; set; } = string.Empty;
    [FlatBufferItem(42)] public string UnownSpawners { get; set; } = string.Empty;
    [FlatBufferItem(43)] public string MultiShapeSoundEvent { get; set; } = string.Empty;
    [FlatBufferItem(44)] public ulong Field_44 { get; set; }
    [FlatBufferItem(45)] public string Path { get; set; } = string.Empty;
    [FlatBufferItem(46)] public string PopupEvent { get; set; } = string.Empty;
    [FlatBufferItem(47)] public string Invisible { get; set; } = string.Empty;
    [FlatBufferItem(48)] public string LocalWeather { get; set; } = string.Empty;
    [FlatBufferItem(49)] public string SoundBank { get; set; } = string.Empty;
    [FlatBufferItem(50)] public AreaSettings8a_F50 Field_50 { get; set; } = new();
    [FlatBufferItem(51)] public string Archive { get; set; } = string.Empty;
    [FlatBufferItem(52)] public PlacementV3f8a Field_52 { get; set; } = new();
    [FlatBufferItem(53)] public PlacementV3f8a Field_53 { get; set; } = new();
    [FlatBufferItem(54)] public ulong VisibleFlagHash { get; set; } //A flag from system_works
    [FlatBufferItem(55)] public bool Field_55 { get; set; }

    // Get the in game name of the area
    public string FriendlyAreaName
    {
        get
        {
            return Name switch
            {
                "ha_area00" => "Jubilife Village",
                "ha_area01" => "Obsidian Fieldlands",
                "ha_area02" => "Crimson Mirelands",
                "ha_area03" => "Cobalt Coastlands",
                "ha_area04" => "Cornet Highlands",
                "ha_area05" => "Alabaster Icelands",
                "ha_area06" => "Ancient Retreat",
                _ => string.Format("Unknown Area ({0})", Name),
            };
        }
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaSettings8a_F50
{
    [FlatBufferItem(0)] public int Field_00 { get; set; }
    [FlatBufferItem(1)] public int Field_01 { get; set; }
    [FlatBufferItem(2)] public int Field_02 { get; set; }
    [FlatBufferItem(3)] public int Field_03 { get; set; }
}
