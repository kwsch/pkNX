using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmdl


[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FileReference
{
    public override string ToString() => $"{{ {Filename} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LODIndex
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LOD
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Model
{
}
