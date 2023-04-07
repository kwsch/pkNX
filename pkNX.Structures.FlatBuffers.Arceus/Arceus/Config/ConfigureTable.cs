using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ConfigArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ConfigEntry
{
    public string ConfiguredValue
    {
        get => Parameters[0];
        set => Parameters[0] = value;
    }
}
