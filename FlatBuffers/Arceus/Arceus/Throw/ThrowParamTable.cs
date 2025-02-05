namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class ThrowParam
{
    public string Dump() => $"{ThrowParamType:X16}\t{Velocity}\t{Arc}\t{GravityDirection}\t{ThrowAngle}";
}
