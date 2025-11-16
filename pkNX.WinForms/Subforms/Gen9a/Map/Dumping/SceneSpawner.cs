using System.Collections.Generic;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public sealed record SceneSpawner(
    PackedVec3f Scale,
    PackedVec3f Rotation,
    PackedVec3f Position,
    string Type,
    string Name)
{
    public List<object> Inner { get; set; } = [];
}
