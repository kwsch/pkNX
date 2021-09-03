using System.IO;

namespace pkNX.Structures.FlatBuffers
{
    public static class FlatDumper
    {
        public static string GetTable<T1, T2>(string path) where T1 : class, IFlatBufferArchive<T2> where T2 : class
        {
            var data = File.ReadAllBytes(path);
            var obj = FlatBufferConverter.DeserializeFrom<T1>(data);
            var table = obj.Table;
            return TableUtil.GetTable(table);
        }
    }
}
