using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

// poke_resource_table.trpmcatalog

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeResourceMeta { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeModelConfig { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeModelSpeciesInfo { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FileNamePathPair { }

/// <summary> <see cref="PokeResourceTable"/> for Legends: Arceus, adding <see cref="PokeModelConfig.ArceusType"/></summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeResourceTable
{
    public PokeModelConfig GetEntry(ushort species, ushort form, byte gender)
    {
        return Table.FirstOrDefault(x => x.SpeciesInfo.Species == species && x.SpeciesInfo.Form == form && x.SpeciesInfo.Gender == gender) ?? new()
        {
            Animations = Array.Empty<FileNamePathPair>(),
            SpeciesInfo = new PokeModelSpeciesInfo(),
            ModelPath = string.Empty,
            MaterialTablePath = string.Empty,
            ConfigPath = string.Empty,
            Effects = Array.Empty<FileNamePathPair>(),
        };
    }

    public bool HasEntry(ushort species, ushort form, byte gender)
    {
        return Table.Any(x => x.SpeciesInfo.Species == species && x.SpeciesInfo.Form == form && x.SpeciesInfo.Gender == gender);
    }

    public PokeModelConfig AddEntry(ushort species, ushort form, byte gender)
    {
        Debug.Assert(!HasEntry(species, form, gender), "The resource info table already contains an entry for the same species!");

        string pmStr = $"pm{species:0000}_{gender:00}_{form:00}";
        string basePath = $"bin/pokemon/pm{species:0000}/{pmStr}";
        var entry = new PokeModelConfig
        {
            SpeciesInfo = new()
            {
                Species = species,
                Form = form,
                Gender = gender,
            },
            ModelPath = $"{basePath}/mdl/{pmStr}.trmdl",
            MaterialTablePath = $"{basePath}/mdl/{pmStr}.trmmt",
            ConfigPath = $"{basePath}/{pmStr}.trpokecfg",

            Animations = new FileNamePathPair[] {
                new(){
                    Name = "base",
                    Path = $"{basePath}/anm/{pmStr}_base.tracn",
                },
            },

            Effects = new FileNamePathPair[] {
                new() {
                    Name = "eff",
                    Path = $"{basePath}/locators/{pmStr}_eff.trskl",
                },
            },
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.SpeciesInfo.Species)
            .ThenBy(x => x.SpeciesInfo.Form)
            .ThenBy(x => x.SpeciesInfo.Gender)
            .ToArray();
        return entry;
    }
}
