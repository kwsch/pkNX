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
public class ThrowParamTable8a : IFlatBufferArchive<ThrowParam8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowParam8a[] Table { get; set; } = Array.Empty<ThrowParam8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowParam8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public float Velocity { get; set; } // more speed
    [FlatBufferItem(02)] public float Arc { get; set; } // more arc
    [FlatBufferItem(03)] public float GravityDirection { get; set; } // higher values make it fall less, and go less forward
    [FlatBufferItem(04)] public float ThrowAngle { get; set; } // higher values go straight up?

    public string Dump() => $"{Hash:X16}\t{Velocity}\t{Arc}\t{GravityDirection}\t{ThrowAngle}";
}
