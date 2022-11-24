using System;
using FlatSharp.Attributes;
using System.ComponentModel;
#nullable disable
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrDataMainArray : IFlatBufferArchive<TrDataMain>
{
    [FlatBufferItem(0)] public TrDataMain[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrDataMain : IFlatBufferArchive<PokeDataBattle>
{
    [FlatBufferItem(00)] public string TrId { get; set; }
    [FlatBufferItem(01)] public string TrNameLabel { get; set; }
    [FlatBufferItem(02)] public string TrainerType { get; set; }
    [FlatBufferItem(03)] public bool IsStrong { get; set; }
    [FlatBufferItem(04)] public BattleType BattleType { get; set; }
    [FlatBufferItem(05)] public DataType DataType { get; set; }
    [FlatBufferItem(06)] public sbyte MoneyRate { get; set; }
    [FlatBufferItem(07)] public bool ChangeGem { get; set; }
    [FlatBufferItem(08)] public PokeDataBattle Poke1 { get; set; }
    [FlatBufferItem(09)] public PokeDataBattle Poke2 { get; set; }
    [FlatBufferItem(10)] public PokeDataBattle Poke3 { get; set; }
    [FlatBufferItem(11)] public PokeDataBattle Poke4 { get; set; }
    [FlatBufferItem(12)] public PokeDataBattle Poke5 { get; set; }
    [FlatBufferItem(13)] public PokeDataBattle Poke6 { get; set; }
    [FlatBufferItem(14)] public bool AiBasic { get; set; }
    [FlatBufferItem(15)] public bool AiHigh { get; set; }
    [FlatBufferItem(16)] public bool AiExpert { get; set; }
    [FlatBufferItem(17)] public bool AiDouble { get; set; }
    [FlatBufferItem(18)] public bool AiRaid { get; set; }
    [FlatBufferItem(19)] public bool AiWeak { get; set; }
    [FlatBufferItem(20)] public bool AiItem { get; set; }
    [FlatBufferItem(21)] public bool AiChange { get; set; }
    [FlatBufferItem(22)] public string PopupLabelNormal1 { get; set; }
    [FlatBufferItem(23)] public string PopupLabelNormal2 { get; set; }
    [FlatBufferItem(24)] public string PopupLabelPinch1 { get; set; }
    [FlatBufferItem(25)] public string PopupLabelPinch2 { get; set; }

    [Browsable(false)]
    public PokeDataBattle[] Table { get => new[] {Poke1, Poke2, Poke3, Poke4, Poke5, Poke6}; set => throw new ArgumentException(nameof(Table)); }
}
