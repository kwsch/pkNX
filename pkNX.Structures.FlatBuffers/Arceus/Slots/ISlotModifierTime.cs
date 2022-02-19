using System;

namespace pkNX.Structures.FlatBuffers;

public interface ISlotModifierTime
{
    float TimeOfDayMultiplier_0 { get; set; }
    float TimeOfDayMultiplier_1 { get; set; }
    float TimeOfDayMultiplier_2 { get; set; }
    float TimeOfDayMultiplier_3 { get; set; }
}

public static class SlotModifierTimeExtensions
{
    public static float GetTimeMultiplier(this ISlotModifierTime x, int index) => index switch
    {
        0 => x.TimeOfDayMultiplier_0,
        1 => x.TimeOfDayMultiplier_1,
        2 => x.TimeOfDayMultiplier_2,
        3 => x.TimeOfDayMultiplier_3,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public static bool HasTimeModifier(this ISlotModifierTime t, int index) => t.GetTimeMultiplier(index) != -1.0f;
}
