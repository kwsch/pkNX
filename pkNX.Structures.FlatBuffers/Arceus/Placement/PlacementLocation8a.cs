using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using FlatSharp.Attributes;
using pkNX.Containers;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementLocation8a
{
    public const ulong ShapeIdSphere = 0x645FB59E96A6F998;
    public const ulong ShapeIdBox = 0xE6B50E272D8CD992;
    public const ulong ShapeNone = 0xCBF29CE484222645;

    [FlatBufferEnum(typeof(ulong))]
    public enum LocationType8a : ulong
    {
        Bgm = 0xD090DB268FBCE531, // "Bgm"
        BuddyInfeasibleOcclusion = 0x1713B588A00FF9F2, // "BuddyInfeasibleOcclusion"
        Evolution = 0x51550D70709EA65E, // "Evolution"
        LightOcclusion = 0x94FEC9F97E988350, // "LightOcclusion"
        NoRespawnOcclusion = 0xD91EE555A7E6DC07, // "NoRespawnOcclusion"
        OcclusionSpace = 0x613F7FD830366C6C, // "OcclusionSpace"
        PlaceName = 0x0351C91B3FD84C97, // "PlaceName
        RideInfeasibleOcclusion = 0x677D529DBB999AD6, // "RideInfeasibleOcclusion"
        SafetyArea = 0x4C62CAA0FF9B445A, // "SafetyArea"
        SaveGameOver = 0x77EEDD438D48D1B8, // "SaveGameOver"
        TakeOffShoesOcclusion = 0x09FBFD6B423FAEA6, // "TakeOffShoesOcclusion"
        TreeCullingFrustumFar = 0xD55673931A300690, // "TreeCullingFrustumFar"
        WeatherOcclusion = 0x2B3435F7DF6CF454, // "WeatherOcclusion"
    };

    [FlatBufferItem(0)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(1)] public ulong Field_01 { get; set; }
    [FlatBufferItem(2)] public PlacementParameters8a[] ParameterSet { get; set; } = { new() };
    [FlatBufferItem(3)] public ulong ShapeID { get; set; }
    [FlatBufferItem(4)] public float SizeX { get; set; }
    [FlatBufferItem(5)] public float SizeY { get; set; }
    [FlatBufferItem(6)] public float SizeZ { get; set; }
    [FlatBufferItem(7)] public ulong LocationTypeID { get; set; }
    [FlatBufferItem(8)] public string LocationTypeArg1 { get; set; } = string.Empty;
    [FlatBufferItem(9)] public ulong LocationTypeArg2 { get; set; }

    public PlacementParameters8a Parameters
    {
        get { if (ParameterSet.Length != 1) throw new ArgumentException($"Invalid {nameof(ParameterSet)}"); return ParameterSet[0]; }
        set { if (ParameterSet.Length != 1) throw new ArgumentException($"Invalid {nameof(ParameterSet)}"); ParameterSet[0] = value; }
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
            if (!Parameters.Scale.IsDefault)
                throw new NotImplementedException("Scaled spheres not yet supported!");

            var radius = Math.Abs(SizeX);
            var distance = Parameters.Coordinates.DistanceTo(x, y, z);
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
            if (!Parameters.Scale.IsDefault)
                throw new NotImplementedException("Scaled spheres not yet supported!");

            var radius = Math.Abs(SizeX);
            var distance = Parameters.Coordinates.DistanceTo(x, y, z);
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

    public bool Contains(PlacementV3f8a v) => Contains(v.X, v.Y, v.Z);

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

    public string LocationTypeIdSummary
    {
        get
        {
            if (!Enum.IsDefined(typeof(LocationType8a), LocationTypeID))
                throw new ArgumentOutOfRangeException(nameof(LocationTypeID), LocationTypeID, $"Unknown 0x{LocationTypeID:X16}.");
            return Enum.GetName(typeof(LocationType8a), LocationTypeID);
        }
    }

    // lazy init
    private static IReadOnlyDictionary<ulong, string>? _locationArgMap;
    private static IReadOnlyDictionary<ulong, string> GetLocationArgMap() => _locationArgMap ??= GenerateLocationArgMap();

    private static IReadOnlyDictionary<ulong, string> GenerateLocationArgMap()
    {
        var result = new Dictionary<ulong, string>();
        result[0xCBF29CE484222645] = "";

        // PlaceName
        result[FnvHash.HashFnv1a_64("NoneReport")] = "NoneReport";

        // Bgm
        result[FnvHash.HashFnv1a_64("LOW")] = "LOW";
        result[FnvHash.HashFnv1a_64("HIGH")] = "HIGH";
        return result;
    }

    public string LocationArg2Summary => GetLocationArgMap().TryGetValue(LocationTypeArg2, out var arg) ? $"\"{arg}\"" : $"0x{LocationTypeArg2:X16}";

    public override string ToString() => $"Location(\"{Field_00}\", 0x{Field_01:X16}, {Parameters}, {ShapeSummary}, {SizeX}, {SizeY}, {SizeZ}, {LocationTypeSummary})";
}
