using System;
using System.ComponentModel;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementSpawnerF21
{
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

            var slots = rawSlots.Skip(1).Take(NumVarHashes - 1).Select(EncounterSlot.GetSlotName);

            return $"/* Slots = */ 0x{VarHash0:X16}({string.Join(", ", slots)})";
        }
    }

    public override string ToString() => $"Spawn_21({SlotSummary}, {Field06}, \"{Field07}\", {Scalar}, {Field09}, {Field10})";
}
