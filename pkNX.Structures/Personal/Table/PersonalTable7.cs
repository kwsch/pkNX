using System;
using static pkNX.Structures.Species;

namespace pkNX.Structures;

/// <summary>
/// Personal Table storing <see cref="PersonalInfo7SM"/> used in Generation 7 games.
/// </summary>
public sealed class PersonalTable7SM : IPersonalTable, IPersonalTable<PersonalInfo7SM>
{
    public PersonalInfo7SM[] Table { get; }
    private const int SIZE = PersonalInfo7SM.SIZE;
    public int MaxSpeciesID { get; }

    public PersonalTable7SM(ReadOnlySpan<byte> data, int maxSpecies)
    {
        MaxSpeciesID = maxSpecies;
        Table = new PersonalInfo7SM[data.Length / SIZE];
        var count = data.Length / SIZE;
        for (int i = 0, ofs = 0; i < count; i++, ofs += SIZE)
        {
            var slice = data.Slice(ofs, SIZE).ToArray();
            Table[i] = new PersonalInfo7SM(slice);
        }
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public PersonalInfo7SM this[int index] => Table[(uint)index < Table.Length ? index : 0];
    public PersonalInfo7SM this[ushort species, byte form] => Table[GetFormIndex(species, form)];
    public PersonalInfo7SM GetFormEntry(ushort species, byte form) => Table[GetFormIndex(species, form)];

    public int GetFormIndex(ushort species, byte form)
    {
        if ((uint)species <= MaxSpeciesID)
            return Table[species].FormIndex(species, form);
        return 0;
    }

    public bool IsSpeciesInGame(ushort species) => (uint)species <= MaxSpeciesID;
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
            (int)Arceus => form < 18,
            (int)Genesect => form <= 4,
            (int)Scatterbug or (int)Spewpa => form < 18,
            (int)Vivillon => form < 20,
            (int)Deerling or (int)Sawsbuck => form < 4,
            (int)Flabébé or (int)Florges => form < 5,
            (int)Floette => form < 6,
            (int)Xerneas => form == 1,
            (int)Silvally => form < 18,
            _ => false,
        };
    }

    IPersonalInfo[] IPersonalTable.Table => Table;
    IPersonalInfo IPersonalTable.this[int index] => this[index];
    IPersonalInfo IPersonalTable.this[ushort species, byte form] => this[species, form];
    IPersonalInfo IPersonalTable.GetFormEntry(ushort species, byte form) => GetFormEntry(species, form);
}
