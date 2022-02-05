using System;
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
public class PlacementSpawnerF218a
{
    [FlatBufferItem(0)] public ulong VarHash0 { get; set; }
    [FlatBufferItem(1)] public ulong VarHash1 { get; set; } // Sometimes contains Slot ID from specific encounter table specified by the F20 in the same Spawner?
    [FlatBufferItem(2)] public ulong VarHash2 { get; set; } // Sometimes contains Slot ID from specific encounter table specified by the F20 in the same Spawner?
    [FlatBufferItem(3)] public ulong VarHash3 { get; set; } // ??? Is this as 01/02?
    [FlatBufferItem(4)] public ulong VarHash4 { get; set; } // ??? Is this as 01/02?
    [FlatBufferItem(5)] public ulong VarHash5 { get; set; } // ??? Is this as 01/02?
    [FlatBufferItem(6)] public PlacementV3f8a Field_06 { get; set; } = new();
    [FlatBufferItem(7)] public string Field_07 { get; set; } = string.Empty;
    [FlatBufferItem(8)] public float Scalar { get; set; }
    [FlatBufferItem(9)] public PlacementV3f8a Field_09 { get; set; } = new();
    [FlatBufferItem(10)] public PlacementV3f8a Field_10 { get; set; } = new();
    [FlatBufferItem(11)] public int NumVarHashes { get; set; }

    public string SlotSummary
    {
        get
        {
            var rawSlots = new[] { VarHash0, VarHash1, VarHash2, VarHash3, VarHash4, VarHash5 };
            for (var i = 0; i < NumVarHashes; i++)
            {
                if (rawSlots[i] == 0xCBF29CE484222645)
                    throw new ArgumentException("VarHash shouldn't be empty!");
            }
            for (var i = NumVarHashes; i < rawSlots.Length; i++)
            {
                if (rawSlots[i] != 0xCBF29CE484222645)
                    throw new ArgumentException("VarHash should be empty!");
            }

            if (NumVarHashes == 0)
                return "/* Slots = */ None()";

            if (NumVarHashes == 1)
                throw new ArgumentException("Invalid NumVarHashes!");

            var slots = rawSlots.Skip(1).Take(NumVarHashes - 1).Select(EncounterSlot8a.GetSlotName);

            return $"/* Slots = */ 0x{VarHash0:X16}({string.Join(", ", slots)})";
        }
    }

    public override string ToString() => $"Spawn_21({SlotSummary}, {Field_06}, \"{Field_07}\", {Scalar}, {Field_09}, {Field_10})";
}
