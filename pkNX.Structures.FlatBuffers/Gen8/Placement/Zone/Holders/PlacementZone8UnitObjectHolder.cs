using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    // Gates, Elevators, Tents, Flags, FossilRepair?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8UnitObjectHolder
    {
        [FlatBufferItem(00)] public PlacementZone8UnitObject Object { get; set; } = new();

        public override string ToString() => Object.Model;
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8UnitObject
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public string Model { get; set; } = "";
        [FlatBufferItem(02)] public string Animation { get; set; } = "";
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public string Field_05 { get; set; } = ""; // none have this
        [FlatBufferItem(06)] public string Field_06 { get; set; } = ""; // none have this
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
        [FlatBufferItem(11)] public PlacementZoneDeepY8 Unknown { get; set; } = new();
        [FlatBufferItem(12)] public byte Number { get; set; }
        [FlatBufferItem(13)] public PlacementZone8UnitObjectDetails Details { get; set; } = new();
        [FlatBufferItem(14)] public PlacementZone8UnitObjectToggle Dummy { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8UnitObjectDetails
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public float Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }

    // probably a union, with only 1 object type used
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8UnitObjectToggle
    {
        [FlatBufferItem(00)] public bool Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8UnitObjectInner Field_01 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8UnitObjectInner
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; } // 50 for only 1 entry
        [FlatBufferItem(01)] public float Field_01 { get; set; }
    }
}
