using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ParamSet
{
    [FlatBufferItem(0)] public int HP { get; set; }
    [FlatBufferItem(1)] public int ATK { get; set; }
    [FlatBufferItem(2)] public int DEF { get; set; }
    [FlatBufferItem(3)] public int SPA { get; set; }
    [FlatBufferItem(4)] public int SPD { get; set; }
    [FlatBufferItem(5)] public int SPE { get; set; }

    public int[] Stats
    {
        get => new int[] { HP, ATK, DEF, SPA, SPD, SPE };
        set
        {
            if (value?.Length != 6) return;
            HP = (sbyte)value[0];
            ATK = (sbyte)value[1];
            DEF = (sbyte)value[2];
            SPA = (sbyte)value[3];
            SPD = (sbyte)value[4];
            SPE = (sbyte)value[5];
        }
    }
}
