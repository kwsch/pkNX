using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures.FlatBuffers;
using pkNX.WinForms.Subforms;

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

// TRModel
// Missing Field_06
// ~ Missing LODs

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
        Debug.Assert(PLAModel.Config.SomeTypeEnum_023 is 0 or 2 or 3, "Here's one!");
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
            Debug.Assert(mesh.Field_00 == 0, "Here's one!");

            foreach (var shape in mesh.Shapes)
            {
                Debug.Assert(shape.IndexLayoutType == IndexLayoutType.IndexLayoutType_Uint16, "Here's one!");
                Debug.Assert(shape.Field_05 == 0, "Here's one!");
                Debug.Assert(shape.Field_06 == 0, "Here's one!");
                Debug.Assert(shape.Field_07 == 0, "Here's one!");
                Debug.Assert(shape.Field_08 == 0, "Here's one!");
                Debug.Assert(string.IsNullOrEmpty(shape.Field_11), "Here's one!");

                foreach (var attribute in shape.Attributes)
                {
                    foreach (var attr in attribute.Attrs)
                    {
                        Debug.Assert(attr.Field_00 == 0, "Here's one!");
                        Debug.Assert(attr.AttributeLayer is 0 or 1, "Here's one!");
                        Debug.Assert(attr.Attribute <= VertexAttributeIndex.BLEND_WEIGHTS, "Here's one!");
                        Debug.Assert(attr.Type is 20 or 22 or 39 or 43 or 48 or 51 or 54, "Here's one!");
                    }
                }

                foreach (var subMesh in shape.SubMeshes)
                {
                    Debug.Assert(subMesh.Field_02 == 0, "Here's one!");
                    Debug.Assert(subMesh.Field_04 == 0, "Here's one!");
                }
            }
        }

        LoadMeshBuffers(PLAModel.Meshes, pack);
    }

    private void LoadMeshBuffers(TRMesh[] trmeshes, GFPack pack)
    {
        PLAModel.MeshDataBuffers = trmeshes.Select(x => x.BufferFileName)
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMeshBuffer>(pack.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();

        foreach (var buffer in PLAModel.MeshDataBuffers)
        {
            Debug.Assert(buffer.Field_00 == 0, "Here's one!");
        }
    }

    private void B_Convert_Click(object sender, EventArgs e)
    {
        ConvertToTRConfig();
        ConvertToTRModel();
        PG_Converted.SelectedObject = Result;
    }

    private void ConvertToTRSkeleton()
    {
        Bone8[] skeleton = SWSHModel.GFBModel.Skeleton;

        // TODO:
        // Result.Skeleton.SomeTypeEnum_023;
        // Result.Skeleton.BoneParams[];
        // BoneParams bone matrix is maya’s transform matrix inverted
        // Result.Skeleton.Iks[];

        // TODO: Probably need to ignore bones @ shape.BoneId;

        var transformNodes = new List<TransformNode>();
        int rigStart = int.MaxValue;
        int rigEnd = int.MaxValue;
        for (int i = 0; i < skeleton.Length; ++i)
        {
            var bone8 = skeleton[i];

            if (bone8.Type == BoneType.Transparency_Group)
                continue;

            // TODO:
            // bone8.Effect;
            // bone8.Visible;
            // bone8.IsSkin;

            transformNodes.Add(new()
            {
                Name = bone8.Name,
                Transform = new Transform { Scale = bone8.VecScale, Rotate = bone8.VecRot, Translate = bone8.VecTranslate },
                ScalePivot = bone8.VecScalePivot,
                RotatePivot = bone8.VecRotatePivot,
                ParentIdx = bone8.Parent, // TODO: If some are removed, this id needs to be corrected
                RigIdx = (i >= rigStart && i <= rigEnd ? (i - rigStart) : -1), // TODO
                LocatorBone = string.Empty,
                Type = (NodeType)bone8.Type,
            });

            if (bone8.Name.Equals("origin", StringComparison.InvariantCultureIgnoreCase))
            {
                rigStart = i + 1;
            }

            if (bone8.Name.Equals("tail", StringComparison.InvariantCultureIgnoreCase))
            {
                rigEnd = i + 1;
            }
        }

        Debug.Assert(rigStart < int.MaxValue, "Couldn't find 'origin' bone in this rig.");
        //Debug.Assert(rigEnd < int.MaxValue, "Couldn't find 'tail' bone in this rig.");

        Result.Skeleton.RigOffset = rigStart - 2; // By default we always skip the first two 2 nodes. Any additional offset should be marked.
        Result.Skeleton.Bones = transformNodes.ToArray();
        //Result.Skeleton.BoneParams = new Bone[rigEnd];
    }

    private VertexLayoutType ConvertVertexLayoutType(VertexAttribute8 attribute)
    {
        var result = (attribute.Format, attribute.Count) switch
        {
            (DataType8.UByte, 4) => VertexLayoutType.W8_X8_Y8_Z8_Unsigned,
            (DataType8.HalfFloat, 4) => VertexLayoutType.W16_X16_Y16_Z16_Float,
            //(DataType8.FixedPoint, 4) => VertexLayoutType.W16_X16_Y16_Z16_Signed_Normalized,
            (DataType8.Float, 4) => VertexLayoutType.W32_X32_Y32_Z32_Float,
            (DataType8.FixedPoint, 4) => VertexLayoutType.R8_G8_B8_A8_Unsigned_Normalized,
            (DataType8.Float, 2) => VertexLayoutType.X32_Y32_Float,
            (DataType8.Float, 3) => VertexLayoutType.X32_Y32_Z32_Float,
            _ => VertexLayoutType.None
        };

        Debug.Assert(result != VertexLayoutType.None, "Error: Conversion resulted in VertexLayoutType.None!");
        return result;
    }
    private VertexAttributeIndex ConvertVertexLayoutSlot(VertexAttribute8 attribute)
    {
        var result = attribute.Type switch
        {
            Attribute8.Position => VertexAttributeIndex.POSITION,
            Attribute8.Normal => VertexAttributeIndex.NORMAL,
            Attribute8.Tangent => VertexAttributeIndex.TANGENT,
            Attribute8.Texcoord_0 => VertexAttributeIndex.TEX_COORD,
            Attribute8.Texcoord_1 => VertexAttributeIndex.TEX_COORD,
            Attribute8.Texcoord_2 => VertexAttributeIndex.TEX_COORD,
            Attribute8.Texcoord_3 => VertexAttributeIndex.TEX_COORD,

            Attribute8.Color_0 => VertexAttributeIndex.COLOR,
            Attribute8.Color_1 => VertexAttributeIndex.COLOR,
            Attribute8.Color_2 => VertexAttributeIndex.COLOR,
            Attribute8.Color_3 => VertexAttributeIndex.COLOR,

            Attribute8.Group_Idx => VertexAttributeIndex.BLEND_INDEX,
            Attribute8.Group_Weight => VertexAttributeIndex.BLEND_WEIGHTS,

            _ => VertexAttributeIndex.NONE
        };

        Debug.Assert(result != VertexAttributeIndex.NONE, "Error: Conversion resulted in VertexAttributeIndex.NONE!");
        return result;
    }

    private uint SizeOfVertexLayoutType(VertexLayoutType type)
    {
        var result = type switch
        {
            VertexLayoutType.W8_X8_Y8_Z8_Unsigned => 4u,
            VertexLayoutType.W16_X16_Y16_Z16_Float => 8u,
            VertexLayoutType.W16_X16_Y16_Z16_Signed_Normalized => 8u,
            VertexLayoutType.W32_X32_Y32_Z32_Float => 16u,
            VertexLayoutType.R8_G8_B8_A8_Unsigned_Normalized => 4u,
            VertexLayoutType.X32_Y32_Float => 8u,
            VertexLayoutType.X32_Y32_Z32_Float => 12u,

            _ => 0u
        };

        Debug.Assert(result != 0u, $"Error: Size of {type} resulted in '0'!");
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
        for (int i = 0; i < shapes.Length; ++i)
        {
            var shape = shapes[i];
            var buffer = buffers[shape.ShapeId];
            var bone = bones[shape.BoneId];
            var subMeshName = bone.Name;

            subMeshName = subMeshName.Replace(sourceFileName, "", StringComparison.InvariantCultureIgnoreCase);
            subMeshName = subMeshName.Replace("skin", "", StringComparison.InvariantCultureIgnoreCase);
            subMeshName = subMeshName.ToLowerInvariant();

            // TODO:
            // shape.SortPriority;
            // buffer.Attributes; Proper conversion
            // buffer.Polygons;
            // buffer.Vertices;

            uint layoutOffset = 0;
            var vertexAttributes = new List<VertexAttribute>();
            foreach (var attribute in buffer.Attributes)
            {
                var type = ConvertVertexLayoutType(attribute);

                vertexAttributes.Add(new VertexAttribute
                {
                    Type = (uint)type,
                    Attribute = ConvertVertexLayoutSlot(attribute),
                    AttributeLayer = 0, // TODO
                    Offset = layoutOffset,
                });

                layoutOffset += SizeOfVertexLayoutType(type);
            }

            var subMeshes = new List<SubMesh>();
            uint currentOffset = 0;
            foreach (var subMesh in buffer.Polygons)
            {
                var mat = materials[subMesh.MaterialId];
                subMeshes.Add(new SubMesh
                {
                    PolyCount = (uint)subMesh.Tris.Length,
                    PolyOffset = currentOffset,
                    MaterialName = mat.Name, // TODO
                });

                currentOffset += (uint)subMesh.Tris.Length;
                // TODO: MeshBuffer.IndexBuffer.Data =  subMesh.Tris;
            }

            var bounds = shape.PackedAabb / 100;
            meshShapes.Add(new MeshShape
            {
                MeshShapeName = $"{resultFileName}_{subMeshName}_mesh_shape", // TODO: Use in MMT
                MeshName = $"{resultFileName}_{subMeshName}_mesh",
                Attributes = new VertexAttributeLayout[]
                {
                    new()
                    {
                        Attrs = vertexAttributes.ToArray(),
                        Size = new VertexSize[] { new(){ Size = layoutOffset } },
                    }
                },
                BoundingSphere = new PackedSphere(bounds),
                Bounds = bounds,
                IndexLayoutType = IndexLayoutType.IndexLayoutType_Uint16, // Always Uint16 on PLA all models
                Weights = new BoneWeights[] { }, // TODO
                SubMeshes = subMeshes.ToArray(),
            });

            meshBuffers.Add(new MeshBuffer
            {
                IndexBuffer = new ByteBuffer[] { new() { Data = new byte[] { } } }, // TODO
                VertexBuffer = new ByteBuffer[] { new() { Data = new byte[] { } } }, // TODO
            });
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
        Shape[] shapes = SWSHModel.GFBModel.Shapes;
        for (int i = 0; i < shapes.Length; ++i)
        {
            var shape = shapes[i];
        }

        Result.MeshDataBuffers = new TRMeshBuffer[1];

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
        // SWSHModel.Config.SizeIndex;
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


        Result.Config.InframeVerticalRotYOrigin = SWSHModel.Config.InframeVerticalRotYOrigin / 100;
        Result.Config.InframeBottomYOffset = SWSHModel.Config.InframeBottomYOffset / 100;
        Result.Config.InframeCenterYOffset = SWSHModel.Config.InframeCenterYOffset / 100;
        Result.Config.InframeLeftRotation = SWSHModel.Config.InframeLeftRotation;
        Result.Config.InframeRightRotation = SWSHModel.Config.InframeRightRotation;

        // TODO:
        // Result.Config.SomeTypeEnum_023;
        // Result.Config.RandOffsetScaleMin;
        // Result.Config.RandOffsetScaleMax;
        // Result.Config.BaseScale;
        // Result.Config.BaseX;
        // Result.Config.RandOffsetYMin;
        // Result.Config.RandOffsetYMax;
        // Result.Config.BaseY;
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
