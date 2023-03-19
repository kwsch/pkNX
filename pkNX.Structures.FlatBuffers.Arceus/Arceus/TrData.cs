using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrainerData
{
    public string TeamSummary => Environment.NewLine + string.Join(Environment.NewLine, Team.Select(z => "\t" + z));
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrainerPoke
{
    public override string ToString()
    {
        return $"{((Species)Species) + (Form == 0 ? "" : $"-{Form}"),-15}|({Move01,4},{Move02,4},{Move03,4},{Move04,4})|{Level,2}|{Nature,8}|{Gender}|({GVHP}/{GVATK}/{GVDEF}/{GVSPA}/{GVSPD}/{GVSPE})";
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class WazaSet
{
    public override string ToString() => $"{Move}{(Mastered ? "*":"")}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrFloatQuad
{
    public override string ToString() => $"({Float00},{Float01},{Float02},{Float03})";
}
