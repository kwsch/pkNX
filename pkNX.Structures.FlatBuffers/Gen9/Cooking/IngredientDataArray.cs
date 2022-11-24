using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientDataArray : IFlatBufferArchive<IngredientData>
{
    [FlatBufferItem(0)] public IngredientData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientData
{
    [FlatBufferItem(0)] public IngredientType Type { get; set; }
    [FlatBufferItem(1)] public IngredientParam Param { get; set; }
    [FlatBufferItem(2)] public FoodPowerParam Power { get; set; }
    [FlatBufferItem(3)] public FoodPokeTypeParam PokeTypePower { get; set; }
    [FlatBufferItem(4)] public string ObjTempName { get; set; }
    [FlatBufferItem(5)] public int SetNum { get; set; }
    [FlatBufferItem(6)] public int OnDishNum { get; set; }
}
