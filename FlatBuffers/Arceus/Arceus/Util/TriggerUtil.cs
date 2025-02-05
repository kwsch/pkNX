namespace pkNX.Structures.FlatBuffers.Arceus;

public static class TriggerUtil
{
    public static IEnumerable<string> GetTriggerTableSummary(TriggerTable tab)
    {
        foreach (var trg in tab.Table)
        {
            yield return "Trigger:";
            foreach (var line in GetTriggerSummary(trg))
                yield return $"\t{line}";
        }
    }

    public static IEnumerable<string> GetTriggerSummary(Trigger trg)
    {
        yield return "Meta:";
        yield return $"\t{GetTriggerMetaSummary(trg.Meta)}";
        yield return "Conditions:";
        foreach (var line in GetTriggerConditionsSummary(trg.Conditions))
            yield return $"\t{line}";
        yield return "Commands:";
        foreach (var line in GetTriggerCommandsSummary(trg.Commands))
            yield return $"\t{line}";
    }

    public static string GetTriggerMetaSummary(TriggerMeta meta)
    {
        if (meta.Unused01 != 0)
            throw new ArgumentException("TriggerMeta has unused field set?");

        var argsSummary = GetTriggerArgsSummary([meta.TriggerMetaArg1, meta.TriggerMetaArg2, meta.TriggerMetaArg3]);

        return $"0x{(ulong)meta.TriggerTypeID:X16}({argsSummary})";
    }

    public static IEnumerable<string> GetTriggerConditionsSummary(IEnumerable<TriggerCondition> conds)
    {
        foreach (var cond in conds)
            yield return $"{cond.GetConditionTypeSummary()}: {cond.GetConditionSummary()}";
    }

    public static IEnumerable<string> GetTriggerCommandsSummary(IEnumerable<TriggerCommand> cmds)
    {
        foreach (var cmd in cmds)
            yield return GetTriggerCommandSummary(cmd);
    }

    public static string GetTriggerCommandSummary(TriggerCommand cmd)
    {
        var argsSummary = GetTriggerArgsSummary(cmd.Arguments);

        return $"0x{(ulong)cmd.CommandTypeID:X16}({argsSummary})";
    }

    public static string GetTriggerArgsSummary(IList<string> args)
    {
        var firstEmpty = -1;
        for (var i = 0; i < args.Count; i++)
        {
            if (firstEmpty >= 0 && !string.IsNullOrEmpty(args[i]))
                throw new ArgumentException($"Invalid TriggerArg at index {i}!");
            if (firstEmpty < 0 && string.IsNullOrEmpty(args[i]))
                firstEmpty = i;
        }

        return string.Join(", ", args.Select(s => $"\"{s}\"").Take(firstEmpty >= 0 ? firstEmpty : args.Count));
    }
}
