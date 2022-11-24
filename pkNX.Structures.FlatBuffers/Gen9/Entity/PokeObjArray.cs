using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeObjArray : IFlatBufferArchive<PokeObj>
{
    [FlatBufferItem(0)] public PokeObj[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeObj
{
    [FlatBufferItem(0)] public DevID Devid { get; set; }
    [FlatBufferItem(1)] public BodyCollision Bodycollision { get; set; }
    [FlatBufferItem(2)] public CharaCollision Characollision { get; set; }
    [FlatBufferItem(3)] public GrassCollision Grasscollision { get; set; }
    [FlatBufferItem(4)] public FlatBuffer FlatBuffer { get; set; }
    [FlatBufferItem(5)] public CGemParam Gemparam { get; set; }
    [FlatBufferItem(6)] public PokeParameter Poke { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GrassCollision
{
    [FlatBufferItem(0)] public float Offsety { get; set; }
    [FlatBufferItem(1)] public float Radius { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CGemParam
{
    [FlatBufferItem(0)] public CVector3 Pos { get; set; }
    [FlatBufferItem(1)] public CVector3 Rot { get; set; }
    [FlatBufferItem(2)] public CVector3 Scale { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeParameter
{
    [FlatBufferItem(0)] public float ReachDistance { get; set; }
    [FlatBufferItem(1)] public float AcceleStartDistance { get; set; }
    [FlatBufferItem(2)] public float DeceleStartDistance { get; set; }
    [FlatBufferItem(3)] public float MoveAccele { get; set; }
    [FlatBufferItem(4)] public float RotationSpeed { get; set; }
    [FlatBufferItem(5)] public float MinWaterDepthThreshold { get; set; }
    [FlatBufferItem(6)] public float MaxWaterDepthThreshold { get; set; }
    [FlatBufferItem(7)] public float MinAltitudeThreshold { get; set; }
    [FlatBufferItem(8)] public float MaxAltitudeThreshold { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FlatBuffer
{
    [FlatBufferItem(0)] public string FileName { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CharaCollision
{
    [FlatBufferItem(0)] public CollisionShape Shape { get; set; }
    [FlatBufferItem(1)] public CVector3 Pos { get; set; }
    [FlatBufferItem(2)] public float Radius { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BodyCollision
{
    [FlatBufferItem(0)] public CollisionShape Shape { get; set; }
    [FlatBufferItem(1)] public CVector3 Pos { get; set; }
    [FlatBufferItem(2)] public float Radius { get; set; }
}
