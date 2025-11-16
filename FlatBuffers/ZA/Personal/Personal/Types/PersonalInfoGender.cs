namespace pkNX.Structures.FlatBuffers.ZA;

public partial class PersonalInfoGender
{
    public const int RatioMagicMale = 0;
    public const int RatioMagicFemale = 254;
    public const int RatioMagicGenderless = 255;
    public byte RatioMagicEquivalent() => Group switch
    {
        0 => Ratio switch
        {
            12 => 0x1F, // 12.5%
            25 => 0x3F, // 25%
            50 => 0x7F, // 50%
            75 => 0xBF, // 75%
            89 => 0xE1, // 87.5%
            _ => throw new ArgumentOutOfRangeException(nameof(Ratio)),
        },
        SexGroup.MALE => RatioMagicMale,
        SexGroup.FEMALE => RatioMagicFemale,
        SexGroup.UNKNOWN => RatioMagicGenderless,
        _ => throw new ArgumentOutOfRangeException(nameof(Group)),
    };
}
