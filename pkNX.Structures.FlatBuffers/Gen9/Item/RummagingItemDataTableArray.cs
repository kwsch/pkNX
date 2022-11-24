using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RummagingItemDataTableArray : IFlatBufferArchive<RummagingItemDataTable>
{
    [FlatBufferItem(0)] public RummagingItemDataTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RummagingItemDataTable
{
    [FlatBufferItem(0)] public RummagingCategory Category { get; set; }
    [FlatBufferItem(1)] public RummagingPattern Pattern { get; set; }
    [FlatBufferItem(2)] public ItemID Item00 { get; set; }
    [FlatBufferItem(3)] public ItemID Item01 { get; set; }
    [FlatBufferItem(4)] public ItemID Item02 { get; set; }
    [FlatBufferItem(5)] public ItemID Item03 { get; set; }
    [FlatBufferItem(6)] public ItemID Item04 { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum RummagingCategory
{
    NONE = 0,
    BUSH = 1,
    ROCK = 2,
    UNDER_WATER = 3,
    IN_THE_GROUND = 4,
    HIGH_ALTITUDE = 5,
}

[FlatBufferEnum(typeof(int))]
public enum RummagingPattern
{
    NONE = 0,
    NORMAL = 1,
    NUT = 2,
    RARE = 3,
}
