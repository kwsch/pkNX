namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class AreaWeather
{
    public int WeightTotal => WeightSunny + WeightCloudy + WeightRain + WeightSnow + WeightDrought + WeightFog + WeightRainstorm + WeightSnowstorm;
}
