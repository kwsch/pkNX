using System.Collections.Generic;

namespace pkNX.WinForms;

/// <summary>
/// Wrapper for concise exports for viewing/importing Json data of spawners in Gen 9a maps.
/// </summary>
public sealed record FakeSpawner9a
{
    public required SceneSpawner Link { private get; init; }

    public string Name => Link.Name;
    public string Type => Link.Type;
    public string Scale => $"{Link.Scale.X:F2}, {Link.Scale.Y:F2}, {Link.Scale.Z:F2}";
    public string Rotation => $"{Link.Rotation.X:F2}, {Link.Rotation.Y:F2}, {Link.Rotation.Z:F2}";
    public string Position => $"{Link.Position.X:F2}, {Link.Position.Y:F2}, {Link.Position.Z:F2}";

    public required int SpawnCountMin { get; init; }
    public required int SpawnCountMax { get; init; }
    public required string Location { get; init; }
    public required float Cooldown { get; init; }
    public required int CooldownCondition { get; init; }
    public required string Conditions { get; init; }
    public List<DetailedSpawn9a> Spawns { get; } = [];
}
