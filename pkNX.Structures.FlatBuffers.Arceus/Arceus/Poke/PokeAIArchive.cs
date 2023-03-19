using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAIArchive
{
    public PokeAI GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(z => z.Species == species && z.Form == form) ?? PokeAI.GetNew();
    }

    public bool HasEntry(ushort species, ushort form, bool isAlpha)
    {
        return Table.Any(x => x.Species == species && x.Form == form && x.IsAlpha == isAlpha);
    }

    public PokeAI AddEntry(ushort species, ushort form, bool isAlpha)
    {
        Debug.Assert(!HasEntry(species, form, isAlpha), "The symbol behave table already contains an entry for the same species + form!");

        var entry = PokeAI.GetNew(species, form, isAlpha);
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ThenBy(x => x.IsAlpha)
            .ToArray();
        return entry;
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAI
{
    public static PokeAI GetNew(ushort species = 0, ushort form = 0, bool isAlpha = false) => new()
    {
        Species = species,
        Form = form,
        IsAlpha = isAlpha,

        Behavior1 = string.Empty,
        Behavior2 = string.Empty,
        Behavior3 = string.Empty,
        Field09 = PokeAI_F09.DefaultBehaviour_F09,
        Field17 = PokeAI_F09.DefaultBehaviour_F09,
        Field18 = new(),
        Field19 = new(),
        Field20 = new(),
        Field21 = new(),
        MoveEffect1 = string.Empty,
        MoveEffect2 = string.Empty,
        Field24 = new(),
        Field27 = new(),
        Field28 = new(),
        Field29 = new(),
        Field30 = new(),
    };
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAI_F24 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAI_F18 { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAI_F09
{
    public static readonly PokeAI_F09 DefaultBehaviour02 = new()
    {
        Field00 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field01 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field02 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field03 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field04 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field05 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field06 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field07 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
        Field08 = new() { BehaviourHash = 9252365659083253459, Parameters = new[] { "0.5", "35" } },
    };

    public static readonly PokeAI_F09 DefaultBehaviour_F09 = new()
    {
        Field00 = PokeAIBehaviour.DefaultBehaviour02,
        Field01 = PokeAIBehaviour.Behaviour_X_Params,
        Field02 = PokeAIBehaviour.DefaultBehaviour02,
        Field03 = PokeAIBehaviour.DefaultBehaviour02,
        Field04 = PokeAIBehaviour.Behaviour_04_Params,
        Field05 = PokeAIBehaviour.DefaultBehaviour02,
        Field06 = PokeAIBehaviour.DefaultBehaviour02,
        Field07 = PokeAIBehaviour.DefaultBehaviour02,
        Field08 = PokeAIBehaviour.Behaviour_X2_Params,
    };

    public static readonly PokeAI_F09 DefaultBehaviour_F11 = new()
    {
        Field00 = PokeAIBehaviour.DefaultBehaviour02_20,
        Field01 = PokeAIBehaviour.Behaviour_04_Params,
        Field02 = PokeAIBehaviour.DefaultBehaviour02,
        Field03 = PokeAIBehaviour.DefaultBehaviour02,
        Field04 = PokeAIBehaviour.Behaviour_04_Params,
        Field05 = PokeAIBehaviour.DefaultBehaviour02,
        Field06 = PokeAIBehaviour.DefaultBehaviour02,
        Field07 = PokeAIBehaviour.DefaultBehaviour02,
        Field08 = PokeAIBehaviour.DefaultBehaviour02,
    };

    public static readonly PokeAI_F09 DefaultBehaviour05 = new()
    {
        Field00 = PokeAIBehaviour.DefaultBehaviour05,
        Field01 = PokeAIBehaviour.DefaultBehaviour05,
        Field02 = PokeAIBehaviour.DefaultBehaviour05,
        Field03 = PokeAIBehaviour.DefaultBehaviour05,
        Field04 = PokeAIBehaviour.Behaviour_04_Params,
        Field05 = PokeAIBehaviour.DefaultBehaviour05,
        Field06 = PokeAIBehaviour.DefaultBehaviour05,
        Field07 = PokeAIBehaviour.DefaultBehaviour05,
        Field08 = PokeAIBehaviour.DefaultBehaviour05,
    };
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeAIBehaviour
{
    public static readonly PokeAIBehaviour DefaultBehaviour02 = new() { BehaviourHash = 9252365659083253459, Parameters = new [] { "0.2", "35" } };
    public static readonly PokeAIBehaviour DefaultBehaviour02_20 = new() { BehaviourHash = 9252365659083253459, Parameters = new [] { "0.2", "20" } };
    public static readonly PokeAIBehaviour DefaultBehaviour05 = new() { BehaviourHash = 9252365659083253459, Parameters = new [] { "0.5", "35" } };
    public static readonly PokeAIBehaviour Behaviour_X_Params = new() { BehaviourHash = 17205188591700921149, Parameters = new [] { "10", "3", "run", "Normal" } };
    public static readonly PokeAIBehaviour Behaviour_X2_Params = new() { BehaviourHash = 17205188591700921149, Parameters = new [] { "10", "0.5", "run", "Normal" } };
    public static readonly PokeAIBehaviour Behaviour_04_Params = new() { BehaviourHash = 1234775724179408742, Parameters = new [] { "3", "5", "60", "90", "4", "run", "Normal" } };
}
