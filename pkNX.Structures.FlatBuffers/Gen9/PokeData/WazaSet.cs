using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WazaSet
{
    [FlatBufferItem(0)] public WazaID WazaId { get; set; }
    [FlatBufferItem(1)] public sbyte PointUp { get; set; }
}
