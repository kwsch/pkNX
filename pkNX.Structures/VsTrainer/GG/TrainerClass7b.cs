namespace pkNX.Structures
{
    public class TrainerClass7b : TrainerClass
    {
        public sealed override int SIZE => 0x24;
        public TrainerClass7b(byte[] data = null) => Data = data ?? new byte[SIZE];

        public byte Unk_0x00 { get => Data[0]; set => Data[0] = value; }
        public override int Group { get => Data[1]; set => Data[1] = (byte)value; }
        public override int BallID { get => Data[2]; set => Data[2] = (byte)value; }
        public byte Unk_0x03 { get => Data[3]; set => Data[3] = value; }

        // model hash at end (name, form, variation)?

        public override int MegaItemID => 773;
    }
}