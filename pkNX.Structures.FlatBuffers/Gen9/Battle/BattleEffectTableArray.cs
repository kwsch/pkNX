using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleEffectTableArray : IFlatBufferArchive<BattleEffectTable>
{
    [FlatBufferItem(0)] public BattleEffectTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleEffectTable
{
    [FlatBufferItem(0)] public bool Enable { get; set; }
    [FlatBufferItem(1)] public string TimelinePath { get; set; }
    [FlatBufferItem(2)] public string TemplatePath { get; set; }
    [FlatBufferItem(3)] public short RaidFrameLength { get; set; }
}
