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
    public class PokeResourceTable : IFlatBufferArchive<Inner>
    {
        [FlatBufferItem(00)] public DualInt Dual { get; set; }
        [FlatBufferItem(01)] public Inner[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Inner
    {
        [FlatBufferItem(00)] public EightBytes Field_00 { get; set; }
        [FlatBufferItem(01)] public string Model { get; set; }
        [FlatBufferItem(02)] public string Config { get; set; }
        [FlatBufferItem(03)] public string ArcPack { get; set; }
        [FlatBufferItem(04)] public AnimationConfigStringTuple[] Animations { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class DualInt
    {
        [FlatBufferItem(00)] public int Field0 { get; set; } = 4;
        [FlatBufferItem(01)] public int Field1 { get; set; } = 2;
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EightBytes
    {
        [FlatBufferItem(00)] public ushort Field_00 { get; set; }
        [FlatBufferItem(01)] public ushort Field_01 { get; set; }
        [FlatBufferItem(02)] public byte Field_02 { get; set; }
        [FlatBufferItem(03)] public byte Field_03 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class AnimationConfigStringTuple
    {
        [FlatBufferItem(00)] public string Name { get; set; }
        [FlatBufferItem(01)] public string Path { get; set; }
    }
}
