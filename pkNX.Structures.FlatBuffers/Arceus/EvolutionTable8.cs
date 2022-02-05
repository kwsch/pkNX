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
public class EvolutionTable8 : IFlatBufferArchive<EvolutionSet8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public EvolutionSet8a[] Table { get; set; } = Array.Empty<EvolutionSet8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EvolutionSet8a
{
    [FlatBufferItem(0)] public ushort Index { get; set; }
    [FlatBufferItem(1)] public ushort Form { get; set; }
    [FlatBufferItem(2)] public EvolutionEntry8a[]? Table { get; set; } = Array.Empty<EvolutionEntry8a>();

    public byte[] Write()
    {
        if (Table is null)
            return Array.Empty<byte>();

        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);
        foreach (var evo in Table)
        {
            // ReSharper disable RedundantCast
            bw.Write((ushort)evo.Method);
            bw.Write((ushort)evo.Argument);
            bw.Write((ushort)evo.Species);
            bw.Write((sbyte)evo.Form);
            bw.Write((byte)evo.Level);
            // ReSharper restore RedundantCast
        }
        return ms.ToArray();
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EvolutionEntry8a
{
    [FlatBufferItem(0)] public ushort Method { get; set; }
    [FlatBufferItem(1)] public ushort Argument { get; set; }
    [FlatBufferItem(2)] public ushort Species { get; set; }
    [FlatBufferItem(3)] public byte Form { get; set; }
    [FlatBufferItem(4)] public byte Level { get; set; }
}
