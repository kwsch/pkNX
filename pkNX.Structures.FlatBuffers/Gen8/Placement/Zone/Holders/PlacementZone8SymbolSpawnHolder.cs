using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    // wild encounter spawner?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SymbolSpawnHolder
    {
        [FlatBufferItem(00)] public PlacementZone8SymbolSpawn Object { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SymbolSpawn
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Identifier { get; set; } = new();
        [FlatBufferItem(01)] public int Field_01 { get; set; }
        [FlatBufferItem(02)] public PlacementZone8_F20_Sub Field_02 { get; set; } = new();
        [FlatBufferItem(03)] public PlacementZone8_F20_Sub Field_03 { get; set; } = new();
        [FlatBufferItem(04)] public PlacementZone8_F20_Sub Field_04 { get; set; } = new();
        [FlatBufferItem(05)] public PlacementZone8_F20_Sub Field_05 { get; set; } = new();
        [FlatBufferItem(06)] public int Field_06 { get; set; }
        [FlatBufferItem(07)] public ulong SymbolHash { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F20_Sub
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; } // unused
        [FlatBufferItem(02)] public float Field_02 { get; set; } // unused
        [FlatBufferItem(03)] public float Field_03 { get; set; } // unused
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public float Field_05 { get; set; } // unused
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; } // unused
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }
}
