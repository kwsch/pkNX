using System.Collections.Generic;
using System.Text;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public static class ActivationConditionUtil9a
{
    public static string GetSummary(this IList<ActivationCondition>? list)
    {
        if (list == null)
            return "";
        var sb = new StringBuilder();
        foreach (var cond in list)
        {
            if (cond.Element is null)
                continue;
            sb.Append(GetSummary(cond.Element));
        }
        return sb.ToString();
    }

    private static string GetSummary(this IList<ActivationConditionElement> list)
    {
        var sb = new StringBuilder();
        foreach (var cond in list)
            sb.AppendJoin("&&", cond.Summarize());
        return sb.ToString();
    }

    public static string GetSummary(this IEnumerable<IList<ActivationConditionElement>?> list)
    {
        var sb = new StringBuilder();
        foreach (var element in list)
        {
            if (element == null)
                continue;
            sb.Append(GetSummary(element));
        }
        return sb.ToString();
    }
}
