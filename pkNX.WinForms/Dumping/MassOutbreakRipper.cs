using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PKHeX.Core;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;
using static pkNX.Structures.Species;

namespace pkNX.WinForms;

public static class MassOutbreakRipper
{
    private static readonly List<PickledOutbreak> Encounters = new();
    private static int EncounterIndex;
    private static Dictionary<string, (string Name, int Index)> NameDict = new();

    public static void DumpDeliveryOutbreaks(IFileInternal ROM, string path, string dump)
    {
        Encounters.Clear();

        var cfg = new TextConfig(Structures.GameVersion.SV);
        var place_names = GetCommonText(ROM, "place_name", "English", cfg);
        var data = ROM.GetPackedFile("message/dat/English/common/place_name.tbl");
        var ahtb = new AHTB(data);
        NameDict = EncounterDumperSV.GetPlaceNameMap(place_names, ahtb);

        var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).OrderBy(z => z);
        foreach (var dir in dirs)
            DumpDeliveryOutbreakData(ROM, dir);
        ExportPickle(dump, Encounters);
    }

    private sealed record PickledOutbreak
    {
        public required CachedOutbreak Parent { get; init; }
        public required ushort Species { get; init; }
        public required byte Form { get; init; }
        public byte Gender { get; init; } = 0xFF;

        public required RibbonIndex Ribbon { get; init; }
        public required byte MetBase { get; init; }

        public required Dictionary<LevelRange, UInt128> MetPermit { get; init; }

        public required bool ForceScaleRange { get; init; }
        public required byte ScaleMin { get; init; }
        public required byte ScaleMax { get; init; }
        public required bool IsShiny { get; init; }
    }

    private static void DumpDeliveryOutbreakData(IFileInternal ROM, string path)
    {
        var zoneF0 = Path.Combine(path, "zone_main_array_2_0_0");
        var zoneF1 = Path.Combine(path, "zone_su1_array_2_0_0");
        var zoneF2 = Path.Combine(path, "zone_su2_array_2_0_0");
        var pokedata = Path.Combine(path, "pokedata_array_2_0_0");

        if (!File.Exists(pokedata))
            return;
        if (!File.Exists(zoneF2)) // temporary workaround until DLC 2
            zoneF2 = zoneF1;

        var dataZoneF0 = GetDistributionContents(zoneF0);
        var dataZoneF1 = GetDistributionContents(Path.Combine(path, zoneF1));
        var dataZoneF2 = GetDistributionContents(Path.Combine(path, zoneF2));
        var dataPokeData = GetDistributionContents(Path.Combine(path, pokedata));

        var tableZoneF0 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF0);
        var tableZoneF1 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF1);
        var tableZoneF2 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF2);
        var tablePokeData = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakPokeDataArray>(dataPokeData);

        var dirDistText = Path.Combine(path, "parse");
        ExportParse(dirDistText, tableZoneF0, tableZoneF1, tableZoneF2, tablePokeData);

        AddToPickleJar(ROM, tableZoneF0, tableZoneF1, tableZoneF2, tablePokeData);
        DumpPretty(ROM, dirDistText);
        EncounterIndex += Encounters.Count;
    }

    private static void ExportPickle(string dump, IEnumerable<PickledOutbreak> encounters)
    {
        var dest = Path.Combine(dump, "encounter_outbreak_paldea.pkl");
        using var fs = File.Create(dest);
        using var bw = new BinaryWriter(fs);
        foreach (var enc in encounters.DistinctBy(x => x))
        {
            var ordered = enc.MetPermit
                .OrderBy(z => z.Key.Min)
                .ThenBy(z => z.Key.Max);

            foreach (var (range, permit) in ordered)
                WriteEncounter(enc, bw, range, permit);
        }
    }

    private static void WriteEncounter(PickledOutbreak enc, BinaryWriter bw, LevelRange range, UInt128 permit)
    {
        bw.Write(enc.Species);
        bw.Write(enc.Form);
        bw.Write(enc.Gender);

        bw.Write(range.Min);
        bw.Write(range.Max);
        bw.Write((byte)enc.Ribbon);
        bw.Write(enc.MetBase);

        bw.Write(enc.ForceScaleRange ? (byte)1 : (byte)0);
        bw.Write(enc.ScaleMin);
        bw.Write(enc.ScaleMax);
        bw.Write(enc.IsShiny ? (byte)1 : (byte)0);

        bw.Write((ulong)permit);
        bw.Write((ulong)(permit >> 64));

        return;
    }

    private static byte ClampAddBoost(byte val, int b) => (byte)Math.Clamp(val + b, 1, 100);

    private static void AddToPickleJar(IFileInternal ROM, DeliveryOutbreakArray f0, DeliveryOutbreakArray f1, DeliveryOutbreakArray f2, DeliveryOutbreakPokeDataArray pd)
    {
        var dpF0 = ROM.GetPackedFile("world/data/encount/point_data/outbreak_point_data/outbreak_point_main.bin");
        var dpF1 = ROM.GetPackedFile("world/data/encount/point_data/outbreak_point_data/outbreak_point_su1.bin");
      //var dpF2 = ROM.GetPackedFile("world/data/encount/point_data/outbreak_point_data/outbreak_point_su2.bin");

        var pointsF0 = FlatBufferConverter.DeserializeFrom<OutbreakPointArray>(dpF0).Table;
        var pointsF1 = FlatBufferConverter.DeserializeFrom<OutbreakPointArray>(dpF1).Table;
      //var pointsF2 = FlatBufferConverter.DeserializeFrom<OutbreakPointArray>(dpF2).Table;

        var field = new PaldeaFieldModel(ROM);
        var scene = new PaldeaSceneModel(ROM, field);

        ScanAssertions(pd);
        AddForMap(pointsF0, f0, pd, scene, NameDict, PaldeaFieldIndex.Paldea, 6);
        AddForMap(pointsF1, f1, pd, scene, NameDict, PaldeaFieldIndex.Kitakami, 132);
      //AddForMap(pointsF2, f2, pd, scene, NameDict, PaldeaFieldIndex.Blueberry, 170);
    }

    private static void ScanAssertions(params DeliveryOutbreakPokeDataArray[] obs)
    {
        // Ensure unused fields are not observed in the data -- if seen, pickle format needs to be updated.
        foreach (var ob in obs)
        {
            foreach (var o in ob.Table)
            {
                if (o.EnableRarePercentage) // Likely will never be 100%, so if we ever see this, change the check to assert that instead.
                    throw new Exception($"Elevated shiny rate {o.RarePercentage}%!");
                if (o.EnableScaleRange)
                    throw new Exception($"Enforced scale range {o.MinScale}-{o.MaxScale}!");
            }
        }
    }

    private const float Tolerance = 30f;
    private const float TolX = Tolerance, TolY = Tolerance, TolZ = Tolerance;

    private static void AddForMap(IEnumerable<OutbreakPointData> points,
        DeliveryOutbreakArray possible, DeliveryOutbreakPokeDataArray pd,
        PaldeaSceneModel scene, Dictionary<string, (string Name, int Index)> placeNameMap, PaldeaFieldIndex fieldIndex,
        byte baseMet)
    {
        var encs = GetMetaEncounter(possible.Table, pd);
        foreach (var enc in encs)
            enc.MetBase = baseMet;

        var areas = scene.AreaInfos[(int)fieldIndex];
        var areaNames = scene.AreaNames[(int)fieldIndex];
        var atlantis = scene.IsAtlantis[(int)fieldIndex];
        if (fieldIndex == PaldeaFieldIndex.Kitakami)
        {
            areas = areas.Where(z => z.Value.AdjustEncLv != 0)
                .ToDictionary(z => z.Key, z => z.Value);
            areaNames = areas.Keys.ToList();
        }
        foreach (var point in points)
        {
            foreach (var enc in encs)
            {
                var poke = enc.Poke;
                if (!poke.IsLevelRangeCompatible(point.LevelRange))
                    continue;
                if (!poke.IsEnableCompatible(point.EnableTable))
                    continue;
                if (!poke.IsCompatibleArea((byte)point.AreaNo))
                    continue;
                if (!poke.IsCompatibleArea(point.AreaName))
                    continue;

                var min = Math.Max((byte)poke.MinLevel, (byte)point.LevelRange.X);
                var max = Math.Min((byte)poke.MaxLevel, (byte)point.LevelRange.Y);
                // Cool, can spawn at this point.
                // Find all met location IDs we can wander to, then bitflag them into the enc field.
                var metData = GetMetFlags(scene, placeNameMap, fieldIndex, baseMet, areaNames, areas, point, atlantis);
                var tuple = new LevelRange(min, max);
                enc.MetInfo.TryGetValue(tuple, out var val);
                enc.MetInfo[tuple] = val | metData.Met;
                if (metData.Boost != 0)
                {
                    tuple = new LevelRange(ClampAddBoost(tuple.Min, metData.Boost), ClampAddBoost(tuple.Max, metData.Boost));
                    enc.MetInfo.TryGetValue(tuple, out val);
                    enc.MetInfo[tuple] = val | metData.Met;
                }
            }
        }

        ConsolidateMetInfo(encs);

        foreach (var e in encs)
        {
            if (e.MetInfo.Count == 0)
                throw new Exception("No met flags found for encounter!");
        }

        AddToJar(encs);
    }

    private static void ConsolidateMetInfo(IEnumerable<CachedOutbreak> encs)
    {
        foreach (var e in encs)
        {
            var ranges = e.MetInfo.ToList();
            // If the level ranges overlap for any key value pair with the same flags, consolidate them.
            // For each entry, if a later entry has the same flags, and the level ranges overlap, delete the later entry after merging the level ranges into the first entry.
            for (int i = 0; i < ranges.Count; i++)
            {
                var ((min, max), met) = ranges[i];
                for (int j = i + 1; j < ranges.Count; j++)
                {
                    var (min2, max2) = ranges[j].Key;
                    var met2 = ranges[j].Value;
                    if (met != met2)
                        continue;
                    if (min2 > max || max2 < min)
                        continue;
                    // Overlap, merge.
                    min = Math.Min(min, min2);
                    max = Math.Max(max, max2);
                    ranges.RemoveAt(j);
                    j--;
                }
                ranges[i] = new(new(min, max), met);
            }
            e.MetInfo.Clear();
            foreach (var r in ranges)
                e.MetInfo[r.Key] = r.Value;
        }
    }

    private record struct LevelRange(byte Min, byte Max);

    private static void AddToJar(IEnumerable<CachedOutbreak> encs)
    {
        foreach (var enc in encs)
        {
            var pk = enc.Poke;
            ushort species = SpeciesConverterSV.GetNational9((ushort)pk.DevId);
            byte form = (byte)pk.FormId;
            if (species is (int)Scatterbug or (int)Spewpa or (int)Vivillon)
                form = 30;

            var pickled = new PickledOutbreak
            {
                Parent = enc,
                Species = species,
                Form = form,
                Gender = pk.Sex switch
                {
                    SexType.DEFAULT => 0xFF,
                    SexType.MALE => 0,
                    SexType.FEMALE => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(SexType), pk.Sex, "Unknown Gender"),
                },
                Ribbon = unchecked((RibbonIndex)(pk.AddRibbonPercentage == 0 ? -1 : (int)pk.AddRibbonType - 1)),

                ForceScaleRange = pk.EnableScaleRange,
                ScaleMin = (byte)pk.MinScale,
                ScaleMax = (byte)pk.MaxScale,
                IsShiny = pk.RarePercentage >= 100,
                MetBase = enc.MetBase,
                MetPermit = enc.MetInfo,
            };
            Encounters.Add(pickled);
        }
    }

    private static (UInt128 Met, int Boost) GetMetFlags(PaldeaSceneModel scene, Dictionary<string, (string Name, int Index)> placeNameMap, PaldeaFieldIndex fieldIndex,
        byte baseMet, List<string> areaNames, Dictionary<string, AreaInfo> areas, OutbreakPointData point, Dictionary<string, bool> atlantis)
    {
        UInt128 result = 0;
        int boost = 0;
        for (var i = areaNames.Count - 1; i >= 0; i--)
        {
            var areaName = areaNames[i];
            var areaInfo = areas[areaName];
            if (atlantis[areaName])
                continue;

            if (areaInfo.Tag is AreaTag.NG_Encount)
                continue;
            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out var collider))
                continue;
            var pt = point.Position;
            if (!collider.ContainsPoint(pt.X, pt.Y, pt.Z))
                continue;
            if (!EncounterDumperSV.TryGetPlaceName(ref areaName, areaInfo, pt, placeNameMap, areas, scene, fieldIndex, out var placeName))
                continue;
            var loc = placeNameMap[placeName].Index;

            var info = areas[areaName];
            boost = info.AdjustEncLv;
            var actual = loc - baseMet;
            result |= (UInt128)1 << actual;

            for (var x = 0; x < areaNames.Count; x++)
            {
                var area = areaNames[x];
                if (area == areaName)
                    continue;

                if (!scene.TryGetContainsCheck(fieldIndex, area, out var subCol))
                    continue;
                if (!subCol.ContainsPoint(pt.X, pt.Y, pt.Z, TolX, TolY, TolZ))
                    continue;
                if (!EncounterDumperSV.TryGetPlaceName(ref area, areaInfo, pt, placeNameMap, areas, scene, fieldIndex, out placeName))
                    continue;
                loc = placeNameMap[placeName].Index;
                if (!EncounterDumperSV.IsCrossoverAllowed(loc))
                    continue;

                actual = loc - baseMet;
                result |= (UInt128)1 << actual;
            }

            break;
        }
        return (result, boost);
    }

    private class CachedOutbreak
    {
        public ulong ZoneID { get; init; }
        public required DeliveryOutbreakPokeData Poke { get; init; }

        public byte MetBase { get; set; }
        public readonly Dictionary<LevelRange, UInt128> MetInfo = new();
    }

    private static CachedOutbreak[] GetMetaEncounter(IEnumerable<DeliveryOutbreak> possibleTable, DeliveryOutbreakPokeDataArray pd)
    {
        var ret = new List<CachedOutbreak>();
        var hs = new HashSet<ulong>();
        foreach (var outbreak in possibleTable)
        {
            TryAdd(outbreak.Poke1, outbreak.Poke1LotValue);
            TryAdd(outbreak.Poke2, outbreak.Poke2LotValue);
            TryAdd(outbreak.Poke3, outbreak.Poke3LotValue);
            TryAdd(outbreak.Poke4, outbreak.Poke4LotValue);
            TryAdd(outbreak.Poke5, outbreak.Poke5LotValue);
            continue;
            void TryAdd(ulong ID, short rate)
            {
                if (ID == 0 || rate <= 0 || !hs.Add(ID))
                    return;
                var poke = pd.Table.First(z => z.ID == ID);
                ret.Add(new CachedOutbreak { ZoneID = outbreak.ZoneID, Poke = poke });
            }
        }
        return ret.ToArray();
    }

    private static byte[] GetDistributionContents(string path) => File.ReadAllBytes(path);

    private static string Humanize(EnableTable enable)
    {
        var sb = new StringBuilder();
        if (enable.Air1) sb.Append(" Air1");
        if (enable.Air2) sb.Append(" Air2");
        if (enable.Land) sb.Append(" Land");
        if (enable.UpWater) sb.Append(" UpWater");
        if (enable.Underwater) sb.Append(" Underwater");
        if (sb.Length == 0)
            return "Enable: Unrestricted";
        return "Enable:" + sb;
    }

    private static string Humanize(VersionTable? ver) => ver switch
    {
        { A: true, B: true } => "Version: Both",
        { A: true } => "Version: Scarlet",
        { B: true } => "Version: Violet",
        _ => "Version: None",
    };

    private static void ExportParse(string dir, DeliveryOutbreakArray tableZoneF0, DeliveryOutbreakArray tableZoneF1, DeliveryOutbreakArray tableZoneF2, DeliveryOutbreakPokeDataArray tablePokeData)
    {
        var dumpZ0 = TableUtil.GetTable(tableZoneF0.Table);
        var dumpZ1 = TableUtil.GetTable(tableZoneF1.Table);
      //var dumpZ2 = TableUtil.GetTable(tableZoneF2.Table);
        var dumpPD = TableUtil.GetTable(tablePokeData.Table);
        var dump = new[]
        {
            ("zone_main", dumpZ0),
            ("zone_su1", dumpZ1),
          //("zone_su2", dumpZ2),
            ("pokedata", dumpPD),
        };

        Directory.CreateDirectory(dir);
        foreach (var (name, data) in dump)
        {
            var path2 = Path.Combine(dir, $"{name}.txt");
            File.WriteAllText(path2, data);
        }

        DumpJson(tableZoneF0, dir, "zone_main");
        DumpJson(tableZoneF1, dir, "zone_su1");
      //DumpJson(tableZoneF2, dir, "zone_su2");
        DumpJson(tablePokeData, dir, "pokedata");
    }

    private static void DumpJson(object flat, string dir, string name)
    {
        var opt = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(flat, opt);

        var fileName = Path.ChangeExtension(name, ".json");
        File.WriteAllText(Path.Combine(dir, fileName), json);
    }

    private static string[] GetCommonText(IFileInternal ROM, string name, string lang, TextConfig cfg)
    {
        var data = ROM.GetPackedFile($"message/dat/{lang}/common/{name}.dat");
        return new TextFile(data, cfg).Lines;
    }

    private static void DumpPretty(IFileInternal ROM, string dir)
    {
        string ident = $"{Encounters[EncounterIndex].Parent.Poke.ID}"[..8]; // yyyyMMdd, remove 3 digit index
        string fileName = $"pretty_{ident}.txt";
        using var sw = File.CreateText(Path.Combine(dir, fileName));

        foreach (var enc in Encounters)
        {
            if (EncounterIndex > Encounters.IndexOf(enc))
                continue;
            var parent = enc.Parent;
            WriteParent(ROM, sw, parent);
            WriteLocationList(sw, parent.MetInfo, parent.MetBase);
            sw.WriteLine();
        }
    }

    private static void WriteLocationList(TextWriter sw, Dictionary<LevelRange, UInt128> metFlags, byte baseMet)
    {
        foreach ((var range, UInt128 flags) in metFlags.OrderBy(z => z.Key.Min).ThenBy(z => z.Key.Max))
        {
            for (int i = 0; i < 128; i++)
            {
                if (((flags >> i) & 1) != 1)
                    continue;

                var met = baseMet + i;
                var location = NameDict.First(z => z.Value.Index == met);
                sw.WriteLine($"\tLv. {range.Min}-{range.Max} @ {location}");
            }
        }
    }

    private static void WriteParent(IFileInternal ROM, StreamWriter sw, CachedOutbreak parent)
    {
        const string lang = "English";
        var cfg = new TextConfig(Structures.GameVersion.SV);
        var enc = parent.Poke;

        var species = GetCommonText(ROM, "monsname", lang, cfg);
        var ribbons = GetCommonText(ROM, "ribbon", lang, cfg);

        var formName = TeraRaidRipper.GetFormName(ROM, (ushort)enc.DevId, (byte)enc.FormId);
        var form = formName switch
        {
            not "" when enc.FormId is 0 => $" ({formName})",
            not "" => $"-{enc.FormId} ({formName})",
            _ => string.Empty,
        };
        var shiny = enc.EnableRarePercentage ? $"{enc.RarePercentage}%" : "Standard";

        sw.WriteLine($"ID: {enc.ID}");
        sw.WriteLine($"{species[(int)enc.DevId]}{form}");
        sw.WriteLine(Humanize(enc.Enable!.Value));
        sw.WriteLine(Humanize(enc.Version));
        sw.WriteLine($"Base Level Range: {enc.MinLevel}-{enc.MaxLevel}");
        if (enc.Sex is not SexType.DEFAULT)
            sw.WriteLine($"Gender: {enc.Sex}");
        if (enc.EnableScaleRange)
            sw.WriteLine($"Scale: {enc.MinScale}-{enc.MaxScale}");
        sw.WriteLine($"Shiny Rate: {shiny}");
        if (enc.AddRibbonPercentage != 0)
            sw.WriteLine($"Ribbon/Mark: {ribbons[(int)enc.AddRibbonType - 1]} ({enc.AddRibbonPercentage}%)");
    }
}
