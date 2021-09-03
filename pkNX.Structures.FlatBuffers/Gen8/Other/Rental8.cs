using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Rental8Archive : IFlatBufferArchive<Rental8>
    {
        [FlatBufferItem(00)] public Rental8[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Rental8
    {
        [FlatBufferItem(00)] public byte EV_SPE { get; set; }
        [FlatBufferItem(01)] public byte EV_ATK { get; set; }
        [FlatBufferItem(02)] public byte EV_DEF { get; set; }
        [FlatBufferItem(03)] public byte EV_HP { get; set; }
        [FlatBufferItem(04)] public byte EV_SPA { get; set; }
        [FlatBufferItem(05)] public byte EV_SPD { get; set; }
        [FlatBufferItem(06)] public byte Form { get; set; }
        [FlatBufferItem(07)] public int Ball { get; set; }
        [FlatBufferItem(08)] public ulong Hash1 { get; set; }
        [FlatBufferItem(09)] public int Item { get; set; }
        [FlatBufferItem(10)] public byte Level { get; set; }
        [FlatBufferItem(11)] public int Species { get; set; }
        [FlatBufferItem(12)] public ulong Hash2 { get; set; }
        [FlatBufferItem(13)] public uint TrainerID { get; set; } // maybe?? no entries have this
        [FlatBufferItem(14)] public int Nature { get; set; }
        [FlatBufferItem(15)] public int Gender { get; set; }
        [FlatBufferItem(16)] public sbyte IV_SPE { get; set; }
        [FlatBufferItem(17)] public sbyte IV_ATK { get; set; }
        [FlatBufferItem(18)] public sbyte IV_DEF { get; set; }
        [FlatBufferItem(19)] public sbyte IV_HP { get; set; }
        [FlatBufferItem(20)] public sbyte IV_SPA { get; set; }
        [FlatBufferItem(21)] public sbyte IV_SPD { get; set; }
        [FlatBufferItem(22)] public int Ability { get; set; } // 0,1,2(Hidden)
        [FlatBufferItem(23)] public int Move1 { get; set; }
        [FlatBufferItem(24)] public int Move2 { get; set; }
        [FlatBufferItem(25)] public int Move3 { get; set; }
        [FlatBufferItem(26)] public int Move4 { get; set; }

        public Species SpeciesID => (Species)Species;
    }
}
