namespace pkNX.Structures
{
    public interface IFlatBufferArchive<T> where T : class
    {
        T[] Table { get; set; }
    }
}