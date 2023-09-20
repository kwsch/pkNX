using System;
using System.IO;

namespace pkNX.Containers;

public static class StreamExtensions
{
    public static string ReadAllText(this Stream s)
    {
        using var reader = new StreamReader(s);
        return reader.ReadToEnd();
    }

    public static byte[] ReadAllBytes(this Stream s)
    {
        byte[] buffer = new byte[(int)s.Length];
        s.ReadAllBytes(buffer);
        return buffer;
    }

    public static void ReadAllBytes(this Stream s, Span<byte> destination)
    {
        int bytesRead = s.Read(destination);

        if (bytesRead != destination.Length)
            throw new IOException("Could not read all bytes.");
    }
}
