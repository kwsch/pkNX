using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldRide
{
    [FlatBufferItem(00)] public UniqueParameter Unique { get; set; }
    [FlatBufferItem(01)] public GravityParameter Gravity { get; set; }
    [FlatBufferItem(02)] public InputParameter Input { get; set; }
    [FlatBufferItem(03)] public MoveParameter Move { get; set; }
    [FlatBufferItem(04)] public JumpParameter Jump { get; set; }
    [FlatBufferItem(05)] public GroundParameter Ground { get; set; }
    [FlatBufferItem(06)] public DashParameter Dash { get; set; }
    [FlatBufferItem(07)] public ClimbParameter Climb { get; set; }
    [FlatBufferItem(08)] public SlideDropParameter SlideDrop { get; set; }
    [FlatBufferItem(09)] public GroundKinesisParameter GroundKinesis { get; set; }
    [FlatBufferItem(10)] public AerialKinesisParameter AerialKinesis { get; set; }
    [FlatBufferItem(11)] public SlideDropKinesisParameter SlideDropKinesis { get; set; }
    [FlatBufferItem(12)] public GlideKinesisParameter GlideKinesis { get; set; }
    [FlatBufferItem(13)] public GlideParameter Glide { get; set; }
    [FlatBufferItem(14)] public SwimKinesisParameter SwimKinesis { get; set; }
    [FlatBufferItem(15)] public SwimKinesisParameter BadSwimKinesis { get; set; }
    [FlatBufferItem(16)] public FloatationParameter Floatation { get; set; }
    [FlatBufferItem(17)] public FacialParameter Facial { get; set; }
    [FlatBufferItem(18)] public EffectParameter Effect { get; set; }
    [FlatBufferItem(19)] public FlightKinesisParameter FlightKinesis { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class UniqueParameter
{
    [FlatBufferItem(0)] public float FrontLegDistance { get; set; }
    [FlatBufferItem(1)] public float RearLegDistance { get; set; }
    [FlatBufferItem(2)] public float GlideTurnCameraFactor { get; set; }
    [FlatBufferItem(3)] public float IdlingIntervalMin { get; set; }
    [FlatBufferItem(4)] public float IdlingIntervalMax { get; set; }
    [FlatBufferItem(5)] public float ImpactAngle { get; set; }
    [FlatBufferItem(6)] public float TreeImpactRate { get; set; }
    [FlatBufferItem(7)] public float RollbackDepth { get; set; }
    [FlatBufferItem(8)] public ChangeOffsetSet ChangeOffsetA { get; set; }
    [FlatBufferItem(9)] public ChangeOffsetSet ChangeOffsetB { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SwimKinesisParameter
{
    [FlatBufferItem(0)] public float SelfSpeedMax { get; set; }
    [FlatBufferItem(1)] public float Accel { get; set; }
    [FlatBufferItem(2)] public float Friction { get; set; }
    [FlatBufferItem(3)] public float MinGripAngle { get; set; }
    [FlatBufferItem(4)] public float MaxGripAngle { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SquatKinesisParameter
{
    [FlatBufferItem(0)] public float SpeedFixed { get; set; }
    [FlatBufferItem(1)] public float SlipFriction { get; set; }
    [FlatBufferItem(2)] public float GripFactor { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SlidingKinesisParameter
{
    [FlatBufferItem(0)] public float Friction { get; set; }
    [FlatBufferItem(1)] public float SlipFriction { get; set; }
    [FlatBufferItem(2)] public float InitVelocityRate { get; set; }
    [FlatBufferItem(3)] public float BreakDecrease { get; set; }
    [FlatBufferItem(4)] public float BreakIncrease { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SlideDropParameter
{
    [FlatBufferItem(0)] public float ForwardDirFactor { get; set; }
    [FlatBufferItem(1)] public float BackwardDirFactor { get; set; }
    [FlatBufferItem(2)] public float UpwardDirFactor { get; set; }
    [FlatBufferItem(3)] public float BackwardTime { get; set; }
    [FlatBufferItem(4)] public float LeanFactor { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SlideDropKinesisParameter
{
    [FlatBufferItem(00)] public float ForwardGripFactor { get; set; }
    [FlatBufferItem(01)] public float BackwardGripFactor { get; set; }
    [FlatBufferItem(02)] public float Accel { get; set; }
    [FlatBufferItem(03)] public float Friction { get; set; }
    [FlatBufferItem(04)] public float SpeedMax { get; set; }
    [FlatBufferItem(05)] public float CurveAngle { get; set; }
    [FlatBufferItem(06)] public float SpeedUpInputThreshold { get; set; }
    [FlatBufferItem(07)] public float SideInputTolerance { get; set; }
    [FlatBufferItem(08)] public float SideMaxVelocityRate { get; set; }
    [FlatBufferItem(09)] public float SideFriction { get; set; }
    [FlatBufferItem(10)] public float SpeedUpRate { get; set; }
    [FlatBufferItem(11)] public float SpeedDownRate { get; set; }
    [FlatBufferItem(12)] public float ForwardSideAccel { get; set; }
    [FlatBufferItem(13)] public float BackwardSideAccel { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class RadialBlurParameter
{
    // EaseFloatValue[3]
    // EaseFloatValue[3]
    [FlatBufferItem(00)] public EaseFloatValue Intensities0 { get; set; }
    [FlatBufferItem(01)] public EaseFloatValue Intensities1 { get; set; }
    [FlatBufferItem(02)] public EaseFloatValue Intensities2 { get; set; }
    [FlatBufferItem(03)] public EaseFloatValue Offsets0 { get; set; }
    [FlatBufferItem(04)] public EaseFloatValue Offsets1 { get; set; }
    [FlatBufferItem(05)] public EaseFloatValue Offsets2 { get; set; }
    [FlatBufferItem(06)] public float Duration { get; set; }
    [FlatBufferItem(07)] public float YawLimit { get; set; }
    [FlatBufferItem(08)] public float PitchLimit { get; set; }
    [FlatBufferItem(09)] public float KillTime { get; set; }
    [FlatBufferItem(10)] public Easing KillEase { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class OffsetVector
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MoveParameter
{
    [FlatBufferItem(00)] public float PitchFactor { get; set; }
    [FlatBufferItem(01)] public float LeanAngle { get; set; }
    [FlatBufferItem(02)] public float LeanFactor { get; set; }
    [FlatBufferItem(03)] public float RotationFactor { get; set; }
    [FlatBufferItem(04)] public float ReverseSpeedFactor { get; set; }
    [FlatBufferItem(05)] public float ReverseTurnAngle { get; set; }
    [FlatBufferItem(06)] public float ReverseFrictionFactor { get; set; }
    [FlatBufferItem(07)] public float LightLandingHeight { get; set; }
    [FlatBufferItem(08)] public float HeavyLandingHeight { get; set; }
    [FlatBufferItem(09)] public int DynamicsResetCount { get; set; }
    [FlatBufferItem(10)] public int SpinCount { get; set; }
    [FlatBufferItem(11)] public float SpinMaxSpeed { get; set; }
    [FlatBufferItem(12)] public float SpinMaxAnimationSpeed { get; set; }
    [FlatBufferItem(13)] public float SpinInterpolation { get; set; }
    [FlatBufferItem(14)] public float SlideDropToleranceTime { get; set; }
    [FlatBufferItem(15)] public float StopToleranceTime { get; set; }
    [FlatBufferItem(16)] public float TurnStickDiffThreashold { get; set; }
    [FlatBufferItem(17)] public float SliderModeRate { get; set; }
    [FlatBufferItem(18)] public float BallThrowFrictionFactor { get; set; }
    [FlatBufferItem(19)] public float BallThrowBackFrictionFactor { get; set; }
    [FlatBufferItem(20)] public float UnlandablePushVelocity { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class JumpParameter
{
    [FlatBufferItem(0)] public float HeightHi { get; set; }
    [FlatBufferItem(1)] public float HeightLow { get; set; }
    [FlatBufferItem(2)] public float TapDuration { get; set; }
    [FlatBufferItem(3)] public float WaterHeightHi { get; set; }
    [FlatBufferItem(4)] public float WaterHeightLow { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class InputParameter
{
    [FlatBufferItem(0)] public float StickTolerance { get; set; }
    [FlatBufferItem(1)] public float SpinThreshold { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GroundParameter
{
    [FlatBufferItem(0)] public float SlideDropThreshold { get; set; }
    [FlatBufferItem(1)] public float VerticalMargin { get; set; }
    [FlatBufferItem(2)] public float WallHitAngle { get; set; }
    [FlatBufferItem(3)] public float WaterDraftLine { get; set; }
    [FlatBufferItem(4)] public float WaterSurfaceMargin { get; set; }
    [FlatBufferItem(5)] public float WaterWithstandLine { get; set; }
    [FlatBufferItem(6)] public float UnderWaterThreshold { get; set; }
    [FlatBufferItem(7)] public float ShallowWaterThreshold { get; set; }
    [FlatBufferItem(8)] public float AcceptableRadiusRate { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GroundKinesisParameter
{
    [FlatBufferItem(00)] public float FirstSpeedMax { get; set; }
    [FlatBufferItem(01)] public float SecondSpeedMax { get; set; }
    [FlatBufferItem(02)] public float SlowSpeedMax { get; set; }
    [FlatBufferItem(03)] public float FirstAccel { get; set; }
    [FlatBufferItem(04)] public float SecondAccel { get; set; }
    [FlatBufferItem(05)] public float SlowAccel { get; set; }
    [FlatBufferItem(06)] public float Friction { get; set; }
    [FlatBufferItem(07)] public float SlipFriction { get; set; }
    [FlatBufferItem(08)] public float SlipFrictionExp { get; set; }
    [FlatBufferItem(09)] public float MaxGripFactor { get; set; }
    [FlatBufferItem(10)] public float MinGripFactor { get; set; }
    [FlatBufferItem(11)] public float RunStickThreshold { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GravityParameter
{
    [FlatBufferItem(0)] public float Accel { get; set; }
    [FlatBufferItem(1)] public float SpeedMax { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GlideParameter
{
    [FlatBufferItem(0)] public float DampingRate { get; set; }
    [FlatBufferItem(1)] public float DampingThreshold { get; set; }
    [FlatBufferItem(2)] public float StaminaMax { get; set; }
    [FlatBufferItem(3)] public float StaminaCost { get; set; }
    [FlatBufferItem(4)] public float StaminaGain { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GlideKinesisParameter
{
    [FlatBufferItem(0)] public float SelfSpeedMax { get; set; }
    [FlatBufferItem(1)] public float Accel { get; set; }
    [FlatBufferItem(2)] public float Friction { get; set; }
    [FlatBufferItem(3)] public float MinGripAngle { get; set; }
    [FlatBufferItem(4)] public float MaxGripAngle { get; set; }
    [FlatBufferItem(5)] public float GravityRate { get; set; }
    [FlatBufferItem(6)] public float FlareSpeedMax { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FloatationParameter
{
    [FlatBufferItem(0)] public float Floatage { get; set; }
    [FlatBufferItem(1)] public float Dumping { get; set; }
    [FlatBufferItem(2)] public float SpeedMax { get; set; }
    [FlatBufferItem(3)] public float SurfaceDumpingRate { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FlightKinesisParameter
{
    [FlatBufferItem(00)] public float Accel { get; set; }
    [FlatBufferItem(01)] public float PitchMaxUpAngle { get; set; }
    [FlatBufferItem(02)] public float PitchMaxDownAngle { get; set; }
    [FlatBufferItem(03)] public float PitchMaxTotalAngle { get; set; }
    [FlatBufferItem(04)] public float YawAngleSpeed { get; set; }
    [FlatBufferItem(05)] public float PitchAngleSpeed { get; set; }
    [FlatBufferItem(06)] public float PitchUpSpeedRate { get; set; }
    [FlatBufferItem(07)] public float PitchDownSpeedRate { get; set; }
    [FlatBufferItem(08)] public float PitchStableSpeedRate { get; set; }
    [FlatBufferItem(09)] public float PitchBaseAngle { get; set; }
    [FlatBufferItem(10)] public float RollMaxAngle { get; set; }
    [FlatBufferItem(11)] public float RollFactor { get; set; }
    [FlatBufferItem(12)] public float UpAccelRate { get; set; }
    [FlatBufferItem(13)] public float DownAccelRate { get; set; }
    [FlatBufferItem(14)] public float MaxSpeed { get; set; }
    [FlatBufferItem(15)] public float MaxDiveSpeed { get; set; }
    [FlatBufferItem(16)] public float PitchUpSpeedThreshold { get; set; }
    [FlatBufferItem(17)] public float PitchRateExp { get; set; }
    [FlatBufferItem(18)] public float ReadyScale { get; set; }
    [FlatBufferItem(19)] public float FaintThreshold { get; set; }
    [FlatBufferItem(20)] public float AirRegist { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FacialParameter
{
    [FlatBufferItem(0)] public float AutoBlinkIntervalMin { get; set; }
    [FlatBufferItem(1)] public float AutoBlinkIntervalMax { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class EmissiveColorParameter
{
    // EaseColorValue[3] Blend
    // EaseColorValue[3] Rim
    // EaseColorValue[3] Power
    // EaseColorValue[3] Intensity
    // float Duration
    [FlatBufferItem(00)] public EaseColorValue Blend0 { get; set; }
    [FlatBufferItem(01)] public EaseColorValue Blend1 { get; set; }
    [FlatBufferItem(02)] public EaseColorValue Blend2 { get; set; }
    [FlatBufferItem(03)] public EaseColorValue Rim0 { get; set; }
    [FlatBufferItem(04)] public EaseColorValue Rim1 { get; set; }
    [FlatBufferItem(05)] public EaseColorValue Rim2 { get; set; }
    [FlatBufferItem(06)] public EaseFloatValue Power0 { get; set; }
    [FlatBufferItem(07)] public EaseFloatValue Power1 { get; set; }
    [FlatBufferItem(08)] public EaseFloatValue Power2 { get; set; }
    [FlatBufferItem(09)] public EaseFloatValue Intensity0 { get; set; }
    [FlatBufferItem(10)] public EaseFloatValue Intensity1 { get; set; }
    [FlatBufferItem(11)] public EaseFloatValue Intensity2 { get; set; }
    [FlatBufferItem(12)] public float Duration { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EffectParameter
{
    [FlatBufferItem(0)] public EmissiveColorParameter Emissive { get; set; }
    [FlatBufferItem(1)] public RadialBlurParameter DashRadialBlur { get; set; }
    [FlatBufferItem(2)] public RadialBlurParameter SwimRadialBlur { get; set; }
    [FlatBufferItem(3)] public RadialBlurParameter GlideRadialBlur { get; set; }
    [FlatBufferItem(4)] public float GlideBlurThreshold { get; set; }
    [FlatBufferItem(5)] public float LargeSplayRangeMin { get; set; }
    [FlatBufferItem(6)] public float LargeSplayRangeMax { get; set; }
    [FlatBufferItem(7)] public float SmallSplayRangeMin { get; set; }
    [FlatBufferItem(8)] public float SmallSplayRangeMax { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class EaseFloatValue
{
    [FlatBufferItem(0)] public float Time { get; set; }
    [FlatBufferItem(1)] public float Value { get; set; }
    [FlatBufferItem(2)] public Easing Ease { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class EaseColorValue
{
    [FlatBufferItem(0)] public float Time { get; set; }
    [FlatBufferItem(1)] public EaseColor Value { get; set; }
    [FlatBufferItem(2)] public Easing Ease { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DashParameter
{
    [FlatBufferItem(0)] public float SpeedMax { get; set; }
    [FlatBufferItem(1)] public float AccelTime { get; set; }
    [FlatBufferItem(2)] public float AccelRate { get; set; }
    [FlatBufferItem(3)] public float AccelAnimationRate { get; set; }
    [FlatBufferItem(4)] public float SwimSpeedMax { get; set; }
    [FlatBufferItem(5)] public float SwimAccelTime { get; set; }
    [FlatBufferItem(6)] public float SwimAccelRate { get; set; }
    [FlatBufferItem(7)] public float SwimAccelAnimationRate { get; set; }
    [FlatBufferItem(8)] public float GripFactor { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class EaseColor
{
    [FlatBufferItem(0)] public float Red { get; set; }
    [FlatBufferItem(1)] public float Green { get; set; }
    [FlatBufferItem(2)] public float Blue { get; set; }
    [FlatBufferItem(3)] public float Alpha { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ClimbParameter
{
    [FlatBufferItem(00)] public float BackJumpInitialVelocity { get; set; }
    [FlatBufferItem(01)] public float UpStickAngle { get; set; }
    [FlatBufferItem(02)] public float SideUpperStickAngle { get; set; }
    [FlatBufferItem(03)] public float SideLowerStickAngle { get; set; }
    [FlatBufferItem(04)] public float MoveSpeed { get; set; }
    [FlatBufferItem(05)] public float PositionMergeFactor { get; set; }
    [FlatBufferItem(06)] public float VertUpMergeFactor { get; set; }
    [FlatBufferItem(07)] public float HorzUpMergeFactor { get; set; }
    [FlatBufferItem(08)] public float Radius { get; set; }
    [FlatBufferItem(09)] public float DownAccel { get; set; }
    [FlatBufferItem(10)] public float ReverseAngle { get; set; }
    [FlatBufferItem(11)] public float FlipEndCheckOffset { get; set; }
    [FlatBufferItem(12)] public float FlipEndCheckLength { get; set; }
    [FlatBufferItem(13)] public float JumpChargeExponent { get; set; }
    [FlatBufferItem(14)] public float GroundDistanceTolerance { get; set; }
    [FlatBufferItem(15)] public float WallHitLength { get; set; }
    [FlatBufferItem(16)] public float WallHitOffset { get; set; }
    [FlatBufferItem(17)] public float OppositeAngle { get; set; }
    [FlatBufferItem(18)] public float AutoStartToleranceFactor { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class ChangeOffsetSet
{
    [FlatBufferItem(0)] public ChangeOffset Stand { get; set; }
    [FlatBufferItem(1)] public ChangeOffset Run { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class ChangeOffset
{
    [FlatBufferItem(0)] public OffsetVector Trans { get; set; }
    [FlatBufferItem(1)] public OffsetVector Rot { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AerialKinesisParameter
{
    [FlatBufferItem(0)] public float Accel { get; set; }
    [FlatBufferItem(1)] public float Friction { get; set; }
    [FlatBufferItem(2)] public float SelfSpeedMax { get; set; }
    [FlatBufferItem(3)] public float BackwardAngle { get; set; }
    [FlatBufferItem(4)] public float GripAngle { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum Easing
{
    InSine = 0,
    OutSine = 1,
    InOutSin = 2,
    InQuad = 3,
    OutQuad = 4,
    InOutQuad = 5,
    InCubic = 6,
    OutCubic = 7,
    InOutCubic = 8,
    InQuart = 9,
    OutQuart = 10,
    InOutQuart = 11,
    InQuint = 12,
    OutQuint = 13,
    InOutQuint = 14,
    InExpo = 15,
    OutExpo = 16,
    InOutExpo = 17,
    InCirc = 18,
    OutCirc = 19,
    InOutCirc = 20,
    InBack = 21,
    OutBack = 22,
    InOutBack = 23,
    InElastic = 24,
    OutElastic = 25,
    InOutElastic = 26,
    InBounce = 27,
    OutBounce = 28,
    InOutBounce = 29,
    Linear = 30,
}
