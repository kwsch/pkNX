using pkNX.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeshMaterialWrapper
{
    [FlatBufferItem(0)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(1)] public TRMaterial[] Materials { get; set; } = Array.Empty<TRMaterial>();
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonModelArchive
{
    public GFPack SourceArchive { get; set; }

    public TRPokeConfig Config { get; set; }
    public TRMultiMaterialTable TRMMT { get; set; }
    public TRModel TRModel { get; set; }
    public TRMesh[] Meshes { get; set; } = Array.Empty<TRMesh>();
    public TRMeshBuffer[] MeshDataBuffers { get; set; } = Array.Empty<TRMeshBuffer>();
    public TRMaterial[] DefaultMaterials { get; set; } = Array.Empty<TRMaterial>();
    public MeshMaterialWrapper[] MeshMaterials { get; set; } = Array.Empty<MeshMaterialWrapper>();
    public TRSkeleton Skeleton { get; set; } = new();

    public string[] UsedTextures { get; set; } = Array.Empty<string>();

    public PokemonModelArchive(GFPack sourceArchive)
    {
        SourceArchive = sourceArchive;

        string FileName = Path.GetFileNameWithoutExtension(sourceArchive.FilePath);
        int SpeciesId = int.Parse(FileName.Substring(2, 4));

        string BasePath = $"bin/pokemon/pm{SpeciesId:0000}/{FileName}/";
        string ModelPath = BasePath + "mdl/";
        string AnimationsPath = BasePath + "anm/";

        Config = FlatBufferConverter.DeserializeFrom<TRPokeConfig>(SourceArchive.GetDataFullPath(BasePath + $"{FileName}.trpokecfg"));
        TRMMT = FlatBufferConverter.DeserializeFrom<TRMultiMaterialTable>(SourceArchive.GetDataFullPath(ModelPath + $"{FileName}.trmmt"));
        TRModel = FlatBufferConverter.DeserializeFrom<TRModel>(SourceArchive.GetDataFullPath(ModelPath + $"{FileName}.trmdl"));
        Skeleton = FlatBufferConverter.DeserializeFrom<TRSkeleton>(SourceArchive.GetDataFullPath(ModelPath + $"{TRModel.Skeleton.Filename}"));

        DefaultMaterials = TRModel.Materials
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMaterial>(SourceArchive.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();

        UsedTextures = DefaultMaterials.SelectMany(material =>
            material.MaterialPasses.SelectMany(pass =>
                pass.TextureParameters.Select(texture => texture.TextureFile)
            )
        ).ToHashSet().ToArray();

        MeshMaterials = TRMMT.Material.Select(
            x => new MeshMaterialWrapper
            {
                Name = x.Name,
                Materials = x.FileNames.Select(
                    fileName => FlatBufferConverter.DeserializeFrom<TRMaterial>(SourceArchive.GetDataFullPath(ModelPath + $"{fileName}"))
                ).ToArray()
            }
        ).ToArray();

        Meshes = TRModel.Meshes
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMesh>(SourceArchive.GetDataFullPath(ModelPath + $"{x.Filename}")))
            .ToArray();

        MeshDataBuffers = Meshes.Select(x => x.BufferFileName)
            .Select(x => FlatBufferConverter.DeserializeFrom<TRMeshBuffer>(SourceArchive.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();
    }
}
