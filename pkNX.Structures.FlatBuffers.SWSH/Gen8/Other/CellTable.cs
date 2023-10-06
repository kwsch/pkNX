using System.ComponentModel;
using System.Text;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

// poke_memory_data.prmb and others
// this is oddly similar to the map data FlatBuffer, assumed the same schema to encode an excel table?
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class CellTable
{
    public string[] Dump(int cellsPerRow, char tab = '\t')
    {
        var result = new string[MainTable.Count / cellsPerRow];
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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class CellMetaQuad
{
    public override string ToString() => $"{Field0}|{Field1}|{Field2}|{Field3}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct CellUnion
{
    public override string ToString() => Discriminator switch
    {
        1 => Item1.ToString(),
        2 => Item2.ToString(),
        3 => Item3.ToString(),
        4 => Item4.ToString(),
        _ => $"Unrecognized: {Discriminator}",
    };
}

[TypeConverter(typeof(ExpandableObjectConverter))] public partial class CellInt    { public override string ToString() => Value.ToString(); }
[TypeConverter(typeof(ExpandableObjectConverter))] public partial class CellFloat  { public override string ToString() => Value.ToString("R"); }
[TypeConverter(typeof(ExpandableObjectConverter))] public partial class CellBool   { public override string ToString() => Flag.ToString(); }
[TypeConverter(typeof(ExpandableObjectConverter))] public partial class CellString { public override string ToString() => Name; }
[TypeConverter(typeof(ExpandableObjectConverter))] public partial class CellHash   { public override string ToString() => Hash.ToString("X16"); }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class CellMetaHashes { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class CellHashTuple
{
    public override string ToString() => $"{Hash0:X16} {Hash1:X16}";
}
