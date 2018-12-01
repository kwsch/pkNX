namespace pkNX.Game
{
    public abstract class ShinyRateInfo
    {
        public byte[] Data { get; }
        protected ShinyRateInfo(byte[] data) => Data = data;

        public abstract bool IsEditable { get; }

        public abstract bool IsDefault { get; }
        public abstract bool IsFixed { get; }
        public abstract bool IsAlways { get; }

        public abstract int GetFixedRate();
        public abstract void SetDefault();
        public abstract void SetFixedRate(int rerollCount);
        public abstract void SetAlwaysShiny();

        protected bool IsPresent(byte[] source, byte[] pattern, int offset)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (source[i + offset] != pattern[i])
                    return false;
            }
            return true;
        }
    }
}
