using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldPlayer
{
    [FlatBufferItem(00)] public GravityParameter Gravity { get; set; }
    [FlatBufferItem(01)] public InputParameter Input { get; set; }
    [FlatBufferItem(02)] public MoveParameter Move { get; set; }
    [FlatBufferItem(03)] public TotterParameter Totter { get; set; }
    [FlatBufferItem(04)] public GroundParameter Ground { get; set; }
    [FlatBufferItem(05)] public SlideDropParameter SlideDrop { get; set; }
    [FlatBufferItem(06)] public LadderParameter Ladder { get; set; }
    [FlatBufferItem(07)] public FacialParameter Facial { get; set; }
    [FlatBufferItem(08)] public PlayerUniqueParameter Unique { get; set; }
    [FlatBufferItem(09)] public GroundKinesisParameter GroundKinesis { get; set; }
    [FlatBufferItem(10)] public AerialKinesisParameter AerialKinesis { get; set; }
    [FlatBufferItem(11)] public SlideDropKinesisParameter SlideDropKinesis { get; set; }
    [FlatBufferItem(12)] public SquatKinesisParameter SquatKinesis { get; set; }
    [FlatBufferItem(13)] public SlidingKinesisParameter SlidingKinesis { get; set; }
    [FlatBufferItem(14)] public SelfieParameter Selfie { get; set; }
    [FlatBufferItem(15)] public FloatationParameter Floatation { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TotterParameter
{
    [FlatBufferItem(0)] public float Interval { get; set; }
    [FlatBufferItem(1)] public float Height { get; set; }
    [FlatBufferItem(2)] public float DistanceMax { get; set; }
    [FlatBufferItem(3)] public float BackDistance { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SelfieParameter
{
    [FlatBufferItem(0)] public float MaxFactor { get; set; }
    [FlatBufferItem(1)] public float RotAngleMax { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlayerUniqueParameter
{
    [FlatBufferItem(00)] public float IdlingIntervalMin { get; set; }
    [FlatBufferItem(01)] public float IdlingIntervalMax { get; set; }
    [FlatBufferItem(02)] public int FacialIdlingCountMin { get; set; }
    [FlatBufferItem(03)] public int FacialIdlingCountMax { get; set; }
    [FlatBufferItem(04)] public float RunFacialIntervalMin { get; set; }
    [FlatBufferItem(05)] public float RunFacialIntervalMax { get; set; }
    [FlatBufferItem(06)] public float RunMouthIntervalMin { get; set; }
    [FlatBufferItem(07)] public float RunMouthIntervalMax { get; set; }
    [FlatBufferItem(08)] public float SlideDropLeanAngle { get; set; }
    [FlatBufferItem(09)] public float SlideDropFwdAngle { get; set; }
    [FlatBufferItem(10)] public float SquatRate { get; set; }
    [FlatBufferItem(11)] public float RideChangeCheckHeight { get; set; }
    [FlatBufferItem(12)] public float RideChangeBottomOffset { get; set; }
    [FlatBufferItem(13)] public float RideChangeRadiusScale { get; set; }
    [FlatBufferItem(14)] public float HiddenMargin { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LadderParameter
{
    [FlatBufferItem(0)] public float LowerCorrectRate { get; set; }
    [FlatBufferItem(1)] public float UpperCorrectRate { get; set; }
    [FlatBufferItem(2)] public float UpperLaunchAngle { get; set; }
    [FlatBufferItem(3)] public float LowerLaunchAngle { get; set; }
    [FlatBufferItem(4)] public float UpperForwardAngle { get; set; }
    [FlatBufferItem(5)] public float LowerForwardAngle { get; set; }
}
