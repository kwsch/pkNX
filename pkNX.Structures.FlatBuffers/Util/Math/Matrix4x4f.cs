using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Matrix4x4f
{
    [FlatBufferItem(0)] public PackedVec4f AxisX { get; set; }
    [FlatBufferItem(1)] public PackedVec4f AxisY { get; set; }
    [FlatBufferItem(2)] public PackedVec4f AxisZ { get; set; }
    [FlatBufferItem(3)] public PackedVec4f AxisW { get; set; }

    public static readonly Matrix4x4f Identity = new();

    public Matrix4x4f(
        float m00 = 1, float m01 = 0, float m02 = 0, float m03 = 0,
        float m10 = 0, float m11 = 1, float m12 = 0, float m13 = 0,
        float m20 = 0, float m21 = 0, float m22 = 1, float m23 = 0,
        float m30 = 0, float m31 = 0, float m32 = 0, float m33 = 1)
    {
        AxisX = new(m00, m01, m02, m03);
        AxisY = new(m10, m11, m12, m13);
        AxisZ = new(m20, m21, m22, m23);
        AxisW = new(m30, m31, m32, m33);
    }

    public Matrix4x4f(PackedVec4f axisX, PackedVec4f axisY, PackedVec4f axisZ, PackedVec4f axisW)
    {
        AxisX = axisX;
        AxisY = axisY;
        AxisZ = axisZ;
        AxisW = axisW;
    }
}
