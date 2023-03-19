using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaInfo
{
    public int ActualMinLevel => MinEncLv != 0 ? MinEncLv : 1;
    public int ActualMaxLevel => MaxEncLv != 0 ? MaxEncLv : 100;
}
