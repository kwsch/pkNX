using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization
{
    public class MoveRandomizer : Randomizer
    {
        private readonly IReadOnlyList<IMove> MoveData;
        private readonly IPersonalTable SpeciesStat;
        private readonly GameInfo Config;

        private GenericRandomizer<int> RandMove;
        internal MovesetRandSettings Settings = new();

        public MoveRandomizer(GameInfo config, IReadOnlyList<IMove> moves, IPersonalTable t)
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

            var banned = new List<int>();
            banned.AddRange(Legal.Taboo_Moves.Concat(Legal.Z_Moves).Concat(Legal.Max_Moves));
            if (Settings.BanFixedDamageMoves)
                banned.AddRange(FixedDamageMoves);
            banned.AddRange(bannedMoves);

            Settings.BannedMoves = banned.ToArray();

            var all = Enumerable.Range(1, Config.MaxMoveID - 1);
            var moves = all.Except(banned);
            if (MoveData[0] is Move8Fake)
                moves = moves.Where(z => ((Move8Fake)MoveData[z]).CanUseMove);
            RandMove = new GenericRandomizer<int>(moves.ToArray());
        }

        public int[] GetRandomLearnset(int index, int movecount) => GetRandomLearnset(SpeciesStat[index], movecount);

        public int[] GetRandomLearnset(IPersonalType Types, int movecount)
        {
            var oldSTABCount = Settings.STABCount;
            Settings.STABCount = (int)(Settings.STABPercent * movecount / 100);
            int[] moves = GetRandomMoveset(Types, movecount);
            Settings.STABCount = oldSTABCount;
            return moves;
        }

        public int[] GetRandomMoveset(int index, int movecount = 4) => GetRandomMoveset(SpeciesStat[index], movecount);

        public int[] GetRandomMoveset(IPersonalType Types, int movecount = 4)
        {
            int loopctr = 0;
            const int maxLoop = 666;

            int[] moves;
            do { moves = GetRandomMoves(Types, movecount); }
            while (!IsMovesetMeetingRequirements(moves, Types, movecount) && loopctr++ <= maxLoop);

            return moves;
        }

        private int[] GetRandomMoves(IPersonalType Types, int movecount = 4)
        {
            int[] moves = new int[movecount];
            int i = 0;
            if (Settings.STAB)
            {
                for (; i < Settings.STABCount; i++)
                    moves[i] = GetRandomSTABMove(Types);
            }

            for (; i < moves.Length; i++) // remainder of moves
                moves[i] = RandMove.Next();
            return moves;
        }

        private int GetRandomSTABMove(IPersonalType types)
        {
            int move;
            int ctr = 0;
            do { move = RandMove.Next(); }
            while (!types.IsType((Types)MoveData[move].Type) && ctr++ < RandMove.Count);
            return move;
        }

        private bool IsMovesetMeetingRequirements(int[] moves, IPersonalType types, int count)
        {
            if (Settings.DMG && Settings.DMGCount > moves.Count(move => MoveData[move].Category != 0))
                return false;

            if (Settings.STAB)
            {
                var stabCt = moves.Count(move => types.IsType((Types)MoveData[move].Type));
                if (stabCt < Settings.STABCount)
                    return false;
            }

            if (moves.Any(Settings.BannedMoves.Contains))
                return false;

            return moves.Distinct().Count() == count;
        }

        public void ReorderMovesPower(IList<int> moves) => ReorderMovesPower(moves, MoveData);

        private static void ReorderMovesPower(IList<int> moves, IReadOnlyList<IMove> movedata)
        {
            var data = moves.Select((Move, Index) => new { Index, Move, Data = movedata[Move] });
            var powered = data.Where(z => z.Data.Power > 1).ToList();
            var indexes = powered.ConvertAll(z => z.Index);
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

        private static readonly GenericRandomizer<int> first = new(firstMoves);

        public static int GetRandomFirstMoveAny()
        {
            first.Reset();
            return first.Next();
        }

        public int GetRandomFirstMove(int index) => GetRandomFirstMove(SpeciesStat[index]);

        public int GetRandomFirstMove(IPersonalType types)
        {
            first.Reset();
            int ctr = 0;
            int move;
            do
            {
                move = first.Next();
                if (++ctr == firstMoves.Length)
                    return move;
            } while (!types.IsType((Types)MoveData[move].Type));
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