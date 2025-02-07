namespace pkNX.Structures.FlatBuffers.Reflection;

/// <summary>
/// Dumps a <see cref="Schema"/> to a fbs and C# file.
/// </summary>
public sealed record SchemaDumpSettings
{
    public bool StripNamespace { get; init; } = true;
    public string FileNamespace { get; init; } = "pkNX.Structures.FlatBuffers";
}
