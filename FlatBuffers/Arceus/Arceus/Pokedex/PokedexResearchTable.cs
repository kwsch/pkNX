namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PokedexResearchTable
{
    public PokedexResearchTask[] GetEntries(int species)
    {
        return Table.Where(z => z.Species == species).ToArray();
    }

    public PokedexResearchTask AddTask(int species)
    {
        var task = new PokedexResearchTask { Species = species };
        AddTask(task);
        return task;
    }

    public void AddTask(PokedexResearchTask task) => Table = Table.Append(task).ToArray();
    public bool RemoveTask(PokedexResearchTask taskToRemove)
    {
        return Table.Remove(taskToRemove);
    }
}
