using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class WazaSet { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ParamSet
{
    public string SlashSeparated() => $"{HP}/{ATK}/{DEF}/{SPA}/{SPD}/{SPE}";

    public int[] ToArray() => new[] { HP, ATK, DEF, SPA, SPD, SPE };
}
