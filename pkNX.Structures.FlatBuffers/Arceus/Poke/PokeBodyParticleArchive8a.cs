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
public class PokeBodyParticleArchive8a : IFlatBufferArchive<PokeBodyParticle8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeBodyParticle8a[] Table { get; set; } = Array.Empty<PokeBodyParticle8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeBodyParticle8a
{
    [FlatBufferItem(00)] public uint Species { get; set; }
    [FlatBufferItem(01)] public uint Form { get; set; }
    [FlatBufferItem(02)] public bool Flag_02 { get; set; }
    [FlatBufferItem(03)] public PokeBodyParticle8a_F03[] Field_03 { get; set; } = Array.Empty<PokeBodyParticle8a_F03>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeBodyParticle8a_F03
{
    [FlatBufferItem(00)] public bool Flag_00 { get; set; }
    [FlatBufferItem(01)] public bool Flag_01 { get; set; }
    [FlatBufferItem(02)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string Field_05 { get; set; } = string.Empty;
}
