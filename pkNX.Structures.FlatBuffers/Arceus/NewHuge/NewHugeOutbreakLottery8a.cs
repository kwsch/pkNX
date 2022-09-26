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
    private const string OutbreakSetting = nameof(OutbreakSetting);

    [Category(Access), Description("Map hash")]
    [FlatBufferItem(00)] public ulong Hash { get; set; }

    [Category(Access), Description("Map outbreak lottery name")]
    [FlatBufferItem(01)] public string LotteryGroupString { get; set; } = string.Empty;

    [Category(MapSetting), Description("Percent chance a massive mass outbreak occurs for an area map if a mass outbreak is not generated.")]
    [FlatBufferItem(02)] public int OutbreakChance { get; set; }

    [Category(MapSetting), Description("Minimum amount of outbreak spots to create.")]
    [FlatBufferItem(03)] public int OutbreakTotalMin { get; set; }

    [Category(MapSetting), Description("Maximum amount of outbreak spots to create.")]
    [FlatBufferItem(04)] public int OutbreakTotalMax { get; set; }

    [Category(MapSetting), Description("Number of outbreaks that use the Rare2 table."), ]
    [FlatBufferItem(05)] public int OutbreakRare2 { get; set; }

    [Category(MapSetting), Description("Number of outbreaks that use the Rare1 table.")]
    [FlatBufferItem(06)] public int OutbreakRare1 { get; set; }


    [Category(OutbreakSetting), Description("Minimum number of Pokémon to spawn in the first wave.")]
    [FlatBufferItem(07)] public int CountFirstMin { get; set; }

    [Category(OutbreakSetting), Description("Maximum number of Pokémon to spawn in the first wave.")]
    [FlatBufferItem(08)] public int CountFirstMax { get; set; }

    [Category(OutbreakSetting), Description("Minimum number of Pokémon to spawn in the second wave.")]
    [FlatBufferItem(09)] public int CountSecondMin { get; set; }

    [Category(OutbreakSetting), Description("Maximum number of Pokémon to spawn in the second wave.")]
    [FlatBufferItem(10)] public int CountSecondMax { get; set; }

    [Category(MapSetting), Description("Chance to successfully upgrade an outbreak to have a second wave.")]    // Stars are checked before berries
    [FlatBufferItem(11)] public int ChanceExtraBonus { get; set; }

    [Category(MapSetting), Description("Chance for extra bonus outbreaks to be visible with a star. Tries Aguav Berry chance if failed.")]
    [FlatBufferItem(12)] public int ChanceExtraVisibleStar { get; set; }

    [Category(MapSetting), Description("Chance to upgrade an outbreak to Aguav Berry if no second wave.")]
    [FlatBufferItem(13)] public int ChanceAguavBerry { get; set; }

    [Category(OutbreakSetting), Description("Base shiny rolls.")]
    [FlatBufferItem(14)] public int RollBonus { get; set; }
    [FlatBufferItem(15)] public string[] SpawnerFlagNames { get; set; } = Array.Empty<string>(); // all FSYS_NEW_OUTBREAK_AREA0#_#

    [Category(MapSetting), Description("List of outbreak total thresholds to determine minimum stars.")]
    [FlatBufferItem(16)] public int[] OutbreakTotalThresholds { get; set; } = Array.Empty<int>();

    [Category(MapSetting), Description("List of star outbreak counts to give if an outbreak size is reached.")]
    [FlatBufferItem(17)] public int[] StarOutbreaksPerThreshold { get; set; } = Array.Empty<int>();
}
