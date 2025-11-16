using System;

namespace pkNX.Structures;

/// <summary>
/// Misc information pertaining to the game.
/// </summary>
public class GameInfo
{
    public readonly GameVersion Game;
    public readonly int Generation;

    public int MaxSpeciesID { get; private set; }
    public int MaxItemID { get; private set; }
    public int MaxMoveID { get; private set; }
    public ushort[] HeldItems { get; private set; } = [];
    public int MaxAbilityID { get; private set; }

    public bool GG { get; private set; }
    public bool SWSH { get; private set; }
    public bool PLA { get; private set; }
    public bool SV { get; private set; }
    public bool ZA { get; private set; }

    public GameInfo(GameVersion game)
    {
        Game = game;
        Generation = game.GetGeneration();
        GetInitMethod(game)();
    }

    private Action GetInitMethod(GameVersion game) => game switch
    {
        GameVersion.GP => LoadGG,
        GameVersion.GE => LoadGG,
        GameVersion.GG => LoadGG,
        GameVersion.SW => LoadSWSH,
        GameVersion.SH => LoadSWSH,
        GameVersion.SWSH => LoadSWSH,
        GameVersion.PLA => LoadPLA,
        GameVersion.SV => LoadSV,
        GameVersion.ZA => LoadZA,
        _ => throw new ArgumentOutOfRangeException(nameof(game), game, null),
    };

    private void LoadGG()
    {
        GG = true;
        MaxSpeciesID = Legal.MaxSpeciesID_7_GG;
        MaxMoveID = Legal.MaxMoveID_7_GG;
        MaxItemID = Legal.MaxItemID_7_GG;
        HeldItems = new ushort[1];
        MaxAbilityID = Legal.MaxAbilityID_7_GG;
    }

    private void LoadSWSH()
    {
        SWSH = true;
        MaxSpeciesID = Legal.MaxSpeciesID_8;
        MaxMoveID = Legal.MaxMoveID_8;
        MaxItemID = Legal.MaxItemID_8;
        HeldItems = Legal.HeldItems_SWSH;
        MaxAbilityID = Legal.MaxAbilityID_8;
    }

    private void LoadPLA()
    {
        PLA = true;
        MaxSpeciesID = Legal.MaxSpeciesID_8a;
        MaxMoveID = Legal.MaxMoveID_8a;
        MaxItemID = Legal.MaxItemID_8a;
        HeldItems = Legal.HeldItems_SWSH;
        MaxAbilityID = Legal.MaxAbilityID_8a;
    }

    private void LoadSV()
    {
        SV = true;
        MaxSpeciesID = Legal.MaxSpeciesID_9;
        MaxMoveID = Legal.MaxMoveID_9;
        MaxItemID = Legal.MaxItemID_9;
        HeldItems = Legal.HeldItems_SV;
        MaxAbilityID = Legal.MaxAbilityID_9;
    }

    private void LoadZA()
    {
        ZA = true;
        MaxSpeciesID = Legal.MaxSpeciesID_9a;
        MaxMoveID = Legal.MaxMoveID_9a;
        MaxItemID = Legal.MaxItemID_9a;
        HeldItems = Legal.HeldItems_SV;
        MaxAbilityID = Legal.MaxAbilityID_9a;
    }
}
