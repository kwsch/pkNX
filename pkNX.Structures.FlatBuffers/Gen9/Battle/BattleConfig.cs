using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BattleConfig
{
    [FlatBufferItem(00)] public float FcamZoomToprigRadius { get; set; }
    [FlatBufferItem(01)] public float FcamZoomToprigHeight { get; set; }
    [FlatBufferItem(02)] public float FcamZoomMidrigRadius { get; set; }
    [FlatBufferItem(03)] public float FcamZoomMidrigHeight { get; set; }
    [FlatBufferItem(04)] public float FcamZoomLowrigRadius { get; set; }
    [FlatBufferItem(05)] public float FcamZoomLowrigHeight { get; set; }
    [FlatBufferItem(06)] public float FcamZoomToprigRadius2 { get; set; }
    [FlatBufferItem(07)] public float FcamZoomToprigHeight2 { get; set; }
    [FlatBufferItem(08)] public float FcamZoomMidrigRadius2 { get; set; }
    [FlatBufferItem(09)] public float FcamZoomMidrigHeight2 { get; set; }
    [FlatBufferItem(10)] public float FcamZoomLowrigRadius2 { get; set; }
    [FlatBufferItem(11)] public float FcamZoomLowrigHeight2 { get; set; }
    [FlatBufferItem(12)] public float FcamCentToprigRadius { get; set; }
    [FlatBufferItem(13)] public float FcamCentToprigHeight { get; set; }
    [FlatBufferItem(14)] public float FcamCentMidrigRadius { get; set; }
    [FlatBufferItem(15)] public float FcamCentMidrigHeight { get; set; }
    [FlatBufferItem(16)] public float FcamCentLowrigRadius { get; set; }
    [FlatBufferItem(17)] public float FcamCentLowrigHeight { get; set; }
    [FlatBufferItem(18)] public Vec3f FcamOffsetCenter { get; set; }
    [FlatBufferItem(19)] public Vec3f FcamOffsetPoke0 { get; set; }
    [FlatBufferItem(20)] public Vec3f FcamOffsetPoke1 { get; set; }
    [FlatBufferItem(21)] public Vec3f FcamOffsetPoke2 { get; set; }
    [FlatBufferItem(22)] public Vec3f FcamOffsetPoke3 { get; set; }
    [FlatBufferItem(23)] public Vec3f FcamOffsetTr0 { get; set; }
    [FlatBufferItem(24)] public Vec3f FcamOffsetTr1 { get; set; }
    [FlatBufferItem(25)] public Vec3f FcamOffsetTr2 { get; set; }
    [FlatBufferItem(26)] public Vec3f FcamOffsetTr3 { get; set; }
    [FlatBufferItem(27)] public float FcamZoomStartAngle { get; set; }
    [FlatBufferItem(28)] public float FcamZoomMinAngle { get; set; }
    [FlatBufferItem(29)] public float FcamZoomMaxAngle { get; set; }
    [FlatBufferItem(30)] public float FcamZoomStartHeight { get; set; }
    [FlatBufferItem(31)] public float FcamZoomStartAngle2 { get; set; }
    [FlatBufferItem(32)] public float FcamZoomMinAngle2 { get; set; }
    [FlatBufferItem(33)] public float FcamZoomMaxAngle2 { get; set; }
    [FlatBufferItem(34)] public float FcamZoomStartHeight2 { get; set; }
    [FlatBufferItem(35)] public float FcamCentStartHeightS { get; set; }
    [FlatBufferItem(36)] public float FcamCentStartHeightM { get; set; }
    [FlatBufferItem(37)] public float FcamCentStartHeightL { get; set; }
    [FlatBufferItem(38)] public float FcamCentStartHeightLL { get; set; }
    [FlatBufferItem(39)] public string FcamPokeLocator { get; set; }
    [FlatBufferItem(40)] public string FcamTrainerLocator { get; set; }
    [FlatBufferItem(41)] public float FcamRotSpeedX { get; set; }
    [FlatBufferItem(42)] public float FcamRotSpeedY { get; set; }
    [FlatBufferItem(43)] public float FcamStartStickLength { get; set; }
    [FlatBufferItem(44)] public sbyte FieldgemDropupRate { get; set; }
    [FlatBufferItem(45)] public bool WazamsgIsPreMsgTime { get; set; }
    [FlatBufferItem(46)] public float WazamsgPreMsgTime { get; set; }
    [FlatBufferItem(47)] public bool WazamsgIsPostMsgTime { get; set; }
    [FlatBufferItem(48)] public float WazamsgPostMsgTime { get; set; }
    [FlatBufferItem(49)] public bool WazamsgIsSubMsgTime { get; set; }
    [FlatBufferItem(50)] public float WazamsgSubMsgTime { get; set; }
    [FlatBufferItem(51)] public bool WazamsgIsSubMsgCloseTime { get; set; }
    [FlatBufferItem(52)] public float WazamsgSubMsgCloseTime { get; set; }
    [FlatBufferItem(53)] public float WildTrposCheckSize { get; set; }
    [FlatBufferItem(54)] public int RandomCameraStartFrames { get; set; }
    [FlatBufferItem(55)] public int TrainerWaitIntervalFix { get; set; }
    [FlatBufferItem(56)] public int TrainerWaitIntervalRandom { get; set; }
    [FlatBufferItem(57)] public int PokeRoarIntervalFix { get; set; }
    [FlatBufferItem(58)] public int PokeRoarIntervalRandom { get; set; }
    [FlatBufferItem(59)] public int BossCarIdlingIntervalFix { get; set; }
    [FlatBufferItem(60)] public int BossCarIdlingIntervalRandom { get; set; }
    [FlatBufferItem(61)] public int BossCarWaitRate { get; set; }
    [FlatBufferItem(62)] public int BossCarRoarRate { get; set; }
    [FlatBufferItem(63)] public float BattleAreaSize { get; set; }
    [FlatBufferItem(64)] public float BattleHecklerAreaSize { get; set; }
    [FlatBufferItem(65)] public float RaidPowerchargeMessageTime { get; set; }
    [FlatBufferItem(66)] public float PokeMoveConditionDistance { get; set; }
}
