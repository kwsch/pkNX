using System.Diagnostics;

// poke_resource_table.trpmcatalog

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary> <see cref="PokeResourceTable"/> for Legends: Arceus, adding <see cref="PokeModelConfig.ArceusType"/></summary>
public partial class PokeResourceTable
{
    public PokeModelConfig GetEntry(ushort species, ushort form, byte gender)
    {
        return Table.FirstOrDefault(x => x.SpeciesInfo.Species == species && x.SpeciesInfo.Form == form && x.SpeciesInfo.Gender == gender) ?? new()
        {
            Animations = [],
            SpeciesInfo = new PokeModelSpeciesInfo(),
            ModelPath = string.Empty,
            MaterialTablePath = string.Empty,
            ConfigPath = string.Empty,
            Effects = [],
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

            Animations = [
                new(){
                    Name = "base",
                    Path = $"{basePath}/anm/{pmStr}_base.tracn",
                },
            ],

            Effects = [
                new() {
                    Name = "eff",
                    Path = $"{basePath}/locators/{pmStr}_eff.trskl",
                },
            ],
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.SpeciesInfo.Species)
            .ThenBy(x => x.SpeciesInfo.Form)
            .ThenBy(x => x.SpeciesInfo.Gender)
            .ToArray();
        return entry;
    }
}
