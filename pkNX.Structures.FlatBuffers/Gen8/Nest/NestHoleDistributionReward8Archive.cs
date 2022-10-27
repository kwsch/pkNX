using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleDistributionReward8Archive : IFlatBufferArchive<NestHoleDistributionReward8Table>
{
    [FlatBufferItem(0)] public NestHoleDistributionReward8Table[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleDistributionReward8Table : INestHoleRewardTable
{
    [FlatBufferItem(0)] public ulong TableID { get; set; }
    [FlatBufferItem(1)] public NestHoleDistributionReward8[] Entries { get; set; }

    public INestHoleReward[] Rewards => Entries;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleDistributionReward8 : INestHoleReward
{
    [FlatBufferItem(0)] public byte Value0 { get; set; }
    [FlatBufferItem(1)] public byte Value1 { get; set; }
    [FlatBufferItem(2)] public byte Value2 { get; set; }
    [FlatBufferItem(3)] public byte Value3 { get; set; }
    [FlatBufferItem(4)] public byte Value4 { get; set; }
    [FlatBufferItem(5)] public ushort ItemID { get; set; }

    public uint Item => ItemID;

    public uint[] Values
    {
        get => new uint[] { Value0, Value1, Value2, Value3, Value4 };
        set
        {
            Value0 = (byte)value[0];
            Value1 = (byte)value[1];
            Value2 = (byte)value[2];
            Value3 = (byte)value[3];
            Value4 = (byte)value[4];
        }
    }
}
