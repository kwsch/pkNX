using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ObjectGenerationRangeArray : IFlatBufferArchive<ObjectGenerationRange>
{
    [FlatBufferItem(0)] public ObjectGenerationRange[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ObjectGenerationRange
{
    [FlatBufferItem(0)] public ObjectType Type { get; set; }
    [FlatBufferItem(1)] public float MinCreateDistance { get; set; }
    [FlatBufferItem(2)] public float MaxCreateDistance { get; set; }
    [FlatBufferItem(3)] public float MinDestroyDistance { get; set; }
    [FlatBufferItem(4)] public float MaxDestroyDistance { get; set; }
    [FlatBufferItem(5)] public int MaxGenerationNum { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum ObjectType
{
    None = -1,
    RaidGem = 0,
    EncountPokemon = 1,
    DistantViewEffect = 2,
    GroundEffect = 3,
    HiddenItem = 4,
    CrashRock = 5,
    TrafficNpc = 6,
    FieldNpc = 7,
    FixEncout = 8,
}
