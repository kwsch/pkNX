using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RewardDataArray : IFlatBufferArchive<RewardData>
{
    [FlatBufferItem(0)] public RewardData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RewardData
{
    [FlatBufferItem(0)] public PokedexType PokedexType { get; set; }
    [FlatBufferItem(1)] public int CaptureNum { get; set; }
    [FlatBufferItem(2)] public ItemID ItemId { get; set; }
    [FlatBufferItem(3)] public int ItemNum { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum PokedexType
{
    TITAN = 0,
}
