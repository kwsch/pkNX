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
public class PlacementItemArchive8a : IFlatBufferArchive<PlacementItem8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PlacementItem8a[] Table { get; set; } = Array.Empty<PlacementItem8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementItem8a
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public ulong Field_02 { get; set; }
    [FlatBufferItem(03)] public PlacementItem8a_F03[] Field_03 { get; set; } = Array.Empty<PlacementItem8a_F03>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementItem8a_F03
{
    [FlatBufferItem(00)] public byte Field_00 { get; set; }
    [FlatBufferItem(01)] public byte Field_01 { get; set; }
    [FlatBufferItem(02)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string Field_05 { get; set; } = string.Empty;
    [FlatBufferItem(06)] public string Field_06 { get; set; } = string.Empty;
    [FlatBufferItem(07)] public PlacementV3f8a Field_07 { get; set; } = new();
    [FlatBufferItem(08)] public PlacementItem8a_F08 Field_08 { get; set; } = new();
    [FlatBufferItem(09)] public PlacementV3f8a Field_09 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementItem8a_F08
{
    [FlatBufferItem(00)] public byte Field_00 { get; set; } // unk
    [FlatBufferItem(01)] public float Field_01 { get; set; }
}
