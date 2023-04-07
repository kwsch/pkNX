using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokedexResearchTable
{
    public PokedexResearchTask[] GetEntries(int species)
    {
        return Table.Where(z => z.Species == species).ToArray();
    }

    public PokedexResearchTask AddTask(int Species)
    {
        var task = new PokedexResearchTask { Species = Species };
        AddTask(task);
        return task;
    }

    public void AddTask(PokedexResearchTask task) => Table = Table.Append(task).ToArray();
    public bool RemoveTask(PokedexResearchTask taskToRemove)
    {
        return Table.Remove(taskToRemove);
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokedexResearchTask { }
