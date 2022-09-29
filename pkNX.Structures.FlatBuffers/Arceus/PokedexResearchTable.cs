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
public class PokedexResearchTable : IFlatBufferArchive<PokedexResearchTask>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokedexResearchTask[] Table { get; set; } = Array.Empty<PokedexResearchTask>();

    public PokedexResearchTask[] GetEntries(int species)
    {
        return Table.Where(z => z.Species == species).ToArray();
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokedexResearchTask
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public ResearchTaskType TaskType { get; set; }
    [FlatBufferItem(02)] public int Threshold { get; set; }
    [FlatBufferItem(03)] public int Move { get; set; }
    [FlatBufferItem(04)] public int MoveType { get; set; }
    [FlatBufferItem(05)] public int TimeOfDay { get; set; }
    [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
    [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
    [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
    [FlatBufferItem(09)] public int Threshold1 { get; set; }
    [FlatBufferItem(10)] public int Threshold2 { get; set; }
    [FlatBufferItem(11)] public int Threshold3 { get; set; }
    [FlatBufferItem(12)] public int Threshold4 { get; set; }
    [FlatBufferItem(13)] public int Threshold5 { get; set; }
    [FlatBufferItem(14)] public int PointsSingle { get; set; }
    [FlatBufferItem(15)] public int PointsBonus { get; set; }
    [FlatBufferItem(16)] public bool RequiredForCompletion { get; set; } // Unused but referenced by code (bool is set to != 0)
}

[FlatBufferEnum(typeof(int))]
public enum ResearchTaskType
{
    Unknown_0,
    MoveTask,
    DefeatTask,
    Unknown_3,
    Unknown_4,
    Unknown_5,
    Unknown_6,
    Unknown_7,
    Unknown_8,
    Unknown_9,
    Unknown_10,
}