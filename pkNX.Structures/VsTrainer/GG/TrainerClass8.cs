using System.Text;

namespace pkNX.Structures;

public class TrainerClass8(byte[] data) : TrainerClass(data)
{
    // 4 bytes used
    // 4 bytes unused (align?)
    // 10 bytes hash (model?)
    // 0x80 bytes string1 file reference (eyecatch & bgm)
    // 0x80 bytes string2 file reference (battle sequence)

    private const int Size = 280;
    public sealed override int SIZE => 280;
    public TrainerClass8() : this(new byte[Size]) { }

    public byte Unk_0x00 { get => Data[0]; set => Data[0] = value; } // bool?
    public override int Group { get => Data[1]; set => Data[1] = (byte)value; }
    public override int BallID { get => Data[2]; set => Data[2] = (byte)value; }
    public byte Unk_0x03 { get => Data[3]; set => Data[3] = value; } // bool?

    // model hash (name, form, variation)?
    public byte[] Hash => Data.Slice(0x8, 0x10);

    // music?
    public string S1 => Encoding.ASCII.GetString(Data, 0x18, 0x80).TrimEnd('\0');

    // sequence?
    public string S2 => Encoding.ASCII.GetString(Data, 0x88, 0x80).TrimEnd('\0');
}
