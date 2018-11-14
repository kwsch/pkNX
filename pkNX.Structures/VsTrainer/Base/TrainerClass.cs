namespace pkNX.Structures
{
    public abstract class TrainerClass
    {
        public abstract int SIZE { get; }
        protected byte[] Data;

        public virtual int Gender { get; set; } = 0;
        public virtual int Multi { get; set; } = 0;
        public virtual int Group { get; set; } = 0;
        public virtual int BallID { get; set; } = 4;
        public virtual int BattleBackground { get; set; } = 0;
        public virtual int EyeCatchBGM { get; set; } = 0;

        public virtual bool IsBoss => false;
        public virtual int MegaItemID => 773;
    }
}
