using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollisionArchive8a : IFlatBufferArchive<PokeCaptureCollision8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeCaptureCollision8a[] Table { get; set; } = Array.Empty<PokeCaptureCollision8a>();

    public PokeCaptureCollision8a GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(x => x.Species == species && x.Form == form) ??
            new PokeCaptureCollision8a { };
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public PokeCaptureCollision8a AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The encounter rate table already contains an entry for the same species and form!");

        var entry = new PokeCaptureCollision8a
        {
            Species = species,
            Form = form,
            Colliders = new PokeCaptureCollider[]
            {
                new() {
                    Shape = "Capsule",
                    ShapeParameters = new float[]{ 0.5f, 0.2f, 0, 0, 0, 0, 0, 0 },
                    SocketName = "waist",
                    Type = "Normal",
                },
                new() {
                    Shape = "Capsule",
                    ShapeParameters = new float[]{ 0.5f, 0.2f, 0, 0, 0, 0, 0, 0 },
                    Type = "Pysics",
                },
                new() {
                    Shape = "Capsule",
                    ShapeParameters = new float[]{ 0.5f, 0.2f, 0, 0, 0, 0, 0, 0 },
                    SocketName = "origin",
                    Type = "Barrier",
                },
            }
        };

        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollision8a
{
    [FlatBufferItem(00)] public uint Species { get; set; }
    [FlatBufferItem(01)] public uint Form { get; set; }
    [FlatBufferItem(02)] public PokeCaptureCollider[] Colliders { get; set; } = Array.Empty<PokeCaptureCollider>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeCaptureCollider
{
    [FlatBufferItem(00)] public string SocketName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Shape { get; set; } = string.Empty;
    [FlatBufferItem(02)] public float[] ShapeParameters { get; set; } = Array.Empty<float>();
    [FlatBufferItem(03)] public float OffsetX { get; set; }
    [FlatBufferItem(04)] public float OffsetY { get; set; }
    [FlatBufferItem(05)] public float OffsetZ { get; set; }
    [FlatBufferItem(06)] public string Type { get; set; } = string.Empty;
    [FlatBufferItem(07)] public string Field_07 { get; set; } = string.Empty; // Heatran has this on collider[1] (PM0485), some have it set to SemiLegendFlick. Something like a script tag maybe?
}
