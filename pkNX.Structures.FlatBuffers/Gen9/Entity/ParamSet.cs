using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ParamSet
{
    [FlatBufferItem(0)] public int HP { get; set; }
    [FlatBufferItem(1)] public int ATK { get; set; }
    [FlatBufferItem(2)] public int DEF { get; set; }
    [FlatBufferItem(3)] public int SPA { get; set; }
    [FlatBufferItem(4)] public int SPD { get; set; }
    [FlatBufferItem(5)] public int SPE { get; set; }
}
