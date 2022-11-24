using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleBgmSelectTableArray : IFlatBufferArchive<BattleBgmSelectTable>
{
    [FlatBufferItem(0)] public BattleBgmSelectTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleBgmSelectTable
{
    [FlatBufferItem(0)] public string BgmEventCallName { get; set; }
    [FlatBufferItem(1)] public string SoundCallName { get; set; }
    [FlatBufferItem(2)] public string TextLabel { get; set; }
    [FlatBufferItem(3)] public int SortNum { get; set; }
    [FlatBufferItem(4)] public bool IsInitialRelease { get; set; }
    [FlatBufferItem(5)] public string ReleaseFlagName { get; set; }
    [FlatBufferItem(6)] public BGMSELECTTYPE BgmSelectType { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum BGMSELECTTYPE
{
    BGM = 0,
    OMAKASE = 1,
    NO_BGM = 2,
}
