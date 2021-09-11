using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers
{
    // wild encounter spawner?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SymbolSpawnHolder
    {
        [FlatBufferItem(00)] public PlacementZone8SymbolSpawn Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SymbolSpawn
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        // 1
        [FlatBufferItem(02)] public PlacementZone8_F20_Sub Field_02 { get; set; }
        [FlatBufferItem(03)] public PlacementZone8_F20_Sub Field_03 { get; set; }
        [FlatBufferItem(04)] public PlacementZone8_F20_Sub Field_04 { get; set; }
        [FlatBufferItem(05)] public PlacementZone8_F20_Sub Field_05 { get; set; }
        [FlatBufferItem(06)] public int Field_06 { get; set; }
        [FlatBufferItem(07)] public ulong SymbolHash { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F20_Sub
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        // 1
        // 2
        // 3
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }
}
