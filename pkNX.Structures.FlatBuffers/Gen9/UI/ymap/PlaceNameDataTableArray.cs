using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlaceNameDataTableArray : IFlatBufferArchive<PlaceNameDataTable>
{
    [FlatBufferItem(0)] public PlaceNameDataTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlaceNameDataTable
{
    [FlatBufferItem(0)] public string ID { get; set; }
    [FlatBufferItem(1)] public string PlaceNameMstxtLabl { get; set; }
    [FlatBufferItem(2)] public CVector3 Field3DPos { get; set; }
    [FlatBufferItem(3)] public bool IsDisplayInZoomOut { get; set; }
    [FlatBufferItem(4)] public bool IsDisplayInZoomNormal { get; set; }
    [FlatBufferItem(5)] public bool IsDisplayInZoomIn { get; set; }
}
