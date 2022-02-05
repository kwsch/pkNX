using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoList8a : IFlatBufferArchive<PokeInfo8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public int TableDataLength { get; set; }
    [FlatBufferItem(1)] public PokeInfo8a[] Table { get; set; } = Array.Empty<PokeInfo8a>();

    public (ushort[] SF, byte[] Gender) Parse()
    {
        var entries = GetDexFormEntries().GroupBy(z => (int)(ushort)z)
            .Select(Merge).Distinct().OrderBy(z => (ushort)z).ToList();
        var SF = new ushort[entries.Count];
        var Gender = new byte[entries.Count];
        for (int i = 0; i < SF.Length; i++)
        {
            SF[i] = (ushort)entries[i];
            Gender[i] = (byte)(entries[i] >> 16);
        }
        return (SF, Gender);
    }

    private static int Merge(IGrouping<int, int> arg)
    {
        var species = arg.Key;
        foreach (var value in arg)
            species |= value;
        return species;
    }

    public IEnumerable<int> GetDexFormEntries()
    {
        foreach (var x in Table)
        {
            var s = x.Species;
            foreach (var fe in x.Children)
            {
                var f = fe.Form;
                foreach (var w in fe.Detail)
                {
                    bool rare = w.IsRare;
                    foreach (var z in w.Detail)
                    {
                        var unk = z.Unknown;
                        foreach (var r in z.Detail)
                        {
                            var gender = r.Gender.GenderValue;
                            yield return (s | (f << 11)) | ((1 << (int)gender) << 16);
                        }
                    }
                }
            }
        }
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfo8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public PokeInfoDetail8a[] Children { get; set; } = Array.Empty<PokeInfoDetail8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoDetail8a
{
    [FlatBufferItem(00)] public int Form { get; set; }
    [FlatBufferItem(01)] public PokeInfoDetail8a_2[] Detail { get; set; } = Array.Empty<PokeInfoDetail8a_2>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoDetail8a_2
{
    [FlatBufferItem(00)] public bool IsRare { get; set; }
    [FlatBufferItem(01)] public PokeInfoDetail8a_3[] Detail { get; set; } = Array.Empty<PokeInfoDetail8a_3>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoDetail8a_3
{
    [FlatBufferItem(00)] public bool Unknown { get; set; }
    [FlatBufferItem(01)] public PokeInfoDetail8a_5[] Detail { get; set; } = Array.Empty<PokeInfoDetail8a_5>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoDetail8a_5
{
    [FlatBufferItem(00)] public PokeInfoGender8a Gender { get; set; } = new();
    [FlatBufferItem(01)] public string AssetName { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeInfoGender8a
{
    [FlatBufferItem(00)] public PokeInfoGender GenderValue { get; set; }
}

[FlatBufferEnum(typeof(byte))]
public enum PokeInfoGender : byte
{
    GenderDiffMale,
    GenderDiffFemale,
    Genderless,
    DualGenderNoDifference,
    MaleOnly,
    FemaleOnly,
}
