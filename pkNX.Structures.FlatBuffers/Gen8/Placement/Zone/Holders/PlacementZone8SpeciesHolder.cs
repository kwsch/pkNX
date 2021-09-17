using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SpeciesHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F02 Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F02_Field1 Field_01 { get; set; }

        [Description("Species Model to load")]
        [FlatBufferItem(02)] public uint Species { get; set; }
        [Description("Form Model to load")]
        [FlatBufferItem(03)] public uint Form { get; set; }
        [Description("Gender Model to load: Male and Genderless 0, Female 1")]
        [FlatBufferItem(04)] public uint Gender { get; set; }

        [Description("Color Model to load: Normal 0, Shiny 1")]
        [FlatBufferItem(05)] public uint Shiny { get; set; }
        [FlatBufferItem(06)] public uint Unused2 { get; set; }

        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
        [FlatBufferItem(09)] public ulong Hash_09 { get; set; }
        [FlatBufferItem(10)] public FlatDummyEntry[] Field_10 { get; set; } = Array.Empty<FlatDummyEntry>(); // none have this
        [FlatBufferItem(11)] public float Field_11 { get; set; }
        [FlatBufferItem(12)] public PlacementZone8_F02_Nine Field_12 { get; set; }
        [FlatBufferItem(13)] public int Field_13 { get; set; } // 0, 1, 3, 4
        [FlatBufferItem(14)] public int Field_14 { get; set; } // 6, 11, 14 or 0
        [FlatBufferItem(15)] public byte Num_15 { get; set; } // 0 or 1 (bool?)

        public override string ToString() => $"{(Species)Species}{(Form != 0 ? $"-{Form}" : "")}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Nine
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(01)] public byte Field_01 { get; set; }
        [FlatBufferItem(02)] public byte Field_02 { get; set; }
        [FlatBufferItem(03)] public uint Field_03 { get; set; } // either 0 or 1, for only 3 objects in the game
        [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
        [FlatBufferItem(05)] public byte Field_05 { get; set; }
        [FlatBufferItem(06)] public uint Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public uint Field_08 { get; set; }
        [FlatBufferItem(09)] public uint Field_09 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
        [FlatBufferItem(05)] public uint Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(06)] public uint Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(07)] public uint Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(08)] public uint Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(09)] public FlatDummyObject Field_09 { get; set; } // no fields present in any existing
        [FlatBufferItem(10)] public uint Field_10 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(11)] public FlatDummyObject Field_11 { get; set; } // no fields present in any existing
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Field1
    {
        [FlatBufferItem(00)] public PlacementZone8_F02_Inner Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Inner
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public PlacementZone8_F02_IntFloat Field_04 { get; set; }
        [FlatBufferItem(05)] public byte Num_05 { get; set; } // 0 or 1 (bool?)
        [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F02_IntFloat Field_07 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }
}
