using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LbPointsTable : IFlatBufferArchive<LbPointsInfo>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public LbPointsInfo[] Table { get; set; } = Array.Empty<LbPointsInfo>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LbPointsInfo
{
    [FlatBufferItem(00)] public int Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public int   Field_04 { get; set; }
    [FlatBufferItem(05)] public ulong Field_05 { get; set; }
}
