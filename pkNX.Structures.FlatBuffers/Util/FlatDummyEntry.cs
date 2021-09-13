using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable ClassNeverInstantiated.Global

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class FlatDummyEntry
    {
        // Throw on ctor as all arrays of this object type should never have any entries.
        public FlatDummyEntry() => throw new ArgumentException("Cannot create an instance of a dummy object.");
        public override string ToString() => "UNUSED ARRAY";
    }
}
