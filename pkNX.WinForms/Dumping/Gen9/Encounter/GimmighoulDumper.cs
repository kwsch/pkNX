using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public static class GimmighoulDumper
{
    public static void Dump(IFileInternal rom, EncounterDumpConfigSV config, PaldeaSceneModel scene)
    {
        var csym = new PaldeaCoinSymbolModel(rom);
        var eventBattle = FlatBufferConverter.DeserializeFrom<EventBattlePokemonArray>(rom.GetPackedFile("world/data/battle/eventBattlePokemon/eventBattlePokemon_array.bin"));
        Dump(config, csym, scene, eventBattle);
    }

    private static void Dump(EncounterDumpConfigSV config, PaldeaCoinSymbolModel csym, PaldeaSceneModel scene, EventBattlePokemonArray eventBattle)
    {
        using var cw = File.CreateText(Path.Combine(config.Path, "titan_coin_symbol.txt"));
        foreach (var entry in csym.Points[(int)PaldeaFieldIndex.Paldea])
        {
            var areas = new List<string>();
            foreach (var areaName in scene.AreaNames[(int)PaldeaFieldIndex.Paldea])
            {
                if (scene.PaldeaType[(int)PaldeaFieldIndex.Paldea][areaName] != PaldeaPointPivot.Overworld)
                    continue;

                var areaInfo = scene.AreaInfos[(int)PaldeaFieldIndex.Paldea][areaName];
                var name = areaInfo.LocationNameMain;
                if (string.IsNullOrEmpty(name)) // Don't worry about subzones
                    continue;
                if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
                    continue;

                if (scene.IsPointContained(PaldeaFieldIndex.Paldea, areaName, entry.Position.X, entry.Position.Y, entry.Position.Z))
                    areas.Add(areaName);
            }

            // var locs = areas.Select(a => placeNameMap[scene.AreaInfos[a].LocationNameMain].Index).Distinct().ToList();

            cw.WriteLine("===");
            cw.WriteLine(entry.Name);
            cw.WriteLine("===");
            cw.WriteLine($"  First Num:   {entry.FirstNum}");
            cw.WriteLine($"  Coordinates: ({entry.Position.X}, {entry.Position.Y}, {entry.Position.Z})");

            if (entry.IsBox)
            {
                cw.WriteLine($"  Box Label:   {entry.BoxLabel}");
                cw.WriteLine("  PokeData:");
                var pd = eventBattle.Table.First(e => e.Label == entry.BoxLabel).PokeData;

                cw.WriteLine($"    Species: {config[pd.DevId]}");
                cw.WriteLine($"    Form:    {pd.FormId}");
                cw.WriteLine($"    Level:   {pd.Level}");
                cw.WriteLine($"    Sex:     {pd.Sex.Humanize()}");
                cw.WriteLine($"    Shiny:   {pd.RareType.Humanize()}");

                var talentStr = pd.TalentType switch
                {
                    TalentType.RANDOM => "Random",
                    TalentType.V_NUM => $"{pd.TalentVnum} Perfect",
                    TalentType.VALUE => $"{pd.TalentValue.HP}/{pd.TalentValue.ATK}/{pd.TalentValue.DEF}/{pd.TalentValue.SPA}/{pd.TalentValue.SPD}/{pd.TalentValue.SPE}",
                    _ => "Invalid",
                };
                cw.WriteLine($"    IVs:     {talentStr}");
                cw.WriteLine($"    Ability: {pd.Tokusei.Humanize()}");
                switch (pd.WazaType)
                {
                    case WazaType.DEFAULT:
                        cw.WriteLine("    Moves:   Random");
                        break;
                    case WazaType.MANUAL:
                        cw.WriteLine($"    Moves:   {config[pd.Waza1]}/{config[pd.Waza2]}/{config[pd.Waza3]}/{config[pd.Waza4]}");
                        break;
                }

                cw.WriteLine($"    Scale:   {pd.ScaleType.Humanize(pd.ScaleValue)}");
                cw.WriteLine($"    GemType: {(int)pd.GemType}");
            }

            cw.WriteLine("  Areas:");
            foreach (var areaName in areas)
            {
                var areaInfo = scene.AreaInfos[(int)PaldeaFieldIndex.Paldea][areaName];
                var loc = areaInfo.LocationNameMain;
                var (name, index) = config.PlaceNameMap[loc];
                cw.WriteLine($"    - {areaName} - {loc} - {name} ({index})");
            }
        }
    }
}
