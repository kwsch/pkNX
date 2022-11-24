using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSymbolSetting
{
    [FlatBufferItem(0)] public short MinScale { get; set; }
    [FlatBufferItem(1)] public sbyte RepopPercent { get; set; }
    [FlatBufferItem(2)] public sbyte AddItemNum { get; set; }
    [FlatBufferItem(3)] public sbyte AddItemPercent { get; set; }
}
