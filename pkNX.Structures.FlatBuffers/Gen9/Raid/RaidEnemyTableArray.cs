using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidEnemyTableArray : IFlatBufferArchive<RaidEnemyTable>
{
    [FlatBufferItem(0)] public RaidEnemyTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidEnemyTable
{
    [FlatBufferItem(0)] public RaidEnemyInfo RaidEnemyInfo { get; set; }
}
