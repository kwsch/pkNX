using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.WinForms;

public static class MassOutbreakRipper
{
    public static void DumpDeliveryOutbreaks(IFileInternal ROM, string path)
    {
        var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).OrderBy(z => z);
        foreach (var dir in dirs)
            DumpDeliveryOutbreakData(ROM, dir);
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
    }

    private static byte[] GetDistributionContents(string path, out int index)
    {
        index = 0; //  todo
        return File.ReadAllBytes(path);
    }

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
