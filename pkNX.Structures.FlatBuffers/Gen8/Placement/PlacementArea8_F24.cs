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
    public class PlacementArea8_F24
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementAreaUnknownTiny8 Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }

        public override string ToString() => $"{Field_00}, {Field_02}: {Field_01}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementAreaUnknownTiny8
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }

        public override string ToString() => $"{Field_00}, {Field_01}, {Field_02}";
    }
}
