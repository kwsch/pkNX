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
    // pm0059_00_41, pm0112_00_00, pm0143_00_00, pm0226_00_00, pm0365_00_00, pm0389_00_00, pm0450_00_00, pm0452_00_00, pm0460_00_00 == 2
    // pm0095_00_00, pm0130_00_00, pm0208_00_00, pm0486_00_00, pm0751_00_41, pm1007_00_00 == 3
    [FlatBufferItem(00)] public byte SomeTypeEnum_023 { get; set; }

    [FlatBufferItem(01)] public float RandOffsetScaleMin { get; set; }
    [FlatBufferItem(02)] public float RandOffsetScaleMax { get; set; }
    [FlatBufferItem(03)] public float BaseScale { get; set; }

    [FlatBufferItem(04)] public float InframeVerticalRotYOrigin { get; set; }
    [FlatBufferItem(05)] public float InframeBottomYOffset { get; set; }
    [FlatBufferItem(06)] public float InframeCenterYOffset { get; set; }

    [FlatBufferItem(07)] public float InframeLeftRotation { get; set; }
    [FlatBufferItem(08)] public float InframeRightRotation { get; set; }
    [FlatBufferItem(09)] public float BaseX { get; set; }

    [FlatBufferItem(10)] public float RandOffsetYMin { get; set; }
    [FlatBufferItem(11)] public float RandOffsetYMax { get; set; }
    [FlatBufferItem(12)] public float BaseY { get; set; }
}
