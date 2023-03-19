using System;
using System.ComponentModel;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NewHugeOutbreakGroupLotteryArchive
{
    public bool IsAreaGroup(PlacementSpawner spawner, NewHugeOutbreakGroupArchive groups, ulong tableID)
    {
        var hash = spawner.Field20Value.EncounterTableID;
        var group = Table.FirstOrDefault(z => z.LotteryGroup == hash);
        if (group == null)
            return false;

        return group.IsUseTable(groups, tableID);
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NewHugeOutbreakGroupLottery
{
    public int SumTableCommon => TableCommon.Sum(z => z.Rate);
    public int SumTableRare1 => TableRare1.Sum(z => z.Rate);
    public int SumTableRare2 => TableRare2.Sum(z => z.Rate);

    public bool IsUseTable(NewHugeOutbreakGroupArchive groups, ulong tableID)
    {
        foreach (var lotteryChoice in TableCommon)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        foreach (var lotteryChoice in TableRare1)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        foreach (var lotteryChoice in TableRare2)
        {
            if (lotteryChoice.UsesTable(groups, tableID))
                return true;
        }
        return false;
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NewHugeOutbreakGroupLotteryDetail
{
    public override string ToString() => $"{Group:X16}|{Rate}";

    public bool UsesTable(NewHugeOutbreakGroupArchive groups, ulong tableID)
    {
        var lottery = groups.Table.FirstOrDefault(z => z.Group == Group);
        if (lottery is null)
            return false;
        return lottery.UsesTable(tableID);
    }
}
