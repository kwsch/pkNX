namespace pkNX.Structures
{
    public enum EvolutionType : byte
    {
        None,
        LevelUpFriendship,
        LevelUpFriendshipMorning,
        LevelUpFriendshipNight,
        LevelUp,
        Trade,
        TradeHeldItem,
        TradeSpecies,
        UseItem,
        LevelUpATK,
        LevelUpDEF,
        LevelUpAeqD,
        LevelUpECl5,
        LevelUpECgeq5,
        LevelUpNinjask,
        LevelUpShedinja,
        LevelUpBeauty,
        UseItemMale,
        UseItemFemale,
        LevelUpHeldItemDay,
        LevelUpHeldItemNight,
        LevelUpKnowMove,
        LevelUpWithTeammate,
        LevelUpMale,
        LevelUpFemale,
        LevelUpElectric,
        LevelUpForest,
        LevelUpCold,
        LevelUpInverted,
        LevelUpAffection50MoveType,
        LevelUpMoveType,
        LevelUpWeather,
        LevelUpMorning,
        LevelUpNight,
        LevelUpFormFemale1,
        UNUSED,
        LevelUpVersion,
        LevelUpVersionDay,
        LevelUpVersionNight,
        LevelUpSummit,
        LevelUpDusk,
        LevelUpWormhole,
        UseItemWormhole,

        U43, // Farfetch'd (1) with val 3? -> Sirfetch'd
        U44, // Yamask (1) -> Runerigus (val 49) 
        U45, // Milcery->Alcremie (Console Region==Form?) 
        U46, // Toxel->Toxtricity (0)
        U47, // Toxel->Toxtricity (1)
    }

    public enum EvolutionTypeArgumentType
    {
        NoArg,
        Level,
        Items,
        Moves,
        Species,
        Stat,
        Type,
        Version,
    }
}
