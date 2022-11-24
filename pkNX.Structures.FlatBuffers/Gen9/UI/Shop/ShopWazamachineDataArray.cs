using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ShopWazamachineDataArray : IFlatBufferArchive<ShopWazamachineData>
{
    [FlatBufferItem(0)] public ShopWazamachineData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ShopWazamachineData
{
    [FlatBufferItem(00)] public int WazaNo { get; set; }
    [FlatBufferItem(01)] public ItemID WazaItemID { get; set; }
    [FlatBufferItem(02)] public int LP { get; set; }
    [FlatBufferItem(03)] public CondEnum Cond { get; set; }
    [FlatBufferItem(04)] public string CondValue { get; set; }
    [FlatBufferItem(05)] public ItemID Item01 { get; set; }
    [FlatBufferItem(06)] public int ItemNum01 { get; set; }
    [FlatBufferItem(07)] public int DevNo01 { get; set; }
    [FlatBufferItem(08)] public ItemID Item02 { get; set; }
    [FlatBufferItem(09)] public int ItemNum02 { get; set; }
    [FlatBufferItem(10)] public int DevNo02 { get; set; }
    [FlatBufferItem(11)] public ItemID Item03 { get; set; }
    [FlatBufferItem(12)] public int ItemNum03 { get; set; }
    [FlatBufferItem(13)] public int DevNo03 { get; set; }
}
