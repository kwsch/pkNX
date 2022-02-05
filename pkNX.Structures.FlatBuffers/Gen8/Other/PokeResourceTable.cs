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

// poke_resource_table.gfbpmcatalog
/// <summary> <see cref="PokeResourceTable"/> for Sword/Shield</summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeResourceTable : IFlatBufferArchive<PokeModelConfig>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeResourceMeta Meta { get; set; } = new();
    [FlatBufferItem(01)] public PokeModelConfig[] Table { get; set; } = Array.Empty<PokeModelConfig>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeResourceMeta
{
    [FlatBufferItem(00)] public int Field0 { get; set; } = 4;
    [FlatBufferItem(01)] public int Field1 { get; set; } = 2;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelConfig
{
    [FlatBufferItem(00)] public PokeModelMeta Meta { get; set; } = new();
    [FlatBufferItem(01)] public string PathModel { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string PathConfig { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string PathArchive { get; set; } = string.Empty;
    [FlatBufferItem(04)] public byte Unused { get; set; } // unused
    [FlatBufferItem(05)] public AnimationConfigStringTuple[] Base { get; set; } = Array.Empty<AnimationConfigStringTuple>();
    [FlatBufferItem(06)] public AnimationConfigStringTuple[] Eff { get; set; } = Array.Empty<AnimationConfigStringTuple>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelMeta
{
    [FlatBufferItem(00)] public ushort Species { get; set; }
    [FlatBufferItem(01)] public ushort Form { get; set; }
    [FlatBufferItem(02)] public byte Gender { get; set; }
    [FlatBufferItem(03)] public byte Shiny { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AnimationConfigStringTuple
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Path { get; set; } = string.Empty;
}
