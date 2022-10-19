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
public class PokeDefaultLocatorArchive8a : IFlatBufferArchive<PokeDefaultLocator8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeDefaultLocator8a[] Table { get; set; } = Array.Empty<PokeDefaultLocator8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDefaultLocator8a
{
    [FlatBufferItem(00)] public string Locator { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Socket { get; set; } = string.Empty;
    [FlatBufferItem(02)] public Vec3f Position { get; set; } = Vec3f.Zero;
    [FlatBufferItem(03)] public Vec3f Rotation { get; set; } = Vec3f.Zero;
    [FlatBufferItem(04)] public Vec3f Scale { get; set; } = Vec3f.One;
}
