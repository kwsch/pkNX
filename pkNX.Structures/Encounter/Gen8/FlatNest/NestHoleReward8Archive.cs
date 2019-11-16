using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace pkNX.Structures
{
    public class NestHoleReward8Archive
    {
        public NestHoleReward8Table[] Tables { get; set; }
    }

    public interface INestHoleRewardTable
    {
        ulong TableID { get; set; }
        INestHoleReward[] Rewards { get; }
    }

    public class NestHoleReward8Table : INestHoleRewardTable
    {
        public ulong TableID { get; set; }
        public NestHoleReward8[] Entries { get; set; }

        public INestHoleReward[] Rewards => Entries;
    }

    public interface INestHoleReward
    {
        uint ItemID { get; set; }
        uint[] Values { get; set; }
    }

    public class NestHoleReward8 : INestHoleReward
    {
        public uint EntryID { get; set; }
        public uint ItemID { get; set; }
        public uint[] Values { get; set; }
    }
}
