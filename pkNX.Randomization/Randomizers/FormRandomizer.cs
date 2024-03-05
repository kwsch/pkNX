using pkNX.Structures;
using System;
using static pkNX.Structures.Species;

namespace pkNX.Randomization;

public class FormRandomizer(IPersonalTable t)
{
    public int GetRandomForm(int species, bool mega, bool fuse, int generation, IPersonalInfo[]? stats = null)
    {
        stats ??= t.Table;
        if (stats[species].FormCount <= 1)
            return 0;

        if (stats.Length ==  980 && species is (int)Pikachu or (int)Eevee) // ban LGPE starters, they crash trainer battles
            return 0;
        if (stats.Length == 1182 && species is (int)Rayquaza) // Rayquaza form count error in SWSH
            return 0;

        // ban Zen Mode and Galarian Zen Mode
        if (species is (int)Darmanitan && generation >= 8)
        {
            int form = Util.Random.Next(stats[species].FormCount);
            return form & 2;
        }

        return (Species)species switch
        {
            Pikachu or Slowbro when generation >= 8 => GetValidForm(species, generation, t),
            Unown or Deerling or Sawsbuck => 31, // pure random -- todo sv see if this behavior changed
            Greninja when !mega => 0, // treat Ash-Greninja as a Mega
            Scatterbug or Spewpa or Vivillon => 30, // save file specific -- todo sv see if this behavior changed
            Zygarde when generation >= 7 => Util.Random.Next(4), // skip Complete Forme
            Minior => Util.Random.Next(7), // skip Core Forms

            _ when !mega && Legal.BattleMegas.Contains(species) => 0,
            _ when !fuse && Legal.BattleFusions.Contains(species) => 0,
            _ when Legal.BattleForms.Contains(species) => 0,
            _ => Util.Random.Next(stats[species].FormCount),
        };
    }

    private static int GetValidForm(int species, int generation, IPersonalTable stats)
    {
        int form = Util.Random.Next(stats[species].FormCount - 1);
        int banned = GetInvalidForm(species, generation);
        if (form == banned)
            form++;
        return form;
    }

    private static int GetInvalidForm(int species, int generation) => species switch
    {
        (int)Pikachu when generation >= 8 => 8, // LGPE Partner Pikachu
        (int)Slowbro when generation >= 8 => 1, // Mega Slowbro
        _ => throw new ArgumentOutOfRangeException(nameof(species)),
    };
}
