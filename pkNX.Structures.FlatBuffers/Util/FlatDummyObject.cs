using System;
using System.ComponentModel;
using System.Diagnostics;
using FlatSharp.Attributes;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable once UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FlatDummyObject
{
    // Throw on first (fake) field set action to ensure the deserialized data has no fields.
    [FlatBufferItem(0), Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public byte Field_00
    {
        get => 0;
        set
        {
            if (value != 0)
                throw new ArgumentException("This should always be an empty object, and not have a valid vTable.");
        }
    }

    public override string ToString() => "UNUSED OBJECT: NO FIELD DATA";

    public FlatDummyObject Clone() => this;
}
