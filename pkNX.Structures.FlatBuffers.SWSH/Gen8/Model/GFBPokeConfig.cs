using FlatSharp.Attributes;
using System;
using System.ComponentModel;
using FlatSharp;

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshProperty
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshEntries
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SpeciesModelProperties
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialEntry
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class GFBPokeConfig
{
    public AABB Bounds => new AABB() { Min = new() { X = MinBX / 100, Y = MinBY / 100, Z = MinBZ / 100 }, Max = new() { X = MaxBX / 100, Y = MaxBY / 100, Z = MaxBZ / 100 } };
}
