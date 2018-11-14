using System;

namespace pkNX.Structures
{
    public abstract class TrData6 : TrainerData
    {
        protected abstract int Format { get; set; }
        public bool HasItem { get => (Format & 1) == 1; set => Format = (ushort)((Format & ~1) | (value ? 1 : 0)); }
        public bool HasMoves { get => (Format & 2) == 2; set => Format = (ushort)((Format & ~2) | (value ? 2 : 0)); }

        public TrPoke6[] Team { get; set; }

        protected TrData6(byte[] trData) : base(trData) { }
        public byte[] WriteTeam() => WriteTeam(Team, HasItem, HasMoves);
        public TrPoke6[] GetTeam(byte[] trPoke) => GetTeam(trPoke, NumPokemon, HasItem, HasMoves);

        public static byte[] WriteTeam(TrPoke6[] team, bool HasItem, bool HasMoves)
        {
            if (team.Length == 0)
                return Array.Empty<byte>();
            var first = team[0].Write(HasItem, HasMoves);
            byte[] result = new byte[first.Length * team.Length];
            first.CopyTo(result, 0);
            for (int i = 1; i < team.Length; i++)
                team[i].Write(HasItem, HasMoves).CopyTo(result, first.Length * i);
            return result;
        }

        public static TrPoke6[] GetTeam(byte[] trPoke, int numPokemon, bool item, bool moves)
        {
            var team = new TrPoke6[numPokemon];
            byte[][] teamData = new byte[numPokemon][];
            int dataLen = trPoke.Length / numPokemon;
            for (int i = 0; i < teamData.Length; i++)
            {
                var arr = teamData[i] = new byte[dataLen];
                Array.Copy(trPoke, i * dataLen, arr, 0, dataLen);
            }

            for (int i = 0; i < numPokemon; i++)
                team[i] = new TrPoke6(teamData[i], item, moves);
            return team;
        }
    }
}