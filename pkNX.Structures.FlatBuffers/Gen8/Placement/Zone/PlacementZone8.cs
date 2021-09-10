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
    public class PlacementZone8
    {
        [FlatBufferItem(00)] public PlacementZoneMeta8 Meta { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F1Holder[] Field_01 { get; set; }
        [FlatBufferItem(02)] public uint Field_02 { get; set; }
        [FlatBufferItem(03)] public PlacementZone8WarpHolder[] Warps { get; set; }
        [FlatBufferItem(04)] public uint Field_04 { get; set; }
        [FlatBufferItem(05)] public PlacementZone8ParticleHolder[] Particles { get; set; }
        [FlatBufferItem(06)] public uint Field_06 { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F7Holder[] Field_07 { get; set; }
        [FlatBufferItem(08)] public uint Field_08 { get; set; }
        [FlatBufferItem(09)] public uint Field_09 { get; set; }
        [FlatBufferItem(10)] public uint Field_10 { get; set; }
        [FlatBufferItem(11)] public uint Field_11 { get; set; }
        [FlatBufferItem(12)] public uint Field_12 { get; set; }
        [FlatBufferItem(13)] public uint Field_13 { get; set; }
        [FlatBufferItem(14)] public uint Field_14 { get; set; }
        [FlatBufferItem(15)] public uint Field_15 { get; set; }
        [FlatBufferItem(16)] public uint Field_16 { get; set; }
        [FlatBufferItem(17)] public uint Field_17 { get; set; }
        [FlatBufferItem(18)] public uint Field_18 { get; set; }
        [FlatBufferItem(19)] public uint Field_19 { get; set; }
        [FlatBufferItem(20)] public uint Field_20 { get; set; }
        [FlatBufferItem(21)] public uint Field_21 { get; set; }
        [FlatBufferItem(22)] public uint Field_22 { get; set; }
        [FlatBufferItem(23)] public uint Field_23 { get; set; }
        [FlatBufferItem(24)] public uint Field_24 { get; set; }
        [FlatBufferItem(25)] public uint Field_25 { get; set; }
        [FlatBufferItem(26)] public PlacementZone8StaticObjectsHolder[] StaticObjects { get; set; }
        [FlatBufferItem(27)] public uint Field_27 { get; set; }

        // More tables exist here

        public IEnumerable<string> GetSummary(EncounterStatic8[] statics,
            IReadOnlyList<string> species,
            IReadOnlyDictionary<ulong, string> zone_names,
            IReadOnlyDictionary<ulong, string> zone_descs,
            IReadOnlyDictionary<ulong, string> objects,
            IReadOnlyList<string> weathers)
        {
            var zoneID = Meta.ZoneID;
            var name = zone_names[zoneID];
            yield return zone_descs.TryGetValue(zoneID, out var desc)
                ? $"{name} ({desc}):"
                : $"{name}:";

            foreach (var so in StaticObjects)
            {
                var obj = so.Object;
                var ident = obj.Identifier;
                yield return $"    {objects[ident.SpawnerID]}:";
                yield return $"        Location: ({ident.LocationX}, {ident.LocationY}, {ident.LocationZ})";
                if (obj.Spawns?.Length == 0)
                {
                    yield return "        No spawns."; // shouldn't hit here, if we have a holder we should have a spawn to hold.
                    yield break;
                }

                var s = obj.Spawns;
                if (Array.TrueForAll(s, z => z.SpawnID == s[0].SpawnID))
                {
                    yield return "        All Weather:";
                    foreach (var line in s[0].GetSummary(statics, species))
                        yield return $"            {line}";
                }
                else
                {
                    for (var i = 0; i < s.Length; i++)
                    {
                        yield return $"        {weathers[i]}:";
                        foreach (var line in s[i].GetSummary(statics, species))
                            yield return $"            {line}";
                    }
                }
            }

            yield return string.Empty;
        }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneMeta8
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong ZoneID { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        // 3 empty table?
        // 4
        [FlatBufferItem(05)] public string Music { get; set; }
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
        [FlatBufferItem(09)] public ulong Hash_09 { get; set; }
        [FlatBufferItem(10)] public byte Field_10 { get; set; }
        [FlatBufferItem(11)] public byte Field_11 { get; set; }
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
        // 13
        // 14
        [FlatBufferItem(15)] public int Num_15 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneMetaTripleXYZ8
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; } // assumed
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public float Field_05 { get; set; } // assumed
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public ulong Hash_09 { get; set; }
        [FlatBufferItem(10)] public ulong Hash_10 { get; set; }
        [FlatBufferItem(11)] public ulong Hash_11 { get; set; }
    }
}
