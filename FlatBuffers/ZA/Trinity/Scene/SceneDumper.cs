using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.ZA.Trinity;

public static class SceneDumper
{
    private const char PadChar = '\t';
    private static void Write(TextWriter tw, int depth, string str) => tw.WriteLine(new string(PadChar, depth) + str);

    public static string Bucket { get; set; } = "";
    public static string BucketError { get; set; } = "";
    public static bool ThrowOnUnknownType { get; set; } = false;

    public static TrinitySceneObjectTemplate Dump(string path) => Dump(path, Console.Out);

    public static TrinitySceneObjectTemplate Dump(string path, TextWriter tw)
    {
        var data = File.ReadAllBytes(path);
        var scene = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(data);
        Dump(scene, tw);
        return scene;
    }

    private static void Dump(TrinitySceneObjectTemplate scene, TextWriter tw)
    {
        const int depth = 0;
        Dump(scene, tw, depth);
        foreach (var obj in scene.Objects)
            Dump(obj, tw, depth + 1);
    }

    private static void Dump(TrinitySceneObjectTemplateEntry scene, TextWriter tw, int depth)
    {
        Dump(scene.Data, scene.Type, tw, depth);
        Dump(scene.SubObjects, tw, depth);
    }

    private static void Dump(IList<TrinitySceneObjectTemplateEntry> arr, TextWriter tw, int depth)
    {
        foreach (var obj in arr)
        {
            Dump(obj.Data, obj.Type, tw, depth + 1);
            Dump(obj.SubObjects, tw, depth + 1);
        }
    }

