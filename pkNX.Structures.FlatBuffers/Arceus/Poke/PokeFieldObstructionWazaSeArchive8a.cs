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
public class PokeFieldObstructionWazaSeArchive8a : IFlatBufferArchive<PokeFieldObstructionWazaSe8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeFieldObstructionWazaSe8a[] Table { get; set; } = Array.Empty<PokeFieldObstructionWazaSe8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeFieldObstructionWazaSe8a
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; } // ?
    [FlatBufferItem(01)] public uint Field_01 { get; set; } // ?
    [FlatBufferItem(02)] public uint Field_02 { get; set; } // ?
}
