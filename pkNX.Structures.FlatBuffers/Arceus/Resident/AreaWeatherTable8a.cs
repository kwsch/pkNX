using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaWeatherTable8a : IFlatBufferArchive<AreaWeather8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public AreaWeather8a[] Table { get; set; } = Array.Empty<AreaWeather8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaWeather8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public int MinDuration { get; set; }
    [FlatBufferItem(02)] public int MaxDuration { get; set; }
    [FlatBufferItem(03)] public int WeightSunny { get; set; }
    [FlatBufferItem(04)] public int WeightCloudy { get; set; }
    [FlatBufferItem(05)] public int WeightRain { get; set; }
    [FlatBufferItem(06)] public int WeightSnow { get; set; }
    [FlatBufferItem(07)] public int WeightDrought { get; set; }
    [FlatBufferItem(08)] public int WeightFog { get; set; }
    [FlatBufferItem(09)] public int WeightRainstorm { get; set; }
    [FlatBufferItem(10)] public int WeightSnowstorm { get; set; }

    public int WeightTotal => WeightSunny + WeightCloudy + WeightRain + WeightSnow + WeightDrought + WeightFog + WeightRainstorm + WeightSnowstorm;
}
