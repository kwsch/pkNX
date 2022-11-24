using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SeasoningDataArray : IFlatBufferArchive<SeasoningData>
{
    [FlatBufferItem(0)] public SeasoningData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SeasoningData
{
    [FlatBufferItem(0)] public SeasoningType Type { get; set; }
    [FlatBufferItem(1)] public IngredientParam Param { get; set; }
    [FlatBufferItem(2)] public FoodPowerParam Power { get; set; }
    [FlatBufferItem(3)] public FoodPokeTypeParam PokeTypePower { get; set; }
    [FlatBufferItem(4)] public string ObjTempName { get; set; }
    [FlatBufferItem(5)] public int SetNum { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class IngredientParam
{
    [FlatBufferItem(0)] public int Sweet { get; set; }
    [FlatBufferItem(1)] public int Salty { get; set; }
    [FlatBufferItem(2)] public int Sour { get; set; }
    [FlatBufferItem(3)] public int Bitter { get; set; }
    [FlatBufferItem(4)] public int Hot { get; set; }
    [FlatBufferItem(5)] public int Deliciousness { get; set; }
}
