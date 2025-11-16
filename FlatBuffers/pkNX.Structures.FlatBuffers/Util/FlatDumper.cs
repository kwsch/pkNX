using FlatSharp;

namespace pkNX.Structures.FlatBuffers;

public static class FlatDumper
{
    public static string GetTable<T1, T2>(string path, Func<T1, IList<T2>> sel) where T1 : class, IFlatBufferSerializable<T1> where T2 : notnull
    {
        var data = File.ReadAllBytes(path);
        return GetTable(data, sel);
    }

    public static string GetTable<T1, T2>(Memory<byte> data, Func<T1, IList<T2>> sel) where T1 : class, IFlatBufferSerializable<T1> where T2 : notnull
    {
        var obj = FlatBufferConverter.DeserializeFrom<T1>(data);
        var table = sel(obj);
        return TableUtil.GetTable(table);
    }

    public static string GetSchema<T>() where T : class, new()
    {
        var t = typeof(T);
        var obj = new T();
        var dump = new FlatSchemaDump(obj);
        return dump.GetSingleFileSchema(t);
    }
}
