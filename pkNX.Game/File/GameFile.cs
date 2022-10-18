namespace pkNX.Game;

/// <summary>
/// Simple descriptor related to what purpose the game data serves.
/// </summary>
public enum GameFile
{
    /// <summary> Contains the game text that is commonly re-used, not related to the storyline or general overworld content. </summary>
    GameText,

    /// <summary> Localized Game Text for <see cref="GameLanguage.カタカナ"/>. </summary>
    GameText0,

    /// <summary> Localized Game Text for <see cref="GameLanguage.汉字简化方案"/>. </summary>
    GameText1,

    /// <summary> Localized Game Text for <see cref="GameLanguage.English"/>. </summary>
    GameText2,

    /// <summary> Localized Game Text for <see cref="GameLanguage.Français"/>. </summary>
    GameText3,

    /// <summary> Localized Game Text for <see cref="GameLanguage.Italiano"/>. </summary>
    GameText4,

    /// <summary> Localized Game Text for <see cref="GameLanguage.Deutsch"/>. </summary>
    GameText5,

    /// <summary> Localized Game Text for <see cref="GameLanguage.Español"/>. </summary>
    GameText6,

    /// <summary> Localized Game Text for <see cref="GameLanguage.한국"/>. </summary>
    GameText7,

    /// <summary> Localized Game Text for <see cref="GameLanguage.汉字简化方案"/>. </summary>
    GameText8,

    /// <summary> Localized Game Text for <see cref="GameLanguage.漢字簡化方案"/>. </summary>
    GameText9,

    /// <summary> Contains the story text that is used to tell the story via overworld events and interactions. </summary>
    StoryText,

    /// <summary> Localized Story Text for <see cref="GameLanguage.カタカナ"/>. </summary>
    StoryText0,

    /// <summary> Localized Story Text for <see cref="GameLanguage.汉字简化方案"/>. </summary>
    StoryText1,

    /// <summary> Localized Story Text for <see cref="GameLanguage.English"/>. </summary>
    StoryText2,

    /// <summary> Localized Story Text for <see cref="GameLanguage.Français"/>. </summary>
    StoryText3,

    /// <summary> Localized Story Text for <see cref="GameLanguage.Italiano"/>. </summary>
    StoryText4,

    /// <summary> Localized Story Text for <see cref="GameLanguage.Deutsch"/>. </summary>
    StoryText5,

    /// <summary> Localized Story Text for <see cref="GameLanguage.Español"/>. </summary>
    StoryText6,

    /// <summary> Localized Story Text for <see cref="GameLanguage.한국"/>. </summary>
    StoryText7,

    /// <summary> Localized Story Text for <see cref="GameLanguage.汉字简化方案"/>. </summary>
    StoryText8,

    /// <summary> Localized Story Text for <see cref="GameLanguage.漢字簡化方案"/>. </summary>
    StoryText9,

    /// <summary> Overworld grass/etc encounterable species data. </summary>
    Encounters,

    /// <summary> Trainer Data related to Trainers of a shared type. </summary>
    TrainerClass,

    /// <summary> Trainer Data for individual Trainers that can be battled. </summary>
    TrainerData,

    /// <summary> Trainer PKM template data for regular battles. </summary>
    TrainerPoke,

    /// <summary> Move data that defines the properties of in-game moves. </summary>
    MoveStats,

    /// <summary> Egg Moves a species can learn when bred. </summary>
    EggMoves,

    /// <summary> Moves a species can learn via level up. </summary>
    Learnsets,

    /// <summary> Evolutions a species can have under specified conditions. </summary>
    Evolutions,

    /// <summary> Mega Evolutions a species can have under specified conditions. </summary>
    MegaEvolutions,

    /// <summary> In-game stats a species can have. </summary>
    PersonalStats,

    /// <summary> Properties of in-game posessible items. </summary>
    ItemStats,

    /// <summary> Static (fixed position/condition) encounter table. </summary>
    EncounterStatic,

    /// <summary> Post-game roulette trainer data with normal difficulty. </summary>
    FacilityTrainerNormal,

    /// <summary> Post-game roulette trainer data with heightened difficulty. </summary>
    FacilityTrainerSuper,

    /// <summary> Post-game roulette PKM template data for normal difficulty trainers. </summary>
    FacilityPokeNormal,

    /// <summary> Post-game roulette PKM template data for heightened difficulty trainers. </summary>
    FacilityPokeSuper,

    /// <summary> Title Screen staging data. </summary>
    TitleScreen,

    /// <summary> Box Interface wallpapers. </summary>
    Wallpaper,

    /// <summary> Walk/Collision data for individual Maps. </summary>
    MapMatrix,

    /// <summary> Zone assembling information to build large maps from individual small zones. </summary>
    MapGameRegion,

    /// <summary> Area settings and permissives related to in-game areas the player travels to. </summary>
    ZoneData,

    /// <summary> Post-battle items that can be picked up. </summary>
    BattleDrops,

    /// <summary> Items that can be dropped by pokemon in the field. </summary>
    FieldDrops,

    /// <summary> Map data for individual zones. </summary>
    WorldData,

    /// <summary> UI Sprites for pretty in-game move descriptors. </summary>
    MoveSprites,

    /// <summary> Traded Pokémon swap data. </summary>
    EncounterTrade,

    /// <summary> Gift Pokémon data. </summary>
    EncounterGift,

    /// <summary> Nest Data </summary>
    NestData,

    /// <summary> Wild Data </summary>
    WildData,

