using System.ComponentModel;
using FlatSharp;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneStaticObjectsHolder { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneStaticObject { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneStaticObjectSpawn
{
    public IEnumerable<string> GetSummary(IList<EncounterStatic> statics, IReadOnlyList<string> species)
    {
        var enc = statics.BinarySearchByFlatBufferKey(SpawnID)!;
        var index = statics.IndexOf(enc);
        yield return $"{species[enc.Species]}{(enc.Form == 0 ? string.Empty : "-" + enc.Form)} Lv. {enc.Level}";
        yield return $"Index: {index}";
        yield return $"EncounterID: {SpawnID:X016}";
        if (Field02 != 0xCBF29CE484222645)
            yield return $"Hash: {Field02:X16}";
        yield return $"Value: {Field03}";
        yield return $"Unknown: {Field04}";
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneStaticObjectUnknown
{
    public override string ToString() => $"{Field00} {Field01} {Field02} {Field03} {Field04}";
}
