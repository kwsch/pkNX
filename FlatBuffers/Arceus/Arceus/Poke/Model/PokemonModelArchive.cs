using pkNX.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers.Arceus;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshMaterialWrapper
{
    [FlatBufferItem(0)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(1)] public Material[] Materials { get; set; } = [];
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonModelArchive
{
    public GFPack SourceArchive { get; set; }

    public PokeConfig Config { get; set; }
    public MultiMaterialTable TRMMT { get; set; }
    public Model TRModel { get; set; }
    public Mesh[] Meshes { get; set; }
    public MeshBufferTable[] MeshDataBuffers { get; set; }
    public Material[] DefaultMaterials { get; set; }
    public MeshMaterialWrapper[] MeshMaterials { get; set; }
    public Skeleton Skeleton { get; set; }

    public string[] UsedTextures { get; set; }

    public PokemonModelArchive(GFPack sourceArchive)
    {
        SourceArchive = sourceArchive;

        string FileName = Path.GetFileNameWithoutExtension(sourceArchive.FilePath);
        int SpeciesId = int.Parse(FileName.Substring(2, 4));

        string BasePath = $"bin/pokemon/pm{SpeciesId:0000}/{FileName}/";
        string ModelPath = BasePath + "mdl/";
        string AnimationsPath = BasePath + "anm/";

        Config = FlatBufferConverter.DeserializeFrom<PokeConfig>(SourceArchive.GetDataFullPath(BasePath + $"{FileName}.trpokecfg"));
        TRMMT = FlatBufferConverter.DeserializeFrom<MultiMaterialTable>(SourceArchive.GetDataFullPath(ModelPath + $"{FileName}.trmmt"));
        TRModel = FlatBufferConverter.DeserializeFrom<Model>(SourceArchive.GetDataFullPath(ModelPath + $"{FileName}.trmdl"));
        Skeleton = FlatBufferConverter.DeserializeFrom<Skeleton>(SourceArchive.GetDataFullPath(ModelPath + $"{TRModel.Skeleton.Filename}"));

        DefaultMaterials = TRModel.Materials
            .Select(x => FlatBufferConverter.DeserializeFrom<Material>(SourceArchive.GetDataFullPath(ModelPath + $"{x}")))
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
                    fileName => FlatBufferConverter.DeserializeFrom<Material>(SourceArchive.GetDataFullPath(ModelPath + $"{fileName}"))
                ).ToArray(),
            }
        ).ToArray();

        Meshes = TRModel.Meshes
            .Select(x => FlatBufferConverter.DeserializeFrom<Mesh>(SourceArchive.GetDataFullPath(ModelPath + $"{x.Filename}")))
            .ToArray();

        MeshDataBuffers = Meshes.Select(x => x.BufferFileName)
            .Select(x => FlatBufferConverter.DeserializeFrom<MeshBufferTable>(SourceArchive.GetDataFullPath(ModelPath + $"{x}")))
            .ToArray();
    }
}
