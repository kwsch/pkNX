# pkNX

![License](https://img.shields.io/badge/License-GPLv3-blue.svg)

pkNX: A package of Pokémon (Nintendo Switch) ROM Editing Tools, programmed in [C#](https://en.wikipedia.org/wiki/C_Sharp_%28programming_language%29).

Similar to [pk3DS](https://github.com/kwsch/pk3DS) for Nintendo 3DS, pkNX provides an editing environment to manipulate game binary assets such as stats, learnsets, trainers, and more!

![Main Window](https://i.imgur.com/7WiBJpn.png)

## Download

Download the latest version [here](https://dev.azure.com/project-pokemon/pkNX/_build?view=runs).

Click on the latest run at the top, then click `# published` under `Related`, then click `Download artifacts` using the button on the right.
<img src="https://github.com/user-attachments/assets/755435d0-2647-4750-ab73-93e2f65bdf8a" width=95% height=80%>
<img src="https://github.com/user-attachments/assets/456b7d28-bdf4-4aa5-96ce-52d4cdac0667" width=95% height=80%>

## Supported Games

The following games are supported:
* Let's Go, Pikachu! / Let's Go, Eevee!
* Sword / Shield
* Legends: Arceus
* Scarlet / Violet **(Only for dumping data, not to edit files!)**
* Legends: Z-A **(Only for dumping data, not to edit files!)**

pkNX operates under the assumption that your dumped ROM includes the latest available update data for the following games:
* Sword / Shield (Ver. 1.3.2)
* Scarlet / Violet (Ver. 4.0.0)
* Legends: Z-A (Ver. 2.0.0)

## Features

Editors can be launched from the program's main window after opening a dumped & unpacked ROM. 
* To lessen read/write lag, data is only saved when the user cleanly quits the program. 
* Edited files do not overwrite the original dumped file; instead, they are redirected to a "patch folder" for easy use with LayeredFS. 
* When the program requests to read a set of files, it will first check to see if an edited version exists, and if not, falls back to the original dump file.

With Custom Firmware, LayeredFS functionality will selectively redirect file loading to files that are present in the patch folder, removing the need to rebuild a custom ROM.

pkNX also provides some utility to extract from supported container types, e.g. `gfpak`. Simply drag & drop a container (or many) into the main window, and pkNX will unpack all files to a new folder.

## Building

pkNX is a Windows Forms application which requires [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0).

The executable can be built with any compiler that supports C# 13.

## Dependencies

pkNX's shiny sprite collection is taken from [pokesprite](https://github.com/msikma/pokesprite), which is licensed under [the MIT license](https://github.com/msikma/pokesprite/blob/master/LICENSE).