    private static void Dump(TrinitySceneObjectTemplate scene, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(scene.ObjectTemplateName)}: {scene.ObjectTemplateName} ({FnvHash.HashFnv1a_64(scene.ObjectTemplateName):X16})");
        Write(tw, depth, $"{nameof(scene.Field02)}: {scene.Field02}");
        Write(tw, depth, $"{nameof(scene.Field03)}: {scene.Field03}");
        Dump(scene.Field05, tw, depth, nameof(scene.Field05));
        Write(tw, depth, $"{nameof(scene.Objects)}: {scene.Objects.Count}");
    }

    private static void AddToBucket(Span<byte> data, string type, string bucket)
    {
        var hash = FnvHash.HashFnv1a_64(data);
        var dir = Path.Combine(bucket, type);
        var path = Path.Combine(dir, hash.ToString("X16"));
        Directory.CreateDirectory(dir);
        File.WriteAllBytes(path, data.ToArray());
    }

    private static void Dump(Memory<byte> data, string type, TextWriter tw, int depth)
    {
        const string UnknownType = "Unknown Type";
        Write(tw, depth++, type);
        try
        {
            switch (type)
            {
                case "SubScene": DumpSubScene(data, tw, depth); break;
                case "trinity_PropertySheet": DumpPropertySheet(data, tw, depth); break;
                case "trinity_SceneObject": DumpSceneObject(data, tw, depth); break;
                case "trinity_ScenePoint": DumpScenePoint(data, tw, depth); break;
                case "trinity_ObjectTemplate": DumpObjectTemplate(data, tw, depth); break;
                case "trinity_ScriptComponent": DumpScriptComponent(data, tw, depth); break;
                case "trinity_ObjectSwitcher": DumpObjectSwitcher(data, tw, depth); break;
                case "trinity_ModelComponent": DumpModelComponent(data, tw, depth); break;
                case "trinity_CollisionComponent": DumpCollisionComponent(data, tw, depth); break;
                case "trinity_ParticleComponent": DumpParticleComponent(data, tw, depth); break;
                case "trinity_GroundPlaceComponent": DumpGroundPlaceComponent(data, tw, depth); break;
                case "ti_AIPerceptualComponent": DumpTIAIPerceptualComponent(data, tw, depth); break;
                case "ti_ModelDitherFadeComponent": DumpTIModelDitherFadeComponent(data, tw, depth); break;
                case "ti_DynamicExclusionComponent": DumpTIDynamicExclusionComponent(data, tw, depth); break;
                case "trinity_CollisionEventTriggerComponent": DumpCollisionEventTriggerComponent(data, tw, depth); break;
                case "trinity_EnvironmentParameter": DumpTrinityEnvironmentParameter(data, tw, depth); break;
                case "trinity_OverrideSensorData": DumpTrinityOverrideSensorData(data, tw, depth); break;
                case "trinity_PlacementRegistry": DumpPlacementRegistry(data, tw, depth); break;
                default:
                    if (ThrowOnUnknownType)
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    Write(tw, depth, $"{UnknownType}: {type}");
                    break;
            }
        }
        catch (Exception ex) when (!ex.Message.StartsWith(UnknownType))
        {
            Write(tw, depth, $"Error Parsing ({ex.GetType().Name}): {ex.Message}");
            if (BucketError.Length != 0)
                AddToBucket(data.Span, type, BucketError);
        }
        if (Bucket.Length != 0)
            AddToBucket(data.Span, type, Bucket);
    }

    private static void DumpPlacementRegistry(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<PlacementRegistry>(data);
        var entry = props.Entry;
        Write(tw, depth, $"{entry.Kind}:");

        switch (entry.Kind)
        {
            case PlacementEntry.ItemKind.PlacementObjectArray: DumpPlacementObjectArray(entry.PlacementObjectArray, tw, depth); break;
            case PlacementEntry.ItemKind.PlacementObjectTemplateArray: DumpPlacementObjectTemplateArray(entry.PlacementObjectTemplateArray, tw, depth); break;
            case PlacementEntry.ItemKind.PlacementPositionArray: DumpPlacementPositionArray(entry.PlacementPositionArray, tw, depth); break;
            case PlacementEntry.ItemKind.PlacementSpawnerArray: DumpPlacementSpawnerArray(entry.PlacementSpawnerArray, tw, depth); break;
            default: throw new ArgumentException(nameof(entry));
        }
    }

    private static void DumpPlacementObjectArray(PlacementObjectArray value, TextWriter tw, int depth)
    {
        var array = value.Table;
        Write(tw, depth, $"{nameof(array.Count)}: {array.Count}");
        depth++;
        foreach (var entry in array)
        {
            Write(tw, depth, $"{nameof(entry.Name)}: {entry.Name} ({FnvHash.HashFnv1a_64(entry.Name):X16})");
            Write(tw, depth, $"{nameof(entry.Type)}: {entry.Type}");
            Write(tw, depth, $"{nameof(entry.File)}: {entry.File}");
        }
    }

    private static void DumpPlacementObjectTemplateArray(PlacementObjectTemplateArray value, TextWriter tw, int depth)
    {
        var array = value.Table;
        Write(tw, depth, $"{nameof(array.Count)}: {array.Count}");
        depth++;
        foreach (var entry in array)
        {
            Write(tw, depth, $"{nameof(entry.Name)}: {entry.Name} ({FnvHash.HashFnv1a_64(entry.Name):X16})");
            Write(tw, depth, $"{nameof(entry.Path)}: {entry.Path}");
        }
    }

    private static void DumpPlacementPositionArray(PlacementPositionArray value, TextWriter tw, int depth)
    {
        var array = value.Table;
        Write(tw, depth, $"{nameof(array.Count)}: {array.Count}");
        depth++;
        foreach (var entry in array)
        {
            Write(tw, depth, $"{nameof(entry.Name)}: {entry.Name} ({FnvHash.HashFnv1a_64(entry.Name):X16})");
            Write(tw, depth, $"{nameof(entry.Position)}: {entry.Position}");
            Write(tw, depth, $"{nameof(entry.Rotation)}: {entry.Rotation}");
            if (entry.Arguments is null)
                Write(tw, depth, "Arguments: null");
            else
                Write(tw, depth, $"Arguments: [{string.Join(",", entry.Arguments.Select(GetFormattedArg))}]");
        }
    }

    private static void DumpPlacementSpawnerArray(PlacementSpawnerArray value, TextWriter tw, int depth)
    {
        var array = value.Table;
        Write(tw, depth, $"{nameof(array.Count)}: {array.Count}");
        depth++;
        foreach (var entry in array)
        {
            Write(tw, depth, $"{nameof(entry.Name)}: {entry.Name} ({FnvHash.HashFnv1a_64(entry.Name):X16})");
            Write(tw, depth, $"{nameof(entry.Scene)}: {entry.Scene}");
            Write(tw, depth, $"Arguments: {entry.Arguments?.Count}");
            if (entry.Arguments is null)
                continue;
            for (var i = 0; i < entry.Arguments.Count; i++)
            {
                var arg = entry.Arguments[i];
                Write(tw, depth, $"Arg[{i}]:");
                DumpPlacementRule(tw, depth + 1, arg);
            }
        }
    }

    private static void DumpPlacementRule(TextWriter tw, int depth, PlacementLogic logic)
    {
        Write(tw, depth, $"{nameof(logic.Name)}: {logic.Name}");
        if (logic.Expression is null)
            return;
        Write(tw, depth, $"{nameof(logic.Expression)}:");
        Write(tw, depth + 1, logic.Expression);
    }

    private static void Write(TextWriter tw, int depth, LogicExpression exp)
    {
        if (exp.Root is { } root)
        {
            Write(tw, depth, $"{nameof(exp.Root)}:");
            Write(tw, depth + 1, root);
        }
    }

    private static void Write(TextWriter tw, int depth, ExpressionNode node)
    {
        Write(tw, depth, $"{node.Kind}:");
        switch (node.Kind)
        {
            case ExpressionNode.ItemKind.ExpressionLeaf:
                var leaf = node.ExpressionLeaf;
                Write(tw, depth, $"{nameof(leaf.ConditionName)}: {leaf.ConditionName}");
                Write(tw, depth, $"{nameof(leaf.Op)}: {leaf.Op}");
                if (leaf.Arguments is null)
                    Write(tw, depth, "Arguments: null");
                else
                    Write(tw, depth, $"Arguments: [{string.Join(",", leaf.Arguments.Select(GetFormattedArg))}]");

                break;
            case ExpressionNode.ItemKind.ExpressionBranch:
                var branch = node.ExpressionBranch;
                Write(tw, depth, $"{nameof(branch.Operand)}: {branch.Operand}");
                if (branch.Left is { } left)
                {
                    Write(tw, depth, $"{nameof(branch.Left)}:");
                    Write(tw, depth + 1, left);
                }
                if (branch.Right is { } right)
                {
                    Write(tw, depth, $"{nameof(branch.Right)}:");
                    Write(tw, depth + 1, right);
                }

                break;
            default:
                throw new ArgumentException(nameof(node));
        }
    }

    private static string GetFormattedArg(string? arg)
    {
        if (arg is null)
            return string.Empty;
        return $"\"{arg}\"";
    }

    private static void DumpTrinityOverrideSensorData(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityOverrideSensorData>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
        Write(tw, depth, $"{nameof(props.Field03)}: {props.Field03}");
    }

    private static void DumpTrinityEnvironmentParameter(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityEnvironmentParameter>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
    }

    private static void DumpCollisionEventTriggerComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityCollisionEventTriggerComponent>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
        Write(tw, depth, $"{nameof(props.Field03)}: {props.Field03}");
        Write(tw, depth, $"{nameof(props.Field04)}: {props.Field04}");
        Write(tw, depth, $"{nameof(props.Field05)}: {props.Field05}");
    }

    private static void DumpTIDynamicExclusionComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TIDynamicExclusionComponent>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
    }

    private static void DumpTIModelDitherFadeComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TIModelDitherFadeComponent>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
        Write(tw, depth, $"{nameof(props.Field03)}: {props.Field03}");
        Write(tw, depth, $"{nameof(props.Field04)}: {props.Field04}");
    }

    private static void DumpTIAIPerceptualComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TIAIPerceptualComponent>(data);
        Write(tw, depth, $"{nameof(props.Value)}: {props.Value}");
    }

    private static void DumpGroundPlaceComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityGroundPlaceComponent>(data);
        Write(tw, depth, $"{nameof(props.Index)}: {props.Index}");
    }

    private static void DumpParticleComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityParticleComponent>(data);
        Write(tw, depth, $"{nameof(props.ParticleFile)}: {props.ParticleFile}");
        Write(tw, depth, $"{nameof(props.ParticleName)}: {props.ParticleName}");
        Write(tw, depth, $"{nameof(props.ParticleParent)}: {props.ParticleParent}");
    }

    private static void DumpCollisionComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityComponent>(data);
        var shape = props.Component.CollisionComponent.Shape;
        Write(tw, depth, $"{nameof(props.Component.CollisionComponent.Shape)}: {shape.Kind}");
        Write(tw, depth, $"v3f: {GetV4F(props.Component.CollisionComponent.Field08)}");
        depth++;
        switch (shape.Kind)
        {
            case CollisionUnion.ItemKind.Sphere:  DumpCollision(shape.Sphere, tw, depth); break;
            case CollisionUnion.ItemKind.Box:     DumpCollision(shape.Box, tw, depth); break;
            case CollisionUnion.ItemKind.Capsule: DumpCollision(shape.Capsule, tw, depth); break;
            case CollisionUnion.ItemKind.Havok:   DumpCollision(shape.Havok, tw, depth); break;
            default: throw new ArgumentException(nameof(shape));
        }
    }

    private static void DumpModelComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityModelComponent>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
        Write(tw, depth, $"{nameof(props.Field03)}: {props.Field03}");
        Write(tw, depth, $"{nameof(props.Field05)}: {props.Field05}");
        Write(tw, depth, $"{nameof(props.Field06)}: {props.Field06}");
        Write(tw, depth, $"{nameof(props.Field07)}: {props.Field07}");
        Write(tw, depth, $"{nameof(props.Field19)}: {props.Field19}");
        Write(tw, depth, $"{nameof(props.Field20)}: {props.Field20}");
        Write(tw, depth, $"{nameof(props.Field21)}: {props.Field21}");
        Write(tw, depth, $"{nameof(props.Field22)}: {props.Field22}");
    }

    private static void DumpScriptComponent(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityScriptComponent>(data);
        Write(tw, depth, $"{nameof(props.ScriptFileName)}: {props.ScriptFileName}");
        Write(tw, depth, $"{nameof(props.ScriptFileNameHash)}: {props.ScriptFileNameHash}");
        Write(tw, depth, $"{nameof(props.ScriptFileClass)}: {props.ScriptFileClass}");
    }

    private static void DumpSubScene(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<SubScene>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
    }

    private static void DumpObjectSwitcher(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityObjectSwitcher>(data);
        Write(tw, depth, $"{nameof(props.Field00)}: {props.Field00}");
        Write(tw, depth, $"{nameof(props.Field01)}: {props.Field01}");
    }

    private static void DumpObjectTemplate(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(data);
        Write(tw, depth, $"{nameof(props.ObjectTemplateName)}: {props.ObjectTemplateName} ({FnvHash.HashFnv1a_64(props.ObjectTemplateName):X16})");
        Write(tw, depth, $"{nameof(props.ObjectTemplatePath)}: {props.ObjectTemplatePath}");
        Write(tw, depth, $"{nameof(props.ObjectTemplateExtra)}: {props.ObjectTemplateExtra}");
        Write(tw, depth, $"{nameof(props.Field03)}: {props.Field03}");
        Dump(props.Data, props.Type, tw, ++depth);
    }

    private static void DumpScenePoint(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(data);
        Write(tw, depth, $"{nameof(props.Name)}: {props.Name}");
        Write(tw, depth, $"{nameof(props.Position)}: {props.Position}");
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
    }

    private static void DumpSceneObject(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(data);
        Write(tw, depth, $"{nameof(props.Name)}: {props.Name} ({FnvHash.HashFnv1a_64(props.Name):X16})");
        Write(tw, depth, $"{nameof(props.Position)}:");
        Dump(props.Position, tw, depth + 1);
        Write(tw, depth, $"{nameof(props.Field02)}: {props.Field02}");
        Write(tw, depth, $"{nameof(props.ApplySRT)}: {props.ApplySRT}");
        Write(tw, depth, $"{nameof(props.Field04)}: {props.Field04}");
        Write(tw, depth, $"{nameof(props.Field05)}: {props.Field05}");
        Write(tw, depth, $"{nameof(props.Field06)}: {props.Field06}");
        Write(tw, depth, $"{nameof(props.Field07)}: {props.Field07}");
        if (props.Field08 is not null)
            Dump(props.Field08, tw, depth, nameof(props.Field08));
    }

    private static void DumpCollision(Sphere shape, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(shape.Transform)}: {GetV3F(shape.Transform)}");
        Write(tw, depth, $"{nameof(shape.Radius)}: {shape.Radius}");
    }

    private static void DumpCollision(Box shape, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(shape.Size)}: {GetV3F(shape.Size)}");
        Write(tw, depth, $"{nameof(shape.Rotation)}: {GetV3F(shape.Rotation)}");
        Write(tw, depth, $"{nameof(shape.Transform)}: {GetV3F(shape.Transform)}");
    }

    private static void DumpCollision(Capsule shape, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(shape.Height)}: {shape.Height}");
        Write(tw, depth, $"{nameof(shape.Radius)}: {shape.Radius}");
        Write(tw, depth, $"{nameof(shape.Rotation)}: {GetV3F(shape.Rotation)}");
        Write(tw, depth, $"{nameof(shape.Transform)}: {GetV3F(shape.Transform)}");
    }

    private static void DumpCollision(Havok shape, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(shape.TrcolFilePath)}: {shape.TrcolFilePath}");
        Write(tw, depth, $"{nameof(shape.Scale)}: {GetV3F(shape.Scale)}");
        Write(tw, depth, $"{nameof(shape.Rotation)}: {GetV3F(shape.Rotation)}");
        Write(tw, depth, $"{nameof(shape.Transform)}: {GetV3F(shape.Transform)}");
    }

    private static string GetV3F(Vec3f props) => $"({props.X}, {props.Y}, {props.Z})";
    private static string GetV3F(PackedVec3f props) => $"({props.X}, {props.Y}, {props.Z})";
    private static string GetV4F(Vec4f props) => $"({props.X}, {props.Y}, {props.Z} @ {props.W})";

    private static void Dump<T>(IList<T> arr, TextWriter tw, int depth, string name)
    {
        Write(tw, depth, $"{name}:");
        for (var i = 0; i < arr.Count; i++)
            Write(tw, depth + 1, $"[{i}]: {arr[i]}");
    }

    private static void Dump(SRT p, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(p.Scale)}: {p.Scale}");
        Write(tw, depth, $"{nameof(p.Rotation)}: {p.Rotation}");
        Write(tw, depth, $"{nameof(p.Translation)}: {p.Translation}");
    }

    private static void DumpPropertySheet(Memory<byte> data, TextWriter tw, int depth)
    {
        var props = FlatBufferConverter.DeserializeFrom<TrinityPropertySheet>(data);
        Write(tw, depth, $"{nameof(props.Name)}: {props.Name} ({FnvHash.HashFnv1a_64(props.Name):X16})");
        if (!string.IsNullOrWhiteSpace(props.Extra))
            Write(tw, depth, $"{nameof(props.Extra)}: {props.Extra}");
        foreach (var p in props.Properties)
        {
            foreach (var f in p.Fields)
                Dump(f, tw, depth + 1);
        }
    }

    private static void Dump(TrinityPropertySheetField f, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(f.Name)}: {f.Name}");
        //Write(tw, depth, $"{nameof(f.Data.Discriminator)}: {f.Data.Discriminator}");
        Dump(f.Data, tw, depth);
    }

    private static void Dump(TrinityPropertySheetValue v, TextWriter tw, int depth)
    {
        switch (v.Discriminator)
        {
            case 1: Dump(v.Item1, tw, depth); break;
            case 2: Dump(v.Item2, tw, depth); break;
            case 3: Dump(v.Item3, tw, depth); break;
            case 4: Dump(v.Item4, tw, depth); break;
            case 5: Dump(v.Item5, tw, depth); break;
            case 6: Dump(v.Item6, tw, depth); break;
            case 7: Dump(v.Item7, tw, depth); break;
            case 8: Dump(v.Item8, tw, depth); break;
            case 9: Dump(v.Item9, tw, depth); break;
            default: throw new ArgumentOutOfRangeException(nameof(v.Discriminator), v.Discriminator, null);
        }
    }

    private static void Dump(TrinityPropertySheetField1 item, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(item.Value)}: {item.Value}");
        //Write(tw, depth, $"{nameof(item.FieldType)}: {item.FieldType}");
    }

    private static void Dump(TrinityPropertySheetField2 item, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(item.Value)}: {item.Value}");
        //Write(tw, depth, $"{nameof(item.FieldType)}: {item.FieldType}");
    }

    private static void Dump(TrinityPropertySheetFieldStringValue item, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(item.Value)}: {item.Value}");
    }

    private static void Dump(TrinityPropertySheetField4 item, TextWriter tw, int depth) =>
        Write(tw, depth, $"UNDOCUMENTED {nameof(TrinityPropertySheetField4)}");

    private static void Dump(TrinityPropertySheetField5 item, TextWriter tw, int depth) =>
        Write(tw, depth, $"UNDOCUMENTED {nameof(TrinityPropertySheetField5)}");

    private static void Dump(TrinityPropertySheetField6 item, TextWriter tw, int depth) =>
        Write(tw, depth, $"UNDOCUMENTED {nameof(TrinityPropertySheetField6)}");

    private static void Dump(TrinityPropertySheetFieldEnumName item, TextWriter tw, int depth)
    {
        Write(tw, depth, $"{nameof(item.Enum)}:  {item.Enum}");
        Write(tw, depth, $"{nameof(item.Value)}: {item.Value}");
    }

    private static void Dump(TrinityPropertySheetObjectArray item, TextWriter tw, int depth)
    {
        foreach (var obj in item.Value)
            Dump(obj, tw, depth + 1);
    }

    private static void Dump(TrinityPropertySheetObject item, TextWriter tw, int depth)
    {
        foreach (var f in item.Fields)
            Dump(f, tw, depth + 1);
    }
}
