using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EvolutionTable { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EvolutionEntry { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EvolutionSet
{
    public byte[] Write()
    {
        if (Table is null || Table.Count == 0)
            return Array.Empty<byte>();

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        foreach (var evo in Table)
        {
            // ReSharper disable RedundantCast
            bw.Write((ushort)GetMainlineMethod(evo.Method));
            bw.Write((ushort)evo.Argument);
            bw.Write((ushort)evo.Species);
            bw.Write((sbyte)evo.Form);
            bw.Write((byte)evo.Level);
            // ReSharper restore RedundantCast
        }
        return ms.ToArray();

        // Remap so all games have the same method values.
        static ushort GetMainlineMethod(ushort evoMethod) => evoMethod switch
        {
            50 => (ushort)EvolutionType.UseItem, // Ursaluna
            51 => (ushort)EvolutionType.UseMoveAgileStyle, // Wyrdeer
            52 => (ushort)EvolutionType.UseMoveStrongStyle, // Overqwil
            53 => (ushort)EvolutionType.LevelUpRecoilDamageMale, // Basculegion-0
            54 => (ushort)EvolutionType.LevelUpRecoilDamageFemale, // Basculegion-1
            _ => evoMethod,
        };
    }
}
