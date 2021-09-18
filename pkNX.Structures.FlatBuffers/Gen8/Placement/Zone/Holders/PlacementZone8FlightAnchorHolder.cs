using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    // player flying to location
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FlightAnchorHolder
    {
        [FlatBufferItem(00)] public PlacementZone8FlightAnchor FlightAnchor { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FlightAnchor
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Placement { get; set; } = new();
        [FlatBufferItem(01)] public ulong UnlockFlagHash { get; set; }
    }
}
