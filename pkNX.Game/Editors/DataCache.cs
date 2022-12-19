using System;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class DataCache<T> : IDataEditor where T : class
{
    public IFileContainer Data { protected get; set; } = null!;
    public Func<byte[], T> Create { private get; set; } = null!;
    public Func<T, byte[]> Write { protected get; set; } = null!;

    public DataCache(T?[] cache) => Cache = cache;
    public DataCache(IFileContainer f) : this(new T[f.Count]) => Data = f;

    protected readonly T?[] Cache;
    private bool Cached;

    public int Length => Cache.Length;

    public T this[int index]
    {
        get => Cache[index] ??= Create(Data[index]);
        set => Cache[index] = value;
    }

    public void CancelEdits()
    {
        for (int i = 0; i < Cache.Length; i++)
            Cache[i] = default;
    }

    public void Initialize() { }

    public T[] LoadAll()
    {
        if (Cached)
            return Cache!;
        for (int i = 0; i < Length; i++)
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = this[i]; // force load cache
        }

        Cached = true;
        return Cache!;
    }

    public void ClearAll()
    {
        Cached = false;
        for (int i = 0; i < Cache.Length; i++)
            Cache[i] = null;
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

/// <summary>
/// Data 'from a FlatBuffer table
/// </summary>
/// <typeparam name="TTable">The type of table</typeparam>
/// <typeparam name="TData">The type of data inside the table</typeparam>
public class TableCache<TTable, TData>
    where TTable : class, IFlatBufferArchive<TData>, new()
    where TData : class
{
    public IFileContainer File { get; private set; }
    public TTable Root { get; private set; }
    public TData[] Table => Root.Table;
    public DataCache<TData> Cache { get; private set; }

    public TableCache(IFileContainer f)
    {
        File = f;
        Root = FlatBufferConverter.DeserializeFrom<TTable>(f[0]);
        Cache = new DirectCache<TData>(Root.Table);
    }

    public void Save()
    {
        File[0] = FlatBufferConverter.SerializeFrom(Root);
    }

    public TData this[int index]
    {
        get => Cache[index];
        set => Cache[index] = value;
    }

    public int Length => Cache.Length;
}
