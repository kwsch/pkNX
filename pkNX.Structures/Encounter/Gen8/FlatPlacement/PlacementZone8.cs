using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{

    public class PlacementZone8
    {
        public PlacementZoneMeta8 Meta { get; set; }
        public uint Field_01 { get; set; }
        public uint Field_02 { get; set; }
        public uint Field_03 { get; set; }
        public uint Field_04 { get; set; }
        public uint Field_05 { get; set; }
        public uint Field_06 { get; set; }
        public uint Field_07 { get; set; }
        public uint Field_08 { get; set; }
        public uint Field_09 { get; set; }
        public uint Field_0A { get; set; }
        public uint Field_0B { get; set; }
        public uint Field_0C { get; set; }
        public uint Field_0D { get; set; }
        public uint Field_0E { get; set; }
        public uint Field_0F { get; set; }
        public uint Field_11 { get; set; }
        public uint Field_12 { get; set; }
        public uint Field_13 { get; set; }
        public uint Field_14 { get; set; }
        public uint Field_15 { get; set; }
        public uint Field_16 { get; set; }
        public uint Field_17 { get; set; }
        public uint Field_18 { get; set; }
        public uint Field_19 { get; set; }
        public uint Field_1A { get; set; }

        public PlacementZoneStaticObjectsHolder8[] StaticObjects { get; set; }
        public uint Field_1C { get; set; }
        // More tables exist here

        public IEnumerable<string> GetSummary(EncounterStatic8[] statics, IReadOnlyList<string> species, IReadOnlyDictionary<ulong, string> zone_names, IReadOnlyDictionary<ulong, string> zone_descs, IReadOnlyDictionary<ulong, string> objects, IReadOnlyList<string> weathers) 
        {

            if (zone_descs.ContainsKey(Meta.ZoneID))
            {
                yield return $"{zone_names[Meta.ZoneID]} ({zone_descs[Meta.ZoneID]}):";
            }
            else
            {
                yield return $"{zone_names[Meta.ZoneID]}:";
            }

            foreach (var so in StaticObjects)
            {
                var obj = so.Object;
                yield return $"    {objects[obj.Identifier.SpawnerID]}:";
                yield return $"        Location: ({obj.Identifier.LocationX}, {obj.Identifier.LocationY}, {obj.Identifier.LocationZ})";
                if (obj.Spawns.All(z => z.SpawnID == obj.Spawns[0].SpawnID))
                {
                    yield return $"        All Weather:";
                    foreach (var line in obj.Spawns[0].GetSummary(statics, species))
                    {
                        yield return $"            {line}";
                    }
                } 
                else
                {
                    for (var i = 0; i < obj.Spawns.Length; i++)
                    {
                        yield return $"        {weathers[i]}:";
                        foreach (var line in obj.Spawns[i].GetSummary(statics, species))
                        {
                            yield return $"            {line}";
                        }
                    }
                }
            }

            yield return string.Empty;
        }
    }

    public class PlacementZoneMeta8
    {
        public uint Field_00 { get; set; }
        public ulong ZoneID { get; set; }
        // More tables exist here
    }

    public class PlacementZoneStaticObjectsHolder8
    {
        public PlacementZoneStaticObject8 Object { get; set; }
    }

    public class PlacementZoneStaticObject8
    {
        public PlacementZoneStaticObjectIdentifier8 Identifier;
        public uint Field_01 { get; set; }
        public uint Field_02 { get; set; }
        public uint Field_03 { get; set; }
        public byte Field_04 { get; set; }
        public PlacementZoneStaticObjectSpawn8[] Spawns { get; set; }
        public PlacementZoneStaticObjectUnknown8 Field_06 { get; set; }
        public PlacementZoneStaticObjectUnknown8 Field_07 { get; set; }

    }

    public class PlacementZoneStaticObjectIdentifier8
    {
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public float LocationZ { get; set; }
        public uint Field_3 { get; set; }
        public uint Field_4 { get; set; }
        public uint Field_5 { get; set; }
        public float Field_6 { get; set; }
        public float Field_7 { get; set; }
        public float Field_8 { get; set; }
        public ulong SpawnerID { get; set; }
        public ulong Field_A { get; set; }
        public ulong Field_B { get; set; }
    }

    public class PlacementZoneStaticObjectSpawn8
    {
        public ulong SpawnID { get; set; }
        public string Description { get; set; }
        public ulong Field_02 { get; set; }
        public uint Field_03 { get; set; }
        public PlacementZoneStaticObjectUnknown8 Field_04 { get; set; }

        public IEnumerable<string> GetSummary(EncounterStatic8[] statics, IReadOnlyList<string> species)
        {
            var enc = statics.First(z => z.EncounterID == SpawnID);
            yield return $"{species[(int)enc.Species]}{(enc.AltForm == 0 ? string.Empty : "-" + enc.AltForm)} Lv. {enc.Level}";
            yield return $"EncounterID: {SpawnID:X016}";
        }

    }

    public class PlacementZoneStaticObjectUnknown8
    {
        public uint Field_0 { get; set; }
        public uint Field_1 { get; set; }
        public uint Field_2 { get; set; }
        public uint Field_3 { get; set; }
        public float Field_4 { get; set; }
    }

}