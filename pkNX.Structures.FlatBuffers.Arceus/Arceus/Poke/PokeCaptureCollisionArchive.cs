using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeCaptureCollision;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeCaptureCollider;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PokeCaptureCollisionArchive
{
    public PokeCaptureCollision GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(x => x.Species == species && x.Form == form) ?? new()
        {
            Colliders = [],
        };
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public PokeCaptureCollision AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The encounter rate table already contains an entry for the same species and form!");

        var entry = new PokeCaptureCollision
        {
            Species = species,
            Form = form,
            Colliders =
            [
                new() {
                    Shape = "Capsule",
                    ShapeParameters = [0.5f, 0.2f, 0, 0, 0, 0, 0, 0],
                    SocketName = "waist",
                    Type = "Normal",
                    Field07 = string.Empty,
                },
                new() {
                    Shape = "Capsule",
                    ShapeParameters = [0.5f, 0.2f, 0, 0, 0, 0, 0, 0],
                    SocketName = string.Empty,
                    Type = "Pysics",
                    Field07 = string.Empty,
                },
                new() {
                    Shape = "Capsule",
                    ShapeParameters = [0.5f, 0.2f, 0, 0, 0, 0, 0, 0],
                    SocketName = "origin",
                    Type = "Barrier",
                    Field07 = string.Empty,
                },
            ],
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
