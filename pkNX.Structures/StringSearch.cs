using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pkNX.Structures;

public static class StringSearch
{
    public static StringResultBucket[] SearchFiles(IEnumerable<string> files, params ReadOnlySpan<string> patterns)
    {
        var buckets = new StringResultBucket[patterns.Length];
        for (var i = 0; i < patterns.Length; i++)
        {
            var ascii = Encoding.ASCII.GetBytes(patterns[i]);
            buckets[i] = new StringResultBucket(ascii);
        }

        foreach (var file in files)
        {
            var data = File.ReadAllBytes(file).AsSpan();
            for (var i = 0; i < patterns.Length; i++)
            {
                var bucket = buckets[i];
                ScanForPattern(bucket.Ascii, data, bucket, file);
            }
        }
        return buckets;
    }

    private static void ScanForPattern(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> data, StringResultBucket bucket, string file)
    {
        while (true)
        {
            int index = data.IndexOf(pattern);
            if (index == -1)
                break;
            // get the length of the result. is the length of the pattern plus any ascii bytes
            var length = pattern.Length;
            while (index + length < data.Length && data[index + length] is >= 0x20 and <= 0x7E)
                length++;
            // Check backwards too
            while (index > 0 && data[index - 1] is >= 0x20 and <= 0x7E)
            {
                index--;
                length++;
            }

            var result = Encoding.ASCII.GetString(data.Slice(index, length));
            bucket.List.Add(new StringResult(file, result, index));
            data = data[(index + length)..];
        }
    }
}

public class StringResultBucket(byte[] ascii)
{
    public ReadOnlySpan<byte> Ascii => ascii;
    public List<StringResult> List { get; } = [];
}

// ReSharper disable NotAccessedPositionalProperty.Global
public sealed record StringResult(string File, string Result, int Offset);
