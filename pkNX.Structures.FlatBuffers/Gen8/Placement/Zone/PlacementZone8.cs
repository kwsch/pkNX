using System;
using System.Collections.Generic;
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
    public class PlacementZone8
    {
        [FlatBufferItem(00)] public PlacementZoneMeta8 Meta { get; set; } = new();
        [FlatBufferItem(01)] public PlacementZone8UnitObjectHolder[] UnitObjects { get; set; } = Array.Empty<PlacementZone8UnitObjectHolder>();
        [FlatBufferItem(02)] public PlacementZone8SpeciesHolder[] Critters { get; set; } = Array.Empty<PlacementZone8SpeciesHolder>();
        [FlatBufferItem(03)] public PlacementZone8WarpHolder[] Warps { get; set; } = Array.Empty<PlacementZone8WarpHolder>();
        [FlatBufferItem(04)] public PlacementZone8StepJumpHolder[] StepJumps { get; set; } = Array.Empty<PlacementZone8StepJumpHolder>();
        [FlatBufferItem(05)] public PlacementZone8ParticleHolder[] Particles { get; set; } = Array.Empty<PlacementZone8ParticleHolder>();
        [FlatBufferItem(06)] public PlacementZone8FieldItemHolder[] FieldItems { get; set; } = Array.Empty<PlacementZone8FieldItemHolder>();
        [FlatBufferItem(07)] public PlacementZone8TriggerHolder[] Triggers { get; set; } = Array.Empty<PlacementZone8TriggerHolder>();
        [FlatBufferItem(08)] public PlacementZone8TrainerHolder[] Trainers { get; set; } = Array.Empty<PlacementZone8TrainerHolder>();
        [FlatBufferItem(09)] public PlacementZone8TrainerTipHolder[] TrainerTips { get; set; } = Array.Empty<PlacementZone8TrainerTipHolder>();
        [FlatBufferItem(10)] public PlacementZone8EnvironmentHolder[] Environments { get; set; } = Array.Empty<PlacementZone8EnvironmentHolder>();
        [FlatBufferItem(11)] public PlacementZone8FlightAnchorHolder[] FlyTo { get; set; } = Array.Empty<PlacementZone8FlightAnchorHolder>();
        [FlatBufferItem(12)] public PlacementZone8PokeCenterSpawnAnchorHolder[] PokeCenterAnchor { get; set; } = Array.Empty<PlacementZone8PokeCenterSpawnAnchorHolder>();
        [FlatBufferItem(13)] public PlacementZone8NPCHolder[] NPCType1 { get; set; } = Array.Empty<PlacementZone8NPCHolder>();
        [FlatBufferItem(14)] public PlacementZone8AdvancedTipHolder[] AdvancedTips { get; set; } = Array.Empty<PlacementZone8AdvancedTipHolder>();
        [FlatBufferItem(15)] public PlacementZone8MovementPathHolder[] Paths { get; set; } = Array.Empty<PlacementZone8MovementPathHolder>();
        [FlatBufferItem(16)] public PlacementZone8OtherNPCHolder[] NPCType2 { get; set; } = Array.Empty<PlacementZone8OtherNPCHolder>();
        [FlatBufferItem(17)] public PlacementZone8QuadrantHolder[] Quadrants { get; set; } = Array.Empty<PlacementZone8QuadrantHolder>();
        [FlatBufferItem(18)] public PlacementZone8FishingPointHolder[] FishingPoint { get; set; } = Array.Empty<PlacementZone8FishingPointHolder>();
        [FlatBufferItem(19)] public PlacementZone8HiddenItemHolder[] HiddenItems { get; set; } = Array.Empty<PlacementZone8HiddenItemHolder>();
        [FlatBufferItem(20)] public PlacementZone8SymbolSpawnHolder[] Symbols { get; set; } = Array.Empty<PlacementZone8SymbolSpawnHolder>();
        [FlatBufferItem(21)] public PlacementZone8NestHoleHolder[] Nests { get; set; } = Array.Empty<PlacementZone8NestHoleHolder>();
        [FlatBufferItem(22)] public PlacementZone8BerryTreeHolder[] BerryTrees { get; set; } = Array.Empty<PlacementZone8BerryTreeHolder>();
        [FlatBufferItem(23)] public PlacementZone8LadderHolder[] Ladders { get; set; } = Array.Empty<PlacementZone8LadderHolder>();
        [FlatBufferItem(24)] public PlacementZone8PopupHolder[] Popups { get; set; } = Array.Empty<PlacementZone8PopupHolder>();
        [FlatBufferItem(25)] public PlacementZone8IKStepHolder[] IKStep { get; set; } = Array.Empty<PlacementZone8IKStepHolder>();
        [FlatBufferItem(26)] public PlacementZone8StaticObjectsHolder[] StaticObjects { get; set; } = Array.Empty<PlacementZone8StaticObjectsHolder>();
        [FlatBufferItem(27)] public PlacementZone8RotomRallyEntry[] RotomRally { get; set; } = Array.Empty<PlacementZone8RotomRallyEntry>();

        public override string ToString() => Meta.ZoneID.ToString("X16");

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

            foreach (var sym in Symbols)
            {
                var obj = sym.Object;
                var ident = obj.Identifier;
                yield return $"    {objects[ident.HashObjectName]}:";
                yield return $"        Location: {ident.Location3f}";
                if (obj.SymbolHash is (0xCBF29CE484222645 or 0))
                {
                    yield return "        No symbols."; // shouldn't hit here, if we have a holder we should have a symbol to hold.
                    break;
                }

                var line = $"SymbolHash: {obj.SymbolHash:X16}, ObjectHash:{obj.Identifier.HashObjectName:X16}, {nameof(PlacementZone8SymbolSpawn.Field_06)}: {obj.Field_06}, {nameof(PlacementZone8SymbolSpawn.Field_01)}: {obj.Field_01}";
                yield return $"            {line}";
            }

            foreach (var so in StaticObjects)
            {
                var obj = so.Object;
                var ident = obj.Identifier;
                yield return $"    {objects[ident.HashObjectName]}:";
                yield return $"        Location: {ident.Location3f}";
                if (obj.Spawns.Length == 0)
                {
                    yield return "        No spawns."; // shouldn't hit here, if we have a holder we should have a spawn to hold.
                    break;
                }

                var s = obj.Spawns;
                var first = s[0];
                var spawnId = first.SpawnID;
                if (Array.TrueForAll(s, z => z.SpawnID == spawnId))
                {
                    yield return "        All Weather:";
                    foreach (var line in first.GetSummary(statics, species))
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
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public ulong ZoneID { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public string Field_03 { get; set; } = ""; // none have this
        [FlatBufferItem(04)] public uint Field_04 { get; set; }
        [FlatBufferItem(05)] public string Music { get; set; } = "";
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
        [FlatBufferItem(09)] public ulong Hash_09 { get; set; }
        [FlatBufferItem(10)] public byte Field_10 { get; set; }
        [FlatBufferItem(11)] public byte Field_11 { get; set; }
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
        [FlatBufferItem(13)] public byte Field_13 { get; set; }
        [FlatBufferItem(14)] public byte Field_14 { get; set; }
        [FlatBufferItem(15)] public int Num_15 { get; set; }

        public override string ToString() => $"{Field_00.HashObjectName:X16}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneMetaTripleXYZ8
    {
        [FlatBufferItem(00)] public float LocationX { get; set; }
        [FlatBufferItem(01)] public float LocationY { get; set; }
        [FlatBufferItem(02)] public float LocationZ { get; set; }
        [FlatBufferItem(03)] public float RotationX { get; set; } // assumed
        [FlatBufferItem(04)] public float RotationY { get; set; }
        [FlatBufferItem(05)] public float RotationZ { get; set; } // assumed
        [FlatBufferItem(06)] public float ScaleX    { get; set; }
        [FlatBufferItem(07)] public float ScaleY    { get; set; }
        [FlatBufferItem(08)] public float ScaleZ    { get; set; }
        [FlatBufferItem(09)] public ulong HashObjectName { get; set; }
        [FlatBufferItem(10)] public ulong Hash_10   { get; set; }
        [FlatBufferItem(11)] public ulong Hash_11   { get; set; }

        public string Location3f => $"({LocationX}, {LocationY}, {LocationZ})";

        public void Upscale(float factor)
        {
            ScaleX *= factor;
            ScaleY *= factor;
            ScaleZ *= factor;
        }

        public void ResetScale() => ScaleX = ScaleY = ScaleZ = 1;

        public override string ToString() => $"{HashObjectName:X16} @ {Location3f}";

        public PlacementZoneMetaTripleXYZ8 Clone() => new()
        {
            LocationX = LocationX,
            LocationY = LocationY,
            LocationZ = LocationZ,
            RotationX = RotationX,
            RotationY = RotationY,
            RotationZ = RotationZ,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            ScaleZ = ScaleZ,
            HashObjectName = HashObjectName,
            Hash_10 = Hash_10,
            Hash_11 = Hash_11,
        };
    }
}
