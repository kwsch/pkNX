using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8TrainerHolder
{
    [FlatBufferItem(00)] public PlacementZone8_F08 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public ulong TrainerID { get; set; }
    [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
    [FlatBufferItem(04)] public ulong MovementPath { get; set; }
    [FlatBufferItem(05)] public PlacementZone8_F08_ArrayEntry[] Unknown { get; set; } = Array.Empty<PlacementZone8_F08_ArrayEntry>();
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
    [FlatBufferItem(07)] public PlacementZone8_F08_Nine Field_07 { get; set; } = new();
    [FlatBufferItem(08)] public uint Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(09)] public uint Field_09 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(10)] public uint Field_10 { get; set; }
    [FlatBufferItem(11)] public uint Field_11 { get; set; }
    [FlatBufferItem(12)] public uint Field_12 { get; set; }

    public override string ToString()
    {
        var hashModel = Field_00.Field_00.HashModel;
        var name = PlacementZone8OtherNPCHolder.Models.TryGetValue(hashModel, out var model) ? model : hashModel.ToString("X16");
        return name;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F08_ArrayEntry
{
    // same as PlacementZone8_F16_ArrayEntry
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public uint Field_01 { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public byte Field_04 { get; set; }
    [FlatBufferItem(05)] public ulong Field_05 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F08_Nine
{
    [FlatBufferItem(0)] public byte Field_00 { get; set; }
    [FlatBufferItem(1)] public byte Field_01 { get; set; }
    [FlatBufferItem(2)] public byte Field_02 { get; set; }
    [FlatBufferItem(3)] public uint Field_03 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(4)] public ulong Hash_04 { get; set; }
    [FlatBufferItem(5)] public byte Field_05 { get; set; }
    [FlatBufferItem(6)] public uint Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(7)] public ulong Hash_07 { get; set; }
    [FlatBufferItem(8)] public uint Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F08
{
    [FlatBufferItem(0)] public PlacementZone8_F08_A Field_00 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F08_A
{
    [FlatBufferItem(0)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(2)] public ulong HashModel { get; set; }
    [FlatBufferItem(3)] public ulong Hash_03 { get; set; }
    [FlatBufferItem(4)] public PlacementZone8_F08_IntFloat Field_04 { get; set; } = new();
    [FlatBufferItem(5)] public uint Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(6)] public ulong Hash_06 { get; set; }
    [FlatBufferItem(7)] public PlacementZone8_F08_IntFloat Field_07 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F08_IntFloat
{
    [FlatBufferItem(0)] public int Field_00 { get; set; }
    [FlatBufferItem(1)] public float Field_01 { get; set; }
    [FlatBufferItem(2)] public float Field_02 { get; set; }
    [FlatBufferItem(3)] public float Field_03 { get; set; }
    [FlatBufferItem(4)] public float Field_04 { get; set; }
}
