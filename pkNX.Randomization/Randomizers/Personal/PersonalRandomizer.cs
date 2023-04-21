using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization;

public class PersonalRandomizer : Randomizer
{
    private const int tmcount = 100;
    private const int eggGroupCount = 16;
    private const int TypeCount = 18;

    private readonly GameInfo Game;
    private readonly IPersonalTable Table;
    private readonly EvolutionSet[] Evolutions;

    public PersonalRandSettings Settings { get; set; } = new();

    public PersonalRandomizer(IPersonalTable table, GameInfo game, EvolutionSet[] evolutions)
    {
        Game = game;
        Table = table;
        Evolutions = evolutions;

        const string bannedExternalFile = "bannedabilites.txt";
        if (File.Exists(bannedExternalFile))
        {
            var list = new List<int>(BannedAbilities);
            var data = File.ReadLines(bannedExternalFile);
            list.AddRange(data.Select(z => Convert.ToInt32(z)));
            BannedAbilities = list;
        }
    }

    public override void Execute()
    {
        if (Settings.ModifyByEvolutions)
            RandomizeChains();
        else
            RandomizeAllSpecies();
    }

    private void RandomizeAllSpecies()
    {
        for (ushort species = 0; species <= Game.MaxSpeciesID; species++)
            RandomizeSpecies(species);
    }

    private bool[] processed = Array.Empty<bool>();

    private void RandomizeChains()
    {
        processed = new bool[Table.Table.Length];
        for (ushort species = 0; species <= Game.MaxSpeciesID; species++)
        {
            for (byte f = 0; f <= Table[species].FormCount; f++)
                RandomizeChain(species, f);
        }
    }

    private bool AlreadyProcessed(int index)
    {
        var p = processed;
        if (p.Length <= index)
            return false;
        if (p[index])
            return true;
        p[index] = true;
        return false;
    }

    private void RandomizeChain(ushort species, byte form)
    {
        var index = Table.GetFormIndex(species, form);
        if (AlreadyProcessed(index))
            return;
        processed[index] = true;

        var entry = Table[index];
        Randomize(entry, species);
        ProcessEvolutions(species, form, index);
    }

    private void ProcessEvolutions(int species, int form, int devolvedIndex)
    {
        var evoindex = GetEvolutionEntry((ushort)species, (byte)form);
        var evos = Evolutions[evoindex];

        if (evos.PossibleEvolutions.Length == 1)
        {
            var evo = evos.PossibleEvolutions[0];
            var ei = Table.GetFormIndex(evo.Species, evo.Form);

            if (AlreadyProcessed(ei))
                return;
            RandomizeSingleChain(evo, devolvedIndex);
            ProcessEvolutions(evo.Species, evo.Form, ei);
        }
        else
        {
            foreach (var evo in evos.PossibleEvolutions)
            {
                var ei = Table.GetFormIndex(evo.Species, evo.Form);
                if (AlreadyProcessed(ei))
                    return;
                RandomizeSplitChain(evo, devolvedIndex);
                ProcessEvolutions(evo.Species, evo.Form, ei);
            }
        }
    }

    private void RandomizeSingleChain(EvolutionMethod evo, int devolvedIndex)
    {
        var child = Table[devolvedIndex];
        var z = Table.GetFormEntry(evo.Species, evo.Form);
        RandomizeFrom(z, child, evo.Species);
    }

    private void RandomizeSplitChain(EvolutionMethod evo, int devolvedIndex)
    {
        var child = Table[devolvedIndex];
        var z = Table.GetFormEntry(evo.Species, evo.Form);
        RandomizeFrom(z, child, evo.Species);
    }

    private int GetEvolutionEntry(ushort species, byte form)
    {
        if (Game.Generation < 7)
            return species;
        return Table.GetFormIndex(species, form);
    }

    private void RandomizeSpecies(ushort species)
    {
        var entry = Table[species];
        Randomize(entry, species);
        var formCount = entry.FormCount;
        for (byte form = 1; form <= formCount; form++)
        {
            entry = Table.GetFormEntry(species, form);
            Randomize(entry, species);
        }
    }

