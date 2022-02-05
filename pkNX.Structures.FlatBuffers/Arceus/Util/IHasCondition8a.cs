// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

public interface IHasCondition8a
{
    ConditionType8a ConditionTypeID { get; set; }
    Condition8a ConditionID { get; set; }
    string ConditionArg1 { get; set; }
    string ConditionArg2 { get; set; }
    string ConditionArg3 { get; set; }
    string ConditionArg4 { get; set; }
    string ConditionArg5 { get; set; }
}
