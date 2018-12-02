using System;
using pkNX.Containers;

namespace pkNX.Game
{
    public class DataCache<T> : IDataEditor where T : class
    {
        public IFileContainer Data { protected get; set; }
        public Func<byte[], T> Create { private get; set; }
        public Func<T, byte[]> Write { protected get; set; }

        protected DataCache(T[] cache) => Cache = cache;
        public DataCache(IFileContainer f) : this(new T[f.Count]) => Data = f;

        protected readonly T[] Cache;
        private bool Cached;

        public int Length => Cache.Length;

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
            if (Cached)
                return Cache;
            for (int i = 0; i < Length; i++)
            {
                // ReSharper disable once AssignmentIsFullyDiscarded
                _ = this[i]; // force load cache
            }

            Cached = true;
            return Cache;
        }

        /// <summary>
        /// Pushes changes back to the <see cref="IFileContainer"/>.
        /// </summary>
        public virtual void Save()
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

    /// <summary>
    /// Data with already known contents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DirectCache<T> : DataCache<T> where T : class
    {
        public DirectCache(T[] cache) : base(cache) { }
        public override void Save() { }
    }
}