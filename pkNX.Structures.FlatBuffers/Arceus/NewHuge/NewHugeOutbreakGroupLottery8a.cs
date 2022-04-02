using System;
using System.ComponentModel;
using System.Linq;
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
        var group = Array.Find(Table, z => z.LotteryGroup == hash);
        if (group == null)
            return false;

        return group.IsUseTable(groups, tableID);
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLottery8a
{
    [FlatBufferItem(00)] public ulong LotteryGroup { get; set; }
    [FlatBufferItem(01)] public NewHugeOutbreakGroupLotteryDetail8a[] Table1 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(02)] public NewHugeOutbreakGroupLotteryDetail8a[] Table2 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(03)] public NewHugeOutbreakGroupLotteryDetail8a[] Table3 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();

    public int SumTable1 => Table1.Sum(z => z.Rate);
    public int SumTable2 => Table1.Sum(z => z.Rate);
    public int SumTable3 => Table1.Sum(z => z.Rate);

    public bool IsUseTable(NewHugeOutbreakGroupArchive8a groups, ulong tableID)
    {
        foreach (var lotteryChoice in Table1)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        foreach (var lotteryChoice in Table2)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        foreach (var lotteryChoice in Table3)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        return false;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLotteryDetail8a
{
    [FlatBufferItem(00)] public ulong Group { get; set; }
    [FlatBufferItem(01)] public int Rate { get; set; }

    public override string ToString() => $"{Group:X16}|{Rate}";

    public bool UsesTable(NewHugeOutbreakGroupArchive8a groups, ulong tableID)
    {
        var lottery = Array.Find(groups.Table, z => z.Group == Group);
        if (lottery is null)
            return false;
        return lottery.UsesTable(tableID);
    }
}
