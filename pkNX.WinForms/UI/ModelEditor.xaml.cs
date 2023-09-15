using pkNX.Containers.VFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Containers;

namespace pkNX.WinForms;

/// <summary>
/// Interaction logic for ModelEditor.xaml
/// </summary>
public partial class ModelEditor : Window
{
    public ModelEditor()
    {
        InitializeComponent();

        VirtualFileSystem vfs = VirtualFileSystem.Current;

        var settings = ProgramSettings.LoadSettings();
        var loadedGames = settings.GameConfigs
            .Where(x => x.IsValid && vfs.IsGameLoaded(x.Game))
            .ToList();

        CB_GameSource.ItemsSource = loadedGames;
        CB_GameSource.SelectedIndex = loadedGames.FindIndex(x => x.Game == settings.GameOverride);
    }

    private void B_Convert_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CB_GameSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CB_GameSource.SelectedItem is not GameConfig config)
            return;

        VirtualFileSystem vfs = VirtualFileSystem.Current;
        FileSystemPath pokemonModelDir = config.Game.ToMountPath() + GamePath.GetDirectoryPath(GameFile.PokemonArchiveFolder, config.Game);

        var f = vfs.GetFilesInDirectory(pokemonModelDir);
        CB_Species.ItemsSource = vfs.GetFiles(pokemonModelDir, path => path.EntityName != "pokeconfig.gfpak");

    }

    private void CB_Species_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var file = (VirtualFile)CB_Species.SelectedItem;

        switch (((GameConfig)CB_GameSource.SelectedItem).Game)
        {
            case GameVersion.SW:
            case GameVersion.SH:
            case GameVersion.SWSH:
                break;
            case GameVersion.PLA:
                LoadFile_PLA(file);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void LoadFile_PLA(VirtualFile file)
    {
        using var reader = new BinaryReader(file.OpenRead());
        PG_Model.SelectedObject = new GFPack(reader);
    }
}
