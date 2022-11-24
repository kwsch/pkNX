using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupStyleDataArray : IFlatBufferArchive<DressupStyleData>
{
    [FlatBufferItem(0)] public DressupStyleData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupStyleData
{
    [FlatBufferItem(0)] public int DressupStyleId { get; set; }
    [FlatBufferItem(1)] public DressupStyleType DressupStyleType { get; set; }
    [FlatBufferItem(2)] public int Sortnum { get; set; }
    [FlatBufferItem(3)] public string Name { get; set; }
    [FlatBufferItem(4)] public int PatternNum { get; set; }
    [FlatBufferItem(5)] public int MultiplePatternNum { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum DressupStyleType
{
    Eye = 0,
    EyeColor = 1,
    Eyelashes = 2,
    EyelashesColor = 3,
    Eyebrows = 4,
    EyeBrowsColor = 5,
    Mouth = 6,
    Lip = 7,
    Mole = 8,
    Freckles = 9,
}
