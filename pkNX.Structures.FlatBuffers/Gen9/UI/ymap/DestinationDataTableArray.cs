using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DestinationDataTableArray : IFlatBufferArchive<DestinationDataTable>
{
    [FlatBufferItem(0)] public DestinationDataTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DestinationDataTable
{
    [FlatBufferItem(0)] public string EventID { get; set; }
    [FlatBufferItem(1)] public MapIconMode MapIconMode { get; set; }
    [FlatBufferItem(2)] public string IconID { get; set; }
    [FlatBufferItem(3)] public bool IsNotice { get; set; }
    [FlatBufferItem(4)] public string ChapterTitle { get; set; }
    [FlatBufferItem(5)] public string DestinationName { get; set; }
    [FlatBufferItem(6)] public string SubDestinationName { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum MapIconMode
{
    INVISIBLE = 0,
    ACTIVE = 1,
    ADDON_MARK = 2,
}
