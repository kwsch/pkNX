using System;
using System.ComponentModel;
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
        return Array.Find(Table, x => x.SpeciesInfo.Species == species && x.SpeciesInfo.Form == form && x.SpeciesInfo.Gender == gender) ??
            new PokeModelConfig8a();
    }

    public bool HasEntry(ushort species, ushort form, byte gender)
    {
        return Table.Any(x => x.SpeciesInfo.Species == species && x.SpeciesInfo.Form == form && x.SpeciesInfo.Gender == gender);
    }

    public PokeModelConfig8a AddEntry(ushort species, ushort form, byte gender)
    {
        Debug.Assert(!HasEntry(species, form, gender), "The resource info table already contains an entry for the same species!");

        string pmStr = $"pm{species:0000}_{gender:00}_{form:00}";
        string basePath = $"bin/pokemon/pm{species:0000}/{pmStr}";
        var entry = new PokeModelConfig8a
        {
            SpeciesInfo = new() { Species = species, Form = form, Gender = gender },
            ModelPath = $"{basePath}/mdl/{pmStr}.trmdl",
            MaterialTablePath = $"{basePath}/mdl/{pmStr}.trmmt",
            ConfigPath = $"{basePath}/{pmStr}.trpokecfg",

            Animations = new FileReference8a[] {
                new(){
                    Name = "base",
                    Path = $"{basePath}/anm/{pmStr}_base.tracn",
                }
            },

            Effects = new FileReference8a[] {
                new() {
                    Name = "eff",
                    Path = $"{basePath}/locators/{pmStr}_eff.trskl",
                }
            },
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.SpeciesInfo.Species)
            .ThenBy(x => x.SpeciesInfo.Form)
            .ThenBy(x => x.SpeciesInfo.Gender)
            .ToArray();
        return entry;
    }

    [FlatBufferItem(00)] public PokeResourceVersionInfo8a Version { get; set; } = new();
    [FlatBufferItem(01)] public PokeModelConfig8a[] Table { get; set; } = Array.Empty<PokeModelConfig8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeResourceVersionInfo8a
{
    [FlatBufferItem(00)] public int Major { get; set; } = 5; // 4 in prior game format
    [FlatBufferItem(01)] public int Minor { get; set; } = 4; // 2 in prior game format
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelConfig8a
{
    [FlatBufferItem(00)] public PokeModelSpeciesInfo8a SpeciesInfo { get; set; } = new();
    [FlatBufferItem(01)] public string ModelPath { get; set; } = string.Empty; // string
    [FlatBufferItem(02)] public string MaterialTablePath { get; set; } = string.Empty; // string
    [FlatBufferItem(03)] public string ConfigPath { get; set; } = string.Empty; // string
    [FlatBufferItem(04)] public byte Unused { get; set; } // unused!
    [FlatBufferItem(05)] public FileReference8a[] Animations { get; set; } = Array.Empty<FileReference8a>();
    [FlatBufferItem(06)] public FileReference8a[] Effects { get; set; } = Array.Empty<FileReference8a>();
    [FlatBufferItem(07)] public int? ArceusType { get; set; } // Specified by Arceus' forms -- NEW!
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeModelSpeciesInfo8a
{
    [FlatBufferItem(00)] public ushort Species { get; set; }
    [FlatBufferItem(01)] public ushort Form { get; set; }
    [FlatBufferItem(02)] public byte Gender { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FileReference8a
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Path { get; set; } = string.Empty;
}
