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
    // poke_memory_data.prmb
    // this is oddly similar to the map data FlatBuffer, assumed the same schema to encode an excel table?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MemoryTable
    {
        [FlatBufferItem(0)] public MemoryQuadInt[] QuadTable { get; set; }
        [FlatBufferItem(1)] public MemoryUnion[] MainTable { get; set; }
        [FlatBufferItem(2)] public MemoryTable2[] DualTable { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MemoryQuadInt
    {
        [FlatBufferItem(0)] public int Field0 { get; set; }
        [FlatBufferItem(1)] public int Field1 { get; set; }
        [FlatBufferItem(2)] public int Field2 { get; set; }
        [FlatBufferItem(3)] public int Field3 { get; set; }

        public override string ToString() => $"{Field0}|{Field1}|{Field2}|{Field3}";
    }

	[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MemoryUnion
    {
#nullable enable
        [FlatBufferItem(0)] public FlatBufferUnion<MT1_1, MT1_2, MT1_3, MT1_4>? Field1 { get; set; }
#nullable disable
        public override string ToString() => Field1?.Discriminator switch
        {
            1 => Field1.Item1.ToString(),
            2 => Field1.Item3.ToString(),
            4 => Field1.Item4.ToString(),
            _ => "Empty",
        };
    }

	[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class MT1_1 {[FlatBufferItem(0)] public int Value { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class MT1_2 {[FlatBufferItem(0)] public byte Dummy { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class MT1_3 {[FlatBufferItem(0)] public string Name { get; set; } }
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class MT1_4 {[FlatBufferItem(0)] public ulong Hash { get; set; } }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MemoryTable2
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public MemoryDualHash[] Pairs { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MemoryDualHash
	{
        [FlatBufferItem(0)] public ulong Hash0 { get; set; }
        [FlatBufferItem(1)] public ulong Hash1 { get; set; }

        public override string ToString() => $"{Hash0:X16} {Hash1:X16}";
    }
}
