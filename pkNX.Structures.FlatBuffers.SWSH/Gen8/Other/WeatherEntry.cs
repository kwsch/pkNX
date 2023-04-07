using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SWSH;

// field\param\weather\weather_data.bin
// field\param\weather\weather_data_alt.bin
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class WeatherTable { }
public partial class WeatherEntry { }
public partial class PentaFloat { }
public partial class QuadFloatSet { }