    /// <summary> Wild Data </summary>
    WildData1,

    /// <summary> Wild Data </summary>
    WildData2,

    /// <summary> Dynamax Adventure Dens </summary>
    DynamaxDens,

    /// <summary> Area Placement Archive </summary>
    Placement,

    /// <summary> Shop Inventory Lists </summary>
    Shops,

    /// <summary> Rental Team Pokémon </summary>
    Rentals,

    /// <summary> Symbol Behavior Definition </summary>
    SymbolBehave,

    ArchiveFolder,

    /// <summary> Area Resident Archive </summary>
    Resident,
    archive_contents,

    /// <summary> "PokeEncount" Rate Multipler Archive </summary>
    EncounterRateTable,

    /// <summary> huge_outbreak.bin </summary>
    Outbreak,

    /// <summary> wazashop_table.bin </summary>
    MoveShop,

    /// <summary> "PokeMisc" Details about a given Species-Form not stored in <see cref="PersonalStats"/> </summary>
    PokeMisc,

    /// <summary> All pokedex research tasks </summary>
    DexResearch,
    DexFormStorage,
    DexRank,

    ThrowParam,
    ThrowPermissionSet,
    ThrowableParam,
    ThrowableResource,
    ThrowableResourceSet,

    WormholeConfig,
    FieldLandmarkConfig,
    FieldSpawnerConfig,
    OutbreakConfig,
    BallThrowConfig,

    HaShop,
    NewHugeGroup,
    NewHugeGroupLottery,
    NewHugeLottery,
    NewHugeTimeLimit,

    AICommonConfig,
    AIExcitingConfig,
    AIFieldWazaConfig,
    AISemiLegendConfig,
    AITirednessConfig,

    AppConfigList,
    AppliHudConfig,
    AppliStaffrollConfig,
    AppliTipsConfig,

    BattleCommonConfig,
    BattleEndConfig,
    BattleInConfig,
    BattleLogicConfig,
    BattleStartConfig,
    BattleViewConfig,
    BattleVsnsConfig,

    BuddyBattleConfig,
    BuddyConfig,
    BuddyDirectItemConfig,
    BuddyGroupTalkConfig,
    BuddyLandmarkConfig,
    BuddyNPCReactionConfig,
    BuddyPlayerModeConfig,
    BuddyWarpConfig,

    CharacterBipedIkConfig,
    CharacterBlinkConfig,
    CharacterControllerConfig,
    CharacterLookAtConfig,

    CaptureConfig,
    CommonGeneralConfig,
    CommonItemConfig,

    DemoConfig,

    EnvPokeVoiceConfig,
    EventBalloonrunConfig,
    EventBalloonthrowConfig,
    EventBanditConfig,
    EventCullingConfig,
    EventDitherConfig,
    EventFarmConfig,
    EventGameOverConfig,
    EventItemConfig,
    EventMkrgRewardConfig,
    EventQuestBoardConfig,
    EventRestrictionBattle,
    EventWork,

    FieldAnimationFramerateConfig,
    FieldAreaSpeedConfig,
    FieldCameraConfig,
    FieldCaptureDirectorConfig,
    FieldCharaViewerConfig,
    FieldCommonConfig,
    FieldDirectItemConfig,
    FieldEnvConfig,
    FieldItem,
    FieldItemRespawn,
    FieldLandmarkInciteConfig,
    FieldLockonConfig,
    FieldBallMissedConfig,
    FieldObstructionWazaConfig,
    FieldPokemonSlopeConfig,
    FieldQuestDestinationConfig,
    FieldShadowConfig,
    FieldThrowConfig,
    FieldThrowableAfterHitConfig,
    FieldVigilanceBgmConfig,
    FieldWeatheringConfig,
    FieldWildPokemonConfig,

    NPCAIConfig,
    NPCControllerConfig,
    NPCCreaterConfig,
    NPCPokemonAIConfig,
    NPCPopupConfig,
    NPCTalkTableConfig,

    PlayerCameraShakeConfig,
    PlayerCollisionConfig,
    PlayerConfig,
    PlayerControllerConfig,
    PlayerFaceConfig,
    Player1DressupTable,
    Player2DressupTable,

    PokemonConfig,
    PokemonControllerConfig,
    EvolutionConfig,
    PokemonFriendshipConfig,
    /// <summary> Gives bonus rolls based on met thresholds. </summary>
    ShinyRolls,
    SizeScaleConfig,

    RideBasuraoCollisionConfig,
    RideBasuraoConfig,
    RideChangeConfig,
    RideCommonConfig,
    RideNyuuraCollisionConfig,
    RideNyuuraConfig,
    RideNyuuraControllerConfig,
    RideOdoshishiCollisionConfig,
    RideOdoshishiConfig,
    RideRingumaCollisionConfig,
    RideRingumaConfig,
    RideRingumaControllerConfig,
    RideWhooguruCollisionConfig,
    RideWhooguruConfig,
    RideWhooguruControllerConfig,

    SoundConfig,

    WaterMotion,

    PokemonResourceList,
    PokeBodyParticle,
    PokeCaptureCollision,
    PokemonResourceTable,
    PokeBattleSpawn,
    PokeDefaultLocator,
    PokeEatingHabits,
    MoveObstructionLegend,
    MoveObstructionLegendEffect,
    MoveObstructionSE,
    MoveObstructionWild,
    MoveObstructionWildEffect,
    MoveObstructionWater,
    MoveObstructionWaterEffect,
    NushiBattleSettings,
}
