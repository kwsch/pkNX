namespace pkNX.Structures;
#pragma warning disable CA1027 // Mark enums with FlagsAttribute
public enum FixedGender : byte
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
{
    Random = 0,
    Male = 1,
    Female = 2,

    Genderless = Random,
}
