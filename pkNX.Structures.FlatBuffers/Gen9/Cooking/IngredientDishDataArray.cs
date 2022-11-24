using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientDishDataArray : IFlatBufferArchive<IngredientDishData>
{
    [FlatBufferItem(0)] public IngredientDishData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientDishData
{
    [FlatBufferItem(0)] public IngredientType Type { get; set; }
    [FlatBufferItem(1)] public IngredientDishParam Param { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientDishParam : IFlatBufferArchive<IngredientSRT>
{
    // IngredientSRT[20]
    [FlatBufferItem(00)] public IngredientSRT Ingredient00 { get; set; }
    [FlatBufferItem(01)] public IngredientSRT Ingredient01 { get; set; }
    [FlatBufferItem(02)] public IngredientSRT Ingredient02 { get; set; }
    [FlatBufferItem(03)] public IngredientSRT Ingredient03 { get; set; }
    [FlatBufferItem(04)] public IngredientSRT Ingredient04 { get; set; }
    [FlatBufferItem(05)] public IngredientSRT Ingredient05 { get; set; }
    [FlatBufferItem(06)] public IngredientSRT Ingredient06 { get; set; }
    [FlatBufferItem(07)] public IngredientSRT Ingredient07 { get; set; }
    [FlatBufferItem(08)] public IngredientSRT Ingredient08 { get; set; }
    [FlatBufferItem(09)] public IngredientSRT Ingredient09 { get; set; }
    [FlatBufferItem(10)] public IngredientSRT Ingredient10 { get; set; }
    [FlatBufferItem(11)] public IngredientSRT Ingredient11 { get; set; }
    [FlatBufferItem(12)] public IngredientSRT Ingredient12 { get; set; }
    [FlatBufferItem(13)] public IngredientSRT Ingredient13 { get; set; }
    [FlatBufferItem(14)] public IngredientSRT Ingredient14 { get; set; }
    [FlatBufferItem(15)] public IngredientSRT Ingredient15 { get; set; }
    [FlatBufferItem(16)] public IngredientSRT Ingredient16 { get; set; }
    [FlatBufferItem(17)] public IngredientSRT Ingredient17 { get; set; }
    [FlatBufferItem(18)] public IngredientSRT Ingredient18 { get; set; }
    [FlatBufferItem(19)] public IngredientSRT Ingredient19 { get; set; }

    [Browsable(false)]
    public IngredientSRT[] Table
    {
        get => new[]
        {
            Ingredient00,
            Ingredient01,
            Ingredient02,
            Ingredient03,
            Ingredient04,
            Ingredient05,
            Ingredient06,
            Ingredient07,
            Ingredient08,
            Ingredient09,
            Ingredient10,
            Ingredient11,
            Ingredient12,
            Ingredient13,
            Ingredient14,
            Ingredient15,
            Ingredient16,
            Ingredient17,
            Ingredient18,
            Ingredient19,
        };
        set => throw new System.InvalidOperationException();
    }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientSRT
{
    [FlatBufferItem(0)] public Float3 Position { get; set; }
    [FlatBufferItem(1)] public Float3 Rotation { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Float3
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }
}
