using pkNX.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using pkNX.Game;

namespace pkNX.WinForms;

public class SettingsModel
{
}

/// <summary>
/// Interaction logic for Settings.xaml
/// </summary>
public partial class Settings
{
    public ProgramSettings Config { get; set; }

    public Settings()
    {
        Config = ProgramSettings.LoadSettings();

        InitializeComponent();
    }

    private async void Window_Closing(object sender, CancelEventArgs e)
    {
        // TODO: Verify workspace paths for each game

        await ProgramSettings.SaveSettings(Config);
    }
}
