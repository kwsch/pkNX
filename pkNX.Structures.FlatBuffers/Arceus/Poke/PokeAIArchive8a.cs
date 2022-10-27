using System;
using System.ComponentModel;
using System.Diagnostics;
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
public class PokeAIArchive8a : IFlatBufferArchive<PokeAI8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokeAI8a[] Table { get; set; } = Array.Empty<PokeAI8a>();

    public PokeAI8a GetEntry(ushort species, ushort form)
    {
        return Array.Find(Table, z => z.Species == species && z.Form == form) ??
            new PokeAI8a();
    }

    public bool HasEntry(ushort species, ushort form, bool isAlpha)
    {
        return Table.Any(x => x.Species == species && x.Form == form && x.IsAlpha == isAlpha);
    }

    public PokeAI8a AddEntry(ushort species, ushort form, bool isAlpha)
    {
        Debug.Assert(!HasEntry(species, form, isAlpha), "The symbol behave table already contains an entry for the same species + form!");

        var entry = new PokeAI8a { Species = species, Form = form, IsAlpha = isAlpha };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ThenBy(x => x.IsAlpha)
            .ToArray();
        return entry;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a
{
    [FlatBufferItem(00)] public int Species { get; set; } // int
    [FlatBufferItem(01)] public int Form { get; set; } // int
    [FlatBufferItem(02)] public bool IsAlpha { get; set; } // bool
    [FlatBufferItem(03)][TypeConverter(typeof(AIBehaviourConverter))] public string Behavior1 { get; set; } = string.Empty;
    [FlatBufferItem(04)][TypeConverter(typeof(AIBehaviourConverter))] public string Behavior2 { get; set; } = string.Empty;
    [FlatBufferItem(05)][TypeConverter(typeof(AIBehaviourConverter))] public string Behavior3 { get; set; } = string.Empty;
    [FlatBufferItem(06)] public bool Field_06 { get; set; } // bool
    [FlatBufferItem(07)] public ulong Field_07 { get; set; } // ulong 2458747221614355867
    [FlatBufferItem(08)] public bool Field_08 { get; set; } = true; // bool, always true
    [FlatBufferItem(09)] public PokeAI8a_F09 Field_09 { get; set; } = PokeAI8a_F09.DefaultBehaviour_F09;
    [FlatBufferItem(10)] public bool Field_10 { get; set; } = true; // bool, always true
    [FlatBufferItem(11)] public PokeAI8a_F09 Field_11 { get; set; } = PokeAI8a_F09.DefaultBehaviour_F11;
    [FlatBufferItem(12)] public bool Field_12 { get; set; } = true; // bool, always true except for Darkrai
    [FlatBufferItem(13)] public PokeAI8a_F09 Field_13 { get; set; } = PokeAI8a_F09.DefaultBehaviour02;
    [FlatBufferItem(14)] public bool Field_14 { get; set; } = true; // bool, always true
    [FlatBufferItem(15)] public PokeAI8a_F09 Field_15 { get; set; } = PokeAI8a_F09.DefaultBehaviour05;
    [FlatBufferItem(16)] public bool Field_16 { get; set; } = true; // bool, always true
    [FlatBufferItem(17)] public PokeAI8a_F09 Field_17 { get; set; } = PokeAI8a_F09.DefaultBehaviour05;
    [FlatBufferItem(18)] public PokeAI8a_F18 Field_18 { get; set; } = new(); // only used by Kleavor-1
    [FlatBufferItem(19)] public PokeAI8a_F18 Field_19 { get; set; } = new(); // assumed same as above, none use
    [FlatBufferItem(20)] public PokeAI8a_F18 Field_20 { get; set; } = new(); // assumed same as above, none use
    [FlatBufferItem(21)] public PokeAI8a_F18 Field_21 { get; set; } = new(); // assumed same as above, only used by Kleavor-1 with fields 0,1,2
    [FlatBufferItem(22)] public string MoveEffect1 { get; set; } = string.Empty;
    [FlatBufferItem(23)] public string MoveEffect2 { get; set; } = string.Empty;
    [FlatBufferItem(24)] public PokeAI8a_F24 Field_24 { get; set; } = new();
    [FlatBufferItem(25)] public float Field_25 { get; set; } = 20; // float
    [FlatBufferItem(26)] public float Field_26 { get; set; } = 30; // float
    [FlatBufferItem(27)] public Vec3f Field_27 { get; set; } = new(20, 50, 100);
    [FlatBufferItem(28)] public Vec3f Field_28 { get; set; } = new(20, 50, 100);
    [FlatBufferItem(29)] public Vec3f Field_29 { get; set; } = new(20, 50, 100);
    [FlatBufferItem(30)] public Vec3f Field_30 { get; set; } = new(15, 10, 15);
    [FlatBufferItem(31)] public int Field_31 { get; set; } // int
    [FlatBufferItem(32)] public int Field_32 { get; set; } = -1;// int
    [FlatBufferItem(33)] public int Field_33 { get; set; } // int
    [FlatBufferItem(34)] public float Field_34 { get; set; } // cascoon/silcoon
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F24
{
    [FlatBufferItem(00)] public float Field_00 { get; set; } = 7;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F18
{
    [FlatBufferItem(00)] public bool Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F09
{
    public static readonly PokeAI8a_F09 DefaultBehaviour02 = new();

    public static readonly PokeAI8a_F09 DefaultBehaviour_F09 = new()
    {
        Field_00 = PokeAIBehaviour.DefaultBehaviour02,
        Field_01 = PokeAIBehaviour.Behaviour_X_Params,
        Field_02 = PokeAIBehaviour.DefaultBehaviour02,
        Field_03 = PokeAIBehaviour.DefaultBehaviour02,
        Field_04 = PokeAIBehaviour.Behaviour_04_Params,
        Field_05 = PokeAIBehaviour.DefaultBehaviour02,
        Field_06 = PokeAIBehaviour.DefaultBehaviour02,
        Field_07 = PokeAIBehaviour.DefaultBehaviour02,
        Field_08 = PokeAIBehaviour.Behaviour_X2_Params,
    };

    public static readonly PokeAI8a_F09 DefaultBehaviour_F11 = new()
    {
        Field_00 = PokeAIBehaviour.DefaultBehaviour02_20,
        Field_01 = PokeAIBehaviour.Behaviour_04_Params,
        Field_02 = PokeAIBehaviour.DefaultBehaviour02,
        Field_03 = PokeAIBehaviour.DefaultBehaviour02,
        Field_04 = PokeAIBehaviour.Behaviour_04_Params,
        Field_05 = PokeAIBehaviour.DefaultBehaviour02,
        Field_06 = PokeAIBehaviour.DefaultBehaviour02,
        Field_07 = PokeAIBehaviour.DefaultBehaviour02,
        Field_08 = PokeAIBehaviour.DefaultBehaviour02,
    };

    public static readonly PokeAI8a_F09 DefaultBehaviour05 = new()
    {
        Field_00 = PokeAIBehaviour.DefaultBehaviour05,
        Field_01 = PokeAIBehaviour.DefaultBehaviour05,
        Field_02 = PokeAIBehaviour.DefaultBehaviour05,
        Field_03 = PokeAIBehaviour.DefaultBehaviour05,
        Field_04 = PokeAIBehaviour.Behaviour_04_Params,
        Field_05 = PokeAIBehaviour.DefaultBehaviour05,
        Field_06 = PokeAIBehaviour.DefaultBehaviour05,
        Field_07 = PokeAIBehaviour.DefaultBehaviour05,
        Field_08 = PokeAIBehaviour.DefaultBehaviour05,
    };

    [FlatBufferItem(00)] public PokeAIBehaviour Field_00 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(01)] public PokeAIBehaviour Field_01 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(02)] public PokeAIBehaviour Field_02 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(03)] public PokeAIBehaviour Field_03 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(04)] public PokeAIBehaviour Field_04 { get; set; } = PokeAIBehaviour.Behaviour_04_Params;
    [FlatBufferItem(05)] public PokeAIBehaviour Field_05 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(06)] public PokeAIBehaviour Field_06 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(07)] public PokeAIBehaviour Field_07 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
    [FlatBufferItem(08)] public PokeAIBehaviour Field_08 { get; set; } = PokeAIBehaviour.DefaultBehaviour02;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAIBehaviour
{
    public static readonly PokeAIBehaviour DefaultBehaviour02 = new() { BehaviourHash = 9252365659083253459, Parameters = new string[] { "0.2", "35" } };
    public static readonly PokeAIBehaviour DefaultBehaviour02_20 = new() { BehaviourHash = 9252365659083253459, Parameters = new string[] { "0.2", "20" } };
    public static readonly PokeAIBehaviour DefaultBehaviour05 = new() { BehaviourHash = 9252365659083253459, Parameters = new string[] { "0.5", "35" } };
    public static readonly PokeAIBehaviour Behaviour_X_Params = new() { BehaviourHash = 17205188591700921149, Parameters = new string[] { "10", "3", "run", "Normal" } };
    public static readonly PokeAIBehaviour Behaviour_X2_Params = new() { BehaviourHash = 17205188591700921149, Parameters = new string[] { "10", "0.5", "run", "Normal" } };
    public static readonly PokeAIBehaviour Behaviour_04_Params = new() { BehaviourHash = 1234775724179408742, Parameters = new string[] { "3", "5", "60", "90", "4", "run", "Normal" } };

    [FlatBufferItem(00)] public ulong BehaviourHash { get; set; } = 9252365659083253459;
    [FlatBufferItem(01)] public string[] Parameters { get; set; } = { "0.5", "35" };
}
