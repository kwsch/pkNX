using System;
using static pkNX.Structures.Species;

namespace pkNX.Structures;

/// <summary>
/// Personal Table storing <see cref="PersonalInfo5B2W2"/> used in Generation 5 games.
/// </summary>
public sealed class PersonalTable5B2W2 : IPersonalTable, IPersonalTable<PersonalInfo5B2W2>
{
    public PersonalInfo5B2W2[] Table { get; }
    private const int SIZE = PersonalInfo5B2W2.SIZE;
    private const int MaxSpecies = 649;
    public int MaxSpeciesID => MaxSpecies;

    public PersonalTable5B2W2(ReadOnlySpan<byte> data)
    {
        Table = new PersonalInfo5B2W2[data.Length / SIZE];
        var count = data.Length / SIZE;
        for (int i = 0, ofs = 0; i < count; i++, ofs += SIZE)
        {
            var slice = data.Slice(ofs, SIZE).ToArray();
            Table[i] = new PersonalInfo5B2W2(slice);
        }
    }

    public PersonalInfo5B2W2 this[int index] => Table[(uint)index < Table.Length ? index : 0];
    public PersonalInfo5B2W2 this[ushort species, byte form] => Table[GetFormIndex(species, form)];
    public PersonalInfo5B2W2 GetFormEntry(ushort species, byte form) => Table[GetFormIndex(species, form)];

    public int GetFormIndex(ushort species, byte form)
    {
        if ((uint)species <= MaxSpecies)
            return Table[species].FormIndex(species, form);
        return 0;
    }

    public bool IsSpeciesInGame(ushort species) => (uint)species <= MaxSpecies;
    public bool IsPresentInGame(ushort species, byte form)
    {
        if (!IsSpeciesInGame(species))
            return false;
        if (form == 0)
            return true;
        if (Table[species].HasForm(form))
            return true;
        return species switch
        {
            (int)Unown => form < 28,
            (int)Burmy => form < 3,
            (int)Mothim => form < 3,
            (int)Cherrim => form == 1,
            (int)Shellos or (int)Gastrodon => form == 1,
            (int)Arceus => form < 17,
            (int)Deerling or (int)Sawsbuck => form < 4,
            (int)Genesect => form <= 4,
            _ => false,
        };
    }

    IPersonalInfo[] IPersonalTable.Table => Table;
    IPersonalInfo IPersonalTable.this[int index] => this[index];
    IPersonalInfo IPersonalTable.this[ushort species, byte form] => this[species, form];
    IPersonalInfo IPersonalTable.GetFormEntry(ushort species, byte form) => GetFormEntry(species, form);
}
