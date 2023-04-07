using System;
using System.Buffers;
using System.IO;
using FlatSharp;
using static FlatSharp.FlatBufferDeserializationOption;

namespace pkNX.Structures.FlatBuffers;

public static class FlatBufferConverter
{
    public static T[] DeserializeFrom<T>(string[] files)
        where T : class, IFlatBufferSerializable<T>
    {
        var result = new T[files.Length];
        for (int i = 0; i < result.Length; i++)
        {
            var file = files[i];
            result[i] = DeserializeFrom<T>(file);
        }
        return result;
    }

    public static T DeserializeFrom<T>(string path) where T : class, IFlatBufferSerializable<T> => DeserializeFrom<T>(path, GreedyMutable);
    public static T DeserializeFrom<T>(byte[] data) where T : class, IFlatBufferSerializable<T> => DeserializeFrom<T>(data, GreedyMutable);
    public static T DeserializeFrom<T>(Memory<byte> data) where T : class, IFlatBufferSerializable<T> => DeserializeFrom<T>(data, GreedyMutable);

    public static T DeserializeFrom<T>(string path, FlatBufferDeserializationOption opt)
        where T : class, IFlatBufferSerializable<T>
    {
        var data = File.ReadAllBytes(path);
        return DeserializeFrom<T>(data, opt);
    }

    public static T DeserializeFrom<T>(byte[] data, FlatBufferDeserializationOption opt)
        where T : class, IFlatBufferSerializable<T> => opt switch
    {
        Lazy => T.LazySerializer.Parse(data),
        Progressive => T.ProgressiveSerializer.Parse(data),
        Greedy => T.GreedySerializer.Parse(data),
        GreedyMutable => T.GreedyMutableSerializer.Parse(data),
        _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null),
    };

    public static T DeserializeFrom<T>(Memory<byte> data, FlatBufferDeserializationOption opt)
        where T : class, IFlatBufferSerializable<T> => opt switch
    {
        Lazy => T.LazySerializer.Parse(data),
        Progressive => T.ProgressiveSerializer.Parse(data),
        Greedy => T.GreedySerializer.Parse(data),
        GreedyMutable => T.GreedyMutableSerializer.Parse(data),
        _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null),
    };

    public static byte[] SerializeFrom<T>(this T obj) where T : class, IFlatBufferSerializable<T>
    {
        var pool = ArrayPool<byte>.Shared;
        var serializer = obj.Serializer;
        var data = pool.Rent(serializer.GetMaxSize(obj));
        var len = serializer.Write(data, obj);
        var result = data.AsSpan(0, len).ToArray();
        pool.Return(data);
        return result;
    }
}
