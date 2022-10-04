# pkNX

![License](https://img.shields.io/badge/License-GPLv3-blue.svg)

pkNX: A package of Pokémon (Nintendo Switch) ROM Editing Tools, programmed in [C#](https://en.wikipedia.org/wiki/C_Sharp_%28programming_language%29).

Similar to [pk3DS](https://github.com/kwsch/pk3ds) for the Nintendo 3DS, pkNX provides an editing environment to manipulate game binary assets such as stats, learnsets, trainers, and more!

![Main Window](https://i.imgur.com/lSYWN4m.png)

## Download
Download the latest version [here](https://dev.azure.com/project-pokemon/pkNX/_build?view=runs).

(click on latest run at the top, then click Artifacts - published, and download the folder)
![image](https://user-images.githubusercontent.com/60387522/193828925-2c5f3142-f8cb-4daa-af49-9663919ec9bf.png)
![image](https://user-images.githubusercontent.com/60387522/193829124-02db4dfa-2e61-421c-a5fd-129552df8aca.png)

## Features
Supports the following games:
* Let's Go, Pikachu! / Let's Go, Eevee!
* Sword / Shield

**For Sword and Shield, pkNX operates under the assumption that your dumped ROM includes the Ver. 1.3.0 patch.**

Editors can be launched from the program's main window after opening a dumped & unpacked ROM. 
* To lessen read/write lag, data is only saved when the user cleanly quits the program. 
* Edited files do not overwrite the original dumped file; instead, they are redirected to a "patch folder" for easy use with layeredFS. 
* When the program requests to read a set of files, it will first check to see if an edited version exists, and if not, falls back to the original dump file.

With custom firmware, layeredFS functionality will selectively redirect file loading to files that are present in the patch folder, removing the need to rebuild a custom ROM.

pkNX also provides some utility to extract from supported container types, e.g. `gfpak`. Simply drag & drop a container (or many) into the main window, and pkNX will unpack all files to a new folder.

## Building

pkNX is a Windows Forms application which requires [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for Windows.

The executable can be built with any compiler that supports C# 10.

## Dependencies

pkNX's shiny sprite collection is taken from [pokesprite](https://github.com/msikma/pokesprite), which is licensed under [the MIT license](https://github.com/msikma/pokesprite/blob/master/LICENSE).
