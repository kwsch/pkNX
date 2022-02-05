using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8SpeciesHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F02 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public PlacementZone8_F02_Field1 Field_01 { get; set; } = new();

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
        [FlatBufferItem(12)] public PlacementZone8_F02_Nine Field_12 { get; set; } = new();
        [FlatBufferItem(13)] public int Field_13 { get; set; } // 0, 1, 3, 4
        [FlatBufferItem(14)] public int Field_14 { get; set; } // 6, 11, 14 or 0
        [FlatBufferItem(15)] public byte Num_15 { get; set; } // 0 or 1 (bool?)

        public override string ToString() => $"{(Species)Species}{(Form != 0 ? $"-{Form}" : "")}";

        public PlacementZone8SpeciesHolder() { }

        public PlacementZone8SpeciesHolder Clone() => new(this);

        public PlacementZone8SpeciesHolder(PlacementZone8SpeciesHolder other) : this()
        {
            Field_00 = other.Field_00.Clone();
            Field_01 = other.Field_01.Clone();
            Field_12 = other.Field_12.Clone();

            Species = other.Species;
            Form = other.Form;
            Gender = other.Gender;
            Shiny = other.Shiny;
            Unused2 = other.Unused2;
            Hash_07 = other.Hash_07;
            Hash_08 = other.Hash_08;
            Hash_09 = other.Hash_09;
            Field_11 = other.Field_11;
            Field_13 = other.Field_13;
            Field_14 = other.Field_14;
            Num_15 = other.Num_15;
        }
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
        [FlatBufferItem(08)] public uint AnimationIndexSecondary { get; set; }
        [FlatBufferItem(09)] public uint Field_09 { get; set; }

        public PlacementZone8_F02_Nine Clone() => new()
        {
            Field_00 = Field_00,
            Field_01 = Field_01,
            Field_02 = Field_02,
            Field_03 = Field_03,
            Hash_04 = Hash_04,
            Field_05 = Field_05,
            Field_06 = Field_06,
            Hash_07 = Hash_07,
            AnimationIndexSecondary = AnimationIndexSecondary,
            Field_09 = Field_09,
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
        [FlatBufferItem(05)] public uint Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(06)] public uint Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(07)] public uint Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(08)] public uint Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(09)] public FlatDummyObject Field_09 { get; set; } = new(); // no fields present in any existing
        [FlatBufferItem(10)] public uint Field_10 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(11)] public FlatDummyObject Field_11 { get; set; } = new(); // no fields present in any existing
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }

        public PlacementZone8_F02 Clone() => new()
        {
            Field_00 = Field_00.Clone(),
            Hash_01 = Hash_01,
            Hash_02 = Hash_02,
            Hash_03 = Hash_03,
            Hash_04 = Hash_04,
            Field_05 = Field_05,
            Field_06 = Field_06,
            Field_07 = Field_07,
            Field_08 = Field_08,
            Field_09 = Field_09.Clone(),
            Field_10 = Field_10,
            Field_11 = Field_11.Clone(),
            Hash_12 = Hash_12,
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Field1
    {
        [FlatBufferItem(00)] public PlacementZone8_F02_Inner Field_00 { get; set; } = new();

        public PlacementZone8_F02_Field1 Clone() => new() { Field_00 = Field_00.Clone() };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Inner
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public PlacementZone8_F02_IntFloat Field_04 { get; set; } = new();
        [FlatBufferItem(05)] public byte Num_05 { get; set; } // 0 or 1 (bool?)
        [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F02_IntFloat Field_07 { get; set; } = new();

        public PlacementZone8_F02_Inner Clone() => new()
        {
            Field_00 = Field_00.Clone(),
            Hash_01 = Hash_01,
            Hash_02 = Hash_02,
            Hash_03 = Hash_03,
            Field_04 = Field_04.Clone(),
            Num_05 = Num_05,
            Hash_06 = Hash_06,
            Field_07 = Field_07.Clone(),
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_IntFloat
    {
        [FlatBufferItem(00)] public int   Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }

        public PlacementZone8_F02_IntFloat Clone() => new()
        {
            Field_00 = Field_00,
            Field_01 = Field_01,
            Field_02 = Field_02,
            Field_03 = Field_03,
            Field_04 = Field_04,
        };
    }
}
