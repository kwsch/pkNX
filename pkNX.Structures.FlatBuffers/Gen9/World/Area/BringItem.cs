using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class BringItem
{
    [FlatBufferItem(0)] public ItemID ItemID { get; set; }
    [FlatBufferItem(1)] public sbyte BringRate { get; set; }
}
