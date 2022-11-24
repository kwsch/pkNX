using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class FoodPokeTypeParam
{
    [FlatBufferItem(00)] public int Normal { get; set; }
    [FlatBufferItem(01)] public int Kakutou { get; set; }
    [FlatBufferItem(02)] public int Hikou { get; set; }
    [FlatBufferItem(03)] public int Doku { get; set; }
    [FlatBufferItem(04)] public int Jimen { get; set; }
    [FlatBufferItem(05)] public int Iwa { get; set; }
    [FlatBufferItem(06)] public int Mushi { get; set; }
    [FlatBufferItem(07)] public int Ghost { get; set; }
    [FlatBufferItem(08)] public int Hagane { get; set; }
    [FlatBufferItem(09)] public int Honoo { get; set; }
    [FlatBufferItem(10)] public int Mizu { get; set; }
    [FlatBufferItem(11)] public int Kusa { get; set; }
    [FlatBufferItem(12)] public int Denki { get; set; }
    [FlatBufferItem(13)] public int Esper { get; set; }
    [FlatBufferItem(14)] public int Koori { get; set; }
    [FlatBufferItem(15)] public int Dragon { get; set; }
    [FlatBufferItem(16)] public int Aku { get; set; }
    [FlatBufferItem(17)] public int Fairy { get; set; }

    public int GetBoostFromIndex(int index) => index switch
    {
        00 => Normal,
        01 => Kakutou,
        02 => Hikou,
        03 => Doku,
        04 => Jimen,
        05 => Iwa,
        06 => Mushi,
        07 => Ghost,
        08 => Hagane,
        09 => Honoo,
        10 => Mizu,
        11 => Kusa,
        12 => Denki,
        13 => Esper,
        14 => Koori,
        15 => Dragon,
        16 => Aku,
        17 => Fairy,
        _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
    };
}
