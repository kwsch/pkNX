using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidPokeScaleDataArray : IFlatBufferArchive<RaidPokeScaleData>
{
    [FlatBufferItem(0)] public RaidPokeScaleData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidPokeScaleData
{
    [FlatBufferItem(0)] public DevID Monsno { get; set; }
    [FlatBufferItem(1)] public short Formno { get; set; }
    [FlatBufferItem(2)] public SexType Sex { get; set; }
    [FlatBufferItem(3)] public float Scale { get; set; }
    [FlatBufferItem(4)] public float Offset { get; set; }
}
