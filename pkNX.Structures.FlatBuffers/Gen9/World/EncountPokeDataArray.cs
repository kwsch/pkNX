using FlatSharp.Attributes;
using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncountPokeDataArray : IFlatBufferArchive<EncountPokeData>
{
    [FlatBufferItem(0)] public EncountPokeData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncountPokeData
{
    [FlatBufferItem(00)] public DevID DevId { get; set; }
    [FlatBufferItem(01)] public SexType Sex { get; set; }
    [FlatBufferItem(02)] public sbyte Form { get; set; }
    [FlatBufferItem(03)] public short MinLevel { get; set; }
    [FlatBufferItem(04)] public short MaxLevel { get; set; }
    [FlatBufferItem(05)] public short LotValue { get; set; }
    [FlatBufferItem(06)] public Biome Biome1 { get; set; }
    [FlatBufferItem(07)] public short LotValue1 { get; set; }
    [FlatBufferItem(08)] public Biome Biome2 { get; set; }
    [FlatBufferItem(09)] public short LotValue2 { get; set; }
    [FlatBufferItem(10)] public Biome Biome3 { get; set; }
    [FlatBufferItem(11)] public short LotValue3 { get; set; }
    [FlatBufferItem(12)] public Biome Biome4 { get; set; }
    [FlatBufferItem(13)] public short LotValue4 { get; set; }
    [FlatBufferItem(14)] public string Area { get; set; }
    [FlatBufferItem(15)] public string LocationName { get; set; }
    [FlatBufferItem(16)] public int MinHeight { get; set; }
    [FlatBufferItem(17)] public int MaxHeight { get; set; }
    [FlatBufferItem(18)] public EnableTable EnableTable { get; set; }
    [FlatBufferItem(19)] public TimeTable TimeTable { get; set; }
    [FlatBufferItem(20)] public string FlagName { get; set; }
    [FlatBufferItem(21)] public short BandRate { get; set; }
    [FlatBufferItem(22)] public BandType BandType { get; set; }
    [FlatBufferItem(23)] public DevID BandPoke { get; set; }
    [FlatBufferItem(24)] public SexType BandSex { get; set; }
    [FlatBufferItem(25)] public sbyte BandForm { get; set; }
    [FlatBufferItem(26)] public sbyte OutbreakLotValue { get; set; }
    [FlatBufferItem(27)] public string PokeVoiceClassification { get; set; }
    [FlatBufferItem(28)] public VersionTable VersionTable { get; set; }
    [FlatBufferItem(29)] public BringItem BringItem { get; set; }
}
