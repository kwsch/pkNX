using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PersonalTable;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PersonalInfo;