    public void RandomizeFrom(IPersonalInfo z, IPersonalInfo child, int species)
    {
        if (Settings.ModifyStats)
            RandomizeStats(z);
        if (Settings.ShuffleStats)
            RandomShuffledStats(z);

        if (Settings.ModifyTypes)
        {
            if (Settings.InheritType && Settings.InheritTypeSetting != ModifyState.All)
            {
                switch (Settings.InheritTypeSetting)
                {
                    case ModifyState.Shared:
                    default:
                        z.Type1 = child.Type1;
                        z.Type2 = child.Type2;
                        break;
                    case ModifyState.Two when Rand.Next(100) < Settings.InheritTypeOnlyOneChance:
                    case ModifyState.One when Rand.Next(100) < Settings.InheritTypeOnlyOneChance:
                        switch (Rand.Next(2))
                        {
                            case 0:
                                z.Type1 = (Types)GetRandomType();
                                z.Type2 = child.Type2;
                                break;
                            case 1:
                                z.Type1 = child.Type1;
                                z.Type2 = (Types)GetRandomType();
                                break;
                        }
                        break;
                    case ModifyState.Two when Rand.Next(100) < Settings.InheritTypeNeitherChance:
                        RandomizeTypes(z);
                        break;
                }
            }
            else
            {
                RandomizeTypes(z);
            }
        }

        if (Settings.ModifyAbility)
        {
            if (Settings.InheritAbility)
            {
                Span<int> abils = stackalloc int[3];
                child.GetAbilities(abils);
                GetRandomAbilities(abils, Settings.InheritAbilitySetting);
                z.SetAbilities(abils);
            }
            else
            {
                RandomizeAbilities(z);
            }
        }

        if (z is IMovesInfo_1 mi)
        {
            if (Settings.ModifyLearnsetTM || Settings.ModifyLearnsetHM)
            {
                if (Settings.InheritChildTM)
                {
                    mi.TMHM = ((IMovesInfo_1)child).TMHM;
                    
                    if (z is IMovesInfo_SWSH mitr)
                        mitr.TR = ((IMovesInfo_SWSH)child).TR;
                }
                else
                {
                    RandomizeTMHM(mi);
                    
                    if (z is IMovesInfo_SWSH mitr)
                        RandomizeTR(mitr);
                }
            }

            if (Settings.ModifyLearnsetTypeTutors)
            {
                if (Settings.InheritChildSpecial)
                    mi.TypeTutors = ((IMovesInfo_1)child).TypeTutors;
                else
                    RandomizeTypeTutors(mi, species);
            }
        }

        if (Settings.ModifyLearnsetMoveTutors && z is IMovesInfo_2 mi2)
        {
            if (Settings.InheritChildTutor)
                mi2.SpecialTutors = ((IMovesInfo_2)child).SpecialTutors;
            else
                RandomizeSpecialTutors(mi2);
        }

        if (Settings.ModifyEgg)
        {
            z.EggGroup1 = child.EggGroup1;
            z.EggGroup2 = child.EggGroup2;
        }

        if (Settings.ModifyHeldItems)
        {
            if (Settings.InheritHeldItem)
            {
                z.Item1 = child.Item1;
                z.Item2 = child.Item2;
                z.Item3 = child.Item3;
            }
            else
            {
                RandomizeHeldItems(z);
            }
        }

        ExecuteCatchRate(z);
    }

    private void GetRandomAbilities(Span<int> abils, ModifyState setting)
    {
        switch (setting)
        {
            case ModifyState.Shared:
                return;
            case ModifyState.Two when Rand.Next(100) < Settings.InheritAbilityNeitherChance:
                GetRandomAbilities(abils, 1);
                break;
            case ModifyState.Two when Rand.Next(100) < Settings.InheritAbilityOnlyOneChance:
            case ModifyState.One when Rand.Next(100) < Settings.InheritAbilityOnlyOneChance:
                GetRandomAbilities(abils, 2);
                break;
            case ModifyState.All:
                GetRandomAbilities(abils);
                if (Rand.Next(100) < Settings.SameAbilityChance)
                {
                    int index = Rand.Next(2);
                    abils[index ^ 1] = abils[index];
                }
                break;
        }
    }

    public void Randomize(IPersonalInfo z, int species)
    {
        if (Settings.ModifyStats)
            RandomizeStats(z);
        if (Settings.ShuffleStats)
            RandomShuffledStats(z);

        if (Settings.ModifyTypes)
            RandomizeTypes(z);

        if (Settings.ModifyAbility)
            RandomizeAbilities(z);

        if (z is IMovesInfo_1 mi)
        {
            if (Settings.ModifyLearnsetTM || Settings.ModifyLearnsetHM)
            {
                RandomizeTMHM(mi);
                
                if (z is IMovesInfo_SWSH mitr)
                    RandomizeTR(mitr);
            }

            if (Settings.ModifyLearnsetTypeTutors)
                RandomizeTypeTutors(mi, species);
        }

        if (Settings.ModifyLearnsetMoveTutors && z is IMovesInfo_2 mi2)
            RandomizeSpecialTutors(mi2);

        if (Settings.ModifyEgg)
            RandomizeEggGroups(z);

        if (Settings.ModifyHeldItems)
            RandomizeHeldItems(z);

        ExecuteCatchRate(z);
    }

    private void ExecuteCatchRate(IPersonalInfo z)
    {
        if (Settings.CatchRate == CatchRate.Random)
            z.CatchRate = Rand.Next(3, 251); // Random Catch Rate between 3 and 250.
        else if (Settings.CatchRate == CatchRate.BSTScaled)
            z.CatchRate = GetBSTCatchRate(z.GetBaseStatTotal());
    }

