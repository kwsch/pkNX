using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FishingPointHolder
    {
        [FlatBufferItem(00)] public PlacementZone8FishingPoint Object { get; set; } = new();

        public override string ToString() => $"{Object.Identifier}" + (Object.IterateForSlotsExceptLastN == 0 ? "" : $" SkipLast{Object.IterateForSlotsExceptLastN}");
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FishingPoint
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Identifier { get; set; } = new();
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public float Field_05 { get; set; }
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; }

        [FlatBufferItem(08), Description("When iterating over slots to pick a random one, the iteration will skip the last (value) amount of slots.")]
        public uint IterateForSlotsExceptLastN { get; set; }
    }
}
