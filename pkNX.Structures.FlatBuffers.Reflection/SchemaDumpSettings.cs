namespace pkNX.Structures.FlatBuffers.Reflection;

/// <summary>
/// Dumps a <see cref="Schema"/> to a fbs and C# file.
/// </summary>
public sealed record SchemaDumpSettings
{
    public bool StripNamespace { get; set; } = true;
    public string FileNamespace { get; set; } = "pkNX.Structures.FlatBuffers";
}
