using System.Diagnostics;

namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class EncounterMultiplier : ISlotModifierTime, ISlotModifierWeather
{
    public float GetTimeMultiplier(int index) => index switch
    {
        0 => TimeOfDayMultiplier0,
        1 => TimeOfDayMultiplier1,
        2 => TimeOfDayMultiplier2,
        3 => TimeOfDayMultiplier3,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public float GetWeatherMultiplier(int index) => index switch
    {
        0 => 1.0f, // "No Weather" results in no modification of rate for all species/forms
        1 => WeatherMultiplier1,
        2 => WeatherMultiplier2,
        3 => WeatherMultiplier3,
        4 => WeatherMultiplier4,
        5 => WeatherMultiplier5,
        6 => WeatherMultiplier6,
        7 => WeatherMultiplier7,
        8 => WeatherMultiplier8,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public bool HasTimeModifier(int index) => GetTimeMultiplier(index) != -1.0f;
    public bool HasWeatherModifier(int index) => index != 0 && GetWeatherMultiplier(index) != -1.0f;
}

public partial class EncounterMultiplierArchive
{
    public EncounterMultiplier GetEncounterMultiplier(EncounterSlot slot)
    {
        return GetEntry((ushort)slot.Species, (ushort)slot.Form) ?? throw new ArgumentException($"Invalid Encounter Slot {slot.Species} - {slot.Form}");
    }

    public EncounterMultiplier GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(x => x.Species == species && x.Form == form) ??
               new EncounterMultiplier();
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public EncounterMultiplier AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The encounter rate table already contains an entry for the same species and form!");

        var entry = new EncounterMultiplier { Species = species, Form = form };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
