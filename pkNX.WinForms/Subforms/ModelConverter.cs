using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FlatSharp.Attributes;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.WinForms;

// Model loading Tasks
// y |  2 | Layout PLA model structure classes
// y |  3 | Load PLA models
// y |  2 | Layout SWSH model structure classes
// y |  2 | Load SWSH models
// p |  0 | Convert SWSH models to PLA
// n |  1 |  - Textures
// p |  3 |  - Materials
// n |  3 |  - Constant buffers (material params)
// n |  4 |  - Shaders
// p |  2 |  - Mesh information
// p |  2 |  - Mesh buffers / vertex layout
// p |  3 |  - Skeleton
// p |  3 |  - LOD structure
// p |  3 |  - Other properties
// n |  2 | Save PLA models
// x | 35 |

// TODO: Material conversion
// What do do with vertex color. Why are there two entries?


// TODO's per file type
// TRConfig -> fill in missing fields
// TRModel -> Auto generate LODs, Field_06
// TRMMT -> MaterialSwitches, MaterialProperties
// TRMesh -> Split eyes into submesh and assign eye shader, maybe sort entries?
// TRSubMesh -> Material name might need to be converted to snake_case
// TRMeshShape -> BoneWeight[] Possibly this is just the sum of all blend indices + weights on the shape
// TRMeshBuffer -> Update BLEND_INDICES, Possibly need to remove vertex color
// TRMaterial -> Properly tackle this, Material name might need to be converted to snake_case
// TRSkeleton -> Name of first bone should be updated, might need to snake_case all names

// TODO SWSH unused properties:
// GFBPokeConfig -> Version, SpeciesId, FormId, Origin, Height, HeightAdjust, FieldAdjust, AABB, 
// InframeHeight, RegionId, Motion
// MaterialEntries

// GFBModel -> Version?, TextureFiles, All shaders
// Mesh8 -> SortPriority (only used rarely)
// Skeleton8 -> Effect and IsVisible 


// Probably need some sort of intermediate class structure

// PLA animation structure classes
// Load PLA animations
// SWSH animation structure classes
// Load SWSH animations
// Convert SWSH animations to PLA
// Save PLA animations

// Remaining Tasks
// Particle Effects
// Other Effects
// Shader converter

public partial class ModelConverter : Form
{
    private GameManager ROM;
    private int SpeciesId;
    private string FileName;
    private string BasePath;

    private string ModelPath => BasePath + "mdl/";
    private string AnimationsPath => BasePath + "anm/";

    private FolderContainer PokemonModelDir;
    private FolderContainer SWSHPokemonModelDir;

