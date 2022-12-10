using FlatSharp.Attributes;
using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers;

// *.trmbf

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ByteBuffer
{
    [FlatBufferItem(00)] public byte[] Data { get; set; } = Array.Empty<byte>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeshBuffer
{
    [FlatBufferItem(00)] public ByteBuffer[] IndexBuffer { get; set; } = Array.Empty<ByteBuffer>();
    [FlatBufferItem(01)] public ByteBuffer[] VertexBuffer { get; set; } = Array.Empty<ByteBuffer>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRMeshBuffer
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public MeshBuffer[] Buffers { get; set; } = Array.Empty<MeshBuffer>();
}
