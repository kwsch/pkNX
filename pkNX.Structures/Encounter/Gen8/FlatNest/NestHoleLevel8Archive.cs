namespace pkNX.Structures
{
    public class NestHoleLevel8Archive
    {
        public NestHoleLevel8Table[] Tables { get; set; }
    }

    public class NestHoleLevel8Table
    {
        public ulong TableID { get; set; }
        public NestHoleLevel8[] Entries { get; set; }
    }

    public class NestHoleLevel8
    {
        public uint MinLevel { get; set; }
        public uint MaxLevel { get; set; }
    }
}
