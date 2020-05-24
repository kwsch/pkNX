using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization
{
    public class MoveRandomizer : Randomizer
    {
        private readonly IReadOnlyList<Move> MoveData;
        private readonly PersonalTable SpeciesStat;
        private readonly GameInfo Config;

        private GenericRandomizer<int> RandMove;
        internal MovesetRandSettings Settings;

        public MoveRandomizer(GameInfo config, IReadOnlyList<Move> moves, PersonalTable t)
        {
            Config = config;
            var maxMoveId = config.MaxMoveID;
            MoveData = moves;
            SpeciesStat = t;
            RandMove = new GenericRandomizer<int>(Enumerable.Range(1, maxMoveId - 1).ToArray());
        }

        public override void Execute() => throw new Exception("Shouldn't be called.");

        public static readonly int[] FixedDamageMoves = { 49, 82 };

        public void Initialize(MovesetRandSettings settings, int[] bannedMoves)
        {
            Settings = settings;

            var nanned = new List<int>();
            nanned.AddRange(new[] { 165, 621, 464 }.Concat(Legal.Z_Moves)); // Struggle, Hyperspace Fury, Dark Void
            if (Settings.BanFixedDamageMoves)
                nanned.AddRange(FixedDamageMoves);
            nanned.AddRange(bannedMoves);

            Settings.BannedMoves = nanned.ToArray();

            var all = Enumerable.Range(1, Config.MaxMoveID - 1);
            var moves = all.Except(nanned);
            RandMove = new GenericRandomizer<int>(moves.ToArray());
        }

        public int[] GetRandomLearnset(int index, int movecount) => GetRandomLearnset(SpeciesStat[index].Types, movecount);

        public int[] GetRandomLearnset(int[] Types, int movecount)
        {
            var oldSTABCount = Settings.STABCount;
            Settings.STABCount = (int)(Settings.STABPercent * movecount / 100);
            int[] moves = GetRandomMoveset(Types, movecount);
            Settings.STABCount = oldSTABCount;
            return moves;
        }

        public int[] GetRandomMoveset(int index, int movecount = 4) => GetRandomMoveset(SpeciesStat[index].Types, movecount);

        public int[] GetRandomMoveset(int[] Types, int movecount = 4)
        {
            int loopctr = 0;
            const int maxLoop = 666;

            int[] moves;
            do { moves = GetRandomMoves(Types, movecount); }
            while (!IsMovesetMeetingRequirements(moves, Types, movecount) && loopctr++ <= maxLoop);

            return moves;
        }

        private int[] GetRandomMoves(int[] Types, int movecount = 4)
        {
            int[] moves = new int[movecount];
            if (Settings.STAB)
            {
                for (int i = 0; i < Settings.STABCount; i++)
                    moves[i] = GetRandomSTABMove(Types);
            }

            for (int i = 0; i < moves.Length; i++) // remainder of moves
                moves[i] = RandMove.Next();
            return moves;
        }

        private int GetRandomSTABMove(int[] types)
        {
            int move;
            int ctr = 0;
            do { move = RandMove.Next(); }
            while (!types.Contains(MoveData[move].Type) && ctr++ < RandMove.Count);
            return move;
        }

        private bool IsMovesetMeetingRequirements(int[] moves, int[] types, int count)
        {
            if (Settings.DMG && Settings.DMGCount > moves.Count(move => MoveData[move].Category != 0))
                return false;

            if (Settings.STAB && Settings.STABCount > moves.Count(move => types.Contains(MoveData[move].Type)))
                return false;

            if (moves.Any(Settings.BannedMoves.Contains))
                return false;

            return moves.Distinct().Count() == count;
        }

        public void ReorderMovesPower(IList<int> moves) => ReorderMovesPower(moves, MoveData);

        private static void ReorderMovesPower(IList<int> moves, IReadOnlyList<Move> movedata)
        {
            var data = moves.Select((Move, Index) => new { Index, Move, Data = movedata[Move] });
            var powered = data.Where(z => z.Data.Power > 1).ToList();
            var indexes = powered.Select(z => z.Index).ToList();
            var order = powered.OrderBy(z => z.Data.Power * Math.Max(1, (z.Data.HitMin + z.Data.HitMax) / 2m)).ToList();

            for (var i = 0; i < order.Count; i++)
                moves[indexes[i]] = order[i].Move;
        }

        private static readonly int[] firstMoves =
        {
            1,   // Pound
            40,  // Poison Sting
            52,  // Ember
            55,  // Water Gun
            64,  // Peck
            71,  // Absorb
            84,  // Thunder Shock
            98,  // Quick Attack
            122, // Lick
            141, // Leech Life
        };

        private static readonly GenericRandomizer<int> first = new GenericRandomizer<int>(firstMoves);

        public static int GetRandomFirstMoveAny()
        {
            first.Reset();
            return first.Next();
        }

        public int GetRandomFirstMove(int index) => GetRandomFirstMove(SpeciesStat[index].Types);

        public int GetRandomFirstMove(int[] types)
        {
            first.Reset();
            int ctr = 0;
            int move;
            do
            {
                move = first.Next();
                if (++ctr == firstMoves.Length)
                    return move;
            } while (!types.Contains(MoveData[move].Type));
            return move;
        }

        public bool SanitizeMovesetForBannedMoves(int[] moves, int index)
        {
            bool updated = false;
            for (int m = 0; m < moves.Length; m++)
            {
                if (!Settings.BannedMoves.Contains(moves[m]))
                    continue;
                updated = true;
                moves[m] = GetRandomFirstMove(index);
            }

            return updated;
        }
    }
}