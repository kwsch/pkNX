using pkNX.Structures;
using System;
using System.IO;
using System.Threading.Tasks;

namespace pkNX.WinForms;

public class ProgramSettings
{
    public static readonly string ProgramSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
    public static ProgramSettings LoadSettings() => SettingsSerializer.GetSettings<ProgramSettings>(ProgramSettingsPath).Result;
    public static async Task SaveSettings(ProgramSettings settings) => await SettingsSerializer.SaveSettings(settings, ProgramSettingsPath);

    public int Language { get; set; } = 2;
    public string GamePath { get; set; } = string.Empty;
    public bool DisplayAdvanced { get; set; } = false;
}
