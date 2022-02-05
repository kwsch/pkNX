using System.IO;

namespace pkNX.Structures.FlatBuffers
{
    public static class FlatDumper
    {
        public static string GetTable<T1, T2>(string path) where T1 : class, IFlatBufferArchive<T2> where T2 : class
        {
            var data = File.ReadAllBytes(path);
            return GetTable<T1, T2>(data);
        }

        public static string GetTable<T1, T2>(byte[] data) where T1 : class, IFlatBufferArchive<T2> where T2 : class
        {
            var obj = FlatBufferConverter.DeserializeFrom<T1>(data);
            var table = obj.Table;
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
}
