using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVHatch
{
    [FlatBufferItem(0)] public ushort Species       { get; set; }
    [FlatBufferItem(1)] public ushort Form          { get; set; }
    [FlatBufferItem(2)] public ushort RegionalFlags { get; set; }
    [FlatBufferItem(3)] public ushort EverstoneForm { get; set; }

    public override string ToString() => $"{Species:000}\t{Form:00}";
}
