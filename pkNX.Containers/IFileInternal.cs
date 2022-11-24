namespace pkNX.Containers;

public interface IFileInternal
{
    byte[] GetPackedFile(string file);
    byte[] GetPackedFile(ulong hash);
}
