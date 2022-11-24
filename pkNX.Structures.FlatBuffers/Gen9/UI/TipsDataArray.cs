using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TipsDataArray : IFlatBufferArchive<TipsData>
{
    [FlatBufferItem(0)] public TipsData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TipsData
{
    [FlatBufferItem(00)] public int Id { get; set; }
    [FlatBufferItem(01)] public int Sortnum { get; set; }
    [FlatBufferItem(02)] public string TipslistTitle { get; set; }
    [FlatBufferItem(03)] public string Page1Title { get; set; }
    [FlatBufferItem(04)] public string Page1Text { get; set; }
    [FlatBufferItem(05)] public string Page1Pic { get; set; }
    [FlatBufferItem(06)] public string Page2Title { get; set; }
    [FlatBufferItem(07)] public string Page2Text { get; set; }
    [FlatBufferItem(08)] public string Page2Pic { get; set; }
    [FlatBufferItem(09)] public string Page3Title { get; set; }
    [FlatBufferItem(10)] public string Page3Text { get; set; }
    [FlatBufferItem(11)] public string Page3Pic { get; set; }
    [FlatBufferItem(12)] public string Page4Title { get; set; }
    [FlatBufferItem(13)] public string Page4Text { get; set; }
    [FlatBufferItem(14)] public string Page4Pic { get; set; }
    [FlatBufferItem(15)] public string Page5Title { get; set; }
    [FlatBufferItem(16)] public string Page5Text { get; set; }
    [FlatBufferItem(17)] public string Page5Pic { get; set; }
    [FlatBufferItem(18)] public string Page6Title { get; set; }
    [FlatBufferItem(19)] public string Page6Text { get; set; }
    [FlatBufferItem(20)] public string Page6Pic { get; set; }
    [FlatBufferItem(21)] public string Page7Title { get; set; }
    [FlatBufferItem(22)] public string Page7Text { get; set; }
    [FlatBufferItem(23)] public string Page7Pic { get; set; }
    [FlatBufferItem(24)] public string Page8Title { get; set; }
    [FlatBufferItem(25)] public string Page8Text { get; set; }
    [FlatBufferItem(26)] public string Page8Pic { get; set; }
    [FlatBufferItem(27)] public string Page9Title { get; set; }
    [FlatBufferItem(28)] public string Page9Text { get; set; }
    [FlatBufferItem(29)] public string Page9Pic { get; set; }
    [FlatBufferItem(30)] public string Page10Title { get; set; }
    [FlatBufferItem(31)] public string Page10Text { get; set; }
    [FlatBufferItem(32)] public string Page10Pic { get; set; }
    [FlatBufferItem(33)] public string NewflagName { get; set; }
    [FlatBufferItem(34)] public string DispflagName { get; set; }
}
