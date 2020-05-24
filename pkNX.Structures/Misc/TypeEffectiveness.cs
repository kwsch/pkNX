namespace pkNX.Structures
{
#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum TypeEffectiveness : byte
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        Immune = 0,
        NotVery = 2,
        Normal = 4,
        Super = 8,
    }
}
