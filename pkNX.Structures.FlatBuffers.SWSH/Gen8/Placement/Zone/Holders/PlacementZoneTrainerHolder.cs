using System;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneTrainerHolder
{
    public override string ToString()
    {
        var hashModel = Field00.Field00.HashModel;
        var name = PlacementZoneOtherNPCHolder.Models.TryGetValue(hashModel, out var model) ? model : hashModel.ToString("X16");
        return name;
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F08_ArrayEntry;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F08_Nine;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F08;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F08_A;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F08_IntFloat;
