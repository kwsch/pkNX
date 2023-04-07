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
public partial class EventEncount { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EventEncountArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EventEncountPoke
{
    public bool HasMoveset => Move1 != 0;
    public bool HasGVs => GVHP != 0 || GVATK != 0 || GVDEF != 0 || GVSPA != 0 || GVSPD != 0 || GVSPE != 0;

    public string Dump(string[] speciesNames, string argEncounterName)
    {
        var natureStr = Nature == NatureType.Random ? "" : $", Nature = (int){(NatureType)Nature}";
        var shinyStr = $", Shiny = {ShinyLock}";
        var gvStr = "";
        if (HasGVs)
            gvStr = $", GVs = new[]{{{GVHP},{GVATK},{GVDEF},{GVSPE},{GVSPA},{GVSPD}}}";

        var moves = !HasMoveset ? "" : $", Moves = new[] {{{Move1:000},{Move2:000},{Move3:000},{Move4:000}}}";
        var mastery = !HasMoveset ? "" : $", Mastery = new[] {{{Mastered1.ToString().ToLower()},{Mastered2.ToString().ToLower()},{Mastered3.ToString().ToLower()},{Mastered4.ToString().ToLower()}}}";
        var alpha = !IsOybn ? "" : ", IsAlpha = true";
        var genderStr = Gender == -1 ? "" : $", Gender = {Gender}";
        var iv = NumPerfectIvs == 0 ? "" : $", FlawlessIVCount = {NumPerfectIvs}";
        string comment = $"// {argEncounterName}: {speciesNames[Species]}{(Form == 0 ? "" : $"-{Form}")}";
        return $"new({Species:000},{Form:000},{Level:00},{Height},{Weight}) {{ Location = -01{shinyStr}{alpha}{genderStr}{natureStr}{iv}{gvStr}{moves}{mastery} }}, {comment}";
    }
}
