using System.IO;
using SharpFileSystem.FileSystems;
using Xunit;

namespace SharpFileSystem.Tests
{
    public class CopierTest
    {
        [Fact]
        public void TestCopy()
        {
            var memFs1 = new MemoryFileSystem();
            var memFs2 = new MemoryFileSystem();

            memFs1.CreateDirectory("/fs1/");
            memFs1.CreateDirectory("/fs1/memory/");

            memFs1.WriteAllText("/fs1/memory/test1.txt", "hello1");
            memFs1.WriteAllText("/fs1/memory/test2.txt", "hello2");


            memFs2.CreateDirectory("/fs2/");
            //memFs2.CreateDirectory("/fs2/memory/");
            var copier = new StandardEntityCopier();
            copier.Copy(memFs1, "/fs1/memory/", memFs2, "/fs2/memory/");
            var entities = memFs2.GetEntities("/fs2/memory/");
            Assert.Equal(2, entities.Count);
            Assert.True(memFs2.Exists("/fs2/memory/test1.txt"));
            Assert.True(memFs2.Exists("/fs2/memory/test2.txt"));
            var content1 = memFs2.ReadAllText("/fs2/memory/test1.txt");
            Assert.Equal("hello1",content1);
            var content2 = memFs2.ReadAllText("/fs2/memory/test2.txt");
            Assert.Equal("hello2",content2);



        }
    }
}
