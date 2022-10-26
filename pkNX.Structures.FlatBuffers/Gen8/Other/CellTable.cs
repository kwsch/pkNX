using System.ComponentModel;
using System.Text;
using FlatSharp;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

// poke_memory_data.prmb and others
// this is oddly similar to the map data FlatBuffer, assumed the same schema to encode an excel table?
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CellTable
{
    [FlatBufferItem(0)] public CellMetaQuad[] QuadTable { get; set; }
    [FlatBufferItem(1)] public CellUnion[] MainTable { get; set; }
    [FlatBufferItem(2)] public CellMetaHashes[] DualTable { get; set; }

    public string[] Dump(int cellsPerRow, char tab = '\t')
    {
        var result = new string[MainTable.Length / cellsPerRow];
        for (int i = 0; i < result.Length; i++)
        {
            var index = i * cellsPerRow;
            var sb = new StringBuilder();
            for (int j = 0; j < cellsPerRow; j++)
            {
                if (j != 0)
                    sb.Append(tab);
                sb.Append(MainTable[index + j]);
            }
            result[i] = sb.ToString();
        }
        return result;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CellMetaQuad
{
    [FlatBufferItem(0)] public int Field0 { get; set; }
    [FlatBufferItem(1)] public int Field1 { get; set; }
    [FlatBufferItem(2)] public int Field2 { get; set; }
    [FlatBufferItem(3)] public int Field3 { get; set; }

    public override string ToString() => $"{Field0}|{Field1}|{Field2}|{Field3}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CellUnion
{
#nullable enable
    [FlatBufferItem(0)] public FlatBufferUnion<CellInt, CellFloat, CellString, CellHash>? Field1 { get; set; }
#nullable disable
    public override string ToString()
    {
        if (Field1 is not { } f)
            return "Empty";
        if (!Field1.HasValue)
            return "No Value";

        return f.Discriminator switch
        {
            1 => Field1.Value.Item1.ToString(),
            2 => Field1.Value.Item2.ToString(),
            3 => Field1.Value.Item3.ToString(),
            4 => Field1.Value.Item4.ToString(),
            _ => $"Unrecognized: {f.Discriminator}",
        };
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class CellInt    { [FlatBufferItem(0)] public int    Value { get; set; } public override string ToString() => Value.ToString(); }
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class CellFloat  { [FlatBufferItem(0)] public float  Value { get; set; } public override string ToString() => Value.ToString("R"); }
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class CellBool   { [FlatBufferItem(0)] public bool   Flag  { get; set; } public override string ToString() => Flag.ToString(); }
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class CellString { [FlatBufferItem(0)] public string Name  { get; set; } public override string ToString() => Name; }
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))] public class CellHash   { [FlatBufferItem(0)] public ulong  Hash  { get; set; } public override string ToString() => Hash.ToString("X16"); }

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CellMetaHashes
{
    [FlatBufferItem(0)] public ulong Hash { get; set; }
    [FlatBufferItem(1)] public CellHashTuple[] Pairs { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CellHashTuple
{
    [FlatBufferItem(0)] public ulong Hash0 { get; set; }
    [FlatBufferItem(1)] public ulong Hash1 { get; set; }

    public override string ToString() => $"{Hash0:X16} {Hash1:X16}";
}
