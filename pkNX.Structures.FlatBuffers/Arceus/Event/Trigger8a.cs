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
public class TriggerTable8a : IFlatBufferArchive<Trigger8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public Trigger8a[] Table { get; set; } = Array.Empty<Trigger8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trigger8a
{
    [FlatBufferItem(00)] public TriggerMeta8a Meta { get; set; } = new();
    [FlatBufferItem(01)] public TriggerCondition8a[] Conditions { get; set; } = Array.Empty<TriggerCondition8a>();
    [FlatBufferItem(02)] public TriggerCommand8a[] Commands { get; set; } = Array.Empty<TriggerCommand8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TriggerMeta8a
{
    [FlatBufferItem(00)] public TriggerType8a TriggerTypeID { get; set; }
    [FlatBufferItem(01)] public ulong Unused_01 { get; set; }
    [FlatBufferItem(02)] public string TriggerMetaArg1 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string TriggerMetaArg2 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string TriggerMetaArg3 { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TriggerCondition8a : IHasCondition8a
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;

    // For interface compat
    public string ConditionArg4 { get; set; } = string.Empty;
    public string ConditionArg5 { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TriggerCommand8a
{
    [FlatBufferItem(00)] public TriggerCommandType8a CommandTypeID { get; set; }
    [FlatBufferItem(01)] public string[] Arguments { get; set; } = Array.Empty<string>();
}
