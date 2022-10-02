namespace pkNX.Structures;

/// <summary>
/// Indicates how much a heal item/move heals.
/// </summary>
/// <remarks>Any other non-enumerated value will be treated as a fixed value heal equal to the value.</remarks>
public enum Heal : byte
{
    None = 0,

    Quarter = 253,
    Half = 254,
    Full = 255,
}
