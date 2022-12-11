using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class PaldeaFixedSymbolModel
{
    public readonly List<PaldeaFixedSymbolPoint> scarletPoints;
    public readonly List<PaldeaFixedSymbolPoint> violetPoints;

    public PaldeaFixedSymbolModel(IFileInternal ROM)
    {
        scarletPoints = new List<PaldeaFixedSymbolPoint>();
        violetPoints = new List<PaldeaFixedSymbolPoint>();

        var p0Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_0.trscn");
        var p1Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_1.trscn");

        var p0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(p0Data);
        var p1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(p1Data);

        scarletPoints.AddRange(GetObjectTemplateSymbolPoints(p0));
        violetPoints.AddRange(GetObjectTemplateSymbolPoints(p1));
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetObjectTemplateSymbolPoints(TrinitySceneObjectTemplateSV template)
    {
        foreach (var obj in template.Objects)
        {
            switch (obj.Type)
            {
                case "trinity_ObjectTemplate":
                {
                    var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateDataSV>(obj.Data);
                    if (sObj.Type != "trinity_ScenePoint")
                        continue;
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(sObj.Data);

                    foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(obj.Data);

                    foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                        yield return f;
                    break;
                }
            }
        }
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetScenePointSymbolPoints(TrinityScenePointSV scenePoint, TrinitySceneObjectTemplateEntrySV[] subObjects)
    {
        // Handle SubObjects
        for (var i = 0; i < subObjects.Length; i++)
        {
            var sobj = subObjects[i];
            switch (sobj.Type)
            {
                case "trinity_PropertySheet":
                {
                    var propSheet = FlatBufferConverter.DeserializeFrom<TrinityPropertySheetSV>(sobj.Data);
                    if (propSheet.Name == "fixed_symbol_point")
                    {
                        var tableKey = GetTableKey(propSheet);
                        if (!string.IsNullOrEmpty(tableKey))
                        {
                            yield return new PaldeaFixedSymbolPoint(tableKey, scenePoint.Position);
                        }
                    }
                    break;
                }
                case "trinity_ScenePoint":
                {
                    var subScenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(sobj.Data);
                    foreach (var f in GetScenePointSymbolPoints(subScenePoint, sobj.SubObjects))
                        yield return f;
                    break;
                }
                default:
                    throw new ArgumentException($"Unknown SubObject {sobj.Type}");
            }
        }
    }

    public static string GetTableKey(TrinityPropertySheetSV propSheet)
    {
        if (propSheet.Name != "fixed_symbol_point")
            throw new ArgumentException($"Invalid PropertySheet {propSheet.Name}");

        if (propSheet.Properties[0].Fields[1].Name != "tableKey")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (!propSheet.Properties[0].Fields[1].Data.TryGet(out TrinityPropertySheetFieldStringValueSV? sv))
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }
}
