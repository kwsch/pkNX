using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TriggerTable;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Trigger;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TriggerMeta;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TriggerCondition : IHasCondition
{
    // For interface compat
    public string ConditionArg4 { get; set; } = string.Empty;
    public string ConditionArg5 { get; set; } = string.Empty;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TriggerCommand;
