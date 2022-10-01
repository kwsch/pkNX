using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.WinForms;

public static class SettingsSerializer
{
    public static async Task<T> GetSettings<T>(string path) where T : new()
    {
        if (!File.Exists(path))
            return new T();

        try
        {
            var data = await File.ReadAllTextAsync(path);
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
            await File.WriteAllTextAsync(path, json, token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}