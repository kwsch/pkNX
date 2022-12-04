using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSettingRootTableArray : IFlatBufferArchive<GemSettingRootTable>
{
    [FlatBufferItem(0)] public GemSettingRootTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSettingRootTable
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public GemParam DefaultModelData { get; set; }
    [FlatBufferItem(2)] public string DefaultModelLocaterData { get; set; }
    [FlatBufferItem(3)] public GemArrayParam IndividualModelData { get; set; }
    [FlatBufferItem(4)] public GemTypeLocaterTable IndividualModelLocaterData { get; set; }
    [FlatBufferItem(5)] public GemParam DefaultEffectData { get; set; }
    [FlatBufferItem(6)] public string DefaultEffectLocaterData { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemParam
{
    [FlatBufferItem(0)] public PackedVec3f Pos { get; set; }
    [FlatBufferItem(1)] public PackedVec3f Rot { get; set; }
    [FlatBufferItem(2)] public float Scale { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemArrayParam
{
    // GemParam[18]
    [FlatBufferItem(00)] public GemParam Group00 { get; set; }
    [FlatBufferItem(01)] public GemParam Group01 { get; set; }
    [FlatBufferItem(02)] public GemParam Group02 { get; set; }
    [FlatBufferItem(03)] public GemParam Group03 { get; set; }
    [FlatBufferItem(04)] public GemParam Group04 { get; set; }
    [FlatBufferItem(05)] public GemParam Group05 { get; set; }
    [FlatBufferItem(06)] public GemParam Group06 { get; set; }
    [FlatBufferItem(07)] public GemParam Group07 { get; set; }
    [FlatBufferItem(08)] public GemParam Group08 { get; set; }
    [FlatBufferItem(09)] public GemParam Group09 { get; set; }
    [FlatBufferItem(10)] public GemParam Group10 { get; set; }
    [FlatBufferItem(11)] public GemParam Group11 { get; set; }
    [FlatBufferItem(12)] public GemParam Group12 { get; set; }
    [FlatBufferItem(13)] public GemParam Group13 { get; set; }
    [FlatBufferItem(14)] public GemParam Group14 { get; set; }
    [FlatBufferItem(15)] public GemParam Group15 { get; set; }
    [FlatBufferItem(16)] public GemParam Group16 { get; set; }
    [FlatBufferItem(17)] public GemParam Group17 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemTypeLocaterTable
{
    [FlatBufferItem(00)] public string Normal { get; set; }
    [FlatBufferItem(01)] public string Kakutou { get; set; }
    [FlatBufferItem(02)] public string Hikou { get; set; }
    [FlatBufferItem(03)] public string Doku { get; set; }
    [FlatBufferItem(04)] public string Jimen { get; set; }
    [FlatBufferItem(05)] public string Iwa { get; set; }
    [FlatBufferItem(06)] public string Mushi { get; set; }
    [FlatBufferItem(07)] public string Ghost { get; set; }
    [FlatBufferItem(08)] public string Hagane { get; set; }
    [FlatBufferItem(09)] public string Honoo { get; set; }
    [FlatBufferItem(10)] public string Mizu { get; set; }
    [FlatBufferItem(11)] public string Kusa { get; set; }
    [FlatBufferItem(12)] public string Denki { get; set; }
    [FlatBufferItem(13)] public string Esper { get; set; }
    [FlatBufferItem(14)] public string Koori { get; set; }
    [FlatBufferItem(15)] public string Dragon { get; set; }
    [FlatBufferItem(16)] public string Aku { get; set; }
    [FlatBufferItem(17)] public string Fairy { get; set; }
}
