using System;
using System.IO;
using pkNX.Structures.FlatBuffers.SV.Trinity;
using Xunit;

namespace pkNX.Tests;

public class SceneTests
{
    [Fact]
    public void TestSceneDump()
    {
        const string file = "";
        if (!File.Exists(file))
            return;

        SceneDumper.Dump(file);
    }

    [Fact]
    public void TestFolderDump()
    {
        const string dir = ""; // input
        SceneDumper.Bucket = ""; // output
        SceneDumper.ThrowOnUnknownType = false;
        var files = Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (!IsTrinityType(file))
                continue;
            try { SceneDumper.Dump(file); } catch { }
        }
    }

    private static bool IsTrinityType(ReadOnlySpan<char> file) => file.Contains("trscn", StringComparison.Ordinal)
                                                               || file.Contains("trsog", StringComparison.Ordinal);
}
