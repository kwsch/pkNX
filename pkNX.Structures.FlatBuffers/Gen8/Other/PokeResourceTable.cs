using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    // poke_resource_table.gfbpmcatalog
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PokeResourceTable : IFlatBufferArchive<PokeModelConfig>
    {
        [FlatBufferItem(00)] public PokeResourceMeta Meta { get; set; }
        [FlatBufferItem(01)] public PokeModelConfig[] Table { get; set; }
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
        [FlatBufferItem(00)] public PokeModelMeta Meta { get; set; }
        [FlatBufferItem(01)] public string ModelPath { get; set; }
        [FlatBufferItem(02)] public string ConfigPath { get; set; }
        [FlatBufferItem(03)] public string ArchivePath { get; set; }
        [FlatBufferItem(04)] public AnimationConfigStringTuple[] Animations { get; set; }
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
        [FlatBufferItem(00)] public string Name { get; set; }
        [FlatBufferItem(01)] public string Path { get; set; }
    }
}
