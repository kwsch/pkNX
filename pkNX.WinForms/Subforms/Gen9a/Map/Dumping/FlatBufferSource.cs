using FlatSharp;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;
using System;

namespace pkNX.WinForms;

public sealed class FlatBufferSource(IFileInternal rom)
{
    public T Get<T>(string path) where T : class, IFlatBufferSerializable<T>
        => Get<T>(rom.GetPackedFile(path));
    public T Get<T>(Memory<byte> data) where T : class, IFlatBufferSerializable<T>
        => FlatBufferConverter.DeserializeFrom<T>(data);
}
