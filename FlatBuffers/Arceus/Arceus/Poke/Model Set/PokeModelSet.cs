using System.Diagnostics;

namespace pkNX.Structures.FlatBuffers.Arceus;

// resident_release.bin -> bin/field/param/placement/common/pokemon_model_set.bin
public partial class PokeModelSet
{
    public PokeModelSetEntry GetEntry(ushort species)
    {
        return Table.FirstOrDefault(z => z.Species == species) ?? new ()
        {
            VariantDesc = string.Empty,
        };
    }

    public bool HasEntry(ushort species)
    {
        return Table.Any(x => x.Species == species);
    }

    public PokeModelSetEntry AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species), "The resource info table already contains an entry for the same species!");

        var entry = new PokeModelSetEntry
        {
            Species = species,
            Form = form,
            VariantDesc = string.Empty,
            PokeModelHash = FnvHash.HashFnv1a_64(""),
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
