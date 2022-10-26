namespace pkNX.Structures;

public class TrainerClass7b : TrainerClass
{
    private const int Size = 0x24;
    public sealed override int SIZE => Size;
    public TrainerClass7b(byte[] data) : base(data) { }
    public TrainerClass7b() : this(new byte[Size]) { }

    public byte Unk_0x00 { get => Data[0]; set => Data[0] = value; }
    public override int Group { get => Data[1]; set => Data[1] = (byte)value; }
    public override int BallID { get => Data[2]; set => Data[2] = (byte)value; }
    public byte Unk_0x03 { get => Data[3]; set => Data[3] = value; }

    // model hash at end (name, form, variation)?

    public override int MegaItemID => 773;
}
