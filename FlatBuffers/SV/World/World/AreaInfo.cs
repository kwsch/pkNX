namespace pkNX.Structures.FlatBuffers.SV;

public partial class AreaInfo
{
    public int ActualMinLevel => MinEncLv != 0 ? MinEncLv : 1;
    public int ActualMaxLevel => MaxEncLv != 0 ? MaxEncLv : 100;
}
