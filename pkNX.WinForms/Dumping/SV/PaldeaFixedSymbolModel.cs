using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.WinForms;

public class PaldeaFixedSymbolModel
{
    public readonly List<PaldeaFixedSymbolPoint>[] scarletPoints = [new(), new(), new()];
    public readonly List<PaldeaFixedSymbolPoint>[] violetPoints = [new(), new(), new()];

    public readonly Dictionary<string, string[]> MultiSpawner;

    public PaldeaFixedSymbolModel(IFileInternal ROM)
    {
        var gemLottery = FlatBufferConverter.DeserializeFrom<GemSymbolLotteryTableArray>(
            ROM.GetPackedFile("world/data/field/fixed_symbol/gem_symbol_lottery_table/gem_symbol_lottery_table_array.bin")).Table;
        MultiSpawner = gemLottery.ToDictionary(z => z.LotteryKey, GetTableGetKeys);

        // Paldea
        var p0Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_0.trscn");
        var p1Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_1.trscn");

        var p0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(p0Data);
        var p1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(p1Data);

        scarletPoints[(int)PaldeaFieldIndex.Paldea].AddRange(GetObjectTemplateSymbolPoints(p0));
        violetPoints[(int)PaldeaFieldIndex.Paldea].AddRange(GetObjectTemplateSymbolPoints(p1));

        // Kitakami
        var k0Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/su1_world_fixed_placement_symbol_/su1_world_fixed_placement_symbol_0.trscn");
        var k1Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/su1_world_fixed_placement_symbol_/su1_world_fixed_placement_symbol_1.trscn");

        var k0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(k0Data);
        var k1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(k1Data);

        scarletPoints[(int)PaldeaFieldIndex.Kitakami].AddRange(GetObjectTemplateSymbolPoints(k0));
        violetPoints[(int)PaldeaFieldIndex.Kitakami].AddRange(GetObjectTemplateSymbolPoints(k1));

        // Terarium
        var b0Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/su2_world_fixed_placement_symbol_/su2_world_fixed_placement_symbol_0.trscn");
        var b1Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/su2_world_fixed_placement_symbol_/su2_world_fixed_placement_symbol_1.trscn");

        var b0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(b0Data);
        var b1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(b1Data);

        scarletPoints[(int)PaldeaFieldIndex.Terarium].AddRange(GetObjectTemplateSymbolPoints(b0));
        violetPoints[(int)PaldeaFieldIndex.Terarium].AddRange(GetObjectTemplateSymbolPoints(b1));

        // Underdepths
        //var u0Data = ROM.GetPackedFile("world/scene/parts/field/room/a_w23_d10/a_w23_d10_event/a_w23_d10_fixed_placement_symbol_/a_w23_d10_fixed_placement_symbol_0.trscn");
        //var u1Data = ROM.GetPackedFile("world/scene/parts/field/room/a_w23_d10/a_w23_d10_event/a_w23_d10_fixed_placement_symbol_/a_w23_d10_fixed_placement_symbol_1.trscn");
        //var u0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(u0Data);
        //var u1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(u1Data);
        //
        //scarletPoints[(int)PaldeaFieldIndex.Paldea].AddRange(GetObjectTemplateSymbolPoints(u0));
        //violetPoints[(int)PaldeaFieldIndex.Paldea].AddRange(GetObjectTemplateSymbolPoints(u1));
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
                    var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(obj.Data);
                    if (sObj.Type != "trinity_ScenePoint")
                        continue;
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(sObj.Data);

                    foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(obj.Data);

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
        for (var i = 0; i < subObjects.Count; i++)
        {
            var sobj = subObjects[i];
            switch (sobj.Type)
            {
                case "trinity_PropertySheet":
                {
                    var propSheet = FlatBufferConverter.DeserializeFrom<TrinityPropertySheet>(sobj.Data);
                    if (propSheet.Name == "fixed_symbol_point")
                    {
                        var tableKey = GetTableKey(propSheet);
                        if (!string.IsNullOrEmpty(tableKey))
                        {
                            if (MultiSpawner.TryGetValue(tableKey, out var others))
                            {
                                // Can spawn multiple fixed encounters.
                                foreach (var other in others)
                                    yield return new PaldeaFixedSymbolPoint(other, scenePoint.Position);
                            }
                            else
                            {
                                yield return new PaldeaFixedSymbolPoint(tableKey, scenePoint.Position);
                            }
                        }
                    }
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var subScenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(sobj.Data);
                    foreach (var f in GetScenePointSymbolPoints(subScenePoint, sobj.SubObjects))
                        yield return f;
                    break;
                    }
                case "trinity_ObjectTemplate":
                    {
                        var ssObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(sobj.Data);
                        if (ssObj.Type != "trinity_ScenePoint")
                            continue;
                        var subScenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(ssObj.Data);

                        foreach (var f in GetScenePointSymbolPoints(subScenePoint, sobj.SubObjects))
                            yield return f;
                        break;
                    }
                default:
                    throw new ArgumentException($"Unknown SubObject {sobj.Type}");
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
