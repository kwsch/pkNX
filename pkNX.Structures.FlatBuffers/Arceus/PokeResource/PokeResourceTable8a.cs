using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

// poke_resource_table.trpmcatalog
/// <summary> <see cref="PokeResourceTable"/> for Legends: Arceus, adding <see cref="PokeModelConfig8a.ArceusType"/></summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeResourceTable8a : IFlatBufferArchive<PokeModelConfig8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    public PokeModelConfig8a GetEntry(ushort species, ushort form, byte gender)
    {
        return Table.FirstOrDefault(x => x.Meta.Species == species && x.Meta.Form == form && x.Meta.Gender == gender) ??
            new PokeModelConfig8a { };
    }

    public bool HasEntry(ushort species, ushort form, byte gender)
    {
        return Table.Any(x => x.Meta.Species == species && x.Meta.Form == form && x.Meta.Gender == gender);
    }

    public PokeModelConfig8a AddEntry(ushort species, ushort form, byte gender)
    {
        Debug.Assert(!HasEntry(species, form, gender), "The resource info table already contains an entry for the same species!");

        string pmStr = $"pm{species:0000}_{gender:00}_{form:00}";
        string basePath = $"bin/pokemon/pm{species:0000}/{pmStr}";
        var entry = new PokeModelConfig8a
        {
            Meta = new() { Species = species, Form = form, Gender = gender },
            PathModel = $"{basePath}/mdl/{pmStr}.trmdl",
            PathConfig = $"{basePath}/mdl/{pmStr}.trmmt",
            PathArchive = $"{basePath}/{pmStr}.trpokecfg",

            Field_05 = new AnimationConfigStringTuple8a[] {
                new(){
                    Name = "base",
                    Path = $"{basePath}/anm/{pmStr}_base.tracn",
                }
            },

            Field_06 = new AnimationConfigStringTuple8a[] {
                new() {
                    Name = "eff",
                    Path = $"{basePath}/locators/{pmStr}_eff.trskl",
                }
            },
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.Meta.Species)
            .ThenBy(x => x.Meta.Form)
            .ThenBy(x => x.Meta.Gender)
            .ToArray();
        return entry;
    }

    [FlatBufferItem(00)] public PokeResourceMeta8a Meta { get; set; } = new();
    [FlatBufferItem(01)] public PokeModelConfig8a[] Table { get; set; } = Array.Empty<PokeModelConfig8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeResourceMeta8a
{
    [FlatBufferItem(00)] public int Field0 { get; set; } = 5; // 4 in prior game format
    [FlatBufferItem(01)] public int Field1 { get; set; } = 4; // 2 in prior game format
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelConfig8a
{
    [FlatBufferItem(00)] public PokeModelMeta8a Meta { get; set; } = new();
    [FlatBufferItem(01)] public string PathModel { get; set; } = string.Empty; // string
    [FlatBufferItem(02)] public string PathConfig { get; set; } = string.Empty; // string
    [FlatBufferItem(03)] public string PathArchive { get; set; } = string.Empty; // string
    [FlatBufferItem(04)] public byte Unused { get; set; } // unused!
    [FlatBufferItem(05)] public AnimationConfigStringTuple8a[] Field_05 { get; set; } = Array.Empty<AnimationConfigStringTuple8a>();
    [FlatBufferItem(06)] public AnimationConfigStringTuple8a[] Field_06 { get; set; } = Array.Empty<AnimationConfigStringTuple8a>();
    [FlatBufferItem(07)] public int? ArceusType { get; set; } // Specified by Arceus' forms -- NEW!
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelMeta8a
{
    [FlatBufferItem(00)] public ushort Species { get; set; }
    [FlatBufferItem(01)] public ushort Form { get; set; }
    [FlatBufferItem(02)] public byte Gender { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AnimationConfigStringTuple8a
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Path { get; set; } = string.Empty;
}
