using System;

namespace pkNX.Structures.FlatBuffers.Arceus;

public interface ISlotModifierTime
{
    float TimeOfDayMultiplier0 {get;set;}
    float TimeOfDayMultiplier1 {get;set;}
    float TimeOfDayMultiplier2 {get;set;}
    float TimeOfDayMultiplier3 {get;set;}
}

public static class SlotModifierTimeExtensions
{
    public static float GetTimeMultiplier(this ISlotModifierTime x, int index) => index switch
    {
        0 => x.TimeOfDayMultiplier0,
        1 => x.TimeOfDayMultiplier1,
        2 => x.TimeOfDayMultiplier2,
        3 => x.TimeOfDayMultiplier3,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public static bool HasTimeModifier(this ISlotModifierTime t, int index) => t.GetTimeMultiplier(index) != -1.0f;
}
