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
using Species = pkNX.Structures.Species;

namespace pkNX.WinForms;

public static class MassOutbreakRipper
{
    private static readonly List<PickledOutbreak> Encounters = new();

    public static void DumpDeliveryOutbreaks(IFileInternal ROM, string path)
    {
        Encounters.Clear();
        var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).OrderBy(z => z);
        foreach (var dir in dirs)
            DumpDeliveryOutbreakData(ROM, dir);
        ExportPickle(Encounters, path);
    }

    private sealed record PickledOutbreak
    {
        public required ushort Species { get; init; }
        public required byte Form { get; init; }
        public byte Gender { get; init; } = 0xFF;

        public required byte LevelMin { get; init; }
        public required byte LevelMax { get; init; }
        public required RibbonIndex Ribbon { get; init; }
        public required byte MetBase { get; init; }

        public required Dictionary<int, UInt128> MetPermit { get; init; }

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

        var dataZoneF0 = GetDistributionContents(zoneF0, out int indexZoneF0);
        var dataZoneF1 = GetDistributionContents(Path.Combine(path, zoneF1), out int indexZoneF1);
        var dataZoneF2 = GetDistributionContents(Path.Combine(path, zoneF2), out int indexZoneF2);
        var dataPokeData = GetDistributionContents(Path.Combine(path, pokedata), out int indexPokeData);

        var tableZoneF0 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF0);
        var tableZoneF1 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF1);
        var tableZoneF2 = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakArray>(dataZoneF2);
        var tablePokeData = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakPokeDataArray>(dataPokeData);

        var dirDistText = Path.Combine(path, "parse");
        ExportParse(ROM, dirDistText, tableZoneF0, tableZoneF1, tableZoneF2, tablePokeData);

        AddToPickleJar(ROM, tableZoneF0, tableZoneF1, tableZoneF2, tablePokeData);
    }

    private static void ExportPickle(List<PickledOutbreak> encounters, string path)
    {
        var dest = Path.Combine(path, "encounters", "outbreak_sv.pkl");
        using var fs = File.Create(dest);
        using var bw = new BinaryWriter(fs);
        foreach (var enc in encounters.DistinctBy(x => x))
        {
            foreach (var kvp in enc.MetPermit)
            {
                var boost = kvp.Key;
                var permit = kvp.Value;
                WriteEncounter(enc, bw, boost, permit);
            }
        }
    }

    private static void WriteEncounter(PickledOutbreak enc, BinaryWriter bw, int boost, UInt128 permit)
    {
        bw.Write(enc.Species);
        bw.Write(enc.Form);
        bw.Write(enc.Gender);

        bw.Write(Clamp(enc.LevelMin, boost));
        bw.Write(Clamp(enc.LevelMax, boost));
        bw.Write((byte)enc.Ribbon);
        bw.Write(enc.MetBase);

        bw.Write(enc.ForceScaleRange ? (byte)1 : (byte)0);
        bw.Write(enc.ScaleMin);
        bw.Write(enc.ScaleMax);
        bw.Write(enc.IsShiny ? (byte)1 : (byte)0);

        bw.Write((ulong)permit);
        bw.Write((ulong)(permit >> 64));

        return;
        static byte Clamp(byte val, int b) => (byte)Math.Clamp(val + b, 1, 100);
    }

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

        var cfg = new TextConfig(Structures.GameVersion.SV);
        var place_names = GetCommonText(ROM, "place_name", "English", cfg);
        var data = ROM.GetPackedFile("message/dat/English/common/place_name.tbl");
        var ahtb = new AHTB(data);
        var nameDict = EncounterDumperSV.GetPlaceNameMap(place_names, ahtb);

        ScanAssertions(pd);
        AddForMap(pointsF0, f0, pd, scene, nameDict, PaldeaFieldIndex.Paldea, 6);
        AddForMap(pointsF1, f1, pd, scene, nameDict, PaldeaFieldIndex.Kitakami, 132);
      //AddForMap(pointsF2, f2, pd, scene, nameDict, PaldeaFieldIndex.Blueberry, 170);
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
        foreach (var point in points)
        {
            foreach (var enc in encs)
            {
                var poke = enc.Poke;
                if (!poke.IsEnableCompatible(point.EnableTable))
                    continue;
                if (!poke.IsCompatibleArea((byte)point.AreaNo))
                    continue;
                if (!poke.IsCompatibleArea(point.AreaName))
                    continue;

                // Cool, can spawn at this point.
                // Find all met location IDs we can wander to, then bitflag them into the enc field.
                var metData = GetMetFlags(scene, placeNameMap, fieldIndex, baseMet, areaNames, areas, point);
                enc.BoostMetFlags.TryGetValue(metData.Boost, out var val);
                enc.BoostMetFlags[metData.Boost] = val | metData.Met;
            }
        }

        AddToJar(encs);
    }

    private static void AddToJar(CachedOutbreak[] encs)
    {
        foreach (var enc in encs)
        {
            var pk = enc.Poke;
            ushort species = SpeciesConverterSV.GetNational9((ushort)pk.DevId);
            byte form = (byte)pk.FormId;
            if (species is (int)Species.Scatterbug or (int)Species.Spewpa or (int)Species.Vivillon)
                form = 30;

            var pickled = new PickledOutbreak
            {
                Species = species,
                Form = form,
                Gender = pk.Sex switch
                {
                    SexType.DEFAULT => 0xFF,
                    SexType.MALE => 0,
                    SexType.FEMALE => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(SexType), pk.Sex, "Unknown Gender"),
                },
                LevelMin = (byte)pk.MinLevel,
                LevelMax = (byte)pk.MaxLevel,
                Ribbon = pk.AddRibbonPercentage == 0 ? unchecked((RibbonIndex)(-1)) : (RibbonIndex)pk.AddRibbonType,

                ForceScaleRange = pk.EnableScaleRange,
                ScaleMin = (byte)pk.MinScale,
                ScaleMax = (byte)pk.MaxScale,
                IsShiny = pk.RarePercentage >= 100,
                MetBase = enc.MetBase,
                MetPermit = enc.BoostMetFlags,
            };
            Encounters.Add(pickled);
        }
    }

    private static (UInt128 Met, int Boost)  GetMetFlags(PaldeaSceneModel scene, Dictionary<string, (string Name, int Index)> placeNameMap, PaldeaFieldIndex fieldIndex,
        byte baseMet, List<string> areaNames, Dictionary<string, AreaInfo> areas, OutbreakPointData point)
    {
        UInt128 result = 0;
        int boost = 0;
        for (var i = areaNames.Count - 1; i >= 0; i--)
        {
            var areaName = areaNames[i];
            var areaInfo = areas[areaName];
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
        public readonly Dictionary<int, UInt128> BoostMetFlags = new();
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

    private static byte[] GetDistributionContents(string path, out int index)
    {
        index = 0; //  todo
        return File.ReadAllBytes(path);
    }

    private static string Humanize(OutbreakEnableTable enable)
    {
        var sb = new StringBuilder();
        if (enable.Air1) sb.Append(" Air1");
        if (enable.Air2) sb.Append(" Air2");
        if (enable.Land) sb.Append(" Land");
        if (enable.UpWater) sb.Append(" UpWater");
        if (enable.UnderWater) sb.Append(" Underwater");
        if (sb.Length == 0)
            return "Enable: Unrestricted";
        return "Enable:" + sb;
    }

    private static string Humanize(VersionTable ver) => ver switch
    {
        { A: true, B: true } => "Version: Both",
        { A: true } => "Version: Scarlet",
        { B: true } => "Version: Violet",
        _ => "Version: None",
    };

    private static void ExportParse(IFileInternal ROM, string dir, DeliveryOutbreakArray tableZoneF0, DeliveryOutbreakArray tableZoneF1, DeliveryOutbreakArray tableZoneF2, DeliveryOutbreakPokeDataArray tablePokeData)
    {
        var dumpZ0 = TableUtil.GetTable(tableZoneF0.Table);
        var dumpZ1 = TableUtil.GetTable(tableZoneF1.Table);
        var dumpZ2 = TableUtil.GetTable(tableZoneF2.Table);
        var dumpPD = TableUtil.GetTable(tablePokeData.Table);
        var dump = new[]
        {
            ("zone_main", dumpZ0),
            ("zone_su1", dumpZ1),
            ("zone_su2", dumpZ2),
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
        DumpJson(tableZoneF2, dir, "zone_su2");
        DumpJson(tablePokeData, dir, "pokedata");
        DumpPretty(ROM, tableZoneF0, tableZoneF1, tableZoneF2, tablePokeData, dir); // todo
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

    private static void DumpPretty(IFileInternal ROM, DeliveryOutbreakArray tableZoneF0, DeliveryOutbreakArray tableZoneF1, DeliveryOutbreakArray tableZoneF2, DeliveryOutbreakPokeDataArray tablePokeData, string dir)
    {
        // todo
    }
}
