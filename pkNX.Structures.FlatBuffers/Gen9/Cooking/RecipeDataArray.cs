using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RecipeDataArray : IFlatBufferArchive<RecipeData>
{
    [FlatBufferItem(0)] public RecipeData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RecipeData
{
    [FlatBufferItem(00)] public RecipeType Type { get; set; }
    [FlatBufferItem(01)] public SeasoningType Seasoning0 { get; set; }
    [FlatBufferItem(02)] public SeasoningType Seasoning1 { get; set; }
    [FlatBufferItem(03)] public SeasoningType Seasoning2 { get; set; }
    [FlatBufferItem(04)] public SeasoningType Seasoning3 { get; set; }
    [FlatBufferItem(05)] public IngredientType Ingredient0 { get; set; }
    [FlatBufferItem(06)] public IngredientType Ingredient1 { get; set; }
    [FlatBufferItem(07)] public IngredientType Ingredient2 { get; set; }
    [FlatBufferItem(08)] public IngredientType Ingredient3 { get; set; }
    [FlatBufferItem(09)] public IngredientType Ingredient4 { get; set; }
    [FlatBufferItem(10)] public IngredientType Ingredient5 { get; set; }
}
