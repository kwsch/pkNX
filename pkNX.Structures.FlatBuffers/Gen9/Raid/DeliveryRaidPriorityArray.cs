using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidPriorityArray : IFlatBufferArchive<DeliveryRaidPriority>
{
    [FlatBufferItem(0)] public DeliveryRaidPriority[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidPriority
{
    [FlatBufferItem(0)] public int VersionNo { get; set; }
    [FlatBufferItem(1)] public DeliveryGroupID DeliveryGroupID { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryGroupID
{
    // GroupID[10] : byte
    [FlatBufferItem(0)] public sbyte GroupID01 { get; set; }
    [FlatBufferItem(1)] public sbyte GroupID02 { get; set; }
    [FlatBufferItem(2)] public sbyte GroupID03 { get; set; }
    [FlatBufferItem(3)] public sbyte GroupID04 { get; set; }
    [FlatBufferItem(4)] public sbyte GroupID05 { get; set; }
    [FlatBufferItem(5)] public sbyte GroupID06 { get; set; }
    [FlatBufferItem(6)] public sbyte GroupID07 { get; set; }
    [FlatBufferItem(7)] public sbyte GroupID08 { get; set; }
    [FlatBufferItem(8)] public sbyte GroupID09 { get; set; }
    [FlatBufferItem(9)] public sbyte GroupID10 { get; set; }
}
