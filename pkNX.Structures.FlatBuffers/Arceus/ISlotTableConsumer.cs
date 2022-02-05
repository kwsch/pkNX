namespace pkNX.Structures.FlatBuffers;

public interface ISlotTableConsumer
{
    bool UsesTable(ulong table);
}
