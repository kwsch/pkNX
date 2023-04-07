using System;
using System.ComponentModel;

// ReSharper disable ClassNeverInstantiated.Global
namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FlatDummyEntry
{
    // Throw on ctor as all arrays of this object type should never have any entries.
    public FlatDummyEntry() => throw new ArgumentException("Cannot create an instance of a dummy object.");
    public override string ToString() => "UNUSED ARRAY";
}
