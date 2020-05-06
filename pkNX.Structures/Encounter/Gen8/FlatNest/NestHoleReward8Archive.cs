using System.Runtime.InteropServices.ComTypes;﻿
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace pkNX.Structures
{
    public class NestHoleReward8Archive
    {
        public NestHoleReward8Table[] Tables { get; set; }
    }

    public interface INestHoleRewardTable
    {
        ulong TableID { get; set; }
        [JsonIgnore]
        INestHoleReward[] Rewards { get; }
    }

    public class NestHoleReward8Table : INestHoleRewardTable
    {
        public ulong TableID { get; set; }
        public NestHoleReward8[] Entries { get; set; }

        [Browsable(false)]
        [JsonIgnore]
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

        public override string ToString() => $"{EntryID:0} - {ItemID:0000}";
    }
}
