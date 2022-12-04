using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventSkyFlyArray : IFlatBufferArchive<EventSkyFly>
{
    [FlatBufferItem(0)] public EventSkyFly[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventSkyFly
{
    [FlatBufferItem(00)] public string ID { get; set; }
    [FlatBufferItem(01)] public MapIconKind MapIconKind { get; set; }
    [FlatBufferItem(02)] public string FollowSceneName { get; set; }
    [FlatBufferItem(03)] public string FollowObjectName { get; set; }
    [FlatBufferItem(04)] public Vec3f FollowPositionOffset { get; set; }
    [FlatBufferItem(05)] public Vec3f IconPosition { get; set; }
    [FlatBufferItem(06)] public float EffectRadius { get; set; }
    [FlatBufferItem(07)] public string ReleasePointFlagStr { get; set; }
    [FlatBufferItem(08)] public bool IsIncludeSkyFly { get; set; }
    [FlatBufferItem(09)] public float EndPlayerRotationY { get; set; }
    [FlatBufferItem(10)] public EndCameraType EndCameraType { get; set; }
    [FlatBufferItem(11)] public string EnableFlyFlagStr { get; set; }
    [FlatBufferItem(12)] public SkyFlyPointType SkyFlyPointKind { get; set; }
    [FlatBufferItem(13)] public Vec3f SkyFlyPointOffset { get; set; }
    [FlatBufferItem(14)] public OnCursorDataKind OnCursorDataKind { get; set; }
    [FlatBufferItem(15)] public string OnCursorDataID { get; set; }
    [FlatBufferItem(16)] public string IconTextureReplaceFileName { get; set; }
    [FlatBufferItem(17)] public bool IsDisplayInZoomOut { get; set; }
    [FlatBufferItem(18)] public bool IsDisplayInZoomNormal { get; set; }
    [FlatBufferItem(19)] public bool IsDisplayInZoomIn { get; set; }
    [FlatBufferItem(20)] public string PointName { get; set; }
    [FlatBufferItem(21)] public string ExInfoTextureName { get; set; }
    [FlatBufferItem(22)] public string ExInfoTextLabelName { get; set; }
    [FlatBufferItem(23)] public string IconThumbnailTextureName { get; set; }
    [FlatBufferItem(24)] public IconDisplayLevel IconDisplayLevel { get; set; }
    [FlatBufferItem(25)] public IconAnimPtn IconAnimPtn { get; set; }
    [FlatBufferItem(26)] public IconColorPtn IconColorPtn { get; set; }
}

[FlatBufferEnum(typeof(int))]
public enum EndCameraType
{
    FRONT = 0,
    BACK = 1,
}

[FlatBufferEnum(typeof(int))]
public enum IconAnimPtn
{
    NONE = 0,
    MUSHI = 1,
    KUSA = 2,
    MIZU = 3,
    DENKI = 4,
    NORMAL = 5,
    KOORI = 6,
    GHOST = 7,
    ESPER = 8,
    AKU = 9,
    FAIRY = 10,
    HONOO = 11,
    DOKU = 12,
    KAKUTOU = 13,
    HIKOU = 14,
    HAGANE = 15,
    IWA = 16,
    JIMEN = 17,
    DRAGON = 18,
}

[FlatBufferEnum(typeof(int))]
public enum IconColorPtn
{
    GYM = 0,
    DAN = 1,
    NUSHI = 2,
    UTIL = 3,
}

[FlatBufferEnum(typeof(int))]
public enum IconDisplayLevel
{
    FULLVIEW = 0,
    ZOOMOUT = 1,
    DEFAULT = 2,
    ZOOMIN = 3,
}

[FlatBufferEnum(typeof(int))]
public enum MapIconKind
{
    POKECEN = 0,
    JIM = 1,
    DAN = 2,
    NUSHI = 3,
    COMMON = 4,
    TREASURE_RAID = 5,
    GREAT_OCCUR = 6,
    NETWORK_PLAYER = 7,
    TOWN = 8,
    HAIRSALON = 9,
    BOUTIQUE = 10,
    RESTAURANT = 11,
    SHOP_UTIL = 12,
    FOOD_SHOP = 13,
    LANDMARK = 14,
    IDO_JUNDEN = 15,
    FLY_POINT = 16,
}

[FlatBufferEnum(typeof(int))]
public enum OnCursorDataKind
{
    NONE = 0,
    TOWN = 1,
    POINT = 2,
    MISSION = 3,
}

[FlatBufferEnum(typeof(int))]
public enum SkyFlyPointType
{
    NORMAL = 0,
    SAFETY = 1,
    PLAYER = 2,
}
