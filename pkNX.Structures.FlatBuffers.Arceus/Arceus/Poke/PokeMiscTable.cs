using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeMisc { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeMiscTable
{
    public PokeMisc GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(z => z.Species == species && z.Form == form) ?? new()
        {
            Value = $"{species}-{form} is not in {nameof(PokeMiscTable)}.",
            Field10 = Array.Empty<uint>(),
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
            Field10 = Array.Empty<uint>(),
        };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
