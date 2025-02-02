using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaWeatherTable;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaWeather
{
    public int WeightTotal => WeightSunny + WeightCloudy + WeightRain + WeightSnow + WeightDrought + WeightFog + WeightRainstorm + WeightSnowstorm;
}
