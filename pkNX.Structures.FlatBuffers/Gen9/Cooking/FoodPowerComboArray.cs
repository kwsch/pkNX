using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FoodPowerComboArray : IFlatBufferArchive<FoodPowerCombo>
{
    [FlatBufferItem(0)] public FoodPowerCombo[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FoodPowerCombo
{
    [FlatBufferItem(0)] public FoodPowerComboParam Param { get; set; }
}
