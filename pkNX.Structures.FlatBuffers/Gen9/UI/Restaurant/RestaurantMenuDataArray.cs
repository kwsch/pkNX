using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RestaurantMenuDataArray : IFlatBufferArchive<RestaurantMenuData>
{
    [FlatBufferItem(0)] public RestaurantMenuData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RestaurantMenuData
{
    [FlatBufferItem(00)] public string Menuid { get; set; }
    [FlatBufferItem(01)] public int Sortnum { get; set; }
    [FlatBufferItem(02)] public bool IsPicnic { get; set; }
    [FlatBufferItem(03)] public int FoodTextureIndex { get; set; }
    [FlatBufferItem(04)] public string Cond { get; set; }
    [FlatBufferItem(05)] public int Price { get; set; }
    [FlatBufferItem(06)] public string Menuname { get; set; }
    [FlatBufferItem(07)] public string MenuInfoText { get; set; }
    [FlatBufferItem(08)] public string Bufid { get; set; }
    [FlatBufferItem(09)] public TasteCategory TasteCategory { get; set; }
    [FlatBufferItem(10)] public TasteDeliciousness TasteDeliciousness { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum TasteCategory
{
    Spicy = 0,
    Sweet = 1,
    Salty = 2,
    Sour = 3,
    Bitter = 4,
}

[FlatBufferEnum(typeof(int))]
public enum TasteDeliciousness
{
    Bad = 0,
    Normal = 1,
    Delicious = 2,
    VeryDelicious = 3,
}
