using System.IO;

namespace pkNX.Containers
{
    public static class FileMitm
    {
        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void WriteAllBytes(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        public static string GetRedirectedPath(string path)
        {
            return path;
        }
    }
}