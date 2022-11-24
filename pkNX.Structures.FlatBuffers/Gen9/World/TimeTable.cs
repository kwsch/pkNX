using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TimeTable
{
    [FlatBufferItem(0)] public bool Morning { get; set; }
    [FlatBufferItem(1)] public bool Noon { get; set; }
    [FlatBufferItem(2)] public bool Evening { get; set; }
    [FlatBufferItem(3)] public bool Night { get; set; }
}
