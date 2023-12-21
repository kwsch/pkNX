using pkNX.Containers.VFS;
using pkNX.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Game;

public static class GamePath
{
    private static GameVersion CurrentGame { get; set; }
    private static int CurrentLanguage { get; set; }

    public static void Initialize(GameVersion game, int language)
    {
        CurrentGame = game;
        CurrentLanguage = language;
    }

    public static FileSystemPath GetDirectoryPath(GameFile file)
    {
        return GetDirectoryPath(file, CurrentGame, CurrentLanguage);
    }

    public static FileSystemPath GetDirectoryPath(GameFile file, int language)
    {
        return GetDirectoryPath(file, CurrentGame, language);
    }

    public static FileSystemPath GetDirectoryPath(GameFile file, GameVersion game, int language)
    {
        if (file is GameFile.GameText or GameFile.StoryText)
            file += language + 1; // shift to localized language enum

        return game switch
        {
            GameVersion.GG => GetDirectoryPath_GG(file),
            GameVersion.SWSH => GetDirectoryPath_SWSH(file),
            GameVersion.PLA => GetDirectoryPath_PLA(file),
            GameVersion.SV => GetDirectoryPath_SV(file),

            _ => throw new NotSupportedException($"The selected game ({game}) is currently not mapped"),
        };
    }

    private static FileSystemPath GetDirectoryPath_GG(GameFile file)
    {
        throw new NotImplementedException();
    }

    private static FileSystemPath GetDirectoryPath_SWSH(GameFile file)
    {
        throw new NotImplementedException();
    }

    // Search replace regex for conversion from GameFileMapping.cs:
    // Search for   : new\((\w+), (?:[^",]+, )?
    // Replace with : GameFile.$1 =>

    // Then to replace all ", " between each folder repeat the following:
    // Search for   : (?<=GameFile.\w+ => "[^"]+)", "
    // Replace with : /
    private static FileSystemPath GetDirectoryPath_PLA(GameFile file) => file switch
    {
        GameFile.GameText => throw new ArgumentException("Please shift language enum before calling this method."),
        GameFile.GameText0 => "/romfs/bin/message/JPN/common/",
        GameFile.GameText1 => "/romfs/bin/message/JPN_KANJI/common/",
        GameFile.GameText2 => "/romfs/bin/message/English/common/",
        GameFile.GameText3 => "/romfs/bin/message/French/common/",
        GameFile.GameText4 => "/romfs/bin/message/Italian/common/",
        GameFile.GameText5 => "/romfs/bin/message/German/common/",
        GameFile.GameText6 => "/romfs/bin/message/Spanish/common/",
        GameFile.GameText7 => "/romfs/bin/message/Korean/common/",
        GameFile.GameText8 => "/romfs/bin/message/Simp_Chinese/common/",
        GameFile.GameText9 => "/romfs/bin/message/Trad_Chinese/common/",

        GameFile.StoryText => throw new ArgumentException("Please shift language enum before calling this method."),
        GameFile.StoryText0 => "/romfs/bin/message/JPN/script/",
        GameFile.StoryText1 => "/romfs/bin/message/JPN_KANJI/script/",
        GameFile.StoryText2 => "/romfs/bin/message/English/script/",
        GameFile.StoryText3 => "/romfs/bin/message/French/script/",
        GameFile.StoryText4 => "/romfs/bin/message/Italian/script/",
        GameFile.StoryText5 => "/romfs/bin/message/German/script/",
        GameFile.StoryText6 => "/romfs/bin/message/Spanish/script/",
        GameFile.StoryText7 => "/romfs/bin/message/Korean/script/",
        GameFile.StoryText8 => "/romfs/bin/message/Simp_Chinese/script/",
        GameFile.StoryText9 => "/romfs/bin/message/Trad_Chinese/script/",

        _ => throw new NotSupportedException($"The selected file ({file}) is currently not mapped"),
    };

    private static FileSystemPath GetDirectoryPath_SV(GameFile file)
    {
        throw new NotImplementedException();
    }
}
