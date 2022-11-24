using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FixedSymbolGeneration
{
    [FlatBufferItem(0)] public float MinCreateDistance { get; set; }
    [FlatBufferItem(1)] public float MaxCreateDistance { get; set; }
    [FlatBufferItem(2)] public float MinDestroyDistance { get; set; }
    [FlatBufferItem(3)] public float MaxDestroyDistance { get; set; }
    [FlatBufferItem(4)] public GenerationPattern GenerationPattern { get; set; }
    [FlatBufferItem(5)] public bool FirstGenerate { get; set; }
    [FlatBufferItem(6)] public int RepopProbability { get; set; }
}
