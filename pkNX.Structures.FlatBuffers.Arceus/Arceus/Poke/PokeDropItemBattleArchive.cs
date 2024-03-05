using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeDropItemBattleArchive;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeDropItemBattle
{
    public string Dump(string[] n) => $"{RateItem1}\t{n[IndexItem1]}\t{RateItem2}\t{n[IndexItem2]}\t{RateItem3}\t{n[IndexItem3]}\t{RateItem4}\t{n[IndexItem4]}\t{RateItem5}\t{n[IndexItem5]}";
}
