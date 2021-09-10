using System;
using System.Collections.Generic;
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
    public class PlacementZone8StaticObjectsHolder
    {
        [FlatBufferItem(0)] public PlacementZoneStaticObject8 Object { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneStaticObject8
    {
        [FlatBufferItem(0)] public PlacementZoneStaticObjectIdentifier8 Identifier { get; set; }
        [FlatBufferItem(1)] public uint Field_01 { get; set; }
        [FlatBufferItem(2)] public uint Field_02 { get; set; }
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public byte Field_04 { get; set; }
        [FlatBufferItem(5)] public PlacementZoneStaticObjectSpawn8[] Spawns { get; set; }
        [FlatBufferItem(6)] public PlacementZoneStaticObjectUnknown8 Field_06 { get; set; }
        [FlatBufferItem(7)] public PlacementZoneStaticObjectUnknown8 Field_07 { get; set; }
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
        [FlatBufferItem(1)] public string Description { get; set; }
        [FlatBufferItem(2)] public ulong Field_02 { get; set; } // default hash for all, likely empty string
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public PlacementZoneStaticObjectUnknown8 Field_04 { get; set; }

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
        [FlatBufferItem(0)] public uint Field_0 { get; set; }
        [FlatBufferItem(1)] public uint Field_1 { get; set; }
        [FlatBufferItem(2)] public uint Field_2 { get; set; }
        [FlatBufferItem(3)] public uint Field_3 { get; set; }
        [FlatBufferItem(4)] public float Field_4 { get; set; }

        public override string ToString() => $"{Field_0} {Field_1} {Field_2} {Field_3} {Field_4}";
    }
}
