using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfo9SVGender
{
    [FlatBufferItem(0)] public byte Group { get; set; } // (0 either, 1 male, 2 female, 3 genderless)
    [FlatBufferItem(1)] public byte Ratio { get; set; } // {rand(100) < value} => gender.

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
        1 => RatioMagicMale,
        2 => RatioMagicFemale,
        3 => RatioMagicGenderless,
        _ => throw new ArgumentOutOfRangeException(nameof(Group)),
    };
}
