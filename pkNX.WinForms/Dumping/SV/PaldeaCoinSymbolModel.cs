using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class PaldeaCoinSymbolModel
{
    public readonly List<PaldeaCoinSymbolPoint> Points;

    public PaldeaCoinSymbolModel(IFileInternal ROM)
    {
        Points = new List<PaldeaCoinSymbolPoint>();

        var cData = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_coin_placement_symbol_/world_coin_placement_symbol_0.trscn");
        // NOTE: Fine to only use Scarlet, Violet data is identical.

        var c = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(cData);

        Points.AddRange(GetObjectTemplateSymbolPoints(c));
    }

    private IEnumerable<PaldeaCoinSymbolPoint> GetObjectTemplateSymbolPoints(TrinitySceneObjectTemplateSV template)
    {
        foreach (var obj in template.Objects)
        {
            switch (obj.Type)
            {
                case "trinity_ScenePoint":
                {
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(obj.Data);

                    if (obj.SubObjects.Length != 1)
                        throw new ArgumentException($"Unexpected CoinSymbolPoint SubObject Count {obj.SubObjects.Length}");

                    if (obj.SubObjects[0].Type != "trinity_PropertySheet")
                        throw new ArgumentException($"Unexpected CoinSymbolPoint SubObject ({obj.SubObjects[0].Type})");

                    var propSheet = FlatBufferConverter.DeserializeFrom<TrinityPropertySheetSV>(obj.SubObjects[0].Data);

                    yield return ParseCoinSymbolPoint(scenePoint, propSheet);
                    break;
                }
                default:
                    throw new ArgumentException($"Unsupported CoinPlacement Object Type {obj.Type}");
            }
        }
    }

    private PaldeaCoinSymbolPoint ParseCoinSymbolPoint(TrinityScenePointSV sp, TrinityPropertySheetSV ps)
    {
        switch (ps.Name)
        {
            case "coin_walk_symbol_point":
                return new PaldeaCoinSymbolPoint(sp.Name, GetFirstNum(ps), string.Empty, sp.Position);
            case "coin_box_symbol_point":
                return new PaldeaCoinSymbolPoint(sp.Name, GetFirstNum(ps), GetBoxLabel(ps), sp.Position);
            default:
                throw new ArgumentException($"Unknown CoinSymbol PropertySheet {ps.Name}");
        }
    }

    private ulong GetFirstNum(TrinityPropertySheetSV ps)
    {
        if (ps.Properties[0].Fields[0].Name != "firstNum")
            throw new ArgumentException("Invalid PropertySheet field layout");
        
        if (!ps.Properties[0].Fields[0].Data.TryGet(out TrinityPropertySheetField1SV? sv))
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }

    private string GetBoxLabel(TrinityPropertySheetSV ps)
    {
        if (ps.Properties[0].Fields[1].Name != "label")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (!ps.Properties[0].Fields[1].Data.TryGet(out TrinityPropertySheetFieldStringValueSV? sv))
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }
}
