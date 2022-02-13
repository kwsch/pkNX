using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures.FlatBuffers
{
    public static class Trigger8aUtil
    {
        public static IEnumerable<string> GetTriggerTableSummary(TriggerTable8a tab)
        {
            foreach (var trg in tab.Table)
            {
                yield return "Trigger:";
                foreach (var line in GetTriggerSummary(trg))
                    yield return $"\t{line}";
            }
        }

        public static IEnumerable<string> GetTriggerSummary(Trigger8a trg)
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

        public static string GetTriggerMetaSummary(TriggerMeta8a meta)
        {
            if (meta.Unused_01 != 0)
                throw new ArgumentException("TriggerMeta has unused field set?");

            var argsSummary = GetTriggerArgsSummary(meta.TriggerMetaArg1, meta.TriggerMetaArg2, meta.TriggerMetaArg3);

            if (Enum.IsDefined(typeof(TriggerType8a), meta.TriggerTypeID))
                return $"{meta.TriggerTypeID}({argsSummary})";
            else 
                return $"0x{(ulong)meta.TriggerTypeID:X16}({argsSummary})";
        }

        public static IEnumerable<string> GetTriggerConditionsSummary(IEnumerable<TriggerCondition8a> conds)
        {
            foreach (var cond in conds)
                yield return $"{Condition8aUtil.GetConditionTypeSummary(cond)}: {Condition8aUtil.GetConditionSummary(cond)}";
        }

        public static IEnumerable<string> GetTriggerCommandsSummary(IEnumerable<TriggerCommand8a> cmds)
        {
            foreach (var cmd in cmds)
                yield return GetTriggerCommandSummary(cmd);
        }

        public static string GetTriggerCommandSummary(TriggerCommand8a cmd)
        {
            var argsSummary = GetTriggerArgsSummary(cmd.Arguments);

            if (Enum.IsDefined(typeof(TriggerCommandType8a), cmd.CommandTypeID))
                return $"{cmd.CommandTypeID}({argsSummary})";
            else
                return $"0x{(ulong)cmd.CommandTypeID:X16}({argsSummary})";
        }

        public static string GetTriggerArgsSummary(params string[] args)
        {
            var firstEmpty = -1;
            for (var i = 0; i < args.Length; i++)
            {
                if (firstEmpty >= 0 && !string.IsNullOrEmpty(args[i]))
                    throw new ArgumentException($"Invalid TriggerArg at index {i}!");
                else if (firstEmpty < 0 && string.IsNullOrEmpty(args[i]))
                    firstEmpty = i;
            }

            return string.Join(", ", args.Select(s => $"\"{s}\"").Take(firstEmpty >= 0 ? firstEmpty : args.Length));
        }
    }
}
