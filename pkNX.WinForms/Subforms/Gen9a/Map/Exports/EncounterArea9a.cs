using System.Collections.Generic;

namespace pkNX.WinForms;

public sealed record EncounterArea9a
{
    public required List<EncounterSlot9a> Slots { get; init; }
    public required int Location { get; init; }

    public void CondenseList()
    {
        // If level range overlaps, and Species-Form-Gender-Alpha merge the overlap
        // Level (and Alpha) check is before the crypto-secure 64-bit seed generates, so merging level ranges is OK to deduplicate some templates.
        Slots.Sort((a, b) => a.Species.CompareTo(b.Species));
        for (int i = 0; i < Slots.Count - 1; i++)
        {
            var a = Slots[i];
            for (int j = i + 1; j < Slots.Count; j++)
            {
                var b = Slots[j];
                if (!a.IsSame(b))
                    continue;
                if (!a.LevelOverlap(b))
                    continue;
                Slots[i] = a = a.MergeWith(b);
                Slots.RemoveAt(j);
                j--;
            }
        }
    }
}
