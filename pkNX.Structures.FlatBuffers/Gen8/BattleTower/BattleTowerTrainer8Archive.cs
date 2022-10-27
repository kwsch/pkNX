using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleTowerTrainer8Archive
{
    [FlatBufferItem(0)] public BattleTowerTrainer8[] Entries { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleTowerTrainer8
{
    [FlatBufferItem(0)] public ulong Hash0 { get; set; }
    [FlatBufferItem(1)] public ulong Hash1 { get; set; }
    [FlatBufferItem(2)] public ushort EntryID { get; set; }
    [FlatBufferItem(3)] public ushort Field_03 { get; set; }
    [FlatBufferItem(4)] public ushort Field_04 { get; set; }
    [FlatBufferItem(5)] public ushort[] Choices { get; set; }
}
