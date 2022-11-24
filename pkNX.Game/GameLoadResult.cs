using System;

namespace pkNX.Game;

public enum GameLoadResult
{
    Success,
    ExefsNotFound,
    DirectoryNotFound,
    RomfsNotFound,
    RomfsSelected,
    InvalidGameVersion,
    UnsupportedGame,

    Failed = -1,
}

public static class GameLoadResultExt
{
    public static bool IsSuccess(this GameLoadResult result) => result < GameLoadResult.ExefsNotFound;

    public static bool IsFailure(this GameLoadResult result) => !result.IsSuccess();

    public static string GetErrorMsg(this GameLoadResult result) => result switch
    {
        GameLoadResult.Success => "Loaded successfully.",
        GameLoadResult.ExefsNotFound => "Could not detect exefs folder in the selected directory.",
        GameLoadResult.DirectoryNotFound => "The selected directory does not exist!",
        GameLoadResult.RomfsNotFound => "Could not detect romfs folder in the selected directory. Please select the folder that contains the romfs folder.",
        GameLoadResult.RomfsSelected => "You have selected the romfs folder, however pkNX needs to create a separate folder to keep a clean copy of the original romfs.",
        GameLoadResult.InvalidGameVersion => "Couldn't infer game version from romfs file count. Your game dump might have been corrupted.",
        GameLoadResult.UnsupportedGame => "This game is (currently) not supported by pkNX!",
        GameLoadResult.Failed => "Internal error occurred.",
        _ => throw new NotImplementedException(),
    };
}
