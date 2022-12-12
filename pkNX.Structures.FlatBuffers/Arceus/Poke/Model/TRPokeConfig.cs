using FlatSharp.Attributes;
using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers;

// *.trpokecfg

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRPokeConfig
{
    [FlatBufferItem(00, DefaultValue = SpeciesSize.Size_M)] public SpeciesSize SizeIndex { get; set; } = SpeciesSize.Size_M;

    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }

    [FlatBufferItem(04)] public float InframeVerticalRotYOrigin { get; set; }
    [FlatBufferItem(05)] public float InframeBottomYOffset { get; set; }
    [FlatBufferItem(06)] public float InframeCenterYOffset { get; set; }

    [FlatBufferItem(07)] public float InframeLeftRotation { get; set; }
    [FlatBufferItem(08)] public float InframeRightRotation { get; set; }
    [FlatBufferItem(09)] public float Field_09 { get; set; } // Always default

    [FlatBufferItem(10)] public float Field_10_YOffset { get; set; }
    [FlatBufferItem(11)] public float Field_11_YOffset { get; set; }
    [FlatBufferItem(12)] public float Field_12_YOffset { get; set; }
}
