using System;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game;

public class TrainerEditor : IDataEditor
{
    public IFileContainer TrainerData { get; init; } = null!;
    public IFileContainer TrainerPoke { get; init; } = null!;
    public IFileContainer TrainerClass { get; init; } = null!;
    public IFileContainer TrainerMsg { get; init; } = null!;

    public Func<byte[], TrainerData> ReadTrainer { get; init; } = null!;
    public Func<byte[], TrainerPoke> ReadPoke { get; init; } = null!;
    public Func<byte[], TrainerData, TrainerPoke[]> ReadTeam { get; init; } = null!;
    public Func<TrainerPoke[], TrainerData, byte[]> WriteTeam { get; init; } = null!;
    public Func<byte[], TrainerClass> ReadClass { get; init; } = null!;

    private VsTrainer?[] Cache = Array.Empty<VsTrainer>();
    private TrainerClass?[] CacheClass = Array.Empty<TrainerClass>();

    public int Length => Cache.Length;

    public void Initialize()
    {
        Cache = new VsTrainer[TrainerData.Count];
        CacheClass = new TrainerClass[TrainerData.Count];
    }

    public VsTrainer this[int index]
    {
        get => Cache[index] ??= LoadTrainer(index);
        set => Cache[index] = value;
    }

    public TrainerClass GetClass(int index) => CacheClass[index] ??= ReadClass(TrainerClass[index]);

    private VsTrainer LoadTrainer(int index)
    {
        var tr = ReadTrainer(TrainerData[index]);
        var poke = ReadTeam(TrainerPoke[index], tr);
        var data = new VsTrainer(tr) { ID = index };
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

    public VsTrainer[] LoadAll()
    {
        for (int i = 0; i < Length; i++)
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = this[i]; // force load cache
        }

        return Cache!;
    }

    public void CancelEdits()
    {
        TrainerData.CancelEdits();
        TrainerPoke.CancelEdits();
    }
}
