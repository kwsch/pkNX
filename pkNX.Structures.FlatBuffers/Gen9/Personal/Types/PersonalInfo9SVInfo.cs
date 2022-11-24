using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVInfo
{
    [FlatBufferItem(00)] public ushort DexIndexNational { get; set; }
    [FlatBufferItem(01)] public ushort Form             { get; set; }
    [FlatBufferItem(02)] public ushort SpeciesCopy      { get; set; }
    [FlatBufferItem(03)] public byte   Color            { get; set; }
    [FlatBufferItem(04)] public byte   BodyType         { get; set; }
    [FlatBufferItem(05)] public ushort Height           { get; set; }
    [FlatBufferItem(06)] public ushort Weight           { get; set; }
    [FlatBufferItem(07)] public uint   Reserved0        { get; set; }
    [FlatBufferItem(08)] public uint   Reserved1        { get; set; }
    [FlatBufferItem(09)] public uint   Reserved2        { get; set; }
}
