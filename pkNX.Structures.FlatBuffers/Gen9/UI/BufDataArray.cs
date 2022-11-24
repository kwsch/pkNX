using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BufDataArray : IFlatBufferArchive<BufData>
{
    [FlatBufferItem(0)] public BufData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BufData
{
    [FlatBufferItem(0)] public string Bufid { get; set; }
    [FlatBufferItem(1)] public FoodSkillType Buftype01 { get; set; }
    [FlatBufferItem(2)] public int Buflevel01 { get; set; }
    [FlatBufferItem(3)] public MoveType BufPoketype01 { get; set; }
    [FlatBufferItem(4)] public FoodSkillType Buftype02 { get; set; }
    [FlatBufferItem(5)] public int Buflevel02 { get; set; }
    [FlatBufferItem(6)] public MoveType BufPoketype02 { get; set; }
    [FlatBufferItem(7)] public FoodSkillType Buftype03 { get; set; }
    [FlatBufferItem(8)] public int Buflevel03 { get; set; }
    [FlatBufferItem(9)] public MoveType BufPoketype03 { get; set; }
}
