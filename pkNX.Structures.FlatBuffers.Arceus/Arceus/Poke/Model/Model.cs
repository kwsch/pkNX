using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmdl

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FileReference
{
    public override string ToString() => $"{{ {Filename} }}";

    public static FileReference Empty => new() { Filename = string.Empty };
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
    public static Model Empty => new()
    {
        Meshes = Array.Empty<FileReference>(),
        LODs = Array.Empty<LOD>(),
        Skeleton = FileReference.Empty,
        Materials = Array.Empty<string>(),
    };
}
