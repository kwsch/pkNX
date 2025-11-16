using System;
using System.Collections.Generic;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public sealed class DumpHelper9a
{
    public required string[] SpeciesNames { get; init; }
    public required string[] AbilityNames { get; init; }
    public required string[] MoveNames { get; init; }
    public required string[] NatureNames { get; init; }
    public required string[] FormNames { get; init; }

    public required PersonalTable Personal { get; init; }
    public required AHTB Forms { get; init; }

    public string GetAbilityName(DevID species, short form, TokuseiType type)
        => PrettyDumpUtil.GetAbilityName(species, form, AbilityNames, Personal, type);

    public string GetMoveText(WazaSet waza) => PrettyDumpUtil.GetMoveText(waza, MoveNames);
    public List<ushort> GetCurrentMoves(DevID species, short form, byte level)
        => PrettyDumpUtil.GetCurrentMoves(species, form, level, Personal);
    public string GetEVSpread(ReadOnlySpan<int> evs) => PrettyDumpUtil.GetEVSpread(evs);


    public string GetFormName(ushort species, short form) => species switch
    {
        0128 when form is not 0 => $"Paldean Form / {TaurosForms[form]}", // Tauros
        0201 => $"Unown {UnownForms[form]}", // Unown
        0382 or 0383 or 0479 or 0646 or 0649 when form is 0 => string.Empty, // Kyogre, Groudon, Rotom, Kyurem, Genesect
        0658 when form is 1 => "Battle Bond", // Greninja
        0744 when form is 1 => "Own Tempo", // Rockruff
        0774 when form <= 6 => $"{MiniorForms[form]} Meteor Form", // Minior
        _ => PrettyDumpUtil.GetFormString(species, form, FormNames, Forms),
    };

    public string GetFormDisplay(short form, string name) => PrettyDumpUtil.GetFormDisplay(form, name);
    public string GetGender(int gender) => PrettyDumpUtil.GetGender(gender);
    public string GetNature(int nature) => PrettyDumpUtil.GetNature(NatureNames, nature);


    private static readonly string[] TaurosForms = ["", "Combat Breed", "Blaze Breed", "Aqua Breed"];
    private static ReadOnlySpan<char> UnownForms => "ABCDEFGHIJKLMNOPQRSTUVWXYZ!?";
    private static readonly string[] MiniorForms = ["Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"];
}
