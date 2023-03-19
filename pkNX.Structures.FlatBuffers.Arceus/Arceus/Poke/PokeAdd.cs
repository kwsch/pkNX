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
public partial class PokeAddArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAdd
{
    public string Dump(string[] speciesNames)
    {
        var natureStr = Nature == NatureType.Random ? "" : $", Nature = {Nature}";
        var shinyStr = $", Shiny = {ShinyLock}";
        var gvStr = "";
        if (GVHP != 0)
            gvStr += $", GV_HP = {GVHP}";
        if (GVATK != 0)
            gvStr += $", GV_ATK = {GVATK}";
        if (GVDEF != 0)
            gvStr += $", GV_DEF = {GVDEF}";
        if (GVSPA != 0)
            gvStr += $", GV_SPA = {GVSPA}";
        if (GVSPD != 0)
            gvStr += $", GV_SPD = {GVSPD}";
        if (GVSPE != 0)
            gvStr += $", GV_SPE = {GVSPE}";

        var hw = $"HeightScalar = {Height:000}, WeightScalar = {Weight:000}";
        var genderStr = Gender == -1 ? "" : $", Gender = {Gender}";
        var iv = NumPerfectIvs == 0 ? "" : $", FlawlessIVCount = {NumPerfectIvs}";
        string comment = $"// {speciesNames[Species]}{(Form == 0 ? "" : $"-{Form}")}";
        return $"new({Species:000},{Form:000},{Level:00}) {{ Gift = true, {hw}{shinyStr}{genderStr}{natureStr}{iv}{gvStr} }}, {comment}";
    }
}