    private static int GetBSTCatchRate(int BST)
    {
        var c = 11 * (Math.Sqrt(Math.Max(0, 600 - BST)));

        const int min = 3;
        return (int)Math.Min(255, min + c);
    }

    private void RandomizeTMHM(IMovesInfo_1 z)
    {
        var tms = z.TMHM;

        if (Settings.ModifyLearnsetTM)
        {
            for (int j = 0; j < tmcount; j++)
                tms[j] = Rand.Next(100) < Settings.LearnTMPercent;
        }

        if (Settings.ModifyLearnsetHM)
        {
            for (int j = tmcount; j < tms.Length; j++)
                tms[j] = Rand.Next(100) < Settings.LearnTMPercent;
        }

        z.TMHM = tms;
    }
    
    private void RandomizeTR(IMovesInfo_SWSH z)
    {
        var trs = z.TR;

        if (Settings.ModifyLearnsetTM)
        {
            for (int j = 0; j < trs.Length; j++)
                trs[j] = Rand.Next(100) < Settings.LearnTMPercent;
        }

        z.TR = trs;
    }

    private void RandomizeTypeTutors(IMovesInfo_1 z, int species)
    {
        var t = z.TypeTutors;
        for (int i = 0; i < t.Length; i++)
            t[i] = Rand.Next(100) < Settings.LearnTypeTutorPercent;

        // Make sure Rayquaza can learn Dragon Ascent.
        if (!Game.XY && species == (int)Species.Rayquaza)
            t[7] = true;

        z.TypeTutors = t;
    }

    private void RandomizeSpecialTutors(IMovesInfo_2 z)
    {
        var tutors = z.SpecialTutors;
        foreach (bool[] tutor in tutors)
        {
            for (int i = 0; i < tutor.Length; i++)
                tutor[i] = Rand.Next(100) < Settings.LearnMoveTutorPercent;
        }

        z.SpecialTutors = tutors;
    }

    private void RandomizeAbilities(IPersonalAbility z)
    {
        Span<int> abils = stackalloc int[3];
        z.GetAbilities(abils);
        GetRandomAbilities(abils, Settings.Ability);
        z.SetAbilities(abils);
    }

    private void GetRandomAbilities(Span<int> abils, int skip = 0)
    {
        for (int i = 0; i < abils.Length - skip; i++)
            abils[i] = GetRandomAbility();
    }

    private void RandomizeEggGroups(IPersonalEgg z)
    {
        z.EggGroup1 = GetRandomEggGroup();
        z.EggGroup2 = Rand.Next(100) < Settings.SameEggGroupChance ? z.EggGroup1 : GetRandomEggGroup();
    }

    private void RandomizeHeldItems(IPersonalItems z)
    {
        for (int j = 0; j < z.GetNumItems(); j++)
            z.SetItemAtIndex(j, GetRandomHeldItem());
    }

    private void RandomizeTypes(IPersonalType z)
    {
        z.Type1 = (Types)GetRandomType();
        z.Type2 = Rand.Next(0, 100) < Settings.SameTypeChance ? z.Type1 : (Types)GetRandomType();
    }

    private void RandomizeStats(IBaseStat z)
    {
        // Fiddle with Base Stats, don't muck with Shedinja.
        if (z.GetBaseStatValue(0) == 1)
            return;

        int RandDeviation() => Rand.Next(Settings.StatDeviationMin, Settings.StatDeviationMax);
        for (int i = 0; i < z.GetNumBaseStats(); i++)
        {
            if (!Settings.StatsToRandomize[i])
                continue;

            var val = z.GetBaseStatValue(i) * RandDeviation() / 100;
            z.SetBaseStatValue(i, Math.Max(1, Math.Min(255, val)));
        }
    }

    private static void RandomShuffledStats(IBaseStat z)
    {
        // Fiddle with Base Stats, don't muck with Shedinja.
        if (z.GetBaseStatValue(0) == 1)
            return;

        var stats = new int[z.GetNumBaseStats()];
        for (int i = 0; i < z.GetNumBaseStats(); i++)
            stats[i] = z.GetBaseStatValue(i);

        Util.Shuffle(stats);

        for (int i = 0; i < z.GetNumBaseStats(); i++)
            z.SetBaseStatValue(i, stats[i]);
    }

    private int GetRandomType() => Rand.Next(0, TypeCount);
    private int GetRandomEggGroup() => Rand.Next(1, eggGroupCount);
    private int GetRandomHeldItem() => Game.HeldItems.Length > 1 ? Game.HeldItems[Rand.Next(1, Game.HeldItems.Length)] : 0;
    private readonly IList<int> BannedAbilities = Array.Empty<int>();

    private int GetRandomAbility()
    {
        const int WonderGuard = 25;
        while (true)
        {
            int newabil = Rand.Next(1, Game.MaxAbilityID + 1);
            if (newabil == WonderGuard && Settings.WonderGuard == Permissive.No)
                continue;
            if (BannedAbilities.Contains(newabil))
                continue;
            return newabil;
        }
    }
}
