using System.ComponentModel;
using FlatSharp.Attributes;
#nullable disable
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FixedSymbolTableArray : IFlatBufferArchive<FixedSymbolTable>
{
    [FlatBufferItem(0)] public FixedSymbolTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FixedSymbolTable
{
    [FlatBufferItem(0)] public string TableKey { get; set; }
    [FlatBufferItem(1)] public PokeDataSymbol PokeDataSymbol { get; set; }
    [FlatBufferItem(2)] public FixedSymbolAI PokeAI { get; set; }
    [FlatBufferItem(3)] public FixedSymbolGeneration PokeGeneration { get; set; }
}
