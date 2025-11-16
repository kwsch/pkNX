namespace pkNX.Structures.FlatBuffers.ZA;

public interface IAreaLayer
{
    AreaLayer Layer { get; }
    FieldAreaInfo AreaInfo { get; }
}

partial class FieldLocation : IAreaLayer;
partial class FieldSubArea : IAreaLayer;
partial class FieldMainArea : IAreaLayer;
partial class FieldSceneArea : IAreaLayer;

partial class FieldBattleZone : IAreaLayer, IZoneLayer;
partial class FieldWildZone : IAreaLayer, IZoneLayer;

public interface IZoneLayer
{
    string? ZoneID { get; }
}
