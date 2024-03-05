using System.Collections.Generic;
using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterUndergroundArchive;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterUnderground
{
    public int Gender => 0; // Random
    public bool IsGigantamax => GigantamaxState == 2;

    public override string ToString() => $"{IndexNum:00} - {Species:000}";

    public string GetSummary(IReadOnlyList<string> species)
    {
        var gender = Gender == 0 ? string.Empty : $", Gender = {Gender - 1}";
        var comment = $" // {species[Species]}{(Form == 0 ? string.Empty : "-" + Form)}";
        var moves = $", Moves = new[] {{{Move0:000},{Move1:000},{Move2:000},{Move3:000}}}";
        var game = Version != 0 ? Version == 1 ? ", Version = GameVersion.SW" : ", Version = GameVersion.SH" : "";
        var g = IsGigantamax ? ", CanGigantamax = true" : "";
        return $"            new({Species:000},{Form},{Level:00}) {{ Ability = A{Ability}{gender}{moves}{g}{game} }},{comment}";
    }
}
