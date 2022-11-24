using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupStylePresetDataArray : IFlatBufferArchive<DressupStylePresetData>
{
    [FlatBufferItem(0)] public DressupStylePresetData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressupStylePresetData
{
    [FlatBufferItem(00)] public string Base { get; set; }
    [FlatBufferItem(01)] public string Acc { get; set; }
    [FlatBufferItem(02)] public string Bag { get; set; }
    [FlatBufferItem(03)] public string EyeWear { get; set; }
    [FlatBufferItem(04)] public string Foot { get; set; }
    [FlatBufferItem(05)] public string Glove { get; set; }
    [FlatBufferItem(06)] public string Head { get; set; }
    [FlatBufferItem(07)] public string Hair { get; set; }
    [FlatBufferItem(08)] public string Leg { get; set; }
    [FlatBufferItem(09)] public string Uniform { get; set; }
    [FlatBufferItem(10)] public string Face { get; set; }
    [FlatBufferItem(11)] public int Gender { get; set; }
    [FlatBufferItem(12)] public int SkinColor { get; set; }
    [FlatBufferItem(13)] public int Lip { get; set; }
    [FlatBufferItem(14)] public int EyeColor { get; set; }
    [FlatBufferItem(15)] public int Eye { get; set; }
    [FlatBufferItem(16)] public int EyeBrowsColor { get; set; }
    [FlatBufferItem(17)] public int EyeBrows { get; set; }
    [FlatBufferItem(18)] public int EyeBrowsVolume { get; set; }
    [FlatBufferItem(19)] public int EyeLashesColor { get; set; }
    [FlatBufferItem(20)] public int EyeLashes { get; set; }
    [FlatBufferItem(21)] public int EyeLashesVolume { get; set; }
    [FlatBufferItem(22)] public int Mouth { get; set; }
    [FlatBufferItem(23)] public int Mole { get; set; }
    [FlatBufferItem(24)] public int Freckles { get; set; }
    [FlatBufferItem(25)] public int HairColor { get; set; }
}
