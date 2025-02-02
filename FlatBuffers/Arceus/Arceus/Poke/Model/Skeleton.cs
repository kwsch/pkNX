using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trskl

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Skeleton;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TransformNode;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class IKControl;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Bone;
