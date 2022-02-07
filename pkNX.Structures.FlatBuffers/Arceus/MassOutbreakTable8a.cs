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
public class MassOutbreakTable8a : IFlatBufferArchive<MassOutbreak8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public MassOutbreak8a[] Table { get; set; } = Array.Empty<MassOutbreak8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MassOutbreak8a : IFlatBufferArchive<MassOutbreakInfo8a>
{
    [FlatBufferItem(0)] public ulong Hash { get; set; }
    [FlatBufferItem(1)] public int Field_01 { get; set; }
    [FlatBufferItem(2)] public MassOutbreakInfo8a[] Table { get; set; } = Array.Empty<MassOutbreakInfo8a>();
    [FlatBufferItem(3)] public string WorkValueName { get; set; } = string.Empty;
    [FlatBufferItem(4)] public int SwarmEntryCount { get; set; }
    [FlatBufferItem(5)] public int SwarmMinCount { get; set; }
    [FlatBufferItem(6)] public int SwarmMaxCount { get; set; }
    [FlatBufferItem(7)] public int ShinyRollBonus { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MassOutbreakInfo8a
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg5 { get; set; } = string.Empty;
}
