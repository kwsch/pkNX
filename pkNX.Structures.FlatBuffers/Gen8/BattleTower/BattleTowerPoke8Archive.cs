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
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class BattleTowerPoke8Archive : IFlatBufferArchive<BattleTowerPoke8>
    {
        [FlatBufferItem(0)] public BattleTowerPoke8[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class BattleTowerPoke8
    {
        [FlatBufferItem(00)] public bool Field_00 { get; set; }
        [FlatBufferItem(01)] public bool Field_01 { get; set; }
        [FlatBufferItem(02)] public bool Field_02 { get; set; }
        [FlatBufferItem(03)] public uint Field_03 { get; set; }
        [FlatBufferItem(04)] public bool Field_04 { get; set; }
        [FlatBufferItem(05)] public bool Field_05 { get; set; }
        [FlatBufferItem(06)] public bool Field_06 { get; set; }
        [FlatBufferItem(07)] public uint Form { get; set; }
        [FlatBufferItem(08)] public uint Field_08 { get; set; }
        [FlatBufferItem(09)] public uint HeldItem { get; set; }
        [FlatBufferItem(10)] public uint Species { get; set; }
        [FlatBufferItem(11)] public uint EntryID { get; set; }
        [FlatBufferItem(12)] public uint Field_0C { get; set; }
        [FlatBufferItem(13)] public uint Nature { get; set; }
        [FlatBufferItem(14)] public uint Field_0E { get; set; }
        [FlatBufferItem(15)] public uint IV_Hp { get; set; }
        [FlatBufferItem(16)] public uint IV_Atk { get; set; }
        [FlatBufferItem(17)] public uint IV_Def { get; set; }
        [FlatBufferItem(18)] public uint IV_SpAtk { get; set; }
        [FlatBufferItem(19)] public uint IV_SpDef { get; set; }
        [FlatBufferItem(20)] public uint IV_Spe { get; set; }
        [FlatBufferItem(21)] public uint Field_15 { get; set; }
        [FlatBufferItem(22)] public uint Move0 { get; set; }
        [FlatBufferItem(23)] public uint Move1 { get; set; }
        [FlatBufferItem(24)] public uint Move2 { get; set; }
        [FlatBufferItem(25)] public uint Move3 { get; set; }
    }
}
