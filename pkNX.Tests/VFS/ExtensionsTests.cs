using System.Linq;
using Xunit;

namespace pkNX.Tests;

public class ExtensionsTests
{
    [Fact]
    public void testRecursiveDirectoryCreation()
    {
        var mem = new MemoryFileSystem();
        mem.CreateDirectoryRecursive("/memory/deep/deeper/deepest/");
        Assert.True(mem.Exists("/memory/"));
        Assert.True(mem.Exists("/memory/deep/"));
        Assert.True(mem.Exists("/memory/deep/deeper/"));
        Assert.True(mem.Exists("/memory/deep/deeper/deepest/"));
    }

    [Fact]
    public void GetEntitiesRecursiveTest()
    {
        EmbeddedResourceFileSystem eFS = new EmbeddedResourceFileSystem(typeof(ExtensionsTests).Assembly);
        var entities = eFS.GetEntitiesRecursive("/");
        Assert.Equal(4, entities.Count());
    }
}
