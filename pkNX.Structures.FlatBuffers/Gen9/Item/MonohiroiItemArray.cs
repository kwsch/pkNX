using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MonohiroiItemArray : IFlatBufferArchive<MonohiroiItem>
{
    [FlatBufferItem(0)] public MonohiroiItem[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MonohiroiItem
{
    [FlatBufferItem(00)] public OneMonohiroiItem Item01 { get; set; }
    [FlatBufferItem(01)] public OneMonohiroiItem Item02 { get; set; }
    [FlatBufferItem(02)] public OneMonohiroiItem Item03 { get; set; }
    [FlatBufferItem(03)] public OneMonohiroiItem Item04 { get; set; }
    [FlatBufferItem(04)] public OneMonohiroiItem Item05 { get; set; }
    [FlatBufferItem(05)] public OneMonohiroiItem Item06 { get; set; }
    [FlatBufferItem(06)] public OneMonohiroiItem Item07 { get; set; }
    [FlatBufferItem(07)] public OneMonohiroiItem Item08 { get; set; }
    [FlatBufferItem(08)] public OneMonohiroiItem Item09 { get; set; }
    [FlatBufferItem(09)] public OneMonohiroiItem Item10 { get; set; }
    [FlatBufferItem(10)] public OneMonohiroiItem Item11 { get; set; }
    [FlatBufferItem(11)] public OneMonohiroiItem Item12 { get; set; }
    [FlatBufferItem(12)] public OneMonohiroiItem Item13 { get; set; }
    [FlatBufferItem(13)] public OneMonohiroiItem Item14 { get; set; }
    [FlatBufferItem(14)] public OneMonohiroiItem Item15 { get; set; }
    [FlatBufferItem(15)] public OneMonohiroiItem Item16 { get; set; }
    [FlatBufferItem(16)] public OneMonohiroiItem Item17 { get; set; }
    [FlatBufferItem(17)] public OneMonohiroiItem Item18 { get; set; }
    [FlatBufferItem(18)] public OneMonohiroiItem Item19 { get; set; }
    [FlatBufferItem(19)] public OneMonohiroiItem Item20 { get; set; }
    [FlatBufferItem(20)] public OneMonohiroiItem Item21 { get; set; }
    [FlatBufferItem(21)] public OneMonohiroiItem Item22 { get; set; }
    [FlatBufferItem(22)] public OneMonohiroiItem Item23 { get; set; }
    [FlatBufferItem(23)] public OneMonohiroiItem Item24 { get; set; }
    [FlatBufferItem(24)] public OneMonohiroiItem Item25 { get; set; }
    [FlatBufferItem(25)] public OneMonohiroiItem Item26 { get; set; }
    [FlatBufferItem(26)] public OneMonohiroiItem Item27 { get; set; }
    [FlatBufferItem(27)] public OneMonohiroiItem Item28 { get; set; }
    [FlatBufferItem(28)] public OneMonohiroiItem Item29 { get; set; }
    [FlatBufferItem(29)] public OneMonohiroiItem Item30 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class OneMonohiroiItem
{
    [FlatBufferItem(0)] public ItemID ItemId { get; set; }
    [FlatBufferItem(1)] public int Rate { get; set; }
}
