using LZ4;

namespace pkNX.Containers;

public static class LZ4
{
    public static byte[] Decode(byte[] data, int decLength) => LZ4Codec.Decode(data, 0, data.Length, decLength);
    public static byte[] Encode(byte[] data) => LZ4Codec.Encode(data, 0, data.Length);
}
