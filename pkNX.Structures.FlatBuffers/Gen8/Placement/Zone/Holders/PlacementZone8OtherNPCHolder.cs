using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    // more NPCs? Trainers?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8OtherNPCHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F16 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public uint ModelVariant { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public PlacementZone8_F16_ArrayEntry[] Field_04 { get; set; } = Array.Empty<PlacementZone8_F16_ArrayEntry>(); // a_0201.bin[0].[76] @ AAE8
        [FlatBufferItem(05)] public ulong Hash_05 { get; set; }
        [FlatBufferItem(06)] public bool IsSitting { get; set; }
        [FlatBufferItem(07)] public byte Field_07 { get; set; }
        [FlatBufferItem(08)] public uint Field_08 { get; set; }
        [FlatBufferItem(09)] public uint Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
        [FlatBufferItem(11)] public PlacementZone8_F02_Nine Field_11 { get; set; } = new();
        [FlatBufferItem(12)] public uint Field_12 { get; set; }
        [FlatBufferItem(13)] public uint ModelAnimation { get; set; }
        [FlatBufferItem(14)] public uint Field_14 { get; set; }
        [FlatBufferItem(15)] public uint Field_15 { get; set; }
        [FlatBufferItem(16)] public uint Field_16 { get; set; }

        public override string ToString()
        {
            var ident = Field_00.Field_00.Identifier;
            return $"{ident.HashObjectName:X16} v{ModelVariant} @ {ident.Location3f}";
        }

        public PlacementZone8OtherNPCHolder Clone() => new()
        {
            Field_00 = Field_00.Clone(),
            ModelVariant = ModelVariant,
            Hash_02 = Hash_02,
            Hash_03 = Hash_03,
            Field_04 = Field_04,
            Hash_05 = Hash_05,
            IsSitting = IsSitting,
            Field_07 = Field_07,
            Field_08 = Field_08,
            Field_09 = Field_09,
            Field_10 = Field_10,
            Field_11 = Field_11.Clone(),
            Field_12 = Field_12,
            ModelAnimation = ModelAnimation,
            Field_14 = Field_14,
            Field_15 = Field_15,
            Field_16 = Field_16,
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F16_ArrayEntry
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; }
        [FlatBufferItem(01)] public uint Field_01 { get; set; }
        [FlatBufferItem(02)] public uint Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public byte Field_04 { get; set; }
        [FlatBufferItem(05)] public float Field_05 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F16
    {
        [FlatBufferItem(00)] public PlacementZone8_F16_A Field_00 { get; set; } = new();

        public PlacementZone8_F16 Clone() => new()
        {
            Field_00 = Field_00.Clone(),
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F16_A
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Identifier { get; set; } = new();
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong HashModel { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public PlacementZone8_F16_IntFloat Field_04 { get; set; } = new();
        [FlatBufferItem(05)] public bool Flag_05 { get; set; }
        [FlatBufferItem(06)] public ulong HashMessage { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F16_IntFloat Field_07 { get; set; } = new();

        public PlacementZone8_F16_A Clone() => new()
        {
            Identifier = Identifier.Clone(),
            Hash_01 = Hash_01,
            HashModel = HashModel,
            Hash_03 = Hash_03,
            Field_04 = Field_04.Clone(),
            Flag_05 = Flag_05,
            HashMessage = HashMessage,
            Field_07 = Field_07.Clone(),
        };
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F16_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }

        public PlacementZone8_F16_IntFloat Clone() => new()
        {
            Field_00 = Field_00,
            Field_01 = Field_01,
            Field_02 = Field_02,
            Field_03 = Field_03,
            Field_04 = Field_04
        };
    }
}
