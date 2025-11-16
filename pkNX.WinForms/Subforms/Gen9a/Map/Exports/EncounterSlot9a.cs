using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers.ZA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.WinForms;

public sealed record EncounterSlot9a(ushort Species, byte Form, byte LevelMin, byte LevelMax, sbyte Gender, sbyte Nature, bool IsAlpha, Shiny Shiny)
{
    public bool IsSame(EncounterSlot9a b) => Species == b.Species && Form == b.Form && Gender == b.Gender && IsAlpha == b.IsAlpha && Shiny == b.Shiny;
    public bool LevelOverlap(EncounterSlot9a b) => !(LevelMax < b.LevelMin || b.LevelMax < LevelMin);

    public EncounterSlot9a MergeWith(EncounterSlot9a b)
    {
        var newMin = Math.Min(LevelMin, b.LevelMin);
        var newMax = Math.Max(LevelMax, b.LevelMax);
        return this with { LevelMin = newMin, LevelMax = newMax };
    }

    /// <summary>
    /// Creates a copy of the slot as an Alpha spawn, with an applied level boost to both min and max levels.
    /// </summary>
    /// <param name="boost">Amount to increase each level component by.</param>
    public EncounterSlot9a WithAlpha(int boost) => this with { IsAlpha = true, LevelMax = (byte)(LevelMax + boost), LevelMin = (byte)(LevelMin + boost) };

    public void Write(BinaryWriter bw)
    {
        bw.Write(Species);
        bw.Write(Form);
        bw.Write(Gender); // Required; gender is used in generator. Can't generalize to "random gender" via slot compression.

        bw.Write(LevelMin);
        bw.Write(LevelMax);

        bw.Write(IsAlpha ? (byte)1 : (byte)0);
        bw.Write((byte)(Shiny switch
        {
            Shiny.Never => 1,
            Shiny.Always => 2,
            _ => 0, // Random
        })); // can be packed into IsAlpha if we get sizeof(slot) down to 6

        // Assert that none have fixed natures. We have already used 8 bytes of a structure for the above fields.
        // bw.Write(slot.Nature); // seems to always be FF, can be omitted if so
        if (Nature is not unchecked((sbyte)0xFF))
            throw new ArgumentOutOfRangeException(nameof(Nature), Nature, null);
    }

    public static void WritePickleWithoutLocations(IEnumerable<EncounterSlot9a> slots, string path)
    {
        var ms = new MemoryStream();
        var bw = new BinaryWriter(ms);
        foreach (var slot in slots)
            slot.Write(bw);
        File.WriteAllBytes(Path.ChangeExtension(path, ".bin"), ms.ToArray());
    }

    public static void WritePickle(IReadOnlyList<EncounterArea9a> slotSets, string path)
    {
        var result = new byte[slotSets.Count][];
        var i = 0;
        foreach (var area in slotSets.OrderBy(z => z.Location))
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            // keep location header 4 bytes for future-proofing and better aligned slot reads (rather than 2 bytes)
            bw.Write((ushort)area.Location);
            bw.Write((ushort)0); // reserved (slot type) -- location IDs are already pretty high (235) and DLC will undoubtedly overflow above 255.
            foreach (var slot in area.Slots)
                slot.Write(bw);

            result[i++] = ms.ToArray();
        }

        var mini = BinLinkerWriter.Write16(result, "za"u8);
        File.WriteAllBytes(path, mini);
    }

    public static void AddSlots(List<EncounterSlot9a> list, EncountData slot, EncountDataInfo enc)
    {
        var species = SpeciesConverterZA.GetNational9((ushort)slot.DevNo);
        var form = (byte)slot.FormNo;
        const int boost = 0; // enc.AdditionalLevel; // field is unused; assumed to be similar to Kitakami post-game level boost, but there is none applicable in Z-A.
        var minLevel = (byte)(slot.MinLevel + boost);
        var maxLevel = (byte)(slot.MaxLevel + boost);
        var gender = (sbyte)slot.Sex;
        var nature = (sbyte)slot.Seikaku;
        var shiny = GetShiny(slot.Rare);

        var template = new EncounterSlot9a(species, form, minLevel, maxLevel, gender, nature, false, shiny);
        if (slot.OyabunProbability < 100)
            list.Add(template);
        if (slot.OyabunProbability > 0)
            list.Add(template.WithAlpha(slot.OyabunAdditionalLevel));
    }

    private static Shiny GetShiny(int valueRare) => valueRare switch
    {
        0x1FFFFFFF => Shiny.Never,
        0x2FFFFFFF => Shiny.Always,
        0x3FFFFFFF => Shiny.Random,
        _ => throw new ArgumentOutOfRangeException(nameof(valueRare), valueRare, null),
    };
}
