using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PicnicPokemon
{
    [FlatBufferItem(0)] public PettingData Petting { get; set; }
    [FlatBufferItem(1)] public WashData Wash { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WashData
{
    [FlatBufferItem(0)] public PokemonContactJointList HappyBoneInfoList { get; set; }
    [FlatBufferItem(1)] public PokemonContactJointList HateBoneInfoList { get; set; }
    [FlatBufferItem(2)] public bool UseUniqueData { get; set; }
    [FlatBufferItem(3)] public UniqueWashData UniqueData { get; set; }
    [FlatBufferItem(4)] public bool DisableEraseFaceEffect { get; set; }
    [FlatBufferItem(5)] public bool DisableWink { get; set; }
    [FlatBufferItem(6)] public DisableAwaJointParam DisableAwaJoints { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class UniqueWashJointParam
{
    [FlatBufferItem(0)] public bool Valid { get; set; }
    [FlatBufferItem(1)] public string JointName { get; set; }
    [FlatBufferItem(2)] public PackedVec3f Offset { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class UniqueWashData
{
    [FlatBufferItem(00)] public UniqueWashJointParam Joint0 { get; set; }
    [FlatBufferItem(01)] public UniqueWashJointParam Joint1 { get; set; }
    [FlatBufferItem(02)] public UniqueWashJointParam Joint2 { get; set; }
    [FlatBufferItem(03)] public UniqueWashJointParam Joint3 { get; set; }
    [FlatBufferItem(04)] public UniqueWashJointParam Joint4 { get; set; }
    [FlatBufferItem(05)] public UniqueWashJointParam Joint5 { get; set; }
    [FlatBufferItem(06)] public UniqueWashJointParam Joint6 { get; set; }
    [FlatBufferItem(07)] public UniqueWashJointParam Joint7 { get; set; }
    [FlatBufferItem(08)] public UniqueWashJointParam Joint8 { get; set; }
    [FlatBufferItem(09)] public UniqueWashJointParam Joint9 { get; set; }
    [FlatBufferItem(10)] public UniqueWashJointParam Joint10 { get; set; }
    [FlatBufferItem(11)] public UniqueWashJointParam Joint11 { get; set; }
    [FlatBufferItem(12)] public UniqueWashJointParam Joint12 { get; set; }
    [FlatBufferItem(13)] public UniqueWashJointParam Joint13 { get; set; }
    [FlatBufferItem(14)] public UniqueWashJointParam Joint14 { get; set; }
    [FlatBufferItem(15)] public UniqueWashJointParam Joint15 { get; set; }
    [FlatBufferItem(16)] public UniqueWashJointParam Joint16 { get; set; }
    [FlatBufferItem(17)] public UniqueWashJointParam Joint17 { get; set; }
    [FlatBufferItem(18)] public UniqueWashJointParam Joint18 { get; set; }
    [FlatBufferItem(19)] public UniqueWashJointParam Joint19 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonContactJointList
{
    [FlatBufferItem(0)] public PokemonContactJointInfo Value0 { get; set; }
    [FlatBufferItem(1)] public PokemonContactJointInfo Value1 { get; set; }
    [FlatBufferItem(2)] public PokemonContactJointInfo Value2 { get; set; }
    [FlatBufferItem(3)] public PokemonContactJointInfo Value3 { get; set; }
    [FlatBufferItem(4)] public PokemonContactJointInfo Value4 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonContactJointInfo
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public float CollisionRad { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PettingData
{
    [FlatBufferItem(0)] public PokemonContactJointList HappyBoneInfoList { get; set; }
    [FlatBufferItem(1)] public PokemonContactJointList HateBoneInfoList { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DisableAwaJointParam
{
    [FlatBufferItem(00)] public string Joint0 { get; set; }
    [FlatBufferItem(01)] public string Joint1 { get; set; }
    [FlatBufferItem(02)] public string Joint2 { get; set; }
    [FlatBufferItem(03)] public string Joint3 { get; set; }
    [FlatBufferItem(04)] public string Joint4 { get; set; }
    [FlatBufferItem(05)] public string Joint5 { get; set; }
    [FlatBufferItem(06)] public string Joint6 { get; set; }
    [FlatBufferItem(07)] public string Joint7 { get; set; }
    [FlatBufferItem(08)] public string Joint8 { get; set; }
    [FlatBufferItem(09)] public string Joint9 { get; set; }
    [FlatBufferItem(10)] public string Joint10 { get; set; }
    [FlatBufferItem(11)] public string Joint11 { get; set; }
    [FlatBufferItem(12)] public string Joint12 { get; set; }
    [FlatBufferItem(13)] public string Joint13 { get; set; }
    [FlatBufferItem(14)] public string Joint14 { get; set; }
    [FlatBufferItem(15)] public string Joint15 { get; set; }
    [FlatBufferItem(16)] public string Joint16 { get; set; }
    [FlatBufferItem(17)] public string Joint17 { get; set; }
    [FlatBufferItem(18)] public string Joint18 { get; set; }
    [FlatBufferItem(19)] public string Joint19 { get; set; }
}
