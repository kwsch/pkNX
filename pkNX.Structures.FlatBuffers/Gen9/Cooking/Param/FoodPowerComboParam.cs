using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class FoodPowerComboParam
{
    // SeasoningType[4]
    [FlatBufferItem(0)] public SeasoningType Seasoning0 { get; set; }
    [FlatBufferItem(1)] public SeasoningType Seasoning1 { get; set; }
    [FlatBufferItem(2)] public SeasoningType Seasoning2 { get; set; }
    [FlatBufferItem(3)] public SeasoningType Seasoning3 { get; set; }

    // IngredientType[6]
    [FlatBufferItem(4)] public IngredientType Ingredient0 { get; set; }
    [FlatBufferItem(5)] public IngredientType Ingredient1 { get; set; }
    [FlatBufferItem(6)] public IngredientType Ingredient2 { get; set; }
    [FlatBufferItem(7)] public IngredientType Ingredient3 { get; set; }
    [FlatBufferItem(8)] public IngredientType Ingredient4 { get; set; }
    [FlatBufferItem(9)] public IngredientType Ingredient5 { get; set; }

    [FlatBufferItem(10)] public FoodPowerParam FoodPower { get; set; }
    [FlatBufferItem(11)] public FoodPokeTypeParam PokeTypePower { get; set; }
}
