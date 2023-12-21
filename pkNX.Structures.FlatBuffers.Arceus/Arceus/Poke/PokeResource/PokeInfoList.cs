using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

// bin/appli/res_pokemon/list/pokemon_info_list.bin

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoList
{
    public PokeInfo GetEntry(ushort species)
    {
        return Table.FirstOrDefault(z => z.Species == species) ?? new()
        {
            Species = species,
            Children = Array.Empty<PokeInfoDetail>(),
        };
    }

    public bool HasEntry(ushort species)
    {
        return Table.Any(x => x.Species == species);
    }

    public PokeInfo AddEntry(ushort species, byte formCount)
    {
        Debug.Assert(!HasEntry(species), "The resource info table already contains an entry for the same species!");

        var entry = new PokeInfo
        {
            Species = species,
            Children = new PokeInfoDetail[formCount],
        };

        int genderId = 0;

        for (int form = 0; form < formCount; form++)
        {
            entry.Children[form] = new PokeInfoDetail
            {
                Form = form,
                Detail = new PokeInfoDetail_2[]
                {
                    new() {
                        IsRare = false,
                        Detail = new PokeInfoDetail_3[] {
                            new() {
                                Detail = new PokeInfoDetail_5[] {
                                    new() {
                                        AssetName = $"{species:0000}_{genderId:000}_{form:000}_n_00000000_fn_n",
                                        Gender = new PokeInfoGender(),
                                    },
                                },
                            },
                        },
                    },
                    new() {
                        IsRare = true, // Shiny form
                        Detail = new PokeInfoDetail_3[] {
                            new() {
                                Detail = new PokeInfoDetail_5[] {
                                    new() {
                                        AssetName = $"{species:0000}_{genderId:000}_{form:000}_n_00000000_fn_r",
                                        Gender = new PokeInfoGender(),
                                    },
                                },
                            },
                        },
                    },
                },
            };
        }

        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ToArray();
        return entry;
    }

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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfo { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoDetail { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoDetail_2 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoDetail_3 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoDetail_5 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeInfoGender { }
