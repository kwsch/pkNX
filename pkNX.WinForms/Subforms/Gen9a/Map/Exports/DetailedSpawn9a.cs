using pkNX.Structures.FlatBuffers.ZA;

namespace pkNX.WinForms;

public sealed record DetailedSpawn9a
{
    public required string Id { get; init; }
    public required ushort Species { get; init; }
    public required byte Form { get; init; }
    public required int Gender { get; init; }
    public required int Nature { get; init; }

    public required float AlphaProbability { get; init; }

    public required byte LevelMin { get; init; }
    public required byte LevelMax { get; init; }
    public required int LevelBoostFromArea { get; init; }
    public required byte LevelBoostIfAlpha { get; init; }

    public required string Tags { get; init; }
    public required string Condition { get; init; }

    public required int RepopProbabilityAfterCapture { get; init; }
    public required int RepopProbabilityAfterDefeat { get; init; }
    public required int ConditionTime { get; init; }
    public required int ConditionWeather { get; init; }
    public required int RandomWeight { get; init; }
    public required int TotalWeight { get; init; }
    public required int Quantity { get; init; }

    public static DetailedSpawn9a Create(EncountDataInfo enc, EncountData slot, int totalWeight) => new()
    {
        Id = slot.Id,
        Species = SpeciesConverterZA.GetNational9((ushort)slot.DevNo),
        Form = (byte)slot.FormNo,
        LevelMin = (byte)slot.MinLevel,
        LevelMax = (byte)slot.MaxLevel,
        AlphaProbability = slot.OyabunProbability,
        LevelBoostIfAlpha = (byte)slot.OyabunAdditionalLevel,
        Gender = slot.Sex,
        Nature = slot.Seikaku,
        Condition = slot.ActivationConditionArray.GetSummary(),

        LevelBoostFromArea = enc.AdditionalLevel, // unused, future DLC level boost like Kitakami
        Tags = enc.TagList is { } tl ? string.Join("&&", tl) : "",

        RepopProbabilityAfterCapture = enc.Repop?.AfterCaptured ?? -1,
        RepopProbabilityAfterDefeat = enc.Repop?.AfterDefeated ?? -1,

        ConditionTime = enc.AppearedTimeCondition,
        ConditionWeather = enc.AppearedWeatherCondition,

        RandomWeight = enc.Weight,
        TotalWeight = totalWeight,
        Quantity = enc.MaxCount,
    };
}
