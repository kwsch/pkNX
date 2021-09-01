using System;
using System.Collections.Generic;
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
    [FlatBufferTable]
    public sealed class PlacementZone8
    {
        [FlatBufferItem(0)] public PlacementZoneMeta8 Meta { get; set; }
        [FlatBufferItem(1)] public uint Field_01 { get; set; }
        [FlatBufferItem(2)] public uint Field_02 { get; set; }
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public uint Field_04 { get; set; }
        [FlatBufferItem(5)] public uint Field_05 { get; set; }
        [FlatBufferItem(6)] public uint Field_06 { get; set; }
        [FlatBufferItem(7)] public uint Field_07 { get; set; }
        [FlatBufferItem(8)] public uint Field_08 { get; set; }
        [FlatBufferItem(9)] public uint Field_09 { get; set; }
        [FlatBufferItem(10)] public uint Field_0A { get; set; }
        [FlatBufferItem(11)] public uint Field_0B { get; set; }
        [FlatBufferItem(12)] public uint Field_0C { get; set; }
        [FlatBufferItem(13)] public uint Field_0D { get; set; }
        [FlatBufferItem(14)] public uint Field_0E { get; set; }
        [FlatBufferItem(15)] public uint Field_0F { get; set; }
        [FlatBufferItem(16)] public uint Field_11 { get; set; }
        [FlatBufferItem(17)] public uint Field_12 { get; set; }
        [FlatBufferItem(18)] public uint Field_13 { get; set; }
        [FlatBufferItem(19)] public uint Field_14 { get; set; }
        [FlatBufferItem(20)] public uint Field_15 { get; set; }
        [FlatBufferItem(21)] public uint Field_16 { get; set; }
        [FlatBufferItem(22)] public uint Field_17 { get; set; }
        [FlatBufferItem(23)] public uint Field_18 { get; set; }
        [FlatBufferItem(24)] public uint Field_19 { get; set; }
        [FlatBufferItem(25)] public uint Field_1A { get; set; }
        [FlatBufferItem(26)] public PlacementZoneStaticObjectsHolder8[] StaticObjects { get; set; }
        [FlatBufferItem(27)] public uint Field_1C { get; set; }

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

    [FlatBufferTable]
    public sealed class PlacementZoneMeta8
    {
        [FlatBufferItem(0)] public uint Field_00 { get; set; }
        [FlatBufferItem(1)] public ulong ZoneID { get; set; }
        // More tables exist here
    }

    [FlatBufferTable]
    public sealed class PlacementZoneStaticObjectsHolder8
    {
        [FlatBufferItem(0)] public PlacementZoneStaticObject8 Object { get; set; }
    }

    [FlatBufferTable]
    public sealed class PlacementZoneStaticObject8
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

    [FlatBufferTable]
    public sealed class PlacementZoneStaticObjectIdentifier8
    {
        [FlatBufferItem(0)] public float LocationX { get; set; }
        [FlatBufferItem(1)] public float LocationY { get; set; }
        [FlatBufferItem(2)] public float LocationZ { get; set; }
        [FlatBufferItem(3)] public uint Field_3 { get; set; }
        [FlatBufferItem(4)] public uint Field_4 { get; set; }
        [FlatBufferItem(5)] public uint Field_5 { get; set; }
        [FlatBufferItem(6)] public float Field_6 { get; set; }
        [FlatBufferItem(7)] public float Field_7 { get; set; }
        [FlatBufferItem(8)] public float Field_8 { get; set; }
        [FlatBufferItem(9)] public ulong SpawnerID { get; set; }
        [FlatBufferItem(10)] public ulong Field_A { get; set; }
        [FlatBufferItem(11)] public ulong Field_B { get; set; }
    }

    [FlatBufferTable]
    public sealed class PlacementZoneStaticObjectSpawn8
    {
        [FlatBufferItem(0)] public ulong SpawnID { get; set; }
        [FlatBufferItem(1)] public string Description { get; set; }
        [FlatBufferItem(2)] public ulong Field_02 { get; set; }
        [FlatBufferItem(3)] public uint Field_03 { get; set; }
        [FlatBufferItem(4)] public PlacementZoneStaticObjectUnknown8 Field_04 { get; set; }

        public IEnumerable<string> GetSummary(EncounterStatic8[] statics, IReadOnlyList<string> species)
        {
            var enc = Array.Find(statics, z => z.EncounterID == SpawnID);
            yield return $"{species[enc.Species]}{(enc.AltForm == 0 ? string.Empty : "-" + enc.AltForm)} Lv. {enc.Level}";
            yield return $"EncounterID: {SpawnID:X016}";
        }
    }

    [FlatBufferTable]
    public sealed class PlacementZoneStaticObjectUnknown8
    {
        [FlatBufferItem(0)] public uint Field_0 { get; set; }
        [FlatBufferItem(1)] public uint Field_1 { get; set; }
        [FlatBufferItem(2)] public uint Field_2 { get; set; }
        [FlatBufferItem(3)] public uint Field_3 { get; set; }
        [FlatBufferItem(4)] public float Field_4 { get; set; }
    }
}
