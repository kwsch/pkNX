namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class ThrowableParamTable
{
    public void AddEntry(int i)
    {
        Table = Table.Append(new ThrowableParam
        {
            Label01 = string.Empty,
            Label02 = string.Empty,
        }).ToList();
    }
}
