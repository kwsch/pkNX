using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneFishingPointHolder
{
    public override string ToString() => $"{Object.Identifier}" + (Object.IterateForSlotsExceptLastN == 0 ? "" : $" SkipLast{Object.IterateForSlotsExceptLastN}");
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneFishingPoint { }
