using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmmt

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialSwitch
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialMapper
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EmbeddedTracm
{
}

//Appears only on Arceus?
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialProperty
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialTable
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MultiMaterialTable
{
    public static MultiMaterialTable Empty => new()
    {
        Material = Array.Empty<MaterialTable>(),
    };
}
