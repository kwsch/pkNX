using System.ComponentModel;
using FlatSharp;
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
    // map_data.prmb
    // map_placement_data.prmb
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MapContainer
    {
        [FlatBufferItem(0)] public SubTable0[] Field0 { get; set; }
        [FlatBufferItem(1)] public MapUnion[] Field1 { get; set; }
        [FlatBufferItem(2)] public SubTable2[] Field2 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SubTable0
    {
        [FlatBufferItem(0)] public int Field0 { get; set; }
        [FlatBufferItem(1)] public int Field1 { get; set; }
        [FlatBufferItem(2)] public int Field2 { get; set; }
        [FlatBufferItem(3)] public int Field3 { get; set; }

        public override string ToString() => $"{Field0}|{Field1}|{Field2}|{Field3}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MapUnion
    {
#nullable enable
        [FlatBufferItem(0)] public FlatBufferUnion<ST1_1, ST1_2, ST1_3, ST1_4>? Field1 { get; set; }
#nullable disable
        public override string ToString() => Field1?.Discriminator switch
        {
            1 => Field1.Item1.ToString(),
            2 => Field1.Item3.ToString(),
            4 => Field1.Item4.ToString(),
            _ => "Empty",
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class ST1_1 {[FlatBufferItem(0)] public int Value { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class ST1_2 {[FlatBufferItem(0)] public byte Dummy { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class ST1_3 {[FlatBufferItem(0)] public string Name { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class ST1_4 {[FlatBufferItem(0)] public ulong Hash { get; set; } }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SubTable2
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public DualHash[] Pairs { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class DualHash
    {
        [FlatBufferItem(0)] public ulong Hash0 { get; set; }
        [FlatBufferItem(1)] public ulong Hash1 { get; set; }

        public override string ToString() => $"{Hash0:X16} {Hash1:X16}";
    }
}
