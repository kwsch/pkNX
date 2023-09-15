using pkNX.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.WinForms;

public class GameConfig : INotifyPropertyChanged
{
    private string? _workspacePath;

    public GameVersion Game { get; init; } = GameVersion.Invalid;
    public string? WorkspacePath
    {
        get => _workspacePath;
        set
        {
            _workspacePath = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(RomFSPath));
            OnPropertyChanged(nameof(ExeFSPath));
            OnPropertyChanged(nameof(ExportPath));
        }
    }
    public string? RomFSPathOverride { get; set; }
    public string? ExeFSPathOverride { get; set; }
    public string? ExportPathOverride { get; set; }

    public string GameTitle => Game.GetTitleName();
    public bool IsValid => Game != GameVersion.Invalid && !string.IsNullOrWhiteSpace(WorkspacePath);
    public bool OverrideDefaultPaths { get; set; }

    private string GetOverridePathOrDefault(string? overridePath, string defaultFolder)
    {
        if (string.IsNullOrWhiteSpace(WorkspacePath))
            return string.Empty;

        if (OverrideDefaultPaths && !string.IsNullOrWhiteSpace(overridePath))
            return overridePath;

        return Path.Combine(WorkspacePath, defaultFolder);
    }

    public string RomFSPath
    {
        get => GetOverridePathOrDefault(RomFSPathOverride, "romfs");
        set => RomFSPathOverride = value;
    }
    public string ExeFSPath
    {
        get => GetOverridePathOrDefault(ExeFSPathOverride, "exefs");
        set => ExeFSPathOverride = value;
    }
    public string ExportPath
    {
        get => GetOverridePathOrDefault(ExportPathOverride, Game.GetTitleID());
        set => ExportPathOverride = value;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ProgramSettings
{
    public static readonly string ProgramSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
    public static ProgramSettings LoadSettings()
    {
        var settings = SettingsSerializer.GetSettings<ProgramSettings>(ProgramSettingsPath).Result;
        settings.UpdateForSupportedGames();
        return settings;
    }

    public static async Task SaveSettings(ProgramSettings settings) => await SettingsSerializer.SaveSettings(settings, ProgramSettingsPath);

    public int Language { get; set; } = 2;
    public bool DisplayAdvanced { get; set; } = false;

    public string GamePath { get; set; } = string.Empty;
    public GameVersion GameOverride { get; set; } = GameVersion.Any;

    public GameConfig[] GameConfigs { get; set; } = Array.Empty<GameConfig>();

    public void UpdateForSupportedGames()
    {
        // Add all missing supported games to the config
        if (GameConfigs.Length >= Game.GamePath.SupportedGames.Length)
            return;

        var missing = Game.GamePath.SupportedGames.Except(GameConfigs.Select(x => x.Game));
        GameConfigs = GameConfigs
            .Concat(missing.Select(x => new GameConfig { Game = x }))
            .OrderBy(x => x.Game)
            .ToArray();

        GameConfigs.First(x => x.Game == GameOverride).WorkspacePath = GamePath;
    }
}

public static class SettingsSerializer
{
    public static async Task<T> GetSettings<T>(string path, CancellationToken token = default) where T : new()
    {
        if (!File.Exists(path))
            return new T();

        try
        {
            var data = await File.ReadAllTextAsync(path, token).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<T>(data) ?? new T();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to load settings from {path}: {ex.Message}");
            return new T();
        }
    }

    public static async Task SaveSettings<T>(T settings, string path, CancellationToken token = default)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(settings);
            await File.WriteAllTextAsync(path, json, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
