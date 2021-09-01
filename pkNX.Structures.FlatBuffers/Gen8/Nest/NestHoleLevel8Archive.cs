using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class NestHoleLevel8Archive
    {
        [FlatBufferItem(0)] public NestHoleLevel8Table[] Tables { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class NestHoleLevel8Table
    {
        [FlatBufferItem(0)] public ulong TableID { get; set; }
        [FlatBufferItem(1)] public NestHoleLevel8[] Entries { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class NestHoleLevel8
    {
        [FlatBufferItem(0)] public uint MinLevel { get; set; }
        [FlatBufferItem(1)] public uint MaxLevel { get; set; }
    }
}
