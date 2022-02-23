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
public class EventEncountPoke8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public int Form { get; set; }
    [FlatBufferItem(02)] public int Gender { get; set; }
    [FlatBufferItem(03)] public ShinyType8a ShinyLock { get; set; }
    [FlatBufferItem(04)] public int Level { get; set; }
    [FlatBufferItem(05)] public int AbilityRandType { get; set; }
    [FlatBufferItem(06)] public int Nature { get; set; }
    [FlatBufferItem(07)] public int Height { get; set; }
    [FlatBufferItem(08)] public int Weight { get; set; }
    [FlatBufferItem(09)] public bool Field_09 { get; set; }
    [FlatBufferItem(10)] public bool Field_10 { get; set; }
    [FlatBufferItem(11)] public bool Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public string Field_13 { get; set; } = string.Empty;
    [FlatBufferItem(14)] public string Field_14 { get; set; } = string.Empty;
    [FlatBufferItem(15)] public string Field_15 { get; set; } = string.Empty;
    [FlatBufferItem(16)] public int ExperiencePoints { get; set; }
    [FlatBufferItem(17)] public bool IsOybn { get; set; }
    [FlatBufferItem(18)] public int Move1 { get; set; }
    [FlatBufferItem(19)] public bool Mastered1 { get; set; }
    [FlatBufferItem(20)] public int Move2 { get; set; }
    [FlatBufferItem(21)] public bool Mastered2 { get; set; }
    [FlatBufferItem(22)] public int Move3 { get; set; }
    [FlatBufferItem(23)] public bool Mastered3 { get; set; }
    [FlatBufferItem(24)] public int Move4 { get; set; }
    [FlatBufferItem(25)] public bool Mastered4 { get; set; }
    [FlatBufferItem(26)] public int IV_HP { get; set; }
    [FlatBufferItem(27)] public int IV_ATK { get; set; }
    [FlatBufferItem(28)] public int IV_DEF { get; set; }
    [FlatBufferItem(29)] public int IV_SPA { get; set; }
    [FlatBufferItem(30)] public int IV_SPD { get; set; }
    [FlatBufferItem(31)] public int IV_SPE { get; set; }
    [FlatBufferItem(32)] public int NumPerfectIvs { get; set; }
    [FlatBufferItem(33)] public int GV_HP { get; set; }
    [FlatBufferItem(34)] public int GV_ATK { get; set; }
    [FlatBufferItem(35)] public int GV_DEF { get; set; }
    [FlatBufferItem(36)] public int GV_SPA { get; set; }
    [FlatBufferItem(37)] public int GV_SPD { get; set; }
    [FlatBufferItem(38)] public int GV_SPE { get; set; }

    public bool HasMoveset => Move1 != 0;
    public bool HasGVs => GV_HP != 0 || GV_ATK != 0 || GV_DEF != 0 || GV_SPA != 0 || GV_SPD != 0 || GV_SPE != 0;

    public string Dump(string[] speciesNames, string argEncounterName)
    {
        var natureStr = Nature == -1 ? "" : $", Nature = (int){(NatureType8a)Nature}";
        var shinyStr = $", Shiny = {ShinyLock}";
        var gvStr = "";
        if (HasGVs)
            gvStr = $", GVs = new[]{{{GV_HP},{GV_ATK},{GV_DEF},{GV_SPE},{GV_SPA},{GV_SPD}}}";

        var moves = !HasMoveset ? "" : $", Moves = new[] {{{Move1:000},{Move2:000},{Move3:000},{Move4:000}}}";
        var mastery = !HasMoveset ? "" : $", Mastery = new[] {{{Mastered1.ToString().ToLower()},{Mastered2.ToString().ToLower()},{Mastered3.ToString().ToLower()},{Mastered4.ToString().ToLower()}}}";
        var alpha = !IsOybn ? "" : ", IsAlpha = true";
        var genderStr = Gender == -1 ? "" : $", Gender = {Gender}";
        var iv = NumPerfectIvs == 0 ? "" : $", FlawlessIVCount = {NumPerfectIvs}";
        string comment = $"// {argEncounterName}: {speciesNames[Species]}{(Form == 0 ? "" : $"-{Form}")}";
        return $"new({Species:000},{Form:000},{Level:00},{Height},{Weight}) {{ Location = -01{shinyStr}{alpha}{genderStr}{natureStr}{iv}{gvStr}{moves}{mastery} }}, {comment}";
    }
}
