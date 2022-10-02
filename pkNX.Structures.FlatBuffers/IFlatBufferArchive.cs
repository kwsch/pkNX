namespace pkNX.Structures.FlatBuffers;

public interface IFlatBufferArchive<T> where T : class
{
    T[] Table { get; set; }
}
