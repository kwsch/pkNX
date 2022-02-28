using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakLotteryArchive8a : IFlatBufferArchive<NewHugeOutbreakLottery8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakLottery8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakLottery8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakLottery8a
{
    private const string Access = nameof(Access);
    private const string MapSetting = nameof(MapSetting);

    [Category(Access), Description("Map hash")]
    [FlatBufferItem(00)] public ulong Hash { get; set; }

    [Category(Access), Description("Map outbreak lottery name")]
    [FlatBufferItem(01)] public string LotteryGroupString { get; set; } = string.Empty;

    [Category(MapSetting), Description("Percent chance a New Huge Outbreak occurrs for an area map.")]
    [FlatBufferItem(02)] public int OutbreakChance { get; set; }

    [Category(MapSetting), Description("Minimum amount of Outbreak spots to create.")]
    [FlatBufferItem(03)] public int OutbreakTotalMin { get; set; }

    [Category(MapSetting), Description("Maximum amount of Outbreak spots to create.")]
    [FlatBufferItem(04)] public int OutbreakTotalMax { get; set; }

    [Category(MapSetting), Description("Minimum amount of Outbreak spots with the special star mark."), ]
    [FlatBufferItem(05)] public int OutbreakStarMin { get; set; }

    [Category(MapSetting), Description("Maximum amount of Outbreak spots with the special star mark.")]
    [FlatBufferItem(06)] public int OutbreakStarMax { get; set; }

    [FlatBufferItem(07)] public int MinCountFirst { get; set; }
    [FlatBufferItem(08)] public int MaxCountFirst { get; set; }
    [FlatBufferItem(09)] public int MinCountSecond { get; set; }
    [FlatBufferItem(10)] public int MaxCountSecond { get; set; }
    [FlatBufferItem(11)] public int Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public int BerryChance { get; set; }
    [FlatBufferItem(14)] public int RollBonus { get; set; }
    [FlatBufferItem(15)] public string[] SpawnerFlagNames { get; set; } = Array.Empty<string>(); // all FSYS_NEW_OUTBREAK_AREA0#_#
    [FlatBufferItem(16)] public int[] Field_16 { get; set; } = Array.Empty<int>();
    [FlatBufferItem(17)] public int[] Field_17 { get; set; } = Array.Empty<int>();

    // Stars are checked before berries
    public int F17Min => Field_17[0];
    public int F17Max => Field_17[1];
}
