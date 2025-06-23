using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using FlatSharp;

namespace pkNX.Structures.FlatBuffers;

public class PaldeaFixedSymbolModel
{
    public readonly List<PaldeaFixedSymbolPoint>[] PointsScarlet;
    public readonly List<PaldeaFixedSymbolPoint>[] PointsViolet;

    public readonly Dictionary<string, string[]> MultiSpawner;

    private static T Get<T>(Memory<byte> data) where T : class, IFlatBufferSerializable<T>
        => FlatBufferConverter.DeserializeFrom<T>(data);

    public PaldeaFixedSymbolModel(IFileInternal rom)
    {
        var raw = rom.GetPackedFile("world/data/field/fixed_symbol/gem_symbol_lottery_table/gem_symbol_lottery_table_array.bin");
        var gemLottery = Get<GemSymbolLotteryTableArray>(raw).Table;
        MultiSpawner = gemLottery.ToDictionary(z => z.LotteryKey, GetTableGetKeys);

        PointsScarlet = GetSymbols(rom, 0);
        PointsViolet = GetSymbols(rom, 1);
    }

    private List<PaldeaFixedSymbolPoint>[] GetSymbols(IFileInternal rom, int game) =>
    [
        [..GetSymbols(rom, game, "world")],
        [..GetSymbols(rom, game, "su1_world")],
        [..GetSymbols(rom, game, "su2_world")],
    ];

    private IEnumerable<PaldeaFixedSymbolPoint> GetSymbols(IFileInternal rom, int game, string mapName)
    {
        var data = rom.GetPackedFile($"world/scene/parts/field/streaming_event/{mapName}_fixed_placement_symbol_/{mapName}_fixed_placement_symbol_{game}.trscn");
        var template = Get<TrinitySceneObjectTemplate>(data);
        return GetObjectTemplateSymbolPoints(template);
    }

    private static string[] GetTableGetKeys(GemSymbolLotteryTable entry)
    {
        var list = new List<string>();
        if (!string.IsNullOrWhiteSpace(entry.TableKey0)) list.Add(entry.TableKey0);
        if (!string.IsNullOrWhiteSpace(entry.TableKey1)) list.Add(entry.TableKey1);
        if (!string.IsNullOrWhiteSpace(entry.TableKey2)) list.Add(entry.TableKey2);
        if (!string.IsNullOrWhiteSpace(entry.TableKey3)) list.Add(entry.TableKey3);
        if (!string.IsNullOrWhiteSpace(entry.TableKey4)) list.Add(entry.TableKey4);
        if (!string.IsNullOrWhiteSpace(entry.TableKey5)) list.Add(entry.TableKey5);
        if (!string.IsNullOrWhiteSpace(entry.TableKey6)) list.Add(entry.TableKey6);
        if (!string.IsNullOrWhiteSpace(entry.TableKey7)) list.Add(entry.TableKey7);
        if (!string.IsNullOrWhiteSpace(entry.TableKey8)) list.Add(entry.TableKey8);
        if (!string.IsNullOrWhiteSpace(entry.TableKey9)) list.Add(entry.TableKey9);
        return [.. list];
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetObjectTemplateSymbolPoints(TrinitySceneObjectTemplate template)
    {
        foreach (var obj in template.Objects)
        {
            switch (obj.Type)
            {
                case "trinity_ObjectTemplate":
                {
                    var sObj = Get<TrinitySceneObjectTemplateData>(obj.Data);
                    if (sObj.Type != "trinity_ScenePoint")
                        break;
                    var scenePoint = Get<TrinityScenePoint>(sObj.Data);

                    foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var scenePoint = Get<TrinityScenePoint>(obj.Data);

                    foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
            }
        }
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetScenePointSymbolPoints(TrinityScenePoint scenePoint, IList<TrinitySceneObjectTemplateEntry> subObjects)
    {
        // Handle SubObjects
        foreach (var obj in subObjects)
        {
            switch (obj.Type)
            {
                case "trinity_PropertySheet":
                {
                    var propSheet = Get<TrinityPropertySheet>(obj.Data);
                    if (propSheet.Name != "fixed_symbol_point")
                        break;

                    var tableKey = GetTableKey(propSheet);
                    if (string.IsNullOrEmpty(tableKey))
                        break;

                    var position = scenePoint.Position;
                    if (MultiSpawner.TryGetValue(tableKey, out var others))
                    {
                        // Can spawn multiple fixed encounters.
                        foreach (var other in others)
                            yield return new PaldeaFixedSymbolPoint(other, position);
                    }
                    else
                    {
                        yield return new PaldeaFixedSymbolPoint(tableKey, position);
                    }
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var subScenePoint = Get<TrinityScenePoint>(obj.Data);
                    foreach (var f in GetScenePointSymbolPoints(subScenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
                case "trinity_ObjectTemplate":
                {
                    var ssObj = Get<TrinitySceneObjectTemplateData>(obj.Data);
                    if (ssObj.Type != "trinity_ScenePoint")
                            break;
                    var subScenePoint = Get<TrinityScenePoint>(ssObj.Data);

                    foreach (var f in GetScenePointSymbolPoints(subScenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
                default:
                    throw new ArgumentException($"Unknown SubObject {obj.Type}");
            }
        }
    }

    public static string GetTableKey(TrinityPropertySheet propSheet)
    {
        if (propSheet.Name != "fixed_symbol_point")
            throw new ArgumentException($"Invalid PropertySheet {propSheet.Name}");

        if (propSheet.Properties[0].Fields[1].Name != "tableKey")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (propSheet.Properties[0].Fields[1].Data.Item3 is not { } sv)
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }
}
