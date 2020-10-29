using System.Collections.Generic;

using static pkNX.Structures.EvolutionType;
using static pkNX.Structures.EvolutionTypeArgumentType;

namespace pkNX.Structures
{
    public static partial class Extensions
    {
        private static readonly Dictionary<EvolutionType, EvolutionTypeArgumentType> ArgType = new Dictionary<EvolutionType, EvolutionTypeArgumentType>
        {
            [None] = NoArg,
            [LevelUpFriendship] = NoArg,
            [LevelUpFriendshipMorning] = NoArg,
            [LevelUpFriendshipNight] = NoArg,
            [LevelUp] = Level,
            [Trade] = NoArg,
            [TradeHeldItem] = Items,
            [TradeSpecies] = EvolutionTypeArgumentType.Species,
            [UseItem] = Items,

            [LevelUpATK] = Level,
            [LevelUpDEF] = Level,
            [LevelUpAeqD] = Level,
            [LevelUpECl5] = Level,
            [LevelUpECgeq5] = Level,
            [LevelUpNinjask] = Level,
            [LevelUpShedinja] = Level,
            [LevelUpBeauty] = Stat,

            [UseItemMale] = Items,
            [UseItemFemale] = Items,
            [LevelUpHeldItemDay] = Items,
            [LevelUpHeldItemNight] = Items,
            [LevelUpKnowMove] = Moves,
            [LevelUpWithTeammate] = EvolutionTypeArgumentType.Species,
            [LevelUpMale] = Level,
            [LevelUpFemale] = Level,
            [LevelUpElectric] = NoArg,
            [LevelUpForest] = NoArg,
            [LevelUpCold] = NoArg,
            [LevelUpInverted] = NoArg,
            [LevelUpAffection50MoveType] = Type,

            [LevelUpDarkTeammate] = NoArg,
            [LevelUpWeather] = Level,
            [LevelUpMorning] = Level,
            [LevelUpNight] = Level,
            [LevelUpFormFemale1] = Level,
            [UNUSED] = NoArg,
            [LevelUpVersion] = Version,
            [LevelUpVersionDay] = Version,
            [LevelUpVersionNight] = Version,
            [LevelUpSummit] = Level,
            [LevelUpDusk] = Level,
            [LevelUpWormhole] = Level,
            [UseItemWormhole] = Items,

            [CriticalHitsInBattle] = Version,
            [HitPointsLostInBattle] = Version,
            [Spin] = NoArg,
            [LevelUpNatureAmped] = NoArg,
            [LevelUpNatureLowKey] = NoArg,
            [TowerOfDarkness] = NoArg,
            [TowerOfWaters] = NoArg,
        };

        public static EvolutionTypeArgumentType GetArgType(this EvolutionType t) => ArgType[t];
    }
}