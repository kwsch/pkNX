using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class OwnerInfo
{
    [FlatBufferItem(0)] public int TrainerId { get; set; }
    [FlatBufferItem(1)] public SexType Sex { get; set; }
    [FlatBufferItem(2)] public LangType LangId { get; set; }
    [FlatBufferItem(3)] public string Name { get; set; }
}
