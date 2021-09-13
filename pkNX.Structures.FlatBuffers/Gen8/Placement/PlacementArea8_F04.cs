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
    public class PlacementArea8_F04
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets
        [FlatBufferItem(01)] public float Field_01 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets
        [FlatBufferItem(02)] public float Field_02 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets

        [FlatBufferItem(03)] public float Field_03 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets
        [FlatBufferItem(04)] public float Field_04 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets
        [FlatBufferItem(05)] public float Field_05 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets

        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }

        [FlatBufferItem(09)] public ulong Field_09 { get; set; } // unused, assumed to be same shape as other v3f-hash triplets
        [FlatBufferItem(10)] public ulong Field_10 { get; set; }
        [FlatBufferItem(11)] public ulong Field_11 { get; set; }
    }
}
