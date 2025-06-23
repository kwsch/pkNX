using System;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

internal static class EncounterExtensions9
{
    public static string Humanize(this SizeType type, short value) => type switch
    {
        SizeType.RANDOM => "Random",
        SizeType.XS => "XS",
        SizeType.S => "S",
        SizeType.M => "M",
        SizeType.L => "L",
        SizeType.XL => "XL",
        SizeType.VALUE => value.ToString(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string Humanize(this RareType type) => type switch
    {
        RareType.DEFAULT => "Random",
        RareType.NO_RARE => "Never",
        RareType.RARE => "Always",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string Humanize(this SexType type) => type switch
    {
        SexType.DEFAULT => "Random",
        SexType.MALE => "Male",
        SexType.FEMALE => "Female",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string Humanize(this TokuseiType type) => type switch
    {
        TokuseiType.RANDOM_12 => "1/2",
        TokuseiType.RANDOM_123 => "1/2/H",
        TokuseiType.SET_1 => "1",
        TokuseiType.SET_2 => "2",
        TokuseiType.SET_3 => "H",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid ability index"),
    };

    public static bool IsAvoidantAction(this PokemonActionID action) => action switch
    {
        PokemonActionID.FS_POP_PATH_RUN_SPEED_UP_1_NOT_COL_DELAY => true, // lighthouse Wingull (South) and Wattrel (East, West)
        PokemonActionID.FS_POP_PATH_RUN_SPEED_UP_1_NOT_COL_DESTROY => true, // fence Wingull (Cabo Poco)
        PokemonActionID.FS_POP_COMMON_FLIGHT_NOT_COL_DESTROY => true, // starting Fletchling (level 2)
        _ => false,
    };

    public static bool IsStationary(this PokemonActionID action) => action switch
    {
        PokemonActionID.FS_POP_ALWAYS_GAZE_BIRD_TARGET_PLAYER => true,
        PokemonActionID.FS_POP_ALWAYS_GAZE_TARGET_PLAYER => true,
        PokemonActionID.ALWAYS_GAZE_BIRD_TARGET_PLAYER_LOOP => true,
        PokemonActionID.FS_POP_LAND_SLEEPING_CURRENT_LOCATION => true,
        PokemonActionID.FS_POP_LEVITATION_SLEEPING_TREE_BRANCH => true,
        PokemonActionID.FS_POP_AREA22_DRAGONITE => true, // Handle separately.
        _ => false,
    };

}
