using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmbf

public class VertexWrapper(InputLayoutElement layoutElement, object data)
{
    public InputLayoutElement LayoutElement { get; set; } = layoutElement;
    public object Data { get; set; } = data;

    public override string ToString() => $"{{ {LayoutElement.SemanticName}{LayoutElement.SemanticIndex:#}, {Data} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ByteBuffer
{
    public VertexAttributeLayout? Debug_InputLayout { get; set; }
    public object[][] Debug_VertexBuffer => Debug_InputLayout == null ? Array.Empty<object[]>() : FromInputLayout(Debug_InputLayout);
    public ushort[] Debug_IndexBuffer => Debug_InputLayout == null ? ((ReadOnlySpan<byte>)Data.Span).GetArray(ReadUInt16LittleEndian, 2) : [];

    public VertexWrapper[][] FromInputLayout(VertexAttributeLayout inputLayout)
    {
        return ((ReadOnlySpan<byte>)Data.Span).GetArray(data =>
        {
            var vertexData = new VertexWrapper[inputLayout.Elements.Count];
            for (var j = 0; j < inputLayout.Elements.Count; j++)
            {
                var layout = inputLayout.Elements[j];
                var offset = (int)layout.Offset;

                vertexData[j] = new VertexWrapper(layout, layout.Format switch
                {
                    InputLayoutFormat.RGBA_8_UNORM => new Vec4f(new Unorm8(data[offset]), new Unorm8(data[offset + 1]), new Unorm8(data[offset + 2]), new Unorm8(data[offset + 3])),
                    InputLayoutFormat.RGBA_8_UNSIGNED => new Vec4i(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]),
                    InputLayoutFormat.RGBA_16_UNORM => new Vec4f(new Unorm16(ReadUInt16LittleEndian(data[offset..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 2)..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 4)..])), new Unorm16(ReadUInt16LittleEndian(data[(offset + 6)..]))),
                    InputLayoutFormat.RGBA_16_FLOAT => new Vec4f((float)ReadHalfLittleEndian(data[offset..]), (float)ReadHalfLittleEndian(data[(offset + 2)..]), (float)ReadHalfLittleEndian(data[(offset + 4)..]), (float)ReadHalfLittleEndian(data[(offset + 6)..])),
                    InputLayoutFormat.RG_32_FLOAT => new Vec2f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..])),
                    InputLayoutFormat.RGB_32_FLOAT => new Vec3f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..]), ReadSingleLittleEndian(data[(offset + 8)..])),
                    InputLayoutFormat.RGBA_32_FLOAT => new Vec4f(ReadSingleLittleEndian(data[offset..]), ReadSingleLittleEndian(data[(offset + 4)..]), ReadSingleLittleEndian(data[(offset + 8)..]), ReadSingleLittleEndian(data[(offset + 12)..])),
                    InputLayoutFormat.NONE => throw new IndexOutOfRangeException(),
                    _ => throw new IndexOutOfRangeException(),
                });
            }

            return vertexData;
        }, (int)inputLayout.Size[0].Size);
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshBuffer;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MeshBufferTable;
