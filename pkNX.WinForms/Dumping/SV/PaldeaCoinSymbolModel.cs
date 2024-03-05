using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.WinForms;

public class PaldeaCoinSymbolModel
{
    public readonly List<PaldeaCoinSymbolPoint>[] Points = [[], []];

    public PaldeaCoinSymbolModel(IFileInternal ROM)
    {
        var cData = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_coin_placement_symbol_/world_coin_placement_symbol_0.trscn");
        // NOTE: Fine to only use Scarlet, Violet data is identical.

        var c = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(cData);

        Points[(int)PaldeaFieldIndex.Paldea].AddRange(GetObjectTemplateSymbolPoints(c));

        // TODO: are there coin points in su1/su2?
    }

    private static IEnumerable<PaldeaCoinSymbolPoint> GetObjectTemplateSymbolPoints(TrinitySceneObjectTemplate template)
    {
        foreach (var obj in template.Objects)
        {
            switch (obj.Type)
            {
                case "trinity_ScenePoint":
                {
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(obj.Data);

                    if (obj.SubObjects.Count != 1)
                        throw new ArgumentException($"Unexpected CoinSymbolPoint SubObject Count {obj.SubObjects.Count}");

                    if (obj.SubObjects[0].Type != "trinity_PropertySheet")
                        throw new ArgumentException($"Unexpected CoinSymbolPoint SubObject ({obj.SubObjects[0].Type})");

                    var propSheet = FlatBufferConverter.DeserializeFrom<TrinityPropertySheet>(obj.SubObjects[0].Data);

                    yield return ParseCoinSymbolPoint(scenePoint, propSheet);
                    break;
                }
                default:
                    throw new ArgumentException($"Unsupported CoinPlacement Object Type {obj.Type}");
            }
        }
    }

    private static PaldeaCoinSymbolPoint ParseCoinSymbolPoint(TrinityScenePoint sp, TrinityPropertySheet ps)
    {
        return ps.Name switch
        {
            "coin_walk_symbol_point" => new PaldeaCoinSymbolPoint(sp.Name, GetFirstNum(ps), string.Empty, sp.Position),
            "coin_box_symbol_point" => new PaldeaCoinSymbolPoint(sp.Name, GetFirstNum(ps), GetBoxLabel(ps), sp.Position),
            _ => throw new ArgumentException($"Unknown CoinSymbol PropertySheet {ps.Name}"),
        };
    }

    private static ulong GetFirstNum(TrinityPropertySheet ps)
    {
        if (ps.Properties[0].Fields[0].Name != "firstNum")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (ps.Properties[0].Fields[0].Data.Item1 is not { } sv)
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }

    private static string GetBoxLabel(TrinityPropertySheet ps)
    {
        if (ps.Properties[0].Fields[1].Name != "label")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (ps.Properties[0].Fields[1].Data.Item3 is not { } sv)
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }
}
