using System;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupArchive8a : IFlatBufferArchive<NewHugeOutbreakGroup8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakGroup8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakGroup8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroup8a : IFlatBufferArchive<NewHugeOutbreakDetail8a>
{
    [FlatBufferItem(00)] public ulong Group { get; set; }
    [FlatBufferItem(01)] public NewHugeOutbreakDetail8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakDetail8a>();
    [FlatBufferItem(02)] public ulong EncounterTableID { get; set; }

    public int SumTable => Table.Sum(z => z.Rate);

    public bool UsesTable(ulong tableID)
    {
        if (EncounterTableID == tableID)
            return true;
        var matches = Array.Find(Table, z => z.EncounterTableID == tableID);
        return matches is not null;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakDetail8a : IHasCondition8a
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg5 { get; set; } = string.Empty;
    [FlatBufferItem(7)] public ulong EncounterTableID { get; set; }
    [FlatBufferItem(8)] public int Rate { get; set; }
    public override string ToString() => $"PlacementParameters(/* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {EncounterTableID:X16}, {Rate})";
}
