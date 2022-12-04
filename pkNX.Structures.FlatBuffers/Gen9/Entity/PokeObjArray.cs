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
    [FlatBufferItem(0)] public DevID DevId { get; set; }
    [FlatBufferItem(1)] public BodyCollision BodyCollision { get; set; }
    [FlatBufferItem(2)] public CharaCollision CharaCollision { get; set; }
    [FlatBufferItem(3)] public GrassCollision GrassCollision { get; set; }
    [FlatBufferItem(4)] public FlatBuffer FlatBuffer { get; set; }
    [FlatBufferItem(5)] public CGemParam GemParam { get; set; }
    [FlatBufferItem(6)] public PokeParameter Poke { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GrassCollision
{
    [FlatBufferItem(0)] public float OffsetY { get; set; }
    [FlatBufferItem(1)] public float Radius { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CGemParam
{
    [FlatBufferItem(0)] public Vec3f Pos { get; set; }
    [FlatBufferItem(1)] public Vec3f Rot { get; set; }
    [FlatBufferItem(2)] public Vec3f Scale { get; set; }
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
    [FlatBufferItem(1)] public Vec3f Pos { get; set; }
    [FlatBufferItem(2)] public float Radius { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BodyCollision
{
    [FlatBufferItem(0)] public CollisionShape Shape { get; set; }
    [FlatBufferItem(1)] public Vec3f Pos { get; set; }
    [FlatBufferItem(2)] public float Radius { get; set; }
}
