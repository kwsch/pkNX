using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trpokecfg

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeConfig;
