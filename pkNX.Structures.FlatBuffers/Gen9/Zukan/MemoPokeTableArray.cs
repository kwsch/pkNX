using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MemoPokeTableArray : IFlatBufferArchive<MemoPokeTable>
{
    [FlatBufferItem(0)] public MemoPokeTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MemoPokeTable
{
    [FlatBufferItem(0)] public DevID DevNo { get; set; }
    [FlatBufferItem(1)] public Sex Sex { get; set; }
    [FlatBufferItem(2)] public int FormNo { get; set; }
}
