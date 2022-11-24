using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BlacklistArray : IFlatBufferArchive<Blacklist>
{
    [FlatBufferItem(0)] public Blacklist[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Blacklist
{
    [FlatBufferItem(0)] public int DevNo { get; set; }
    [FlatBufferItem(1)] public int FormNo { get; set; }
}

