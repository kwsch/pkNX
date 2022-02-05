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
public class EncounterOybnTraits8a
{
    [FlatBufferItem(00)] public bool Oybn1 { get; set; }
    [FlatBufferItem(01)] public bool Oybn2 { get; set; }
    [FlatBufferItem(02)] public bool Field_02 { get; set; }
    [FlatBufferItem(03)] public bool Field_03 { get; set; }

    public bool IsOybnAny => Oybn1 || Oybn2 || Field_02 || Field_03;
}
