using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlaceNameDataTableArray : IFlatBufferArchive<PlaceNameDataTable>
{
    [FlatBufferItem(0)] public PlaceNameDataTable[] Table { get; set; } = Array.Empty<PlaceNameDataTable>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlaceNameDataTable
{
    [FlatBufferItem(0)] public string ID { get; set; } = string.Empty;
    [FlatBufferItem(1)] public string PlaceNameMstxtLabl { get; set; } = string.Empty;
    [FlatBufferItem(2)] public Vec3f Field3DPos { get; set; } = Vec3f.Zero;
    [FlatBufferItem(3)] public bool IsDisplayInZoomOut { get; set; }
    [FlatBufferItem(4)] public bool IsDisplayInZoomNormal { get; set; }
    [FlatBufferItem(5)] public bool IsDisplayInZoomIn { get; set; }
}
