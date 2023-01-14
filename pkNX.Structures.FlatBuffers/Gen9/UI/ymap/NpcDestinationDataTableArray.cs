using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcDestinationDataTableArray : IFlatBufferArchive<NpcDestinationDataTable>
{
    [FlatBufferItem(0)] public NpcDestinationDataTable[] Table { get; set; } = Array.Empty<NpcDestinationDataTable>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NpcDestinationDataTable
{
    [FlatBufferItem(0)] public string ID { get; set; } = string.Empty;
    [FlatBufferItem(1)] public YmapNpcDistinationType Type { get; set; }
    [FlatBufferItem(2)] public Vec3f Position { get; set; } = Vec3f.Zero;
    [FlatBufferItem(3)] public string MapIconID { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string SceneName { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ObjectName { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string MstxtLabel { get; set; } = string.Empty;
}

[FlatBufferEnum(typeof(int))]
public enum YmapNpcDistinationType
{
    POSITION_XYZ = 0,
    MAPICON_ID = 1,
    SCENE_OBJECT = 2,
}
