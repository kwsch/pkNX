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
    [FlatBufferItem(0)] public Transform ChairPos { get; set; }
    [FlatBufferItem(1)] public Transform WaterBottlePos { get; set; }
    [FlatBufferItem(2)] public Transform TumblerPos { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NormalPicnicTableInfo
{
    [FlatBufferItem(0)] public Transform BucketPos { get; set; }
    [FlatBufferItem(1)] public Transform LanternPos { get; set; }
    [FlatBufferItem(2)] public PicnicTablePlayerItemInfo Player0 { get; set; }
    [FlatBufferItem(3)] public PicnicTablePlayerItemInfo Player1 { get; set; }
    [FlatBufferItem(4)] public PicnicTablePlayerItemInfo Player2 { get; set; }
    [FlatBufferItem(5)] public PicnicTablePlayerItemInfo Player3 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientPicnicTableInfo
{
    [FlatBufferItem(0)] public Transform Pos0 { get; set; }
    [FlatBufferItem(1)] public Transform Pos1 { get; set; }
    [FlatBufferItem(2)] public Transform Pos2 { get; set; }
    [FlatBufferItem(3)] public Transform Pos3 { get; set; }
    [FlatBufferItem(4)] public Transform Pos4 { get; set; }
    [FlatBufferItem(5)] public Transform Pos5 { get; set; }
    [FlatBufferItem(6)] public Transform Pos6 { get; set; }
    [FlatBufferItem(7)] public Transform Pos7 { get; set; }
    [FlatBufferItem(8)] public Transform Pos8 { get; set; }
    [FlatBufferItem(9)] public Transform Pos9 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CookingPicnicTableInfo
{
    [FlatBufferItem(0)] public Transform DishPos { get; set; }
    [FlatBufferItem(1)] public Transform BucketPos { get; set; }
    [FlatBufferItem(2)] public Transform LanternPos { get; set; }
    [FlatBufferItem(3)] public IngredientPicnicTableInfo Ingredient { get; set; }
    [FlatBufferItem(4)] public SeasoningPicnicTableInfo Seasoning { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CookedPicnicTableInfo
{
    [FlatBufferItem(00)] public Transform Berryjam01Pos { get; set; }
    [FlatBufferItem(01)] public Transform Berryjam02Pos { get; set; }
    [FlatBufferItem(02)] public Transform BottleRedPos { get; set; }
    [FlatBufferItem(03)] public Transform BottleYellowPos { get; set; }
    [FlatBufferItem(04)] public Transform BucketPos { get; set; }
    [FlatBufferItem(05)] public Transform Butter01Pos { get; set; }
    [FlatBufferItem(06)] public Transform Butter02Pos { get; set; }
    [FlatBufferItem(07)] public Transform Creamcheese01Pos { get; set; }
    [FlatBufferItem(08)] public Transform DishPos { get; set; }
    [FlatBufferItem(09)] public Transform LanternPos { get; set; }
    [FlatBufferItem(10)] public Transform Marmalade04Pos { get; set; }
    [FlatBufferItem(11)] public Transform Oliveoil03Pos { get; set; }
    [FlatBufferItem(12)] public Transform Smallbowl01OliveoilPos { get; set; }
    [FlatBufferItem(13)] public Transform Smallbowl01VinegarPos { get; set; }
    [FlatBufferItem(14)] public Transform Watercress01Pos { get; set; }
    [FlatBufferItem(15)] public Transform Watercress02Pos { get; set; }
    [FlatBufferItem(16)] public Transform Watercress03Pos { get; set; }
    [FlatBufferItem(17)] public Transform Watercress01Shadow01Pos { get; set; }
    [FlatBufferItem(18)] public Transform Watercress01Shadow02Pos { get; set; }
    [FlatBufferItem(19)] public Transform Cocotteplate06WhippedcreamPos { get; set; }
    [FlatBufferItem(20)] public Transform Marmalade01Pos { get; set; }
    [FlatBufferItem(21)] public Transform Chilisauce01Pos { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SeasoningPicnicTableInfo
{
    [FlatBufferItem(0)] public Transform Pos0 { get; set; }
    [FlatBufferItem(1)] public Transform Pos1 { get; set; }
    [FlatBufferItem(2)] public Transform Pos2 { get; set; }
    [FlatBufferItem(3)] public Transform Pos3 { get; set; }
}
