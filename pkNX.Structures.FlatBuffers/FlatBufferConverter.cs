using System;
using System.IO;
using FlatSharp;

namespace pkNX.Structures.FlatBuffers;

public static class FlatBufferConverter
{
    public static T[] DeserializeFrom<T>(string[] files) where T : class
    {
        var result = new T[files.Length];
        for (int i = 0; i < result.Length; i++)
        {
            var file = files[i];
            result[i] = DeserializeFrom<T>(file);
        }
        return result;
    }

    public static byte[][] SerializeFrom<T>(T[] obj) where T : class
    {
        var result = new byte[obj.Length][];
        for (int i = 0; i < result.Length; i++)
        {
            var file = obj[i];
            result[i] = SerializeFrom(file);
        }
        return result;
    }

    public static T DeserializeFrom<T>(string file) where T : class
    {
        var data = File.ReadAllBytes(file);
        return DeserializeFrom<T>(data);
    }

    public static T DeserializeFrom<T>(byte[] data) where T : class
    {
        return FlatBufferSerializer.Default.Parse<T>(data);
    }

    public static byte[] SerializeFrom<T>(T obj) where T : class
    {
        var serializer = FlatBufferSerializer.Default;
        var size = serializer.GetMaxSize(obj);
        var data = new byte[size];
        var result = serializer.Serialize(obj, data);
        if (result != data.Length)
            Array.Resize(ref data, result);
        return data;
    }
}
