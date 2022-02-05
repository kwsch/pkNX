using System;
using System.ComponentModel;
using System.IO;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Learnset8a : IFlatBufferArchive<Learnset8aMeta>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public Learnset8aMeta[] Table { get; set; } = Array.Empty<Learnset8aMeta>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Learnset8aMeta
{
    [FlatBufferItem(0)] public ushort Species { get; set; }
    [FlatBufferItem(1)] public ushort Form { get; set; }
    [FlatBufferItem(2)] public Learnset8aEntry[] Mainline { get; set; } = Array.Empty<Learnset8aEntry>();
    [FlatBufferItem(3)] public Learnset8aEntry[] Arceus { get; set; } = Array.Empty<Learnset8aEntry>();

    public byte[] WriteAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        foreach (var entry in Arceus)
        {
            br.Write(entry.Move);
            br.Write(entry.Level);
        }
        br.Write(-1);
        return ms.ToArray();
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Learnset8aEntry
{
    [FlatBufferItem(0)] public ushort Level { get; set; }
    [FlatBufferItem(1)] public ushort LevelMaster { get; set; }
    [FlatBufferItem(2)] public ushort Move { get; set; }
}
