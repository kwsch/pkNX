using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcDestinationDataTableArray : IFlatBufferArchive<NpcDestinationDataTable>
{
    [FlatBufferItem(0)] public NpcDestinationDataTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcDestinationDataTable
{
    [FlatBufferItem(0)] public string ID { get; set; }
    [FlatBufferItem(1)] public YmapNpcDistinationType Type { get; set; }
    [FlatBufferItem(2)] public CVector3 Position { get; set; }
    [FlatBufferItem(3)] public string MapIconID { get; set; }
    [FlatBufferItem(4)] public string SceneName { get; set; }
    [FlatBufferItem(5)] public string ObjectName { get; set; }
    [FlatBufferItem(6)] public string MstxtLabel { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum YmapNpcDistinationType
{
    POSITION_XYZ = 0,
    MAPICON_ID = 1,
    SCENE_OBJECT = 2,
}
