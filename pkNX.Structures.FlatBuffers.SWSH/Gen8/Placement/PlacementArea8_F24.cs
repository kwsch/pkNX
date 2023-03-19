using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementArea_F24
{
    public override string ToString() => $"{Field00}, {Field02}: {Field01}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementAreaUnknownTiny
{
    public override string ToString() => $"{Field00}, {Field01}, {Field02}";
}
