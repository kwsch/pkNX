using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

// Root Type: titan_audio.fb.EnvPokeVoiceLotterySetting

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EnvPokeVoiceLotterySetting
{
    [FlatBufferItem(0)] public float LotteryTimer { get; set; }
    [FlatBufferItem(1)] public float SearchRadiusMin { get; set; }
    [FlatBufferItem(2)] public float SearchRadiusMax { get; set; }
}
