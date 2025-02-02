using System;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneMovementPathHolder;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_V3f
{
    public string Location3f => $"({LocationX}, {LocationY}, {LocationZ})";

    public override string ToString() => Location3f;
}
