using System;
using pkNX.Containers;

namespace pkNX.Game
{
    public class DataCache<T> : IDataEditor where T : class
    {
        public IFileContainer Data { protected get; set; }
        public Func<byte[], T> Create { private get; set; }
        public Func<T, byte[]> Write { protected get; set; }

        public DataCache(IFileContainer f)
        {
            Data = f;
            Cache = new T[f.Count];
        }

        private readonly T[] Cache;

        public int Length => Data.Count;

        public T this[int index]
        {
            get => Cache[index] ?? (Cache[index] = Create(Data[index]));
            set => Cache[index] = value;
        }

        public void CancelEdits()
        {
            for (int i = 0; i < Cache.Length; i++)
                Cache[i] = default(T);
        }

        public void Initialize() { }

        public T[] LoadAll()
        {
            for (int i = 0; i < Length; i++)
                // ReSharper disable once AssignmentIsFullyDiscarded
                _ = this[i]; // force load cache
            return Cache;
        }

        /// <summary>
        /// Pushes changes back to the <see cref="IFileContainer"/>.
        /// </summary>
        public void Save()
        {
            for (int i = 0; i < Cache.Length; i++)
            {
                var val = Cache[i];
                if (val == null)
                    continue;
                Data[i] = Write(val);
            }
        }
    }
}