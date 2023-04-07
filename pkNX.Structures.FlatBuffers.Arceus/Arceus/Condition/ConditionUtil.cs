using System;
using System.Linq;

namespace pkNX.Structures.FlatBuffers.Arceus;

public static class ConditionUtil
{
    public static string GetConditionTypeSummary(this IHasCondition cond)
    {
        // todo lookup
        return $"{cond.ConditionTypeID}";
    }

    public static string GetConditionArgsSummary(this IHasCondition cond)
    {
        var args = new[] { cond.ConditionArg1, cond.ConditionArg2, cond.ConditionArg3, cond.ConditionArg4, cond.ConditionArg5 };

        // Sanity check for empty entries
        var firstEmpty = Array.FindIndex(args, string.IsNullOrEmpty);
        for (var i = firstEmpty + 1; i < args.Length; i++)
        {
            if (!string.IsNullOrEmpty(args[i]))
                throw new ArgumentException($"Invalid ConditionArg at index {i}!");
        }

        return string.Join(", ", args.Select(s => $"\"{s}\"").Take(firstEmpty >= 0 ? firstEmpty : args.Length));
    }

    public static string GetConditionSummary(this IHasCondition cond)
    {
        // todo lookup
        return $"{cond.ConditionID}({GetConditionArgsSummary(cond)})";
    }
}
