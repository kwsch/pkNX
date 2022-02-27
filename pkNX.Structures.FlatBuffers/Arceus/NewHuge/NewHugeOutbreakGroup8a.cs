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
public class NewHugeOutbreakGroupLotteryArchive8a : IFlatBufferArchive<NewHugeOutbreakGroupLottery8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakGroupLottery8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakGroupLottery8a>();

    public bool IsAreaGroup(PlacementSpawner8a spawner, NewHugeOutbreakGroupArchive8a groups, ulong tableID)
    {
        var hash = spawner.Field_20_Value.EncounterTableID;
        foreach (var t in Table)
        {
            if (t.LotteryGroup != hash)
                continue;

            foreach (var lotteryChoice in t.Table1)
            {
                var group = lotteryChoice.Group;
                var lottery = Array.Find(groups.Table, z => z.Group == group);
                if (lottery is null)
                    continue;
                var tables = lottery.Table;
                var matches = Array.Find(tables, z => z.EncounterTableID == tableID);
                if (matches is not null)
                    return true;
            }
            foreach (var lotteryChoice in t.Table2)
            {
                var group = lotteryChoice.Group;
                var lottery = Array.Find(groups.Table, z => z.Group == group);
                if (lottery is null)
                    continue;
                var tables = lottery.Table;
                var matches = Array.Find(tables, z => z.EncounterTableID == tableID);
                if (matches is not null)
                    return true;
            }
            foreach (var lotteryChoice in t.Table3)
            {
                var group = lotteryChoice.Group;
                var lottery = Array.Find(groups.Table, z => z.Group == group);
                if (lottery is null)
                    continue;
                var tables = lottery.Table;
                var matches = Array.Find(tables, z => z.EncounterTableID == tableID);
                if (matches is not null)
                    return true;
            }
        }
        return false;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLottery8a
{
    [FlatBufferItem(00)] public ulong LotteryGroup { get; set; }
    [FlatBufferItem(01)] public NewHugeOutbreakGroupLotteryDetail8a[] Table1 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(02)] public NewHugeOutbreakGroupLotteryDetail8a[] Table2 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(03)] public NewHugeOutbreakGroupLotteryDetail8a[] Table3 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLotteryDetail8a
{
    [FlatBufferItem(00)] public ulong Group { get; set; }
    [FlatBufferItem(01)] public int Rate { get; set; }
}
