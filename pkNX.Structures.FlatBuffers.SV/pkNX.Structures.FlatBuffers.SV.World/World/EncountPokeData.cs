using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncountPokeData
{
    public bool IsLevelRangeOverlap(int min, int max) => min <= MaxLevel && max >= MinLevel;
    public bool IsLevelRangeOverlap(float min, float max) => min <= MaxLevel && max >= MinLevel;
}
