using System.Diagnostics;

namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PokeMisc
{
    // lazy set reference for remembering the fetched object
    public PokeDropItem? DropTable { get; set; }
    public PokeDropItem? AlphaDropTable { get; set; }
}

public partial class PokeMiscTable
{
    public PokeMisc GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(z => z.Species == species && z.Form == form) ?? new()
        {
            Value = $"{species}-{form} is not in {nameof(PokeMiscTable)}.",
            Field10 = [],
        };
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public PokeMisc AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The misc table already contains an entry for the same species + form!");

        var entry = new PokeMisc
        {
            Value = $"{species}-{form} is not in {nameof(PokeMiscTable)}.",
            Species = species,
            Form = form,
            Field10 = [],
        };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
