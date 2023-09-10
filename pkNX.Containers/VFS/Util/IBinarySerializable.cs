using System.IO;

namespace pkNX.Containers.VFS;

public interface IBinarySerializable
{
    void Read(BinaryReader br);
    void Write(BinaryWriter bw);
}
