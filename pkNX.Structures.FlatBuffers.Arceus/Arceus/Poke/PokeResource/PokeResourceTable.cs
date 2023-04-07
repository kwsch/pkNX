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
public partial class PokeModelMeta { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AnimationConfigStringTuple { }

/// <summary> <see cref="PokeResourceTable"/> for Legends: Arceus, adding <see cref="PokeModelConfig.ArceusType"/></summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeResourceTable
{
    public PokeModelConfig GetEntry(ushort species, ushort form, byte gender)
    {
        return Table.FirstOrDefault(x => x.Meta.Species == species && x.Meta.Form == form && x.Meta.Gender == gender) ?? new()
        {
            Animations = Array.Empty<AnimationConfigStringTuple>(),
            Meta = new PokeModelMeta(),
            PathModel = string.Empty,
            PathMeshMaterial = string.Empty,
            PathConfig = string.Empty,
            Effects = Array.Empty<AnimationConfigStringTuple>(),
        };
    }

    public bool HasEntry(ushort species, ushort form, byte gender)
    {
        return Table.Any(x => x.Meta.Species == species && x.Meta.Form == form && x.Meta.Gender == gender);
    }

    public PokeModelConfig AddEntry(ushort species, ushort form, byte gender)
    {
        Debug.Assert(!HasEntry(species, form, gender), "The resource info table already contains an entry for the same species!");

        string pmStr = $"pm{species:0000}_{gender:00}_{form:00}";
        string basePath = $"bin/pokemon/pm{species:0000}/{pmStr}";
        var entry = new PokeModelConfig
        {
            Meta = new()
            {
                Species = species,
                Form = form,
                Gender = gender,

            },
            PathModel = $"{basePath}/mdl/{pmStr}.trmdl",
            PathMeshMaterial = $"{basePath}/mdl/{pmStr}.trmmt",
            PathConfig = $"{basePath}/{pmStr}.trpokecfg",

            Animations = new AnimationConfigStringTuple[] {
                new(){
                    Name = "base",
                    Path = $"{basePath}/anm/{pmStr}_base.tracn",
                }
            },

            Effects = new AnimationConfigStringTuple[] {
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
}
