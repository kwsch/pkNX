using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

// No maps have data for this.
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8RotomRallyEntry
{
    [FlatBufferItem(0)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(1)] public uint Field_01 { get; set; }
}
