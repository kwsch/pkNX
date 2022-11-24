using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcTrafficGenerateTableArray : IFlatBufferArchive<NpcTrafficGenerateTable>
{
    [FlatBufferItem(0)] public NpcTrafficGenerateTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcTrafficGenerateTable
{
    [FlatBufferItem(00)] public string Id { get; set; }
    [FlatBufferItem(01)] public NpcTrafficGenerateData Data1 { get; set; }
    [FlatBufferItem(02)] public NpcTrafficGenerateData Data2 { get; set; }
    [FlatBufferItem(03)] public NpcTrafficGenerateData Data3 { get; set; }
    [FlatBufferItem(04)] public NpcTrafficGenerateData Data4 { get; set; }
    [FlatBufferItem(05)] public NpcTrafficGenerateData Data5 { get; set; }
    [FlatBufferItem(06)] public NpcTrafficGenerateData Data6 { get; set; }
    [FlatBufferItem(07)] public NpcTrafficGenerateData Data7 { get; set; }
    [FlatBufferItem(08)] public NpcTrafficGenerateData Data8 { get; set; }
    [FlatBufferItem(09)] public NpcTrafficGenerateData Data9 { get; set; }
    [FlatBufferItem(10)] public NpcTrafficGenerateData Data10 { get; set; }
    [FlatBufferItem(11)] public NpcTrafficGenerateData Data11 { get; set; }
    [FlatBufferItem(12)] public NpcTrafficGenerateData Data12 { get; set; }
    [FlatBufferItem(13)] public NpcTrafficGenerateData Data13 { get; set; }
    [FlatBufferItem(14)] public NpcTrafficGenerateData Data14 { get; set; }
    [FlatBufferItem(15)] public NpcTrafficGenerateData Data15 { get; set; }
    [FlatBufferItem(16)] public NpcTrafficGenerateData Data16 { get; set; }
    [FlatBufferItem(17)] public NpcTrafficGenerateData Data17 { get; set; }
    [FlatBufferItem(18)] public NpcTrafficGenerateData Data18 { get; set; }
    [FlatBufferItem(19)] public NpcTrafficGenerateData Data19 { get; set; }
    [FlatBufferItem(20)] public NpcTrafficGenerateData Data20 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcTrafficGenerateData
{
    [FlatBufferItem(00)] public uint Probability { get; set; }
    [FlatBufferItem(01)] public CharaCreateParam Ccparam { get; set; }
    [FlatBufferItem(02)] public string Messageid { get; set; }
    [FlatBufferItem(03)] public NpcTrafficPartnerType PartnerType { get; set; }
    [FlatBufferItem(04)] public CharaCreateParam PartnerCcparam { get; set; }
    [FlatBufferItem(05)] public PokemonSimpleParam PartnerPkparam { get; set; }
    [FlatBufferItem(06)] public string PartnerMessageid { get; set; }
    [FlatBufferItem(07)] public float PartnerRange { get; set; }
    [FlatBufferItem(08)] public NpcTrafficFormation Formation { get; set; }
    [FlatBufferItem(09)] public int Speed { get; set; }
    [FlatBufferItem(10)] public FieldNpcFaceType FaceType { get; set; }
    [FlatBufferItem(11)] public bool LipSync { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonSimpleParam
{
    [FlatBufferItem(0)] public DevID DevId { get; set; }
    [FlatBufferItem(1)] public short FormId { get; set; }
    [FlatBufferItem(2)] public SexType Sex { get; set; }
    [FlatBufferItem(3)] public bool IsRare { get; set; }
    [FlatBufferItem(4)] public bool IsEgg { get; set; }
    [FlatBufferItem(5)] public float Scale { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldNpcMotionIntValue
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public int Value { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldNpcMotionBoolValue
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public bool Value { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldNpcAnimationConfig
{
    [FlatBufferItem(0)] public string Trigger { get; set; }
    [FlatBufferItem(1)] public FieldNpcMotionIntValue IntValue { get; set; }
    [FlatBufferItem(2)] public FieldNpcMotionBoolValue BoolValue { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CharaCreateParam
{
    [FlatBufferItem(0)] public CharaCreateFilePath FilePath { get; set; }
    [FlatBufferItem(1)] public string Label { get; set; }
    [FlatBufferItem(2)] public uint AnimSlot { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CharaCreateFilePath
{
    [FlatBufferItem(0)] public string Role { get; set; }
    [FlatBufferItem(1)] public string Character { get; set; }
}
