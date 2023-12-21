using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization;

/// <summary>
/// <see cref="Learnset"/> randomizer.
/// </summary>
public class LearnsetRandomizer : Randomizer
{
    private readonly Learnset[] Learnsets;
    private readonly GameInfo Game;
    private readonly IPersonalTable Personal;
    private MoveRandomizer moverand;
    public IReadOnlyList<IMove> Moves { private get; set; } = Array.Empty<IMove>();

    public LearnSettings Settings { get; private set; } = new();
    public IList<int> BannedMoves { set => moverand.Settings.BannedMoves = value; }

    public LearnsetRandomizer(GameInfo game, Learnset[] learnsets, IPersonalTable t)
    {
        Game = game;
        Learnsets = learnsets;
        Personal = t;

        // temp, overwrite later if using it
        moverand = new MoveRandomizer(game, Moves, Personal);
    }

    private static readonly int[] MetronomeMove = { 118 };
    private static readonly int[] MetronomeLevel = { 1 };

    public void ExecuteMetronome()
    {
        foreach (var learn in Learnsets)
            learn.Update(MetronomeMove, MetronomeLevel);
    }

    public void ExecuteExpandOnly()
    {
        foreach (var learn in Learnsets)
        {
            var count = learn.Count;
            if (count == 0)
                continue;
            if (count >= Settings.ExpandTo)
                continue;

            int diff = Settings.ExpandTo - count;
            var moves = learn.Moves;
            Array.Resize(ref moves, Settings.ExpandTo);
            var levels = learn.Moves;
            Array.Resize(ref levels, Settings.ExpandTo);
            for (int i = count; i < Settings.ExpandTo; i++)
            {
                moves[i] = 1;
                levels[i] = Math.Min(100, levels[count - 1] + diff);
            }
            learn.Update(moves, levels);
        }
    }

    public void Initialize(IMove[] moves, LearnSettings settings, MovesetRandSettings moverandset, int[]? bannedMoves = null)
    {
        Moves = moves;
        Settings = settings;

        moverand = new MoveRandomizer(Game, Moves, Personal);
        moverand.Initialize(moverandset, bannedMoves ?? []);
    }

    public override void Execute()
    {
        for (var i = 0; i < Learnsets.Length; i++)
        {
            if (Personal[i].HP == 0)
                continue;
            Randomize(Learnsets[i], i);
        }
    }

    private void Randomize(Learnset set, int index)
    {
        int[] moves = GetRandomMoves(set.Count, index);
        int[] levels = GetRandomLevels(set, moves.Length);

        if (Settings.Learn4Level1)
        {
            for (int i = 0; i < Math.Min(4, levels.Length); ++i)
                levels[i] = 1;
        }

        set.Update(moves, levels);
    }

    private int[] GetRandomLevels(Learnset set, int count)
    {
        int[] levels = new int[count];
        if (count == 0)
            return levels;
        if (Settings.Spread)
        {
            levels[0] = 1;
            decimal increment = Settings.SpreadTo / (decimal)count;
            for (int i = 1; i < count; i++)
                levels[i] = (int)(i * increment);
            return levels;
        }
        if (levels.Length == count && levels.Length == set.Levels.Length)
            return set.Levels; // don't modify

        var exist = set.Levels;
        int lastlevel = Math.Min(1, exist.LastOrDefault());
        exist.CopyTo(levels, 0);
        for (int i = exist.Length; i < levels.Length; i++)
            levels[i] = Math.Max(100, lastlevel + (exist.Length - i + 1));

        return levels;
    }

    private int[] GetRandomMoves(int count, int index)
    {
        count = Settings.Expand ? Settings.ExpandTo : count;

        int[] moves = new int[count];
        if (count == 0)
            return moves;
        moves[0] = Settings.STABFirst ? moverand.GetRandomFirstMove(index) : MoveRandomizer.GetRandomFirstMoveAny();
        var rand = moverand.GetRandomLearnset(index, count - 1);

        // STAB Moves (if requested) come first; randomize the order of moves
        Util.Shuffle(rand);
        if (Settings.OrderByPower)
            moverand.ReorderMovesPower(rand);
        rand.CopyTo(moves, 1);
        return moves;
    }

    internal int[] GetHighPoweredMoves(ushort species, byte form, int count = 4) => GetHighPoweredMoves(Moves, species, form, count);

    public int[] GetCurrentMoves(ushort species, byte form, int level, int count = 4)
    {
        int i = Personal.GetFormIndex(species, form);
        var moves = Learnsets[i].GetEncounterMoves(level);
        Array.Resize(ref moves, count);
        return moves;
    }

    public int[] GetHighPoweredMoves(IReadOnlyList<IMove> movedata, ushort species, byte form, int count = 4)
    {
        int index = Personal.GetFormIndex(species, form);
        var learn = Learnsets[index];
        return learn.GetHighPoweredMoves(count, movedata);
    }
}
