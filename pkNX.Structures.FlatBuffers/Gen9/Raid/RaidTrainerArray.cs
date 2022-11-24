using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidTrainerArray : IFlatBufferArchive<RaidTrainer>
{
    [FlatBufferItem(0)] public RaidTrainer[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidTrainer
{
    [FlatBufferItem(0)] public string RaidTrainerInfo { get; set; }
    [FlatBufferItem(1)] public int NpcPresetIndex { get; set; }
    [FlatBufferItem(2)] public string NameTextLabel { get; set; }
}
