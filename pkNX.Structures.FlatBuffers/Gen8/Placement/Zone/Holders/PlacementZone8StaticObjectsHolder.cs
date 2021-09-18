using System;
using System.Collections.Generic;
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
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8StaticObjectsHolder
    {
        [FlatBufferItem(0)] public PlacementZoneStaticObject8 Object { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneStaticObject8
    {
        [FlatBufferItem(0)] public PlacementZoneStaticObjectIdentifier8 Identifier { get; set; } = new();
        [FlatBufferItem(1)] public uint Field_01 { get; set; }
        [FlatBufferItem(2)] public uint Rate { get; set; } // usually 100, but 
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public byte Field_04 { get; set; }
        [FlatBufferItem(5)] public PlacementZoneStaticObjectSpawn8[] Spawns { get; set; } = Array.Empty<PlacementZoneStaticObjectSpawn8>();
        [FlatBufferItem(6)] public PlacementZoneStaticObjectUnknown8 Field_06 { get; set; } = new();
        [FlatBufferItem(7)] public PlacementZoneStaticObjectUnknown8 Field_07 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneStaticObjectIdentifier8
    {
        [FlatBufferItem(00)] public float LocationX { get; set; }
        [FlatBufferItem(01)] public float LocationY { get; set; }
        [FlatBufferItem(02)] public float LocationZ { get; set; }
        [FlatBufferItem(03)] public uint Field_3 { get; set; }
        [FlatBufferItem(04)] public uint Field_4 { get; set; }
        [FlatBufferItem(05)] public uint Field_5 { get; set; }
        [FlatBufferItem(06)] public float Field_6 { get; set; }
        [FlatBufferItem(07)] public float Field_7 { get; set; }
        [FlatBufferItem(08)] public float Field_8 { get; set; }
        [FlatBufferItem(09)] public ulong SpawnerID { get; set; }
        [FlatBufferItem(10)] public ulong Field_A { get; set; }
        [FlatBufferItem(11)] public ulong Field_B { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneStaticObjectSpawn8
    {
        [FlatBufferItem(0)] public ulong SpawnID { get; set; }
        [FlatBufferItem(1)] public string Behavior { get; set; } = ""; // passed to Lua script for animating
        [FlatBufferItem(2)] public ulong Field_02 { get; set; } // default hash for all, likely empty string
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public PlacementZoneStaticObjectUnknown8 Field_04 { get; set; } = new();

        public IEnumerable<string> GetSummary(EncounterStatic8[] statics, IReadOnlyList<string> species)
        {
            var index = Array.FindIndex(statics, z => z.EncounterID == SpawnID);
            var enc = statics[index];
            yield return $"{species[enc.Species]}{(enc.Form == 0 ? string.Empty : "-" + enc.Form)} Lv. {enc.Level}";
            yield return $"Index: {index}";
            yield return $"EncounterID: {SpawnID:X016}";
            if (Field_02 != 0xCBF29CE484222645)
                yield return $"Hash: {Field_02:X16}";
            yield return $"Value: {Field_03}";
            yield return $"Unknown: {Field_04}";
        }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneStaticObjectUnknown8
    {
        [FlatBufferItem(0)] public uint Field_00 { get; set; }
        [FlatBufferItem(1)] public float Field_01 { get; set; } // unused, assumed same shape as other i4f
        [FlatBufferItem(2)] public float Field_02 { get; set; } // unused, assumed same shape as other i4f
        [FlatBufferItem(3)] public float Field_03 { get; set; } // unused, assumed same shape as other i4f
        [FlatBufferItem(4)] public float Field_04 { get; set; }

        public override string ToString() => $"{Field_00} {Field_01} {Field_02} {Field_03} {Field_04}";
    }
}
