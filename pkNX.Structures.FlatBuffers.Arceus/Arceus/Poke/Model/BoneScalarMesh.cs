using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trbsm

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class UnkEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class BoneEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ScalerEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class BoneMeshScalar { }
