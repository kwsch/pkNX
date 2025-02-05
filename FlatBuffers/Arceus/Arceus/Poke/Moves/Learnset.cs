namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class LearnsetMeta
{
    public byte[] WriteLearnsetAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        foreach (var entry in Arceus)
        {
            br.Write(entry.Move);
            br.Write(entry.Level);
        }
        br.Write(-1);
        return ms.ToArray();
    }

    public byte[] WriteMasteryAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        foreach (var entry in Arceus)
        {
            br.Write(entry.Move);
            br.Write(entry.LevelMaster);
        }
        br.Write(-1);
        return ms.ToArray();
    }
}
