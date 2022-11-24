using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SchoolMapDataArray : IFlatBufferArchive<SchoolMapData>
{
    [FlatBufferItem(0)] public SchoolMapData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SchoolMapData
{
    [FlatBufferItem(0)] public SchoolMapIDEnum MapID { get; set; }
    [FlatBufferItem(1)] public string SystemFlagName { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum SchoolMapIDEnum
{
    Entrance = 0,
    Dining = 1,
    Shop = 2,
    BiologicalRoom = 3,
    ArtRoom = 4,
    HomeEconomicsRoom = 5,
    StaffRoom = 6,
    PrincipalsRoom = 7,
    HealthRoom = 8,
    Schoolyard = 9,
    PlayerClassRoom = 10,
    Friend03ClassRoom = 11,
    Friend02ClassRoom = 12,
    PlayerRoom = 13,
    Friend01Room = 14,
    Friend02Room = 15,
    Friend03Room = 16,
}
