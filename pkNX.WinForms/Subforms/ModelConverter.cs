using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
// n |  3 |  - Materials
// n |  3 |  - Constant buffers (material params)
// n |  4 |  - Shaders
// p |  2 |  - Mesh information
// p |  2 |  - Mesh buffers / vertex layout
// p |  3 |  - Skeleton
// p |  3 |  - LOD structure
// p |  3 |  - Other properties
// n |  2 | Save PLA models
// x | 35 |

// TODO's per file type
// TRConfig -> fill in missing fields
// TRModel -> Auto generate LODs, Field_06
// TRMMT -> MaterialSwitches, MaterialProperties
// TRMesh -> Split eyes into submesh and assign eye shader, maybe sort entries?
// TRSubMesh -> Material name might need to be converted to snake_case
// TRMeshShape -> BoneWeights[]
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

    [TypeConverter(typeof(ExpandableObjectConverter))]
    private class MeshMaterialWrapper
    {
        public string Name { get; set; } = string.Empty;
        public TRMaterial[] Materials { get; set; } = Array.Empty<TRMaterial>();
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    private class ModelWrapper
    {
        public TRPokeConfig Config { get; set; } = new();
        public TRModel TRModel { get; set; } = new();
        public TRMeshMaterial TRMMT { get; set; } = new();
        public TRMesh[] Meshes { get; set; } = Array.Empty<TRMesh>();
        public TRMeshBuffer[] MeshDataBuffers { get; set; } = Array.Empty<TRMeshBuffer>();
        public TRMaterial[] DefaultMaterials { get; set; } = Array.Empty<TRMaterial>();
        public MeshMaterialWrapper[] MeshMaterials { get; set; } = Array.Empty<MeshMaterialWrapper>();
        public TRSkeleton Skeleton { get; set; } = new();

        public string[] UsedTextures { get; set; } = Array.Empty<string>();
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
        PLAModel.TRMMT = FlatBufferConverter.DeserializeFrom<TRMeshMaterial>(pack.GetDataFullPath(ModelPath + $"{FileName}.trmmt"));
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
        LoadMeshMaterials(pack);
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
    }

    private void LoadMeshMaterials(GFPack pack)
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
            Debug.Assert(mesh.Field_00 == 1, "Here's one!");

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
                    Debug.Assert(subMesh.Field_02 == 1, "Here's one!");
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

            subMeshName = subMeshName.Replace(sourceFileName, "", StringComparison.InvariantCultureIgnoreCase);
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
                var bounds = shape.PackedAabb / 100; // Scale down swsh models by 100
                meshShapes.Add(new MeshShape
                {
                    MeshShapeName = $"{resultFileName}_{subMeshName}_mesh_shape", // TODO: Use in MMT
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
                    ShaderValues = new StringParameter[]
                    {
                        new("EnableBaseColorMap", oldFlags["useColorTex"]),
                        new("EnableNormalMap", oldFlags["NormalMapEnable"]),
                        new("EnableParallaxMap", "?"), // TODO
                        new("EnableMetallicMap", "?"), // TODO
                        new("EnableRoughnessMap", "?"), // TODO
                        new("EnableEmissionColorMap", oldFlags["EmissionMaskUse"]),
                        new("EnableAOMap", oldFlags["AmbientMapEnable"]),
                        new("EnableAlphaTest", (oldMaterial.TextureAlphaTest == 0).ToString()), // TODO
                        new("NumMaterialLayer", "5"), // TODO: Adds the ParamName + LayerX parameters
                        new("EnableLerpBaseColorEmission", "?"), // TODO
                        new("EnableVertexBaseColor", "?"), // TODO
                    }
                }
            },
            FloatParameter = new FloatParameter[]
            {
                new("DiscardValue", 0f),                          // TODO
                new("Metallic", 0f),                              // TODO
                new("MetallicLayer1", 0f),                        // TODO
                new("MetallicLayer2", 0f),                        // TODO
                new("MetallicLayer3", 0f),                        // TODO
                new("MetallicLayer4", 0f),                        // TODO
                new("Roughness", 0.5f),                           // TODO
                new("RoughnessLayer1", 0f),                       // TODO
                new("RoughnessLayer2", 0f),                       // TODO
                new("RoughnessLayer3", 0.1f),                     // TODO
                new("RoughnessLayer4", 0.1f),                     // TODO
                new("NormalHeight", 1f),                          // TODO
                new("EmissionIntensity", 0f),                     // TODO
                new("EmissionIntensityLayer1", 0f),               // TODO
                new("EmissionIntensityLayer2", 0f),               // TODO
                new("EmissionIntensityLayer3", 0f),               // TODO
                new("EmissionIntensityLayer4", 0f),               // TODO
                new("LayerMaskScale1", 1f),                       // TODO
                new("LayerMaskScale2", 1f),                       // TODO
                new("LayerMaskScale3", 1f),                       // TODO
                new("LayerMaskScale4", 1f),                       // TODO
            },
            TextureParameters = Array.Empty<TextureParameter>(), // TODO
            Samplers = Array.Empty<SamplerState>(), // TODO
            Field_05 = "", // TODO
            Float4LightParameter = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameter = Array.Empty<Float4Parameter>(), // TODO
            Field_08 = "", // TODO
            IntParameter = new IntParameter[]
            {
                new("CastShadow", oldMaterial.CastShadow),
                new("ReceiveShadow", oldMaterial.ReceiveShadow), // TODO: might want to force this to 1
                new("CategoryLabel", 2), // TODO
                new("UVIndexLayerMask", -1), // TODO
            },
            Field_10 = "", // TODO
            Field_11 = "", // TODO
            Field_12 = "", // TODO
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
            FloatParameter = new FloatParameter[]
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
            Field_05 = "", // TODO
            Float4LightParameter = Array.Empty<Float4Parameter>(), // TODO
            Float4Parameter = Array.Empty<Float4Parameter>(), // TODO
            Field_08 = "", // TODO
            IntParameter = Array.Empty<IntParameter>(), // TODO
            Field_10 = "", // TODO
            Field_11 = "", // TODO
            Field_12 = "", // TODO
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
            // material.Name;
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
            // material.Textures;
            // material.Flags;
            // material.Values;
            // material.Colors;
            // material.ReceiveShadow;
            // material.CastShadow;
            // material.SelfShadow;
            // material.TextureAlphaTest;
            // material.DepthComparisonFunction;
            // material.StaticParam;
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

    private void ConvertToTRMeshMaterial(string resultFileName)
    {
        Result.TRMMT.Material = new Mmt[]
        {
            new ()
            {
                Name = "rare",
                FileNames = new []{ $"{resultFileName}_rare.trmtr" },
                MaterialSwitches = Array.Empty<MaterialSwitches>(), // TODO
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
        ConvertToTRMeshMaterial(resultFileName);
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
}
