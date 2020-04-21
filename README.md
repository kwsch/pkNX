# pkNX

![License](https://img.shields.io/badge/License-GPLv3-blue.svg)

pkNX: A package of Pokémon (Nintendo Switch) ROM Editing Tools, programmed in [C#](https://en.wikipedia.org/wiki/C_Sharp_%28programming_language%29).

Similar to [pk3DS](https://github.com/kwsch/pk3ds) for the Nintendo 3DS, pkNX provides an editing environment to manipulate game binary assets such as stats, learnsets, trainers, and more!

![Main Window](https://i.imgur.com/lSYWN4m.png)

## Features
Supports the following games:
* Let's Go, Pikachu! / Let's Go, Eevee!
* Sword / Shield

Editors can be launched from the program's main window after opening a dumped & unpacked ROM. 
* To lessen read/write lag, data is only saved when the user cleanly quits the program. 
* Edited files do not overwrite the original dumped file; instead, they are redirected to a "patch folder" for easy use with layeredFS. 
* When the program requests to read a set of files, it will first check to see if an edited version exists, and if not, falls back to the original dump file.

With custom firmware, layeredFS functionality will selectively redirect file loading to files that are present in the patch folder, removing the need to rebuild a custom ROM.

pkNX also provides some utility to extract from supported container types, e.g. `gfpak`. Simply drag & drop a container (or many) into the main window, and pkNX will unpack all files to a new folder.

## Building

pkNX is a Windows Forms application which requires [.NET Framework v4.6](https://www.microsoft.com/en-us/download/details.aspx?id=48137).

The executable can be built with any compiler that supports C# 8.

## Dependencies

PKHeX's shiny sprite collection is taken from [pokesprite](https://github.com/msikma/pokesprite), which is licensed under [the MIT license](https://github.com/msikma/pokesprite/blob/master/LICENSE).
