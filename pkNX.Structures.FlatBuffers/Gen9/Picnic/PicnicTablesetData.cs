using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PicnicTablesetData
{
    [FlatBufferItem(0)] public NormalPicnicTableInfo Normal { get; set; }
    [FlatBufferItem(1)] public CookingPicnicTableInfo Cooking { get; set; }
    [FlatBufferItem(2)] public CookedPicnicTableInfo Cooked { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PicnicTablePlayerItemInfo
{
    [FlatBufferItem(0)] public SRT ChairPos { get; set; }
    [FlatBufferItem(1)] public SRT WaterBottlePos { get; set; }
    [FlatBufferItem(2)] public SRT TumblerPos { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NormalPicnicTableInfo
{
    [FlatBufferItem(0)] public SRT BucketPos { get; set; }
    [FlatBufferItem(1)] public SRT LanternPos { get; set; }
    [FlatBufferItem(2)] public PicnicTablePlayerItemInfo Player0 { get; set; }
    [FlatBufferItem(3)] public PicnicTablePlayerItemInfo Player1 { get; set; }
    [FlatBufferItem(4)] public PicnicTablePlayerItemInfo Player2 { get; set; }
    [FlatBufferItem(5)] public PicnicTablePlayerItemInfo Player3 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientPicnicTableInfo
{
    [FlatBufferItem(0)] public SRT Pos0 { get; set; }
    [FlatBufferItem(1)] public SRT Pos1 { get; set; }
    [FlatBufferItem(2)] public SRT Pos2 { get; set; }
    [FlatBufferItem(3)] public SRT Pos3 { get; set; }
    [FlatBufferItem(4)] public SRT Pos4 { get; set; }
    [FlatBufferItem(5)] public SRT Pos5 { get; set; }
    [FlatBufferItem(6)] public SRT Pos6 { get; set; }
    [FlatBufferItem(7)] public SRT Pos7 { get; set; }
    [FlatBufferItem(8)] public SRT Pos8 { get; set; }
    [FlatBufferItem(9)] public SRT Pos9 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CookingPicnicTableInfo
{
    [FlatBufferItem(0)] public SRT DishPos { get; set; }
    [FlatBufferItem(1)] public SRT BucketPos { get; set; }
    [FlatBufferItem(2)] public SRT LanternPos { get; set; }
    [FlatBufferItem(3)] public IngredientPicnicTableInfo Ingredient { get; set; }
    [FlatBufferItem(4)] public SeasoningPicnicTableInfo Seasoning { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CookedPicnicTableInfo
{
    [FlatBufferItem(00)] public SRT Berryjam01Pos { get; set; }
    [FlatBufferItem(01)] public SRT Berryjam02Pos { get; set; }
    [FlatBufferItem(02)] public SRT BottleRedPos { get; set; }
    [FlatBufferItem(03)] public SRT BottleYellowPos { get; set; }
    [FlatBufferItem(04)] public SRT BucketPos { get; set; }
    [FlatBufferItem(05)] public SRT Butter01Pos { get; set; }
    [FlatBufferItem(06)] public SRT Butter02Pos { get; set; }
    [FlatBufferItem(07)] public SRT Creamcheese01Pos { get; set; }
    [FlatBufferItem(08)] public SRT DishPos { get; set; }
    [FlatBufferItem(09)] public SRT LanternPos { get; set; }
    [FlatBufferItem(10)] public SRT Marmalade04Pos { get; set; }
    [FlatBufferItem(11)] public SRT Oliveoil03Pos { get; set; }
    [FlatBufferItem(12)] public SRT Smallbowl01OliveoilPos { get; set; }
    [FlatBufferItem(13)] public SRT Smallbowl01VinegarPos { get; set; }
    [FlatBufferItem(14)] public SRT Watercress01Pos { get; set; }
    [FlatBufferItem(15)] public SRT Watercress02Pos { get; set; }
    [FlatBufferItem(16)] public SRT Watercress03Pos { get; set; }
    [FlatBufferItem(17)] public SRT Watercress01Shadow01Pos { get; set; }
    [FlatBufferItem(18)] public SRT Watercress01Shadow02Pos { get; set; }
    [FlatBufferItem(19)] public SRT Cocotteplate06WhippedcreamPos { get; set; }
    [FlatBufferItem(20)] public SRT Marmalade01Pos { get; set; }
    [FlatBufferItem(21)] public SRT Chilisauce01Pos { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SeasoningPicnicTableInfo
{
    [FlatBufferItem(0)] public SRT Pos0 { get; set; }
    [FlatBufferItem(1)] public SRT Pos1 { get; set; }
    [FlatBufferItem(2)] public SRT Pos2 { get; set; }
    [FlatBufferItem(3)] public SRT Pos3 { get; set; }
}
