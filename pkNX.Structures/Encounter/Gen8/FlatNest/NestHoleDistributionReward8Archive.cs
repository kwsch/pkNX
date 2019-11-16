namespace pkNX.Structures
{
    public class NestHoleDistributionReward8Archive
    {
        public NestHoleDistributionReward8Table[] Tables { get; set; }
    }

    public class NestHoleDistributionReward8Table : INestHoleRewardTable
    {
        public ulong TableID { get; set; }
        public NestHoleDistributionReward8[] Entries { get; set; }

        public INestHoleReward[] Rewards => Entries;
    }

    public class NestHoleDistributionReward8 : INestHoleReward
    {
        public uint Value0;
        public uint Value1;
        public uint Value2;
        public uint Value3;
        public uint Value4;
        public uint ItemID { get; set; }

        public uint[] Values
        {
            get => new [] { Value0, Value1, Value2, Value3, Value4 };
            set
            {
                Value0 = (byte)value[0];
                Value1 = (byte)value[1];
                Value2 = (byte)value[2];
                Value3 = (byte)value[3];
                Value4 = (byte)value[4];
            }
        }
    }
}
