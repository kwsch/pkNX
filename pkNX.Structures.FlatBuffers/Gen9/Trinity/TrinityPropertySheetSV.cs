using System;
using System.ComponentModel;
using FlatSharp;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetSV
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Extra { get; set; } = string.Empty;
    [FlatBufferItem(02)] public TrinityPropertySheetObjectSV[] Properties { get; set; } = Array.Empty<TrinityPropertySheetObjectSV>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetObjectSV
{
    [FlatBufferItem(00)] public TrinityPropertySheetFieldSV[] Fields { get; set; } = Array.Empty<TrinityPropertySheetFieldSV>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetFieldSV
{
    [FlatBufferItem(00)] public string Name { get; set; }
    [FlatBufferItem(01, Required = true)] public FlatBufferUnion<TrinityPropertySheetField1SV, TrinityPropertySheetField2SV, TrinityPropertySheetFieldStringValueSV, TrinityPropertySheetField4SV, TrinityPropertySheetField5SV, TrinityPropertySheetField6SV, TrinityPropertySheetField7SV, TrinityPropertySheetField8SV, TrinityPropertySheetObjectSV> Data { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField1SV { }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField2SV
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public byte FieldType { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetFieldStringValueSV
{
    [FlatBufferItem(00)] public string Value { get; set; } 
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField4SV { }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField5SV { }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField6SV { }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField7SV { }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPropertySheetField8SV { }
