namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class PlacementZone
{
    public override string ToString() => Meta.ZoneID.ToString("X16");

    // More tables exist here

    public IEnumerable<string> GetSummary(IList<EncounterStatic> statics,
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
            if (obj.SymbolHash is 0 or 0xCBF29CE484222645)
            {
                yield return "        No symbols."; // shouldn't hit here, if we have a holder we should have a symbol to hold.
                break;
            }

            var line = $"SymbolHash: {obj.SymbolHash:X16}, ObjectHash:{obj.Identifier.HashObjectName:X16}, {nameof(PlacementZoneSymbolSpawn.Field06)}: {obj.Field06}, {nameof(PlacementZoneSymbolSpawn.Field01)}: {obj.Field01}";
            yield return $"            {line}";
        }

        foreach (var so in StaticObjects)
        {
            var obj = so.Object;
            var ident = obj.Identifier;
            yield return $"    {objects[ident.HashObjectName]}:";
            yield return $"        Location: {ident.Location3f}";
            if (obj.Spawns.Count == 0)
            {
                yield return "        No spawns."; // shouldn't hit here, if we have a holder we should have a spawn to hold.
                break;
            }

            var s = obj.Spawns;
            var first = s[0];
            var spawnId = first.SpawnID;
            if (s.All(z => z.SpawnID == spawnId))
            {
                yield return "        All Weather:";
                foreach (var line in first.GetSummary(statics, species))
                    yield return $"            {line}";
            }
            else
            {
                for (var i = 0; i < s.Count; i++)
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

public partial class PlacementZoneMeta
{
    public override string ToString() => $"{Field00.HashObjectName:X16}";
}

public partial class PlacementZoneMetaTripleXYZ
{
    public string Location3f => $"({LocationX}, {LocationY}, {LocationZ})";

    public void Upscale(float factor)
    {
        ScaleX *= factor;
        ScaleY *= factor;
        ScaleZ *= factor;
    }

    public void ResetScale() => ScaleX = ScaleY = ScaleZ = 1;

    public override string ToString() => $"{HashObjectName:X16} @ {Location3f}";
}
