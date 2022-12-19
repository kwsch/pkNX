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
// TRMeshShape -> BoneWeights[] Possibly this is just the sum of all blend indices + weights on the shape
// TRMeshBuffer -> Update BLEND_INDICES, Possibly need to remove vertex color
// TRMaterial -> Properly tackle this, Material name might need to be converted to snake_case
// TRSkeleton -> Name of first bone should be updated, might need to snake_case all names

// TODO SWSH unused properties:
// GFBPokeConfig -> Version, SpeciesId, FormId, Origin, Height, HeightAdjust, FieldAdjust, AABB, 
// InframeHeight, RegionId, Motion
// MaterialEntries

// GFBModel -> Version?, TextureFiles, All shaders
// Mesh8 -> SortPriority (only used rarely)
// Skeleton8 -> Effect and Visible 


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

        SWSHPokemonModelDir = (FolderContainer)ROM[GameFile.Debug_SWSHPokemonArchiveFolder];
        SWSHPokemonModelDir.Initialize();
        CB_SWSHSpecies.Items.AddRange(SWSHPokemonModelDir.GetFileNames().ToArray());
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
        Debug.Assert((int)PLAModel.Config.Field_09 == 0.0f, "Here's one!");
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

        Debug.Assert(PLAModel.TRMMT.Field_00 == 0, "Here's one!");
        Debug.Assert(PLAModel.TRMMT.Field_01 == 0, "Here's one!");

        Debug.Assert(PLAModel.TRModel.Field_00 == 0, "Here's one!");
        foreach (var lod in PLAModel.TRModel.LODs)
        {
            Debug.Assert(lod.Type == "Custom", "Here's one!");
        }

        Debug.Assert(PLAModel.Skeleton.Field_00 == 0, "Here's one!");
        foreach (var node in PLAModel.Skeleton.Bones)
        {
            Debug.Assert(node.Type is NodeType.Transform or NodeType.Joint or NodeType.Locator, "Here's one!");
        }
        foreach (var boneParam in PLAModel.Skeleton.BoneParams)
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
            Debug.Assert(mesh.Field_00 == 0, "Here's one!");

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

        Result.Config.Field_01 = 0f; // TODO
        Result.Config.Field_02 = 0f; // TODO
        Result.Config.Field_03 = 0f; // TODO
        Result.Config.Field_09 = 0f; // TODO
        Result.Config.Field_10_YOffset = 0f; // TODO
        Result.Config.Field_11_YOffset = 0f; // TODO
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
        // Result.Skeleton.BoneParams[];
        // BoneParams bone matrix is maya’s transform matrix inverted
        // Result.Skeleton.Iks[];

        // TODO: Probably need to ignore bones @ Mesh8.BoneId;

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
            // bone8.Visible;

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
                RigIdx = bone8.IsRigged ? rigIndex++ : -1,
                LocatorBone = string.Empty,
                Type = (NodeType)bone8.Type, // TODO: A lot of these seem to be converted into transform nodes instead of joint nodes
            });

            if (bone8.IsRigged && rigStart == -1)
            {
                rigStart = i;
            }
        }
        int rigEnd = rigIndex;

        Debug.Assert(rigStart > 0, "This skeleton seems to not be skinned.");

        Result.Skeleton.RigOffset = rigStart - 2; // By default we always skip the first two 2 nodes. Any additional offset should be marked. TODO: Is this true?
        Result.Skeleton.Bones = transformNodes.ToArray();

        // TODO: Result.Skeleton.BoneParams = new Bone[rigEnd];
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
                        MaterialName = mat.Name, // TODO
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
                    Weights = new BoneWeights[] { }, // TODO: All bones affected by the sub-mesh + some weight
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

    private TextureParameter[] ConvertTextureParams(Texture8[] oldTextures)
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


        string[] files = SWSHModel.GFBModel.TextureFiles;
        var textures = new TextureParameter[oldTextures.Length];

        for (var i = 0; i < oldTextures.Length; i++)
        {
            var oldTexture = oldTextures[i];

            // TODO: if files[oldTexture.TextureIndex] == "dummy_col" we should probably just remove the entry
            // TODO: Default_lta.bntx, projection_effect_col.bntx
            textures[i] = new TextureParameter
            {
                PropertyBinding = ConvertSamplerName(oldTexture.SamplerName),
                TextureSlot = (uint)i, // TODO
                TextureFile = files[oldTexture.TextureIndex] + ".bntx",
            };
        }

        return textures;
    }

    private SamplerState[] ConvertSamplerState(Texture8[] oldTextures)
    {
        var samplers = new SamplerState[oldTextures.Length + 2];

        for (var i = 0; i < oldTextures.Length; i++)
        {
            // TODO: oldTexture.Settings;

            samplers[i] = new SamplerState
            {
                BorderColor = new PackedColor4f(),
            };
        }

        // Last two are always Wrap. Probably some default samplers
        samplers[^2] = new SamplerState()
        {
            RepeatU = UvWrapMode.Wrap,
            RepeatV = UvWrapMode.Wrap,
        };
        samplers[^1] = new SamplerState()
        {
            RepeatU = UvWrapMode.Wrap,
            RepeatV = UvWrapMode.Wrap,
        };
        return samplers;
    }

    private StringParameter[] ConvertStringParams(Flag[] flags)
    {
        // StringParam possible values:
        // "EnableParallaxMap"
        // "EnableMetallicMap"
        // "EnableRoughnessMap"
        // "EnableEmissionColorMap"
        // "EnableAOMap"
        // "EnableAlphaTest" -> (oldMaterial.TextureAlphaTest == 0).ToString()
        // "NumMaterialLayer" -> Number!
        // "EnableLerpBaseColorEmission"
        // "EnableVertexBaseColor"
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

                // Uber flags
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


    private MaterialPass FromStandardShaderParams(Material8 oldMaterial)
    {
        var oldFlags = oldMaterial.Flags.ToDictionary(flag => flag.FlagName, flag => flag.FlagEnable.ToString());
        var oldUberFlags = oldMaterial.StaticParam.UberFlags.ToDictionary(flag => flag.FlagName, flag => flag.FlagEnable.ToString());

        return new MaterialPass
        {
            Name = oldMaterial.Name,
            Shaders = new Shader[]
            {
                new()
                {
                    ShaderName = "Standard",
                    ShaderValues = ConvertStringParams(oldMaterial.Flags)
                }
            },
            FloatParameters = ConvertFloatParams(oldMaterial.Values),
            TextureParameters = ConvertTextureParams(oldMaterial.Textures),
            Samplers = ConvertSamplerState(oldMaterial.Textures),
            Float4LightParameter = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameters = ConvertColorParams(oldMaterial.Colors),
            IntParameters = new IntParameter[]
            {
                new("CastShadow", oldMaterial.CastShadow),
                new("ReceiveShadow", oldMaterial.ReceiveShadow), // TODO: might want to force this to 1
                new("CategoryLabel", 2), // TODO
            },
            ByteExtra = new WriteMask(), // TODO
            IntExtra = new IntExtra(), // TODO
            AlphaType = "" // TODO
        };
    }

    private MaterialPass FromEyeShaderParams(Material8 oldMaterial)
    {
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
                        new("EnableBaseColorMap", "True"),
                        new("EnableNormalMap", "True"),
                        new("EnableParallaxMap", "False"),
                        new("EnableMetallicMap", "False"),
                        new("EnableRoughnessMap", "False"),
                        new("EnableEmissionColorMap", "False"),
                        new("EnableAOMap", "True"),
                        new("EnableAlphaTest", "False"),
                        new("NumMaterialLayer", "5"),
                        new("EnableLerpBaseColorEmission", "True"),
                        new("EnableVertexBaseColor", "False"),
                    }
                }
            },
            FloatParameters = new FloatParameter[]
            {
                new("DiscardValue", 0.0f),
                new("MetallicLayer3", 0.0f),
                new("MetallicLayer4", 0.0f),
                new("RoughnessLayer3", 0.0f),
                new("RoughnessLayer4", 0.0f),
                new("NormalHeight", 0.0f),
                new("MetallicLayer2", 0.0f),
                new("MetallicLayer1", 0.0f),
                new("RoughnessLayer2", 0.0f),
                new("EmissionIntensityLayer3", 0.0f),
                new("EmissionIntensityLayer4", 0.0f),
                new("RoughnessLayer1", 0.0f),
                new("LayerMaskScale3", 0.0f),
                new("LayerMaskScale4", 0.0f),
                new("Metallic", 0.0f),
                new("EmissionIntensityLayer2", 0.0f),
                new("Roughness", 0.0f),
                new("EmissionIntensityLayer1", 0.0f),
                new("LayerMaskScale2", 0.0f),
                new("EmissionIntensity", 0.0f),
                new("LayerMaskScale1", 0.0f),
            },
            TextureParameters = Array.Empty<TextureParameter>(), // TODO
            Samplers = Array.Empty<SamplerState>(), // TODO
            Float4LightParameter = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameters = Array.Empty<Float4Parameter>(), // TODO
            IntParameters = Array.Empty<IntParameter>(), // TODO
            ByteExtra = new WriteMask(), // TODO
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
                        Field_00 = 0,
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

        // TODO: Result.TRModel.Field_06;

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
