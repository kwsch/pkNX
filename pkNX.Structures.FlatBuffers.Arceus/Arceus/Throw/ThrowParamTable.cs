using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ThrowParamTable;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ThrowParam
{
    public string Dump() => $"{ThrowParamType:X16}\t{Velocity}\t{Arc}\t{GravityDirection}\t{ThrowAngle}";
}
