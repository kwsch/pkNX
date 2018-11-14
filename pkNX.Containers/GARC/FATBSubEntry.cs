namespace pkNX.Containers
{
    public class FATBSubEntry : LargeContainerEntry
    {
        public bool Exists;
        public static string GetFileNumber(int index) => $"{index:00}";
    }
}