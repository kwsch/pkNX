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

/// <summary>
/// Seems to be a file left over from development. Only has test entries
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollisionArchive8a : IFlatBufferArchive<PokeCaptureCollision8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeCaptureCollision8a[] Table { get; set; } = Array.Empty<PokeCaptureCollision8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollision8a
{
    [FlatBufferItem(00)] public uint Species { get; set; }
    [FlatBufferItem(01)] public uint Form { get; set; }
    [FlatBufferItem(02)] public PokeCaptureCollider[] Colliders { get; set; } = Array.Empty<PokeCaptureCollider>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollider
{
    [FlatBufferItem(00)] public string SocketName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Shape { get; set; } = string.Empty;
    [FlatBufferItem(02)] public float[] ShapeParameters { get; set; } = Array.Empty<float>();
    [FlatBufferItem(03)] public float OffsetX { get; set; }
    [FlatBufferItem(04)] public float OffsetY { get; set; }
    [FlatBufferItem(05)] public float OffsetZ { get; set; }
    [FlatBufferItem(06)] public string Type { get; set; } = string.Empty;
    [FlatBufferItem(07)] public string Field_07 { get; set; } = string.Empty;
}
