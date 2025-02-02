// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

public interface IHasCondition
{
    ulong ConditionTypeID { get; set; }
    ulong ConditionID { get; set; }
    string ConditionArg1 { get; set; }
    string ConditionArg2 { get; set; }
    string ConditionArg3 { get; set; }
    string ConditionArg4 { get; set; }
    string ConditionArg5 { get; set; }
}
