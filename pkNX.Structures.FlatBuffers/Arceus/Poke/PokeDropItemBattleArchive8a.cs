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
public class PokeDropItemBattleArchive8a : IFlatBufferArchive<PokeDropItemBattle8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokeDropItemBattle8a[] Table { get; set; } = Array.Empty<PokeDropItemBattle8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDropItemBattle8a
{
    [FlatBufferItem(00)] public int RateItem1 { get; set; }
    [FlatBufferItem(01)] public int IndexItem1 { get; set; }
    [FlatBufferItem(02)] public int RateItem2 { get; set; }
    [FlatBufferItem(03)] public int IndexItem2 { get; set; }
    [FlatBufferItem(04)] public int RateItem3 { get; set; }
    [FlatBufferItem(05)] public int IndexItem3 { get; set; }
    [FlatBufferItem(06)] public int RateItem4 { get; set; }
    [FlatBufferItem(07)] public int IndexItem4 { get; set; }
    [FlatBufferItem(08)] public int RateItem5 { get; set; }
    [FlatBufferItem(09)] public int IndexItem5 { get; set; }

    public string Dump(string[] n) => $"{RateItem1}\t{n[IndexItem1]}\t{RateItem2}\t{n[IndexItem2]}\t{RateItem3}\t{n[IndexItem3]}\t{RateItem4}\t{n[IndexItem4]}\t{RateItem5}\t{n[IndexItem5]}";
}
