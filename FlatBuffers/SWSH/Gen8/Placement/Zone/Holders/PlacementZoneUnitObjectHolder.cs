using System;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

// Gates, Elevators, Tents, Flags, FossilRepair?
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneUnitObjectHolder
{
    public override string ToString() => Object.NameModel;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneUnitObject;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneUnitObjectDetails;

// probably a union, with only 1 object type used
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneUnitObjectToggle;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneUnitObjectInner;
