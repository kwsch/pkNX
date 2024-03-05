using System;
using System.Collections.Generic;
using System.Linq;
using FlatSharp;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class DataCache<T>(IList<T?> cache) : IDataEditor
    where T : class
{
    public IFileContainer Data { protected get; set; } = null!;
    public Func<byte[], T> Create { private get; set; } = null!;
    public Func<T, byte[]> Write { protected get; set; } = null!;

    public DataCache(IFileContainer f) : this(new T[f.Count]) => Data = f;

    protected readonly IList<T?> Cache = cache;
    private bool Cached;

    public int Length => Cache.Count;

    public T this[int index]
    {
        get => Cache[index] ??= Create(Data[index]);
        set => Cache[index] = value;
    }

    public void CancelEdits()
    {
        for (int i = 0; i < Cache.Count; i++)
            Cache[i] = default;
    }

    public void Initialize() { }

    public T[] LoadAll()
    {
        if (Cached)
            return Cache.ToArray()!;
        for (int i = 0; i < Length; i++)
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = this[i]; // force load cache
        }

        Cached = true;
        return Cache.ToArray()!;
    }

    public void ClearAll()
    {
        Cached = false;
        for (int i = 0; i < Cache.Count; i++)
            Cache[i] = null;
    }

    /// <summary>
    /// Pushes changes back to the <see cref="IFileContainer"/>.
    /// </summary>
    public virtual void Save()
    {
        for (int i = 0; i < Cache.Count; i++)
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
public class DirectCache<T>(IList<T?> cache) : DataCache<T>(cache)
    where T : class
{
    public override void Save() { }
}

/// <summary>
/// Data 'from a FlatBuffer table
/// </summary>
/// <typeparam name="TTable">The type of table</typeparam>
/// <typeparam name="TData">The type of data inside the table</typeparam>
public class TableCache<TTable, TData>
    where TTable : class, IFlatBufferSerializable<TTable>
    where TData : class
{
    public IFileContainer File { get; private set; }
    public TTable Root { get; private set; }
    public IList<TData> Table { get; private set; }
    public DataCache<TData> Cache { get; private set; }

    public TableCache(IFileContainer f, Func<TTable, IList<TData>> sel)
    {
        File = f;
        Root = FlatBufferConverter.DeserializeFrom<TTable>(f[0]);
        Table = sel(Root);
        Cache = new DirectCache<TData>(Table!);
    }

    public void Save()
    {
        File[0] = Root.SerializeFrom();
    }

    public TData this[int index]
    {
        get => Cache[index];
        set => Cache[index] = value;
    }

    public int Length => Cache.Length;
}
