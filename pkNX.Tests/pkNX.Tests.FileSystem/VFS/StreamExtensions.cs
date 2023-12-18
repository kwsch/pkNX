using System.IO;

namespace pkNX.Tests;

public static class StreamExtensions
{
    public static string ReadAllText(this Stream s)
    {
        using var reader = new StreamReader(s);
        return reader.ReadToEnd();
    }

    public static byte[] ReadAllBytes(this Stream s)
    {
        using var ms = new MemoryStream();
        s.CopyTo(ms);
        return ms.ToArray();
    }
}
