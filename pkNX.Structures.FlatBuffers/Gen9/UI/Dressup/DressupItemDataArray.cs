using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupItemDataArray : IFlatBufferArchive<DressupItemData>
{
    [FlatBufferItem(0)] public DressupItemData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupItemData
{
    [FlatBufferItem(0)] public int DressupItemid { get; set; }
    [FlatBufferItem(1)] public DressupItemType Dressup { get; set; }
    [FlatBufferItem(2)] public DressupBlandType Bland { get; set; }
    [FlatBufferItem(3)] public string Name { get; set; }
    [FlatBufferItem(4)] public string Subname { get; set; }
    [FlatBufferItem(5)] public int Price { get; set; }
    [FlatBufferItem(6)] public int Sortnum { get; set; }
    [FlatBufferItem(7)] public string ModelLabel { get; set; }
    [FlatBufferItem(8)] public string SoundTag { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum DressupBlandType
{
    DUMMY = -1,
    BROTHERFOOT = 0,
    chica = 1,
    DOPEGRANMA = 2,
    MOTHAT = 3,
    DENSOKU = 4,
    ALL_AGES = 5,
    SO_ATUMORI = 6,
    ULINGEAR = 7,
    STARCRAZY = 8,
    DEFOG = 9,
    YOCINOSOMEI = 10,
    DECA_SPORTS = 11,
    Ruggle = 12,
    Ortiga = 13,
    ROTOM = 14,
}

[FlatBufferEnum(typeof(int))]
public enum DressupItemType
{
    UNIFORM = 0,
    LEG = 1,
    FOOT = 2,
    GLOVE = 3,
    BAG = 4,
    HEAD = 5,
    EYE = 6,
    ROTOM = 7,
}
