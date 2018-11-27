using System.IO;

namespace pkNX.Containers
{
    public static class FileMitm
    {
        public static byte[] ReadAllBytes(string path)
        {
            path = GetRedirectedReadPath(path);
            return File.ReadAllBytes(path);
        }

        public static void WriteAllBytes(string path, byte[] data)
        {
            path = GetRedirectedWritePath(path);
            File.WriteAllBytes(path, data);
        }

        public static string GetRedirectedReadPath(string path)
        {
            if (!Enabled)
                return path;
            var newDest = path.Replace(PathOriginal, PathRedirect);
            if (!File.Exists(newDest))
                return path;
            return newDest;
        }

        public static string GetRedirectedWritePath(string path)
        {
            if (!Enabled)
                return path;
            var newDest = path.Replace(PathOriginal, PathRedirect);
            var parent = Path.GetDirectoryName(newDest);
            Directory.CreateDirectory(parent);
            return newDest;
        }

        public static void SetRedirect(string original, string dest)
        {
            PathOriginal = original;
            PathRedirect = dest;
            Enabled = true;
        }

        public static bool Enabled { get; private set; }

        public static void EnableIfSetup() => Enabled = PathOriginal != null;
        public static void Disable() => Enabled = false;

        private static string PathOriginal;
        private static string PathRedirect;
    }
}