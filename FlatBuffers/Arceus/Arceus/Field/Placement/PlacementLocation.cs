using System.Numerics;

namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PlacementLocation
{
    public const ulong ShapeIdSphere = 0x645FB59E96A6F998;
    public const ulong ShapeIdBox = 0xE6B50E272D8CD992;
    public const ulong ShapeNone = 0xCBF29CE484222645;

    public PlacementParameters Parameters
    {
        get { if (ParameterSet.Count != 1) throw new ArgumentException($"Invalid {nameof(ParameterSet)}"); return ParameterSet[0]; }
        set { if (ParameterSet.Count != 1) throw new ArgumentException($"Invalid {nameof(ParameterSet)}"); ParameterSet[0] = value; }
    }

    public bool IsNamedPlace => LocationTypeID == 0x0351C91B3FD84C97;
    public string PlaceName => LocationTypeArg1;

    public string ShapeSummary => ShapeID switch
    {
        ShapeIdSphere => "/* Shape = */ \"sphere\"",
        ShapeIdBox => "/* Shape = */ \"box\"",
        ShapeNone => "/* Shape = */ \"\"",
        _ => $"/* Shape = */ 0x{ShapeID:X16}",
    };

    public bool Contains(float x, float y, float z)
    {
        if (ShapeID == ShapeIdSphere)
        {
            if (!Parameters.Scale.IsOne)
                throw new NotImplementedException("Scaled spheres not yet supported!");

            var radius = Math.Abs(SizeX);
            var distance = Parameters.Coordinates.DistanceTo(new(x, y, z));
            return distance <= radius;
        }
        if (ShapeID == ShapeIdBox)
        {
            // Get the details of the box
            var center = new Vector3(Parameters.Coordinates.X, Parameters.Coordinates.Y, Parameters.Coordinates.Z);
            var rotation = Quaternion.CreateFromYawPitchRoll(Parameters.Rotation.Y * (float)Math.PI / 180.0f, Parameters.Rotation.X * (float)Math.PI / 180.0f, Parameters.Rotation.Z * (float)Math.PI / 180.0f);
            var inverseRotation = Quaternion.Conjugate(rotation);

            // Get the distance to the point along non-rotated axes
            var delta = Vector3.Transform(new Vector3(x, y, z) - center, inverseRotation);

            var x_unit = Parameters.Scale.X * SizeX / 2;
            var y_unit = Parameters.Scale.Y * SizeY / 2;
            var z_unit = Parameters.Scale.Z * SizeZ / 2;

            // Check that the point is within unit-distance along all axes
            if (Math.Abs(delta.X) > Math.Abs(x_unit))
                return false;
            if (Math.Abs(delta.Y) > Math.Abs(y_unit))
                return false;
            if (Math.Abs(delta.Z) > Math.Abs(z_unit))
                return false;

            return true;
        }
        {
            if (ShapeID != ShapeNone) // ""
                throw new NotImplementedException("Unknown ShapeID!");
            return false;
        }
    }

    public bool IntersectsSphere(float x, float y, float z, float sphereRadius)
    {
        if (sphereRadius == 0)
            return Contains(x, y, z);

        if (ShapeID == ShapeIdSphere)
        {
            if (!Parameters.Scale.IsOne)
                throw new NotImplementedException("Scaled spheres not yet supported!");

            var radius = Math.Abs(SizeX);
            var distance = Parameters.Coordinates.DistanceTo(new(x, y, z));
            return distance <= radius + sphereRadius;
        }
        if (ShapeID == ShapeIdBox)
        {
            // Get the details of the box
            var center = new Vector3(Parameters.Coordinates.X, Parameters.Coordinates.Y, Parameters.Coordinates.Z);
            var rotation = Quaternion.CreateFromYawPitchRoll(Parameters.Rotation.Y * (float)Math.PI / 180.0f, Parameters.Rotation.X * (float)Math.PI / 180.0f, Parameters.Rotation.Z * (float)Math.PI / 180.0f);
            var inverseRotation = Quaternion.Conjugate(rotation);

            // Get the distance to the point along non-rotated axes
            var sphereCenter = new Vector3(x, y, z);
            var delta = Vector3.Transform(sphereCenter - center, inverseRotation);

            var x_unit = Parameters.Scale.X * SizeX / 2;
            var y_unit = Parameters.Scale.Y * SizeY / 2;
            var z_unit = Parameters.Scale.Z * SizeZ / 2;

            var contains = true;

            if (Math.Abs(delta.X) > Math.Abs(x_unit))
            {
                contains = false;
                delta.X = Math.Abs(x_unit) * Math.Sign(delta.X);
            }

            if (Math.Abs(delta.Y) > Math.Abs(y_unit))
            {
                contains = false;
                delta.Y = Math.Abs(y_unit) * Math.Sign(delta.Y);
            }

            if (Math.Abs(delta.Z) > Math.Abs(z_unit))
            {
                contains = false;
                delta.Z = Math.Abs(z_unit) * Math.Sign(delta.Z);
            }

            // If the point is contained, it necessarily intersects
            if (contains)
                return true;

            // Get the point on the box closest to the sphere
            var closest = center + Vector3.Transform(delta, rotation);

            // Check if closest point on box is within sphere's radius
            var distance = Vector3.Distance(closest, sphereCenter);
            return distance <= sphereRadius;
        }
        {
            if (ShapeID != ShapeNone) // ""
                throw new NotImplementedException("Unknown ShapeID!");
            return false;
        }
    }

    public bool Contains(Vec3f v) => Contains(v.X, v.Y, v.Z);

    public string LocationTypeSummary
    {
        get
        {
            if (LocationTypeArg2 != 0xCBF29CE484222645)
                return $"{LocationTypeIdSummary}(\"{LocationTypeArg1}\", {LocationArg2Summary})";

            if (!string.IsNullOrEmpty(LocationTypeArg1))
                return $"{LocationTypeIdSummary}(\"{LocationTypeArg1}\")";

            return LocationTypeIdSummary;
        }
    }

    public string LocationTypeIdSummary =>
        // todo lookup
        (LocationTypeID).ToString();

    // lazy init
    private static IReadOnlyDictionary<ulong, string>? _locationArgMap;
    private static IReadOnlyDictionary<ulong, string> GetLocationArgMap() => _locationArgMap ??= GenerateLocationArgMap();

    private static IReadOnlyDictionary<ulong, string> GenerateLocationArgMap() => new Dictionary<ulong, string>
    {
        [0xCBF29CE484222645] = "",
        // PlaceName
        [FnvHash.HashFnv1a_64("NoneReport")] = "NoneReport",
        // Bgm
        [FnvHash.HashFnv1a_64("LOW")] = "LOW",
        [FnvHash.HashFnv1a_64("HIGH")] = "HIGH",
    };

    public string LocationArg2Summary => GetLocationArgMap().TryGetValue(LocationTypeArg2, out var arg) ? $"\"{arg}\"" : $"0x{LocationTypeArg2:X16}";

    public override string ToString() => $"Location(\"{Field00}\", 0x{Field01:X16}, {Parameters}, {ShapeSummary}, {SizeX}, {SizeY}, {SizeZ}, {LocationTypeSummary})";
}
