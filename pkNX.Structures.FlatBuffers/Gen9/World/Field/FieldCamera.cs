using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCamera
{
    [FlatBufferItem(00)] public float CenterDistance { get; set; }
    [FlatBufferItem(01)] public float TopDistance { get; set; }
    [FlatBufferItem(02)] public float BottomDistance { get; set; }
    [FlatBufferItem(03)] public Vec3f GazeOffset { get; set; }
    [FlatBufferItem(04)] public float RotationTurn { get; set; }
    [FlatBufferItem(05)] public float RotationPitch { get; set; }
    [FlatBufferItem(06)] public float PitchCenter { get; set; }
    [FlatBufferItem(07)] public float PitchUpperMax { get; set; }
    [FlatBufferItem(08)] public float PitchLowerMax { get; set; }
    [FlatBufferItem(09)] public float FollowFactor { get; set; }
    [FlatBufferItem(10)] public float WallOffset { get; set; }
    [FlatBufferItem(11)] public float FloorOffset { get; set; }
    [FlatBufferItem(12)] public float DistanceMin { get; set; }
    [FlatBufferItem(13)] public float DistanceMax { get; set; }
    [FlatBufferItem(14)] public float RevertFactor { get; set; }
    [FlatBufferItem(15)] public float DepthFollowFactor { get; set; }
    [FlatBufferItem(16)] public float DepthFollowLerpExponential { get; set; }
    [FlatBufferItem(17)] public float ZoomHoldTime { get; set; }
    [FlatBufferItem(18)] public float ZoomDistance { get; set; }
    [FlatBufferItem(19)] public float StickTolerance { get; set; }
    [FlatBufferItem(20)] public FieldCameraHidData Hid { get; set; }
    [FlatBufferItem(21)] public FieldCameraTrackingData Tracking { get; set; }
    [FlatBufferItem(22)] public FieldCameraOrbitRotationData Orbit { get; set; }
    [FlatBufferItem(23)] public FieldCameraLockOnData LockOn { get; set; }
    [FlatBufferItem(24)] public FieldCameraCollisionData Collision { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCameraTrackingData
{
    [FlatBufferItem(0)] public Vec3f TrackingOffset { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCameraOrbitRotationData
{
    [FlatBufferItem(0)] public float TargetMoveTolerance { get; set; }
    [FlatBufferItem(1)] public float AutoRotationTimeTolerance { get; set; }
    [FlatBufferItem(2)] public float AutoRotationYawSpeed { get; set; }
    [FlatBufferItem(3)] public float AutoRotationPitchSpeed { get; set; }
    [FlatBufferItem(4)] public float RotationYawSpeed { get; set; }
    [FlatBufferItem(5)] public float RotationPitchSpeed { get; set; }
    [FlatBufferItem(6)] public float DefaultResetRotationTime { get; set; }
    [FlatBufferItem(7)] public float DefaultEasingPosTime { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCameraLockOnData
{
    [FlatBufferItem(0)] public float RotationYawSpeed { get; set; }
    [FlatBufferItem(1)] public float RotationPitchSpeed { get; set; }
    [FlatBufferItem(2)] public float PitchMax { get; set; }
    [FlatBufferItem(3)] public float PitchMin { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCameraHidData
{
    [FlatBufferItem(0)] public float StickTolerance { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldCameraCollisionData
{
    [FlatBufferItem(0)] public float ResolveSpeed { get; set; }
    [FlatBufferItem(1)] public float ResolveStartMargin { get; set; }
    [FlatBufferItem(2)] public float DistanceLimitMax { get; set; }
    [FlatBufferItem(3)] public float DistanceLimitMin { get; set; }
    [FlatBufferItem(4)] public float DistanceToWater { get; set; }
    [FlatBufferItem(5)] public float CastLengthToWater { get; set; }
}
