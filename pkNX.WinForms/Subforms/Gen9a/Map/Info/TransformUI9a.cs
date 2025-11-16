using System;

namespace pkNX.WinForms;

public static class TransformUI9a
{
    public static MapTransform GetMapTransform(LumioseFieldIndex index) => index switch
    {
        LumioseFieldIndex.Overworld => TransformLumiose,
        LumioseFieldIndex.LysandreLabs => TransformLysandreLabs,
        LumioseFieldIndex.SewersCh5 => TransformSewersCh5,
        LumioseFieldIndex.SewersCh6 => TransformSewersCh6,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    // Named transforms for each map index
    // Instantiate transforms for each map index
    public static readonly MapTransform TransformLumiose = new(
        Texture: new(4096.0, 4096.0),
        Range: new(3940.0, 3940.0),
        Scale: new(1000.0, 1000.0),
        Dir: new(-1.0, -1.0),
        Offset: new(500.0, 500.0)
    );

    public static readonly MapTransform TransformLysandreLabs = new(
        Texture: new(2160.0, 2160.0),
        Range: new(1662.0, 2041.0),
        Scale: new(1662.0 / 10.291021, 2041.0 / 10.291021),
        Dir: new(-1.0, -1.0),
        Offset: new(-3.0, -80.0)
    );

    public static readonly MapTransform TransformSewersCh5 = new(
        Texture: new(2160.0, 2160.0),
        Range: new(1364.0, 1975.0),
        Scale: new(1364.0 / 6.2, 1975.0 / 6.2),
        Dir: new(1.0, 1.0),
        Offset: new(1.0, 146.0)
    );

    public static readonly MapTransform TransformSewersCh6 = new(
        Texture: new(2160.0, 2160.0),
        Range: new(1521.0, 1966.0),
        Scale: new(1521.0 / 16.714285, 1966.0 / 16.714285),
        Dir: new(1.0, 1.0),
        Offset: new(39.0, 45.0)
    );
}