    public ModelConverter(GameManager rom)
    {
        ROM = rom;
        InitializeComponent();

        PokemonModelDir = (FolderContainer)ROM[GameFile.PokemonArchiveFolder];
        PokemonModelDir.Initialize();
        CB_Species.Items.AddRange(PokemonModelDir.GetFileNames().Where(x => x != "pokeconfig.gfpak").ToArray());
        CB_Species.SelectedIndex = 165;

        SWSHPokemonModelDir = (FolderContainer)ROM[GameFile.Debug_SWSHPokemonArchiveFolder];
        SWSHPokemonModelDir.Initialize();
        CB_SWSHSpecies.Items.AddRange(SWSHPokemonModelDir.GetFileNames().ToArray());
        CB_SWSHSpecies.SelectedIndex = 1;
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class MeshMaterialWrapper
    {
        [FlatBufferItem(0)] public string Name { get; set; } = string.Empty;
        [FlatBufferItem(1)] public TRMaterial[] Materials { get; set; } = Array.Empty<TRMaterial>();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ModelWrapper
    {
        [FlatBufferItem(0)] public TRPokeConfig Config { get; set; } = new();
        [FlatBufferItem(1)] public TRModel TRModel { get; set; } = new();
        [FlatBufferItem(2)] public TRMultiMaterialTable TRMMT { get; set; } = new();
        [FlatBufferItem(3)] public TRMesh[] Meshes { get; set; } = Array.Empty<TRMesh>();
        [FlatBufferItem(4)] public TRMeshBuffer[] MeshDataBuffers { get; set; } = Array.Empty<TRMeshBuffer>();
        [FlatBufferItem(5)] public TRMaterial[] DefaultMaterials { get; set; } = Array.Empty<TRMaterial>();
        [FlatBufferItem(6)] public MeshMaterialWrapper[] MeshMaterials { get; set; } = Array.Empty<MeshMaterialWrapper>();
        [FlatBufferItem(7)] public TRSkeleton Skeleton { get; set; } = new();

        [FlatBufferItem(8)] public string[] UsedTextures { get; set; } = Array.Empty<string>();
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    private class SWSHModelWrapper
    {
        public GFBPokeConfig Config { get; set; } = new();
        public GFBModel GFBModel { get; set; } = new();
        public GFBModel GFBModelRare { get; set; } = new();

        public string[] UsedTextures { get; set; } = Array.Empty<string>();
    }

    private ModelWrapper PLAModel = new();
    private SWSHModelWrapper SWSHModel = new();
    private ModelWrapper Result = new();

    private void UpdatePLAModel()
    {
        // TODO: Paths are located in 'Pokemon Resource Table', should load through there

        string selectedFile = (string)CB_Species.SelectedItem;
        FileName = Path.GetFileNameWithoutExtension(selectedFile);
        SpeciesId = int.Parse(FileName.Substring(2, 4));

        // TODO: Skip these for now
        if (int.Parse(FileName.Substring(7, 2)) != 0)
            return;

        BasePath = $"bin/pokemon/pm{SpeciesId:0000}/{FileName}/";

        var pack = new GFPack(PokemonModelDir.GetFileData(selectedFile) ?? Array.Empty<byte>());

        PLAModel.Config = FlatBufferConverter.DeserializeFrom<TRPokeConfig>(pack.GetDataFullPath(BasePath + $"{FileName}.trpokecfg"));
        Debug.Assert((int)PLAModel.Config.SizeIndex <= 3, "Here's one!");
        Debug.Assert((int)PLAModel.Config.Reserved_09 == 0.0f, "Here's one!");
        LoadModel(pack);
    }

    private void UpdateSWSHModel()
    {
        string selectedFile = (string)CB_SWSHSpecies.SelectedItem;
        FileName = Path.GetFileNameWithoutExtension(selectedFile);
        SpeciesId = int.Parse(FileName.Substring(2, 4));

        BasePath = $"bin/pokemon/{FileName}/";

        var pack = new GFPack(SWSHPokemonModelDir.GetFileData($"{FileName}.gfpak") ?? Array.Empty<byte>());

        SWSHModel.Config = FlatBufferConverter.DeserializeFrom<GFBPokeConfig>(pack.GetDataFullPath(BasePath + $"{FileName}.gfbpokecfg"));

        LoadSWSHModel(pack);
    }

    private void LoadModel(GFPack pack)
    {
        PLAModel.TRMMT = FlatBufferConverter.DeserializeFrom<TRMultiMaterialTable>(pack.GetDataFullPath(ModelPath + $"{FileName}.trmmt"));
        PLAModel.TRModel = FlatBufferConverter.DeserializeFrom<TRModel>(pack.GetDataFullPath(ModelPath + $"{FileName}.trmdl"));
        PLAModel.Skeleton = FlatBufferConverter.DeserializeFrom<TRSkeleton>(pack.GetDataFullPath(ModelPath + $"{PLAModel.TRModel.Skeleton.Filename}"));

        Debug.Assert(PLAModel.TRMMT.Reserved_00 == 0, "Here's one!");
        Debug.Assert(PLAModel.TRMMT.Reserved_01 == 0, "Here's one!");

        Debug.Assert(PLAModel.TRModel.Reserved_00 == 0, "Here's one!");
        foreach (var lod in PLAModel.TRModel.LODs)
        {
            Debug.Assert(lod.Type == "Custom", "Here's one!");
        }

        Debug.Assert(PLAModel.Skeleton.Reserved_00 == 0, "Here's one!");
        foreach (var node in PLAModel.Skeleton.Nodes)
        {
            Debug.Assert(node.Type is NodeType.Transform or NodeType.Joint or NodeType.Locator, "Here's one!");
        }
        foreach (var boneParam in PLAModel.Skeleton.Bones)
        {
            Debug.Assert(boneParam.Field_01 == 1, "Here's one!");
        }

        LoadMaterials(pack);
        LoadMaterialTable(pack);
        LoadMeshes(pack);

        PG_Test.SelectedObject = PLAModel;
    }

    private void LoadSWSHModel(GFPack pack)
    {
        SWSHModel.GFBModel = FlatBufferConverter.DeserializeFrom<GFBModel>(pack.GetDataFullPath(ModelPath + $"{FileName}.gfbmdl"));
        SWSHModel.GFBModelRare = FlatBufferConverter.DeserializeFrom<GFBModel>(pack.GetDataFullPath(ModelPath + $"{FileName}_rare.gfbmdl"));
        PG_Test_SWSH.SelectedObject = SWSHModel;
    }

    private void LoadMaterials(GFPack pack)
    {
        PLAModel.DefaultMaterials = PLAModel.TRModel.Materials
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMaterial>(pack.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();

        PLAModel.UsedTextures = PLAModel.DefaultMaterials.SelectMany(material =>
                material.MaterialPasses.SelectMany(pass =>
                    pass.TextureParameters.Select(texture => texture.TextureFile)
                )
            ).ToHashSet().ToArray();

        for (var i = 0; i < PLAModel.DefaultMaterials.Length; i++)
        {
            var material = PLAModel.DefaultMaterials[i];

            for (var j = 0; j < material.MaterialPasses.Length; j++)
            {
                var pass = material.MaterialPasses[j];
                var values = pass.Shaders[0].ShaderValues;

                var layerCount = int.Parse(values.First(x => x.PropertyBinding.Equals("NumMaterialLayer")).StringValue);
                Debug.Assert(layerCount == 5, "Here's one!");

                var vertexBaseColor = bool.Parse(values.FirstOrDefault(x => x.PropertyBinding.Equals("EnableVertexBaseColor"))?.StringValue ?? "False");
                Debug.Assert(!vertexBaseColor, "Here's one!");
            }
        }
    }

    private void LoadMaterialTable(GFPack pack)
    {
        PLAModel.MeshMaterials = PLAModel.TRMMT.Material.Select(
                x => new MeshMaterialWrapper
                {
                    Name = x.Name,
                    Materials = x.FileNames.Select(
                        fileName => FlatBufferConverter.DeserializeFrom<TRMaterial>(pack.GetDataFullPath(ModelPath + $"{fileName}"))
                    ).ToArray()
                }
            ).ToArray();
    }

    private void LoadMeshes(GFPack pack)
    {
        PLAModel.Meshes = PLAModel.TRModel.Meshes
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMesh>(pack.GetDataFullPath(ModelPath + $"{x.Filename}")))
            .ToArray();

        foreach (var mesh in PLAModel.Meshes)
        {
            Debug.Assert(mesh.Reserved_00 == 0, "Here's one!");

            foreach (var shape in mesh.Shapes)
            {
                Debug.Assert(shape.IndexLayoutFormat == IndexLayoutFormat.UINT16, "Here's one!");
                Debug.Assert(shape.Field_05 == 0, "Here's one!");
                Debug.Assert(shape.Field_06 == 0, "Here's one!");
                Debug.Assert(shape.Field_07 == 0, "Here's one!");
                Debug.Assert(shape.Field_08 == 0, "Here's one!");
                Debug.Assert(string.IsNullOrEmpty(shape.Field_11), "Here's one!");

                foreach (var attribute in shape.VertexLayout)
                {
                    foreach (var attr in attribute.Elements)
                    {
                        Debug.Assert(attr.Slot == 0, "Here's one!");
                        Debug.Assert(attr.SemanticName <= InputLayoutSemanticName.BLEND_WEIGHTS, "Here's one!");
                        Debug.Assert(attr.Format is InputLayoutFormat.NONE or
                            InputLayoutFormat.RGBA_8_UNORM or
                            InputLayoutFormat.RGBA_8_UNSIGNED or
                            InputLayoutFormat.RGBA_16_UNORM or
                            InputLayoutFormat.RGBA_16_FLOAT or
                            InputLayoutFormat.RG_32_FLOAT or
                            InputLayoutFormat.RGB_32_FLOAT or
                            InputLayoutFormat.RGBA_32_FLOAT, "Here's one!");
                    }
                }

                foreach (var subMesh in shape.SubMeshes)
                {
                    Debug.Assert(subMesh.Field_02 == 0, "Here's one!");
                    Debug.Assert(subMesh.Field_04 is 0, "Here's one!");
                }
            }
        }

        LoadMeshBuffers(PLAModel.Meshes, pack);
    }

    private void LoadMeshBuffers(TRMesh[] trMeshes, GFPack pack)
    {
        PLAModel.MeshDataBuffers = trMeshes.Select(x => x.BufferFileName)
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMeshBuffer>(pack.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();

        for (var i = 0; i < PLAModel.MeshDataBuffers.Length; i++)
        {
            var mesh = PLAModel.Meshes[i];
            var meshBuffer = PLAModel.MeshDataBuffers[i];

            Debug.Assert(meshBuffer.Field_00 == 0, "Here's one!");

            for (var j = 0; j < meshBuffer.Buffers.Length; j++)
            {
                var buffer = meshBuffer.Buffers[j];
                var shape = mesh.Shapes[j];
                buffer.VertexBuffer[0].Debug_InputLayout = shape.VertexLayout[0];
            }
        }
    }

    private void B_Convert_Click(object sender, EventArgs e)
    {
        ConvertToTRConfig();
        ConvertToTRModel();
        PG_Converted.SelectedObject = Result;
    }

    private void ConvertToTRConfig()
    {
        // TODO:
        // SWSHModel.Config.MajorVer;
        // SWSHModel.Config.MinorVer;
        // SWSHModel.Config.SpeciesId;
        // SWSHModel.Config.FormId;
        // SWSHModel.Config.Name;
        // SWSHModel.Config.JpName;
        // SWSHModel.Config.SpeciesOrigin;
        // SWSHModel.Config.Height;
        // SWSHModel.Config.AdjustHeight;
        // SWSHModel.Config.FieldAdjust;
        // SWSHModel.Config.MinBX;
        // SWSHModel.Config.MinBY;
        // SWSHModel.Config.MinBZ;
        // SWSHModel.Config.MaxBX;
        // SWSHModel.Config.MaxBY;
        // SWSHModel.Config.MaxBZ;
        // SWSHModel.Config.InframeHeight;
        // SWSHModel.Config.RegionId;
        // SWSHModel.Config.WaitMotionBRate;
        // SWSHModel.Config.WaitMotionCRate;
        // SWSHModel.Config.Undef26;
        // SWSHModel.Config.Undef27;
        // SWSHModel.Config.MaterialEntries;
        // SWSHModel.Config.SpeciesModelProperty;

        Result.Config.Field_01 = 1.32f; // TODO
        Result.Config.Field_02 = 1.98f; // TODO
        Result.Config.Field_03 = 5.45f; // TODO
        Result.Config.Field_10_YOffset = 0f; // TODO
        Result.Config.Field_11_YOffset = -0.07f; // TODO
        Result.Config.Field_12_YOffset = 0f; // TODO

        Result.Config.SizeIndex = SWSHModel.Config.SizeIndex;
        Result.Config.InframeVerticalRotYOrigin = SWSHModel.Config.InframeVerticalRotYOrigin / 100;
        Result.Config.InframeBottomYOffset = SWSHModel.Config.InframeBottomYOffset / 100;
        Result.Config.InframeCenterYOffset = SWSHModel.Config.InframeCenterYOffset / 100;
        Result.Config.InframeLeftRotation = SWSHModel.Config.InframeLeftRotation;
        Result.Config.InframeRightRotation = SWSHModel.Config.InframeRightRotation;
    }

    private void ConvertToTRSkeleton()
    {
        Bone8[] skeleton = SWSHModel.GFBModel.Skeleton;

        // TODO:
        // Result.Skeleton.SizeType;
        // Result.Skeleton.Bones[];
        // Bones bone matrix is maya’s transform matrix inverted
        // Result.Skeleton.Iks[];

        // TODO: Probably need to ignore bones @ Mesh8.BoneId;
        // TODO: New structure ends with lod groups
        // TODO: Separate locators
        // TODO: Figure out and apply the new bone type structure

        var transformNodes = new List<TransformNode>();
        int rigStart = -1;
        int rigIndex = 0;
        for (int i = 0; i < skeleton.Length; ++i)
        {
            var bone8 = skeleton[i];

            if (bone8.Type == BoneType.Transparency_Group)
                continue; // TODO

            // TODO:
            // bone8.Effect;
            // bone8.IsRigged;

            // TODO: Should be converted using bone8.IsRigged. Only IsRigged bones are used in vertex blend_indices.
            // Meaning an array of IsRigged bones is made and blend_indices index into this array.
            // This might actually one to one convert into rigIndex

            // TODO: Most entries of type Joint in swsh, but are of type Transform in PLA

            transformNodes.Add(new()
            {
                Name = bone8.Name,
                Transform = new Transform
                {
                    Scale = bone8.Scale,
                    Rotate = bone8.Rotation,
                    Translate = bone8.Translation / 100 // Scale down swsh models by 100
                },
                ScalePivot = bone8.ScalePivot,
                RotatePivot = bone8.RotatePivot,
                ParentIdx = bone8.ParentIdx, // TODO: If some are removed, this id needs to be corrected
                RigIdx = bone8.IsVisible ? rigIndex++ : -1,
                LocatorBone = string.Empty,
                Type = (NodeType)bone8.Type, // TODO: A lot of these seem to be converted into transform nodes instead of joint nodes
            });

            if (bone8.IsVisible && rigStart == -1)
            {
                rigStart = i;
            }
        }
        int rigEnd = rigIndex;

        Debug.Assert(rigStart > 0, "This skeleton seems to not be skinned.");

        Result.Skeleton.RigOffset = rigStart - 2; // By default we always skip the first two 2 nodes. Any additional offset should be marked. TODO: Is this true?
        Result.Skeleton.Nodes = transformNodes.ToArray();

        // TODO: Result.Skeleton.Bones = new Bone[rigEnd];
    }

    private static InputLayoutFormat ConvertInputLayoutFormat(VertexAttribute8 attribute)
    {
        var result = (attribute.Format, attribute.Count) switch
        {
            (DataType8.UByte, 4) => InputLayoutFormat.RGBA_8_UNSIGNED,
            (DataType8.HalfFloat, 4) => InputLayoutFormat.RGBA_16_FLOAT,
            (DataType8.UShort, 4) => InputLayoutFormat.RGBA_16_UNORM, // ???
            (DataType8.Float, 4) => InputLayoutFormat.RGBA_32_FLOAT,
            (DataType8.FixedPoint, 4) => InputLayoutFormat.RGBA_8_UNORM,
            (DataType8.Float, 2) => InputLayoutFormat.RG_32_FLOAT,
            (DataType8.Float, 3) => InputLayoutFormat.RGB_32_FLOAT,
            _ => InputLayoutFormat.NONE
        };

        Debug.Assert(result != InputLayoutFormat.NONE, "Error: Conversion resulted in VertexLayoutType.NONE!");
        return result;
    }
    private static (InputLayoutSemanticName Semantic, uint Index) ConvertInputLayoutSemantic(VertexAttribute8 attribute)
    {
        var result = attribute.Type switch
        {
            Attribute8.Position => (InputLayoutSemanticName.POSITION, 0u),
            Attribute8.Normal => (InputLayoutSemanticName.NORMAL, 0u),
            Attribute8.Tangent => (InputLayoutSemanticName.TANGENT, 0u),
            Attribute8.Texcoord_0 => (InputLayoutSemanticName.TEXCOORD, 0u),
            Attribute8.Texcoord_1 => (InputLayoutSemanticName.TEXCOORD, 1u),
            Attribute8.Texcoord_2 => (InputLayoutSemanticName.TEXCOORD, 2u),
            Attribute8.Texcoord_3 => (InputLayoutSemanticName.TEXCOORD, 3u),

            Attribute8.Color_0 => (InputLayoutSemanticName.COLOR, 0u),
            Attribute8.Color_1 => (InputLayoutSemanticName.COLOR, 1u),
            Attribute8.Color_2 => (InputLayoutSemanticName.COLOR, 2u),
            Attribute8.Color_3 => (InputLayoutSemanticName.COLOR, 3u),

            Attribute8.Group_Idx => (BLEND_INDEX: InputLayoutSemanticName.BLEND_INDICES, 0u),
            Attribute8.Group_Weight => (InputLayoutSemanticName.BLEND_WEIGHTS, 0u),

            _ => (InputLayoutSemanticName.NONE, 0u)
        };

        Debug.Assert(result.Item1 != InputLayoutSemanticName.NONE, "Error: Conversion resulted in InputLayoutSemanticName.NONE!");
        return result;
    }

    private static uint SizeOfInputLayoutFormat(InputLayoutFormat format)
    {
        var result = format switch
        {
            InputLayoutFormat.RGBA_8_UNSIGNED => 4u,
            InputLayoutFormat.RGBA_16_FLOAT => 8u,
            InputLayoutFormat.RGBA_16_UNORM => 8u,
            InputLayoutFormat.RGBA_32_FLOAT => 16u,
            InputLayoutFormat.RGBA_8_UNORM => 4u,
            InputLayoutFormat.RG_32_FLOAT => 8u,
            InputLayoutFormat.RGB_32_FLOAT => 12u,

            _ => 0u
        };

        Debug.Assert(result != 0u, $"Error: Size of {format} resulted in '0'!");
        return result;
    }

    private void ConvertToTRMesh(string sourceFileName, string resultFileName)
    {
        Mesh8[] shapes = SWSHModel.GFBModel.Mesh;
        Shape[] buffers = SWSHModel.GFBModel.Shapes;
        Bone8[] bones = SWSHModel.GFBModel.Skeleton;
        Material8[] materials = SWSHModel.GFBModel.Materials;

        var meshShapes = new List<MeshShape>();
        var meshBuffers = new List<MeshBuffer>();
        foreach (var shape in shapes)
        {
            Shape buffer = buffers[shape.ShapeId];
            Bone8 bone = bones[shape.BoneId];
            string subMeshName = bone.Name;

            subMeshName = subMeshName.Replace(sourceFileName + "_", "", StringComparison.InvariantCultureIgnoreCase);
            subMeshName = subMeshName.Replace("skin", "", StringComparison.InvariantCultureIgnoreCase);
            subMeshName = subMeshName.ToLowerInvariant();

            // TODO:
            // shape.SortPriority;

            var inputLayout = new InputLayoutElement[buffer.Attributes.Length];
            uint inputLayoutSize;
            {
                uint layoutOffset = 0;
                for (var i = 0; i < buffer.Attributes.Length; i++)
                {
                    var attribute = buffer.Attributes[i];
                    var type = ConvertInputLayoutFormat(attribute);
                    var slot = ConvertInputLayoutSemantic(attribute);

                    inputLayout[i] = new InputLayoutElement
                    {
                        Format = type,
                        SemanticName = slot.Semantic,
                        SemanticIndex = slot.Index,
                        Offset = layoutOffset,
                    };

                    layoutOffset += SizeOfInputLayoutFormat(type);
                }

                inputLayoutSize = layoutOffset;
            }

            var subMeshes = new SubMesh[buffer.Polygons.Length];
            uint indexCount;
            {
                uint offset = 0;
                for (var i = 0; i < buffer.Polygons.Length; i++)
                {
                    var subMesh = buffer.Polygons[i];
                    var mat = materials[subMesh.MaterialId];
                    subMeshes[i] = new SubMesh
                    {
                        IndexCount = (uint)subMesh.Indices.Length,
                        IndexOffset = offset,
                        AppliedMaterial = mat.Name, // TODO
                    };

                    offset += (uint)subMesh.Indices.Length;
                }

                indexCount = offset;
            }

            var indexBuffer = new byte[indexCount * 2];
            {
                int offset = 0;
                Span<byte> dst = indexBuffer.AsSpan();
                foreach (var subMesh in buffer.Polygons)
                {
                    foreach (var index in subMesh.Indices)
                    {
                        WriteUInt16LittleEndian(dst[offset..], index);
                        offset += 2;
                    }
                }
            }


            var uniqueColors0 = new List<PackedColor4f>();
            var uniqueColors1 = new List<PackedColor4f>();

            VertexWrapper[][] oldVertices = ((ReadOnlySpan<byte>)buffer.Vertices).GetArray(data =>
                {
                    var vertexData = new VertexWrapper[inputLayout.Length];

                    for (var j = 0; j < inputLayout.Length; j++)
                    {
                        var layout = inputLayout[j];
                        var offset = (int)layout.Offset;

                        vertexData[j] = new VertexWrapper(layout, layout.Format switch
                        {
                            InputLayoutFormat.RGBA_8_UNORM => new Vec4f(new Unorm8(data[offset]), new Unorm8(data[offset + 1]), new Unorm8(data[offset + 2]), new Unorm8(data[offset + 3])),
                            InputLayoutFormat.RGBA_8_UNSIGNED => new Vec4i(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]),
                            InputLayoutFormat.RGBA_16_UNORM => new Vec4f(new Unorm16(ReadUInt16LittleEndian(data[offset..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 2)..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 4)..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 6)..]))),
                            InputLayoutFormat.RGBA_16_FLOAT => new Vec4f((float)ReadHalfLittleEndian(data[offset..]), (float)ReadHalfLittleEndian(data[(offset + 2)..]), (float)ReadHalfLittleEndian(data[(offset + 4)..]), (float)ReadHalfLittleEndian(data[(offset + 6)..])),
                            InputLayoutFormat.RG_32_FLOAT => new Vec2f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..])),
                            InputLayoutFormat.RGB_32_FLOAT => new Vec3f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..]), ReadSingleLittleEndian(data[(offset + 8)..])),
                            InputLayoutFormat.RGBA_32_FLOAT => new Vec4f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..]), ReadSingleLittleEndian(data[(offset + 8)..]), ReadSingleLittleEndian(data[(offset + 12)..])),
                            InputLayoutFormat.NONE => throw new IndexOutOfRangeException(),
                            _ => throw new IndexOutOfRangeException()
                        });

                        switch (layout.SemanticName)
                        {
                            case InputLayoutSemanticName.POSITION:
                                vertexData[j].Data = (Vec3f)vertexData[j].Data / 100; // Scale down swsh models by 100
                                break;
                            case InputLayoutSemanticName.COLOR when layout.SemanticIndex == 0:
                            {
                                // TODO: Convert to layer mask format
                                var color = PackedColor4f.FromByteColor((Vec4i)vertexData[j].Data);
                                if (!uniqueColors0.Contains(color))
                                    uniqueColors0.Add(color);
                                // TODO: Only a single color on this layer??
                                // {{ R: 0.69803923, G: 0.49803925, B: 1, A: 1 }}
                                break;
                            }
                            case InputLayoutSemanticName.COLOR when layout.SemanticIndex == 1:
                            {
                                // TODO: Convert to layer mask format
                                var color = PackedColor4f.FromByteColor((Vec4i)vertexData[j].Data);
                                if (!uniqueColors1.Contains(color))
                                    uniqueColors1.Add(color);
                                // TODO: This layer contained 3 different colors
                                // {{ R: 0.5019608, G: 0, B: 0.5411765, A: 1 }}
                                // {{ R: 0.49411768, G: 0, B: 0.5411765, A: 1 }}
                                // {{ R: 0.49803925, G: 0, B: 0.5411765, A: 1 }}
                                break;
                            }
                            case InputLayoutSemanticName.COLOR:
                                Debug.Assert(false, "Unhandled color semantic");
                                break;
                            case InputLayoutSemanticName.BLEND_INDICES:
                                // TODO: Might need to update these indices
                                break;
                        }
                    }

                    return vertexData;
                }
                , (int)inputLayoutSize);

            var vertexBuffer = new byte[oldVertices.Length * inputLayoutSize];
            {
                int offset = 0;
                Span<byte> dst = vertexBuffer.AsSpan();

                foreach (VertexWrapper[] vertices in oldVertices)
                {
                    foreach (VertexWrapper vertex in vertices)
                    {
                        var layout = vertex.LayoutElement;

                        switch (layout.Format)
                        {
                            case InputLayoutFormat.RGBA_8_UNORM:
                            {
                                var data = (Vec4f)vertex.Data;
                                dst[offset + 0] = Unorm8.FromFloat(data.X);
                                dst[offset + 1] = Unorm8.FromFloat(data.Y);
                                dst[offset + 2] = Unorm8.FromFloat(data.Z);
                                dst[offset + 3] = Unorm8.FromFloat(data.W);
                            }
                            break;
                            case InputLayoutFormat.RGBA_8_UNSIGNED:
                            {
                                var data = (Vec4i)vertex.Data;
                                dst[offset + 0] = (byte)data.X;
                                dst[offset + 1] = (byte)data.Y;
                                dst[offset + 2] = (byte)data.Z;
                                dst[offset + 3] = (byte)data.W;
                            }
                            break;
                            case InputLayoutFormat.RGBA_16_UNORM:
                            {
                                var data = (Vec4f)vertex.Data;
                                WriteUInt16LittleEndian(dst[(offset + 0)..], Unorm16.FromFloat(data.X));
                                WriteUInt16LittleEndian(dst[(offset + 2)..], Unorm16.FromFloat(data.Y));
                                WriteUInt16LittleEndian(dst[(offset + 4)..], Unorm16.FromFloat(data.Z));
                                WriteUInt16LittleEndian(dst[(offset + 6)..], Unorm16.FromFloat(data.W));
                            }
                            break;
                            case InputLayoutFormat.RGBA_16_FLOAT:
                            {
                                var data = (Vec4f)vertex.Data;
                                WriteHalfLittleEndian(dst[(offset + 0)..], (Half)data.X);
                                WriteHalfLittleEndian(dst[(offset + 2)..], (Half)data.Y);
                                WriteHalfLittleEndian(dst[(offset + 4)..], (Half)data.Z);
                                WriteHalfLittleEndian(dst[(offset + 6)..], (Half)data.W);
                            }
                            break;
                            case InputLayoutFormat.RG_32_FLOAT:
                            {
                                var data = (Vec2f)vertex.Data;
                                WriteSingleLittleEndian(dst[(offset + 0)..], data.X);
                                WriteSingleLittleEndian(dst[(offset + 4)..], data.Y);
                            }
                            break;
                            case InputLayoutFormat.RGB_32_FLOAT:
                            {
                                var data = (Vec3f)vertex.Data;
                                WriteSingleLittleEndian(dst[(offset + 0)..], data.X);
                                WriteSingleLittleEndian(dst[(offset + 4)..], data.Y);
                                WriteSingleLittleEndian(dst[(offset + 8)..], data.Z);
                            }
                            break;
                            case InputLayoutFormat.RGBA_32_FLOAT:
                            {
                                var data = (Vec4f)vertex.Data;
                                WriteSingleLittleEndian(dst[(offset + 0)..], data.X);
                                WriteSingleLittleEndian(dst[(offset + 4)..], data.Y);
                                WriteSingleLittleEndian(dst[(offset + 8)..], data.Z);
                                WriteSingleLittleEndian(dst[(offset + 12)..], data.W);
                            }
                            break;
                            case InputLayoutFormat.NONE:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        offset += (int)SizeOfInputLayoutFormat(layout.Format);
                    }
                }
            }

            {
                var bounds = shape.Bounds / 100; // Scale down swsh models by 100
                meshShapes.Add(new MeshShape
                {
                    MeshShapeName = $"{resultFileName}_{subMeshName}_mesh_shape",
                    MeshName = $"{resultFileName}_{subMeshName}_mesh",
                    VertexLayout = new VertexAttributeLayout[]
                    {
                        new()
                        {
                            Elements = inputLayout,
                            Size = new VertexSize[] { new() { Size = inputLayoutSize } },
                        }
                    },
                    BoundingSphere = new PackedSphere(bounds),
                    Bounds = bounds,
                    IndexLayoutFormat = IndexLayoutFormat.UINT16, // Always Uint16 on all PLA pokemon models
                    BoneWeights = new BoneWeight[] { }, // TODO: All bones affected by the sub-mesh + some weight
                    SubMeshes = subMeshes,
                });
            }

            {
                meshBuffers.Add(new MeshBuffer
                {
                    IndexBuffer = new ByteBuffer[] { new() { Data = indexBuffer } },
                    VertexBuffer = new ByteBuffer[] { new() { Data = vertexBuffer, Debug_InputLayout = new VertexAttributeLayout
                    {
                        Elements = inputLayout,
                        Size = new VertexSize[] { new() { Size = inputLayoutSize } },
                    } } },
                });
            }
        }


        Result.Meshes = new TRMesh[]
        {
            new()
            {
                BufferFileName = $"{resultFileName}.trmbf",
                Shapes = meshShapes.ToArray(),
            }
        };

        Result.MeshDataBuffers = new TRMeshBuffer[]
        {
            new()
            {
                Buffers = meshBuffers.ToArray()
            }
        };
    }

    private void ConvertToTRMeshShape()
    {

    }

    private StringParameter[] ConvertStringParams(Flag[] flags)
    {
        // StringParam possible values:
        // EnableBaseColorMap  < Maybe 'required'?

        // "EnableBaseColorMap"          -> Allowed values: "True", "False"                                  | shader_bool: 1
        // "EnableNormalMap"             -> Allowed values: "True", "False"
        // "EnableMetallicMap"           -> Allowed values: "True", "False"
        // "EnableRoughnessMap"          -> Allowed values: "True", "False"
        // "EnableEmissionColorMap"      -> Allowed values: "True", "False"
        // "EnableAOMap"                 -> Allowed values: "True", "False"
        // "EnableAlphaTest"             -> Allowed values: "True", "False"                                  (oldMaterial.TextureAlphaTest == 0).ToString()
        // "NumMaterialLayer"            -> Allowed values: "1", "2", "3", "4", "5"
        // "BillboardType"               -> Allowed values: "Disable", "AxisXYZ", "AxisY"
        // "WindReceiverType"            -> Allowed values: "Disable", "Simple", "Standard", "SimpleLeaf"
        // "EnableWindMaskMap"           -> Allowed values: "True", "False"
        // "EnableBaseColorMap1"         -> Allowed values: "True", "False"
        // "EnableNormalMap1"            -> Allowed values: "True", "False"
        // "EnableNormalMap2"            -> Allowed values: "True", "False"
        // "EnableAOMap1"                -> Allowed values: "True", "False"
        // "EnableAOMap2"                -> Allowed values: "True", "False"
        // "LayerMaskSource"             -> Allowed values: "Const", "Texture", "VertexColor"                | shader_bool: 1
        // "LayerMaskSwizzle"            -> Allowed values: "RGBA", "R111", "A111"
        // "LayerBaseMaskSource"         -> Allowed values: "One", "OneMinusLayerMaskSum"                    | shader_bool: 1
        // "EnableVertexBaseColor"       -> Allowed values: "True", "False"
        // "WeatherLayerMaskSource"      -> Allowed values: "Const", "Texture"
        // "EnableDepthFade"             -> Allowed values: "True", "False"
        // "EnablePackedMap"             -> Allowed values: "True", "False"
        // "EnableUVScaleOffsetNormal"   -> Allowed values: "True", "False"

        // Global params:                
        // "EnableDeferredRendering"     -> Allowed values: "True", "False"
        // "InstancingType"              -> Allowed values: "Disable", "World", "Detail"
        // "EnableGrassCollisionMap"     -> Allowed values: "True", "False"
        // "NumRequiredUV"               -> Allowed values: "1", "2"
        // "EnableWeatherLayer"          -> Allowed values: "True", "False"
        // "EnableDisplacementMap"       -> Allowed values: "True", "False"
        // "EnableLerpBaseColorEmission" -> Allowed values: "True", "False"


        // EnableUVScaleOffsetNormal -> enables float4 { UVScaleOffsetNormal: { R: 1, G: 1, B: 0, A: 0 } }

        static string ConvertParamName(string oldName)
        {
            var result = oldName switch
            {
                "useColorTex" => "EnableBaseColorMap",
                "SwitchEmissionMaskTexUV" => "",
                "EmissionMaskUse" => "",
                "SwitchPriority" => "",
                "Layer1Enable" => "",
                "SwitchAmbientTexUV" => "",
                "AmbientMapEnable" => "EnableAOMap", // TODO: New format only has R channel
                "SwitchNormalMapUV" => "",
                "NormalMapEnable" => "EnableNormalMap",
                "LightTableEnable" => "",
                "SpecularMaskEnable" => "",
                "BaseColorAddEnable" => "",
                "SphereMapEnable" => "",
                "SphereMaskEnable" => "",
                "RimMaskEnable" => "",
                "alphaShell" => "",
                "EffectVal" => "",
                "NormalEdgeEnable" => "",
                "OutLineIDEnable" => "",
                "OutLineColFixed" => "",

                // Global flags
                "FogEnable" => "",
                "DiscardEnable" => "", // FloatParam: DiscardValue? 
                "CastShadow" => "",
                "ReceiveShadow" => "",
                "TextureAlphaTestEnable" => "",
                "ShadowMapPrevEnable" => "",
                "LayerCalcMulti" => "",
                "FireMaskPathEnable" => "",
                "GPUInstancingEnable" => "",
                "Wireframe" => "",
                "DepthWrite" => "",
                "DepthTest" => "",
                "IsErase" => "",
                "MayaPreviewEnable" => "",
                _ => string.Empty
            };

            //Debug.Assert(!string.IsNullOrEmpty(result), $"Error: Couldn't convert flag with name: {oldName}!");
            return result;
        }

        var floats = new StringParameter[flags.Length];

        for (var i = 0; i < flags.Length; i++)
        {
            var flag = flags[i];

            floats[i] = new StringParameter
            {
                PropertyBinding = ConvertParamName(flag.FlagName),
                StringValue = flag.FlagEnable.ToString(),
            };
        }

        /*new StringParameter[]
        {
            new("EnableAlphaTest", (oldMaterial.TextureAlphaTest == 0).ToString()), // TODO
            new("NumMaterialLayer", "5"), // TODO: Adds the ParamName + LayerX parameters
            new("EnableLerpBaseColorEmission", "?"), // TODO
            new("EnableVertexBaseColor", "?"), // TODO
        }*/

        return floats;
    }

    private FloatParameter[] ConvertFloatParams(FloatParam[] floatParams)
    {
        static string ConvertParamName(string oldName)
        {
            string? result = oldName switch
            {
                "0" => "DiscardValue",
                "1" => "Metallic",
                "2" => "Roughness",
                "3" => "NormalHeight",
                "4" => "EmissionIntensity",
                "5" => "XLayer#",

                "ColorUVScaleU" => "",
                "ColorUVScaleV" => "",
                "ColorUVTranslateU" => "",
                "ColorBaseU" => "",
                "ColorUVTranslateV" => "",
                "ColorBaseV" => "",
                "ConstantColor0Val" => "",
                "Layer1UVScaleU" => "",
                "Layer1UVScaleV" => "",
                "Layer1UVTranslateU" => "",
                "Layer1BaseU" => "",
                "Layer1UVTranslateV" => "",
                "Layer1BaseV" => "",
                "EmissionMaskVal" => "",
                "ConstantColorSd0Val" => "",
                "ConstantColor1Val" => "",
                "ConstantColorSd1Val" => "",
                "ColorLerpValue" => "",
                "L1ConstantColor0Val" => "",
                "L1AddColor0Val" => "",
                "L1ConstantColor1Val" => "",
                "L1AddColor1Val" => "",
                "L1ConstantColorSd0Val" => "",
                "L1ConstantColorSd1Val" => "",
                "Layer1OverLerpValue" => "",
                "NormalMapUVScaleU" => "",
                "NormalMapUVScaleV" => "",
                "LightTblIndex" => "",
                "LightMul" => "",
                "SpecularPower" => "",
                "SpecularScale" => "",
                "SphereMapColorVal" => "",
                "RimColorVal" => "",
                "RimPower" => "",
                "RimStrength" => "",
                "OnGameEmissionVal" => "",
                "ConstantColorVal" => "",
                "ConstantAlpha" => "",
                "OnGameColorVal" => "",
                "OnGameAlpha" => "",
                "OutLineID" => "",
                "ProgID" => "",
                "Def0_OneMin1_FreCol" => "",
                "DistortionIntensity" => "",
                "Sin01" => "",
                "ScaleUV" => "",
                "EffectTexTranslateU" => "",
                "EffectTexTranslateV" => "",
                "EffectTexRotate" => "",
                "EffectTexScaleU" => "",
                "EffectTexScaleV" => "",
                "EffectColPower" => "",

                // Uber values
                "CullMode" => "",
                "LightSetNo" => "",
                "ShaderType" => "",
                "Priority" => "",
                "MipMapBias" => "",
                "PreMultiplieMode" => "",
                "BlendMode" => "",
                "ColorMapUvIndex" => "",
                "Layer1UvIdx" => "",
                "EmissionMaskTexSS" => "",
                "AmbientTexSS" => "",
                "NormalMapTexSS" => "",
                "Col0TexSS" => "",
                "LyCol0TexSS" => "",
                "PolygonOffset" => "",

                _ => string.Empty
            };

            // Discard null result, this means the value no longer applies
            //Debug.Assert(!result.Equals(string.Empty), $"Error: Couldn't convert float param with name: {oldName}!");
            return result;
        }

        var floats = new FloatParameter[floatParams.Length];

        for (var i = 0; i < floatParams.Length; i++)
        {
            var floatParam = floatParams[i];

            floats[i] = new FloatParameter
            {
                PropertyBinding = ConvertParamName(floatParam.ValueName),
                FloatValue = floatParam.Value,
            };
        }

        return floats;
    }

    private Float4Parameter[] ConvertColorParams(Color3Param[] colorParams)
    {
        static string ConvertParamName(string oldName)
        {
            string? result = oldName switch
            {
                "0" => "",
                "ConstantColor0" => "",
                "ConstantColorSd0" => "",
                "ConstantColor1" => "",
                "ConstantColorSd1" => "",
                "L1ConstantColor0" => "",
                "L1AddColor0" => "",
                "L1ConstantColor1" => "",
                "L1AddColor1" => "",
                "L1ConstantColorSd0" => "",
                "L1ConstantColorSd1" => "",
                "DeepShadowColor" => "",
                "SpecularColor" => "",
                "SphereMapColor" => "",
                "RimColor" => "",
                "RimColorShadow" => "",
                "ConstantColor" => "",
                "OnGameColor" => "",
                "OutLineCol" => "",
                "EffectColor01" => "",
                _ => string.Empty
            };

            // Discard null result, this means the value no longer applies
            //Debug.Assert(!result.Equals(string.Empty), $"Error: Couldn't convert float param with name: {oldName}!");
            return result;
        }

        var colors = new Float4Parameter[colorParams.Length];

        for (var i = 0; i < colorParams.Length; i++)
        {
            var colorParam = colorParams[i];

            colors[i] = new Float4Parameter
            {
                PropertyBinding = ConvertParamName(colorParam.ColorName),
                ColorValue = new PackedColor4f(colorParam.Color.R, colorParam.Color.G, colorParam.Color.B),
            };
        }

        return colors;
    }


    private enum StandardShaderProperties
    {
        // StringParam possible values:
        // "EnableBaseColorMap"          -> Allowed values: "True", "False"                                  | shader_bool: 1 < Maybe 'required'?
        // "EnableNormalMap"             -> Allowed values: "True", "False"
        // "EnableMetallicMap"           -> Allowed values: "True", "False"
        // "EnableRoughnessMap"          -> Allowed values: "True", "False"
        // "EnableEmissionColorMap"      -> Allowed values: "True", "False"
        // "EnableAOMap"                 -> Allowed values: "True", "False"
        // "EnableAlphaTest"             -> Allowed values: "True", "False"                                  (oldMaterial.TextureAlphaTest == 0).ToString()
        // "NumMaterialLayer"            -> Allowed values: "1", "2", "3", "4", "5"
        // "BillboardType"               -> Allowed values: "Disable", "AxisXYZ", "AxisY"
        // "WindReceiverType"            -> Allowed values: "Disable", "Simple", "Standard", "SimpleLeaf"
        // "EnableWindMaskMap"           -> Allowed values: "True", "False"
        // "EnableBaseColorMap1"         -> Allowed values: "True", "False"
        // "EnableNormalMap1"            -> Allowed values: "True", "False"
        // "EnableNormalMap2"            -> Allowed values: "True", "False"
        // "EnableAOMap1"                -> Allowed values: "True", "False"
        // "EnableAOMap2"                -> Allowed values: "True", "False"
        // "LayerMaskSource"             -> Allowed values: "Const", "Texture", "VertexColor"                | shader_bool: 1
        // "LayerMaskSwizzle"            -> Allowed values: "RGBA", "R111", "A111"
        // "LayerBaseMaskSource"         -> Allowed values: "One", "OneMinusLayerMaskSum"                    | shader_bool: 1
        // "EnableVertexBaseColor"       -> Allowed values: "True", "False"
        // "WeatherLayerMaskSource"      -> Allowed values: "Const", "Texture"
        // "EnableDepthFade"             -> Allowed values: "True", "False"
        // "EnablePackedMap"             -> Allowed values: "True", "False"
        // "EnableUVScaleOffsetNormal"   -> Allowed values: "True", "False"

        // Global params:
        // "EnableDeferredRendering"     -> Allowed values: "True", "False"
        // "InstancingType"              -> Allowed values: "Disable", "World", "Detail"
        // "EnableGrassCollisionMap"     -> Allowed values: "True", "False"
        // "NumRequiredUV"               -> Allowed values: "1", "2"
        // "EnableWeatherLayer"          -> Allowed values: "True", "False"
        // "EnableDisplacementMap"       -> Allowed values: "True", "False"
        // "EnableLerpBaseColorEmission" -> Allowed values: "True", "False"

    };

    private class StandardShader
    {
        public static StandardShader FromSWSHStandardShader(SWSHStandardShader shader)
        {
            return new StandardShader()
            {
                EnableBaseColorMap = shader.UseColorTex,
                EnableNormalMap = shader.NormalMapEnable,
                EnableAOMap = shader.AmbientMapEnable,
                // EnableVertexBaseColor = shader.BaseColorAddEnable, // TODO: This seems incorrect
            };
        }

        public bool EnableBaseColorMap { get; set; } = true;
        public bool EnableNormalMap { get; set; } = true;
        public bool EnableMetallicMap { get; set; } = false;
        public bool EnableRoughnessMap { get; set; } = false;
        public bool EnableEmissionColorMap { get; set; } = false;
        public bool EnableAOMap { get; set; } = true;
        public bool EnableAlphaTest { get; set; } = false;
        public int NumMaterialLayer { get; set; } = 1;
        public bool BillboardType { get; set; }
        public bool WindReceiverType { get; set; }
        public bool EnableWindMaskMap { get; set; }
        public bool EnableBaseColorMap1 { get; set; }
        public bool EnableNormalMap1 { get; set; }
        public bool EnableNormalMap2 { get; set; }
        public bool EnableAOMap1 { get; set; }
        public bool EnableAOMap2 { get; set; }
        public bool LayerMaskSource { get; set; }
        public bool LayerMaskSwizzle { get; set; }
        public bool LayerBaseMaskSource { get; set; }
        public bool EnableVertexBaseColor { get; set; }
        public bool WeatherLayerMaskSource { get; set; }
        public bool EnableDepthFade { get; set; }
        public bool EnablePackedMap { get; set; }
        public bool EnableUVScaleOffsetNormal { get; set; }

        // Global params:
        public bool EnableDeferredRendering { get; set; }
        public bool InstancingType { get; set; }
        public bool EnableGrassCollisionMap { get; set; }
        public int NumRequiredUV { get; set; } = 1;
        public bool EnableWeatherLayer { get; set; }
        public bool EnableDisplacementMap { get; set; }
        public bool EnableLerpBaseColorEmission { get; set; }

        public StringParameter[] BuildStringParams()
        {
            var stringParams = new List<StringParameter>
            {
                new("EnableBaseColorMap", EnableBaseColorMap.ToString()),
                new("EnableNormalMap", EnableNormalMap.ToString()),
                new("EnableParallaxMap", "False"), // TODO: This one seems to not be allowed by the default shader, yet it's always added?
                new("EnableMetallicMap", EnableMetallicMap.ToString()),
                new("EnableRoughnessMap", EnableRoughnessMap.ToString()),
                new("EnableEmissionColorMap", EnableEmissionColorMap.ToString()),
                new("EnableAOMap", EnableAOMap.ToString()),
                new("EnableAlphaTest", EnableAlphaTest.ToString()),
                new("NumMaterialLayer", NumMaterialLayer.ToString()),
                new("EnableVertexBaseColor", EnableVertexBaseColor.ToString())
            };

            if (false)
            {
                stringParams.Add(new("BillboardType", BillboardType.ToString()));
                stringParams.Add(new("WindReceiverType", WindReceiverType.ToString()));
                stringParams.Add(new("EnableWindMaskMap", EnableWindMaskMap.ToString()));
                stringParams.Add(new("EnableBaseColorMap1", EnableBaseColorMap1.ToString()));
                stringParams.Add(new("EnableNormalMap1", EnableNormalMap1.ToString()));
                stringParams.Add(new("EnableNormalMap2", EnableNormalMap2.ToString()));
                stringParams.Add(new("EnableAOMap1", EnableAOMap1.ToString()));
                stringParams.Add(new("EnableAOMap2", EnableAOMap2.ToString()));
                stringParams.Add(new("LayerMaskSource", LayerMaskSource.ToString()));
                stringParams.Add(new("LayerMaskSwizzle", LayerMaskSwizzle.ToString()));
                stringParams.Add(new("LayerBaseMaskSource", LayerBaseMaskSource.ToString()));
                stringParams.Add(new("WeatherLayerMaskSource", WeatherLayerMaskSource.ToString()));
                stringParams.Add(new("EnableDepthFade", EnableDepthFade.ToString()));
                stringParams.Add(new("EnablePackedMap", EnablePackedMap.ToString()));
                stringParams.Add(new("EnableUVScaleOffsetNormal", EnableUVScaleOffsetNormal.ToString()));

                stringParams.Add(new("EnableDeferredRendering", EnableDeferredRendering.ToString()));
                stringParams.Add(new("InstancingType", InstancingType.ToString()));
                stringParams.Add(new("EnableGrassCollisionMap", EnableGrassCollisionMap.ToString()));
                stringParams.Add(new("NumRequiredUV", NumRequiredUV.ToString()));
                stringParams.Add(new("EnableWeatherLayer", EnableWeatherLayer.ToString()));
                stringParams.Add(new("EnableDisplacementMap", EnableDisplacementMap.ToString()));
                stringParams.Add(new("EnableLerpBaseColorEmission", EnableLerpBaseColorEmission.ToString()));
            }

            return stringParams.ToArray();
        }
    }

    private class SWSHStandardShader
    {
        public SWSHStandardShader(Material8 source)
        {
            // TODO
            Dictionary<string, string> oldUberFlags = source.StaticParam.UberFlags.ToDictionary(flag => flag.FlagName, flag => flag.FlagEnable.ToString());
            foreach (var flag in source.Flags)
            {
                switch (flag.FlagName)
                {
                    case "useColorTex": UseColorTex = flag.FlagEnable; break;
                    case "SwitchEmissionMaskTexUV": SwitchEmissionMaskTexUV = flag.FlagEnable; break;
                    case "EmissionMaskUse": EmissionMaskUse = flag.FlagEnable; break;
                    case "SwitchPriority": SwitchPriority = flag.FlagEnable; break;
                    case "Layer1Enable": Layer1Enable = flag.FlagEnable; break;
                    case "SwitchAmbientTexUV": SwitchAmbientTexUV = flag.FlagEnable; break;
                    case "AmbientMapEnable": AmbientMapEnable = flag.FlagEnable; break;
                    case "SwitchNormalMapUV": SwitchNormalMapUV = flag.FlagEnable; break;
                    case "NormalMapEnable": NormalMapEnable = flag.FlagEnable; break;
                    case "LightTableEnable": LightTableEnable = flag.FlagEnable; break;
                    case "SpecularMaskEnable": SpecularMaskEnable = flag.FlagEnable; break;
                    case "BaseColorAddEnable": BaseColorAddEnable = flag.FlagEnable; break;
                    case "SphereMapEnable": SphereMapEnable = flag.FlagEnable; break;
                    case "SphereMaskEnable": SphereMaskEnable = flag.FlagEnable; break;
                    case "RimMaskEnable": RimMaskEnable = flag.FlagEnable; break;
                    case "alphaShell": AlphaShell = flag.FlagEnable; break;
                    case "EffectVal": EffectVal = flag.FlagEnable; break;
                    case "NormalEdgeEnable": NormalEdgeEnable = flag.FlagEnable; break;
                    case "OutLineIDEnable": OutLineIDEnable = flag.FlagEnable; break;
                    case "OutLineColFixed": OutLineColFixed = flag.FlagEnable; break;
                    // Global flags
                    case "FogEnable": FogEnable = flag.FlagEnable; break;
                    case "DiscardEnable": DiscardEnable = flag.FlagEnable; break;
                    case "CastShadow": CastShadow = flag.FlagEnable; break;
                    case "ReceiveShadow": ReceiveShadow = flag.FlagEnable; break;
                    case "TextureAlphaTestEnable": TextureAlphaTestEnable = flag.FlagEnable; break;
                    case "ShadowMapPrevEnable": ShadowMapPrevEnable = flag.FlagEnable; break;
                    case "LayerCalcMulti": LayerCalcMulti = flag.FlagEnable; break;
                    case "FireMaskPathEnable": FireMaskPathEnable = flag.FlagEnable; break;
                    case "GPUInstancingEnable": GPUInstancingEnable = flag.FlagEnable; break;
                    case "Wireframe": Wireframe = flag.FlagEnable; break;
                    case "DepthWrite": DepthWrite = flag.FlagEnable; break;
                    case "DepthTest": DepthTest = flag.FlagEnable; break;
                    case "IsErase": IsErase = flag.FlagEnable; break;
                    case "MayaPreviewEnable": MayaPreviewEnable = flag.FlagEnable; break;
                    default:
                        Debug.Assert(false, $"Error: Couldn't convert shader flag with name: {flag.FlagName}!");
                        break;
                }
            }

            Dictionary<string, float> valueLookup = source.Values.ToDictionary(flag => flag.ValueName, flag => flag.Value);
            UVScaleOffset = new Vec4f(valueLookup["ColorUVScaleU"], valueLookup["ColorUVScaleV"], valueLookup["ColorBaseU"], valueLookup["ColorBaseV"]);
            UVScaleOffsetNormal = new Vec4f(valueLookup["NormalMapUVScaleU"], valueLookup["NormalMapUVScaleV"]);
        }

        public bool UseColorTex { get; set; }
        public bool SwitchEmissionMaskTexUV { get; set; }
        public bool EmissionMaskUse { get; set; }
        public bool SwitchPriority { get; set; }
        public bool Layer1Enable { get; set; }
        public bool SwitchAmbientTexUV { get; set; }
        public bool AmbientMapEnable { get; set; }
        public bool SwitchNormalMapUV { get; set; }
        public bool NormalMapEnable { get; set; }
        public bool LightTableEnable { get; set; }
        public bool SpecularMaskEnable { get; set; }
        public bool BaseColorAddEnable { get; set; }
        public bool SphereMapEnable { get; set; }
        public bool SphereMaskEnable { get; set; }
        public bool RimMaskEnable { get; set; }
        public bool AlphaShell { get; set; }
        public bool EffectVal { get; set; }
        public bool NormalEdgeEnable { get; set; }
        public bool OutLineIDEnable { get; set; }
        public bool OutLineColFixed { get; set; }

        // Global flags
        public bool FogEnable { get; set; }
        public bool DiscardEnable { get; set; }
        public bool CastShadow { get; set; }
        public bool ReceiveShadow { get; set; }
        public bool TextureAlphaTestEnable { get; set; }
        public bool ShadowMapPrevEnable { get; set; }
        public bool LayerCalcMulti { get; set; }
        public bool FireMaskPathEnable { get; set; }
        public bool GPUInstancingEnable { get; set; }
        public bool Wireframe { get; set; }
        public bool DepthWrite { get; set; }
        public bool DepthTest { get; set; }
        public bool IsErase { get; set; }
        public bool MayaPreviewEnable { get; set; }

        public Vec4f UVScaleOffset { get; set; }
        public Vec4f UVScaleOffsetNormal { get; set; }

        public bool IsTextureBound(string samplerName)
        {
            return samplerName switch
            {
                "Col0Tex" => UseColorTex,
                "EmissionMaskTex" => EmissionMaskUse,
                "LyCol0Tex" => Layer1Enable,
                "AmbientTex" => AmbientMapEnable,
                "NormalMapTex" => NormalMapEnable,
                "LightTblTex" => false, // Don't need this anymore, so don't care
                "SphereMapTex" => SphereMapEnable,
                "EffectTex" => false, // TODO: EffectVal?
                _ => false
            };
        }
    }

    private (TextureParameter[], SamplerState[]) ConvertTextureParams(Texture8[] oldTextures, SWSHStandardShader shader)
    {
        static string ConvertSamplerName(string swshSamplerName)
        {
            var result = swshSamplerName switch
            {
                "0" => "LayerMaskMap",
                "1" => "MetallicMap",
                "2" => "RoughnessMap",

                "Col0Tex" => "BaseColorMap",
                "EmissionMaskTex" => "",
                "LyCol0Tex" => "",
                "AmbientTex" => "AOMap", // TODO: New format only has R channel
                "NormalMapTex" => "NormalMap",
                "LightTblTex" => "",
                "SphereMapTex" => "",
                "EffectTex" => "",
                _ => string.Empty
            };

            //Debug.Assert(!string.IsNullOrEmpty(result), $"Error: Couldn't convert sampler with name: {swshSamplerName}!");
            return result;
        }

        static UvWrapMode FromOldWrapMode(UVWrapMode8 mode) => mode switch
        {
            UVWrapMode8.CLAMP => UvWrapMode.Clamp,
            UVWrapMode8.BORDER => UvWrapMode.Border,
            UVWrapMode8.WRAP => UvWrapMode.Wrap,
            UVWrapMode8.MIRROR => UvWrapMode.Mirror,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown uv mode")
        };

        string[] files = SWSHModel.GFBModel.TextureFiles;
        var textures = new List<TextureParameter>(oldTextures.Length);
        var samplers = new List<SamplerState>(oldTextures.Length);

        foreach (var oldTexture in oldTextures)
        {
            if (!shader.IsTextureBound(oldTexture.SamplerName))
                continue;

            textures.Add(new TextureParameter
            {
                PropertyBinding = ConvertSamplerName(oldTexture.SamplerName),
                TextureSlot = (uint)textures.Count, // TODO: Might be static ? Aka texture x is always slot y
                TextureFile = files[oldTexture.TextureIndex] + ".bntx",
            });

            samplers.Add(new SamplerState
            {
                RepeatU = FromOldWrapMode(oldTexture.Settings.RepeatU),
                RepeatV = FromOldWrapMode(oldTexture.Settings.RepeatV),
                RepeatW = FromOldWrapMode(oldTexture.Settings.Repeat2),
                BorderColor = oldTexture.Settings.BorderColor,
                //SamplerState0 = oldTexture.Settings.Filtermode,
                //SamplerState1 = oldTexture.Settings.MipMapBias
            });
        }

        // Always two more then the texture count that are always Wrap. Probably some default samplers for deferred render buffers
        samplers.Add(new() { RepeatU = UvWrapMode.Wrap, RepeatV = UvWrapMode.Wrap });
        samplers.Add(new() { RepeatU = UvWrapMode.Wrap, RepeatV = UvWrapMode.Wrap });

        return (textures.ToArray(), samplers.ToArray());
    }

    private MaterialPass FromStandardShaderParams(Material8 oldMaterial)
    {
        var oldShader = new SWSHStandardShader(oldMaterial);
        var newShader = StandardShader.FromSWSHStandardShader(oldShader);
        var (textures, samplers) = ConvertTextureParams(oldMaterial.Textures, oldShader);

        return new MaterialPass
        {
            Name = oldMaterial.Name,
            Shaders = new Shader[]
            {
                new ()
                {
                    ShaderName = "Standard",
                    ShaderValues = newShader.BuildStringParams()
}
            },
            FloatParameters = new FloatParameter[]
            {
                new("DiscardValue", 0),
                new("NormalHeight", 1),
                new("EmissionIntensity", 0),
            },
            TextureParameters = textures,
            Samplers = samplers,
            Float4LightParameters = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameters = new Float4Parameter[]
            {
                new("UVScaleOffset"      , oldShader.UVScaleOffset),
                new("UVScaleOffsetNormal", oldShader.UVScaleOffsetNormal),
                new("EmissionColor"      , new()),
            },
            IntParameters = new IntParameter[]
            {
                new("CastShadow", oldMaterial.CastShadow),
                new("ReceiveShadow", 1), // TODO: might want to force this to 1
                new("CategoryLabel", 2), // TODO
                new("UVIndexLayerMask", -1), // TODO
            },
            WriteMask = new WriteMask(), // TODO
            IntExtra = new IntExtra(), // TODO
            AlphaType = "Opaque" // TODO
        };
    }

    private MaterialPass FromEyeShaderParams(Material8 oldMaterial)
    {
        // "EnableNormalMap"        -> Allowed values: "True", "False"
        // "EnableParallaxMap"      -> Allowed values: "True", "False"
        // "EnableMetallicMap"      -> Allowed values: "True", "False"
        // "EnableRoughnessMap"     -> Allowed values: "True", "False"
        // "EnableEmissionColorMap" -> Allowed values: "True", "False"
        // "EnableAOMap"            -> Allowed values: "True", "False"
        // "EyelidType"             -> Allowed values: "None", "Upper", "Lower", "All"
        // "NumMaterialLayer"       -> Allowed values: "1", "2", "3", "4", "5"
        // "EnableHighlight"        -> Allowed values: "True", "False"
        // "UVTransformMode"        -> Allowed values: "SRT", "T"
        // "EnableOverrideColor"    -> Allowed values: "True", "False"

        // Global:
        // "EnableWeatherLayer"     -> Allowed values: "True", "False"

        return new MaterialPass
        {
            Name = oldMaterial.Name,
            Shaders = new Shader[]
            {
                new()
                {
                    ShaderName = "Eye",
                    ShaderValues = new StringParameter[]
                    {
                        new("EnableNormalMap", "True"),
                        new("EnableParallaxMap", "False"),
                        new("EnableMetallicMap", "False"),
                        new("EnableRoughnessMap", "False"),
                        new("EnableEmissionColorMap", "False"),
                        new("EnableAOMap", "True"),
                        new("NumMaterialLayer", "1"),
                    }
                }
            },
            FloatParameters = new FloatParameter[]
            {
            },
            TextureParameters = Array.Empty<TextureParameter>(), // TODO
            Samplers = Array.Empty<SamplerState>(), // TODO
            Float4LightParameters = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameters = Array.Empty<Float4Parameter>(), // TODO
            IntParameters = Array.Empty<IntParameter>(), // TODO
            WriteMask = new WriteMask(), // TODO
            IntExtra = new IntExtra(), // TODO
            AlphaType = "" // TODO
        };
    }

    private void ConvertToTRMaterial()
    {
        var materialPasses = new List<MaterialPass>();
        foreach (var material in SWSHModel.GFBModel.Materials)
        {
            // TODO:
            // material.Shader;
            // material.SortPriority;
            // material.DepthWrite;
            // material.DepthTest;
            // material.LightSetNum;
            // material.BlendMode;
            // material.CullMode;
            // material.VertexShaderFileId;
            // material.GeomShaderFileId;
            // material.FragShaderFileId;
            // material.ReceiveShadow;
            // material.CastShadow;
            // material.SelfShadow;
            // material.TextureAlphaTest;
            // material.DepthComparisonFunction;
            // material.DepthBias;
            // material.Field_18;
            // material.Field_19;

            materialPasses.Add(FromStandardShaderParams(material));
            //materialPasses.Add(FromEyeShaderParams(material));
        }

        Result.DefaultMaterials = new TRMaterial[]
        {
            new()
            {
                MaterialPasses = materialPasses.ToArray(),
            }
        };

        // TODO: Assign shiny colors
        Result.MeshMaterials = new MeshMaterialWrapper[]
        {
            new()
            {
                Name = "rare",
                Materials = new TRMaterial[]
                {
                    new()
                    {
                        Reserved_00 = 0,
                        MaterialPasses = materialPasses.ToArray(),
                    }
                }
            }
        };
    }

    private void ConvertToMultiMaterialTable(string resultFileName)
    {
        Debug.Assert(Result.Meshes.Length != 0, "Meshes should be converted before materials");

        var allShapes = new List<MaterialSwitches>();
        foreach (var mesh in Result.Meshes)
        {
            foreach (var shape in mesh.Shapes)
            {
                allShapes.Add(new MaterialSwitches
                {
                    Name = shape.MeshShapeName,
                    Flags = 1 // TODO
                });
            }
        }

        Result.TRMMT.Material = new MaterialTable[]
        {
            new ()
            {
                Name = "rare",
                FileNames = new []{ $"{resultFileName}_rare.trmtr" },
                MaterialSwitches = allShapes.ToArray(),
                MaterialProperties = Array.Empty<MaterialProperties>(), // TODO
            },
        };
    }

    private void ConvertToTRModel()
    {
        string resultFileName = $"pm{(ushort)SWSHModel.Config.SpeciesId:0000}_00_00";
        string sourceFileName = $"pm{(ushort)SWSHModel.Config.SpeciesId:0000}_00";

        Result.TRModel.Meshes = new FileReference[]
        {
            new() { Filename = $"{resultFileName}.trmsh" },
        };
        Result.TRModel.Skeleton = new FileReference { Filename = $"{resultFileName}.trskl" };
        Result.TRModel.Materials = new[] { $"{resultFileName}.trmtr", };

        // SWSH models don't have LOD's so we only need one
        Result.TRModel.LODs = new LOD[]
        {
            new()
            {
                Type = "Custom",
                Entries = new LODIndex[]
                {
                    new() { Index = 0 }
                }
            }
        };

        Result.TRModel.Bounds = SWSHModel.GFBModel.BoundingBox / 100;
        Result.TRModel.SphereBounds = new PackedSphere(Result.TRModel.Bounds);

        ConvertToTRSkeleton();
        ConvertToTRMesh(sourceFileName, resultFileName);
        ConvertToTRMeshShape();
        ConvertToTRMaterial();
        ConvertToMultiMaterialTable(resultFileName);
    }

    private void B_ShowConverter_Click(object sender, EventArgs e)
    {
        var converter = new TestMatrixConverter();
        converter.Show();
    }

    private void CB_Species_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdatePLAModel();
    }

    private void CB_SWSHSpecies_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSWSHModel();
    }

    public static T DeepClone<T>(T obj) where T : class, new()
    {
        var data = FlatBufferConverter.SerializeFrom(obj);
        return FlatBufferConverter.DeserializeFrom<T>(data);
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        //Result = DeepClone(PLAModel);

        //Result.DefaultMaterials[0].MaterialPasses[3] = DeepClone(Result.DefaultMaterials[0].MaterialPasses[2]);

        //PG_Converted.SelectedObject = Result;
        //return;
        SpeciesId = (ushort)SWSHModel.Config.SpeciesId;
        string resultFileName = $"pm{SpeciesId:0000}_00_00";

        FileName = resultFileName;
        BasePath = Path.Combine(PokemonModelDir.FilePath!, $"bin/pokemon/pm{SpeciesId:0000}/{FileName}/");

        PokemonModelDir.AddFile(BasePath + $"{FileName}.trpokecfg", FlatBufferConverter.SerializeFrom(Result.Config));
        PokemonModelDir.AddFile(ModelPath + $"{FileName}.trmmt", FlatBufferConverter.SerializeFrom(Result.TRMMT));
        PokemonModelDir.AddFile(ModelPath + $"{FileName}.trmdl", FlatBufferConverter.SerializeFrom(Result.TRModel));
        PokemonModelDir.AddFile(ModelPath + $"{Result.TRModel.Skeleton.Filename}", FlatBufferConverter.SerializeFrom(Result.Skeleton));

        PokemonModelDir.AddFile(ModelPath + $"{FileName}.trpokecfg", FlatBufferConverter.SerializeFrom(Result.Config));

        for (var i = 0; i < Result.TRModel.Materials.Length; i++)
        {
            var materialName = Result.TRModel.Materials[i];
            PokemonModelDir.AddFile(ModelPath + $"{materialName}", FlatBufferConverter.SerializeFrom(Result.DefaultMaterials[i]));
        }

        for (var i = 0; i < Result.MeshMaterials.Length; i++)
        {
            for (var j = 0; j < Result.MeshMaterials[i].Materials.Length; j++)
            {
                if (Result.TRMMT.Material[i].Name == "normal")
                    continue; // The default material was already created

                var materialName = Result.TRMMT.Material[i].FileNames[j];
                var material = Result.MeshMaterials[i].Materials[j];
                PokemonModelDir.AddFile(ModelPath + $"{materialName}", FlatBufferConverter.SerializeFrom(material));
            }
        }

        for (var i = 0; i < Result.TRModel.Meshes.Length; i++)
        {
            var meshName = Result.TRModel.Meshes[i].Filename;
            PokemonModelDir.AddFile(ModelPath + $"{meshName}", FlatBufferConverter.SerializeFrom(Result.Meshes[i]));
        }

        for (var i = 0; i < Result.Meshes.Length; i++)
        {
            var meshBufferName = Result.Meshes[i].BufferFileName;
            PokemonModelDir.AddFile(ModelPath + $"{meshBufferName}", FlatBufferConverter.SerializeFrom(Result.MeshDataBuffers[i]));
        }

        PokemonModelDir.Dump(null, null);
    }
}
