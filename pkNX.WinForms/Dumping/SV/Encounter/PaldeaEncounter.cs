using System;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public record PaldeaEncounter(ushort Species, byte Form, byte Sex, byte MinLevel, byte MaxLevel, byte Time, ushort CrossFromLocation = 0) : IComparable<PaldeaEncounter>
{
    public byte MinLevel { get; private set; } = MinLevel;
    public byte MaxLevel { get; private set; } = MaxLevel;
    public AreaWeather9 Weather { get; set; }

    public int Gender => Sex switch
    {
        1 => 0,
        2 => 1,
        _ => -1,
    };

    // Match the Mark time of day order: Noon, Night, Evening, Morning
    private static byte GetTimeBits(TimeTable t) => (byte)((t.Noon ? 0 : 1) | (t.Night ? 0 : 2) | (t.Evening ? 0 : 4) | (t.Morning ? 0 : 8));

    public static PaldeaEncounter GetNew(EncountPokeData pd, PointData ep, int adjust = 0)
    {
        // Combine the 4 bools into a single byte
        var time = GetTimeBits(pd.Time);
        var min = (byte)(Math.Max(ep.LevelRange.X, pd.MinLevel) + adjust);
        var max = (byte)(Math.Min(ep.LevelRange.Y, pd.MaxLevel) + adjust);
        if (max > 100)
            max = 100;
        return new(SpeciesConverterSV.GetNational9((ushort)pd.DevId), (byte)pd.Form, (byte)pd.Sex, min, max, time);
    }

    public static PaldeaEncounter GetBand(EncountPokeData pd, PointData ep, int adjust = 0)
    {
        // Combine the 4 bools into a single byte
        var time = GetTimeBits(pd.Time);
        var min = (byte)(Math.Max(ep.LevelRange.X, pd.MinLevel) + adjust);
        var max = (byte)(Math.Min(ep.LevelRange.Y, pd.MaxLevel) + adjust);
        return new(SpeciesConverterSV.GetNational9((ushort)pd.BandPoke), (byte)pd.BandForm, (byte)pd.BandSex, min, max, time);
    }

    public string GetEncountString(ReadOnlySpan<string> specNamesInternal)
    {
        var species = specNamesInternal[SpeciesConverterSV.GetInternal9(Species)];
        return GetString(species);
    }

    public override string ToString()
    {
        return GetString(((PKHeX.Core.Species)Species).ToString());
    }

    private string GetString(string species)
    {
        var form = Form == 0 ? "" : $"-{Form}";
        var sex = Sex == 0 ? "" : $" (sex={Sex})";
        return $"{species}{form}{sex} Lv. {MinLevel}-{MaxLevel}";
    }

    public bool Absorb(PaldeaEncounter other)
    {
        if (Time != other.Time)
            return false;
        if (CrossFromLocation != other.CrossFromLocation)
            return false;
        if (Weather != other.Weather)
            return false;
        if (other.MinLevel == MinLevel && other.MaxLevel == MaxLevel)
            return true;

        if (!IsLevelRangeOverlap(other) && !other.IsLevelRangeOverlap(this))
            return false;

        MinLevel = Math.Min(MinLevel, other.MinLevel);
        MaxLevel = Math.Max(MaxLevel, other.MaxLevel);
        return true;
    }

    public bool IsSameSpecFormGender(PaldeaEncounter other)
    {
        if (Species != other.Species || Form != other.Form)
            return false;
        if (Sex != other.Sex)
            return false;
        return true;
    }

    private bool IsLevelRangeOverlap(PaldeaEncounter other)
    {
        // If our level range overlaps with the other (with +/- 1 tolerance), return true.
        return MaxLevel + 1 >= other.MinLevel && MinLevel + 1 <= other.MaxLevel;
    }

    public int CompareTo(PaldeaEncounter? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        int speciesComparison = Species.CompareTo(other.Species);
        if (speciesComparison != 0) return speciesComparison;
        int formComparison = Form.CompareTo(other.Form);
        if (formComparison != 0) return formComparison;
        int sexComparison = Sex.CompareTo(other.Sex);
        if (sexComparison != 0) return sexComparison;
        int minLevelComparison = MinLevel.CompareTo(other.MinLevel);
        if (minLevelComparison != 0) return minLevelComparison;
        int maxLevelComparison = MaxLevel.CompareTo(other.MaxLevel);
        if (maxLevelComparison != 0) return maxLevelComparison;
        int weatherComparison = Weather.CompareTo(other.Weather);
        if (weatherComparison != 0) return weatherComparison;
        int timeComparison = Time.CompareTo(other.Time);
        if (timeComparison != 0) return timeComparison;
        return CrossFromLocation.CompareTo(other.CrossFromLocation);
    }

    public ulong GetHash()
    {
        ulong result = Species;
        result = (result << 16) | CrossFromLocation;

        result = (result << 8) | Form;
        result = (result << 8) | (byte)((byte)(Time << 4) | Sex);
        result = (result << 8) | MinLevel;
        result = (result << 8) | MaxLevel;
        result = (result << 8) | (byte)Weather;
        return result;
    }
}
