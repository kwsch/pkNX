using pkNX.Structures;

namespace pkNX.WinForms;

public class ProgramSettings
{
    public int Language { get; set; } = 2;
    public string GamePath { get; set; } = string.Empty;
    public GameVersion GameOverride { get; set; } = GameVersion.Any;
}
