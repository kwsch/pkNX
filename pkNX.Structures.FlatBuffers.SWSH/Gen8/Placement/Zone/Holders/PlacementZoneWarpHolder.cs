using System;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneWarpHolder
{
    public override string ToString() => $"{Field00.NameAreaOther} via {Field00.NameModel}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneWarp8 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneWarpDetails8 { }
