using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVStats
{
    [FlatBufferItem(0)] public byte HP  { get; set; }
    [FlatBufferItem(1)] public byte ATK { get; set; }
    [FlatBufferItem(2)] public byte DEF { get; set; }
    [FlatBufferItem(3)] public byte SPA { get; set; }
    [FlatBufferItem(4)] public byte SPD { get; set; }
    [FlatBufferItem(5)] public byte SPE { get; set; }

    // Return the 2 bits of each, shifted left with HP as the lowest bits.
    public ushort U16() => (ushort)((HP & 0b11) | ((ATK & 0b11) << 2) | ((DEF & 0b11) << 4) | ((SPA & 0b11) << 6) | ((SPD & 0b11) << 8) | ((SPE & 0b11) << 10));
}
