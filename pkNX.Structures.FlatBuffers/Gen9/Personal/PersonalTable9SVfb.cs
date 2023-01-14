using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.SV"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalTable9SVfb : IFlatBufferArchive<PersonalInfo9SVfb>
{
    [FlatBufferItem(0)] public PersonalInfo9SVfb[] Table { get; set; } = Array.Empty<PersonalInfo9SVfb>();
}

/// <summary>
/// Personal Info class with values from the <see cref="GameVersion.SV"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVfb
{
    [FlatBufferItem(00)] public PersonalInfo9SVInfo Info { get; set; } = new();
    [FlatBufferItem(01)] public bool IsPresentInGame { get; set; } // byte
    [FlatBufferItem(02)] public PersonalInfo9SVDex? Dex { get; set; }
    [FlatBufferItem(03)] public byte Type1 { get; set; } // byte
    [FlatBufferItem(04)] public byte Type2 { get; set; } // byte
    [FlatBufferItem(05)] public ushort Ability1 { get; set; } // ushort
    [FlatBufferItem(06)] public ushort Ability2 { get; set; } // ushort
    [FlatBufferItem(07)] public ushort AbilityH { get; set; } // ushort
    [FlatBufferItem(08)] public byte EXPGrowth { get; set; } // byte
    [FlatBufferItem(09)] public byte CatchRate { get; set; } // byte
    [FlatBufferItem(10)] public PersonalInfo9SVGender Gender { get; set; } = new();
    [FlatBufferItem(11)] public byte EggGroup1 { get; set; } // byte
    [FlatBufferItem(12)] public byte EggGroup2 { get; set; } // byte
    [FlatBufferItem(13)] public PersonalInfo9SVHatch Hatch { get; set; } = new();
    [FlatBufferItem(14)] public byte HatchCycles { get; set; } // byte
    [FlatBufferItem(15)] public byte BaseFriendship { get; set; } // byte
    [FlatBufferItem(16)] public short BaseEXPAddend { get; set; } // short
    [FlatBufferItem(17)] public byte EvoStage { get; set; } // byte
    [FlatBufferItem(18)] public bool IsTypeChangeDisallowed { get; set; } // Silvally, Arceus (and Sylveon in 1.0.0, fixed)
    [FlatBufferItem(19)] public PersonalInfo9SVStats EVYield { get; set; } = new();
    [FlatBufferItem(20)] public PersonalInfo9SVStats Base { get; set; } = new();
    [FlatBufferItem(21)] public PersonalInfo9SVEvolutions[] Evolutions { get; set; } = Array.Empty<PersonalInfo9SVEvolutions>();
    [FlatBufferItem(22)] public ushort[] TechnicalMachine { get; set; } = Array.Empty<ushort>();
    [FlatBufferItem(23)] public ushort[] EggMoves { get; set; } = Array.Empty<ushort>();
    [FlatBufferItem(24)] public ushort[] ReminderMoves { get; set; } = Array.Empty<ushort>();
    [FlatBufferItem(25)] public PersonalInfo9SVMove[] Learnset { get; set; } = Array.Empty<PersonalInfo9SVMove>();
}
