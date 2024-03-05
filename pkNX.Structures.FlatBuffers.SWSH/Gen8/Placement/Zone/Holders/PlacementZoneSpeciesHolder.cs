using System;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneSpeciesHolder
{
    public override string ToString() => $"{(Species)Species}{(Form != 0 ? $"-{Form}" : "")}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F02_Nine;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F02;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F02_Field1;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F02_Inner;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F02_IntFloat;
