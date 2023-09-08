using System;
using Xunit;

namespace SharpFileSystem.Tests
{
    public static class EAssert
    {
        public static void Throws<T>(Action a)
            where T : Exception
        {
            try
            {
                a();
            }
            catch (T)
            {
                return;
            }
            Assert.False(true, $"The exception '{typeof(T).FullName}' was not thrown.");
        }
    }
}
