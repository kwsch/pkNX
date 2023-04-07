using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelSet { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelSetEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelAnimations { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelMeshes { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelRigs { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NPCModelColorConfig { }
