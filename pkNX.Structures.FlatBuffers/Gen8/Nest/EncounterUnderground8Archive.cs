using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterUnderground8Archive : IFlatBufferArchive<EncounterUnderground8>
    {
        [FlatBufferItem(0)] public EncounterUnderground8[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterUnderground8
    {
        [FlatBufferItem(00)] public bool HasFlagRequirement { get; set; }
        [FlatBufferItem(01)] public ulong FlagRequirementID { get; set; }
        [FlatBufferItem(02)] public byte Field_02 { get; set; } // all zero, Gender?
        [FlatBufferItem(03)] public byte Form { get; set; }
        [FlatBufferItem(04)] public uint GigantamaxState { get; set; }
        [FlatBufferItem(05)] public uint Ball { get; set; }
        [FlatBufferItem(06)] public uint IndexNum { get; set; }
        [FlatBufferItem(07)] public uint Level { get; set; }
        [FlatBufferItem(08)] public int Species { get; set; }
        [FlatBufferItem(09)] public ulong UiMessageID { get; set; }
        [FlatBufferItem(10)] public uint OT_Gender { get; set; }
        [FlatBufferItem(11)] public byte Version { get; set; }
        [FlatBufferItem(12)] public uint Shiny { get; set; }
        [FlatBufferItem(13)] public sbyte IV_SPE { get; set; }
        [FlatBufferItem(14)] public sbyte IV_ATK { get; set; }
        [FlatBufferItem(15)] public sbyte IV_DEF { get; set; }
        [FlatBufferItem(16)] public sbyte IV_HP { get; set; }
        [FlatBufferItem(17)] public sbyte IV_SPA { get; set; }
        [FlatBufferItem(18)] public sbyte IV_SPD { get; set; }
        [FlatBufferItem(19)] public uint Ability { get; set; } // 1,2,4
        [FlatBufferItem(20)] public byte Field_14 { get; set; } // ultra beasts only, selectability
        [FlatBufferItem(21)] public uint Move0 { get; set; }
        [FlatBufferItem(22)] public uint Move1 { get; set; }
        [FlatBufferItem(23)] public uint Move2 { get; set; }
        [FlatBufferItem(24)] public uint Move3 { get; set; }

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
}
