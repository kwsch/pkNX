using System;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game
{
    public class TrainerEditor : IDataEditor
    {
        public IFileContainer TrainerData;
        public IFileContainer TrainerPoke;
        public IFileContainer TrainerClass;
        public IFileContainer TrainerMsg;

        public Func<byte[], TrainerData> ReadTrainer;
        public Func<byte[], TrainerPoke> ReadPoke;
        public Func<byte[], TrainerData, TrainerPoke7b[]> ReadTeam;
        public Func<TrainerPoke7b[], TrainerData, byte[]> WriteTeam;
        public Func<byte[], TrainerClass> ReadClass;

        private VsTrainer[] Cache;
        private TrainerClass[] CacheClass;

        public int Length => Cache.Length;

        public void Initialize()
        {
            Cache = new VsTrainer[TrainerData.Count];
            CacheClass = new TrainerClass[TrainerData.Count];
        }

        public VsTrainer this[int index]
        {
            get => Cache[index] ?? (Cache[index] = LoadTrainer(index));
            set => Cache[index] = value;
        }

        public TrainerClass GetClass(int index) => CacheClass[index] ?? (CacheClass[index] = ReadClass(TrainerClass[index]));

        private VsTrainer LoadTrainer(int index)
        {
            var tr = ReadTrainer(TrainerData[index]);
            var poke = ReadTeam(TrainerPoke[index], tr);
            var data = new VsTrainer
            {
                ID = index,
                Self = tr,
            };
            data.Team.AddRange(poke);
            return data;
        }

        public void Save()
        {
            for (int i = 0; i < Length; i++)
            {
                var data = Cache[i];
                if (data == null)
                    continue;
                data.Self.NumPokemon = data.Team.Count;
                TrainerData[i] = data.Self.Write();
                TrainerPoke[i] = data.Team.SelectMany(z => z.Write()).ToArray();
            }
        }

        public void CancelEdits()
        {
            TrainerData.CancelEdits();
            TrainerPoke.CancelEdits();
        }
    }
}