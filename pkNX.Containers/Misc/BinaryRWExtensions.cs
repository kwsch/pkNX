using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace pkNX.Containers
{
    public static class BinaryRWExtensions
    {
        public static T ReadStruct<T>(this BinaryReader br) where T : struct
        {
            var bytes = br.ReadBytes(Marshal.SizeOf<T>());
            return bytes.ToStructure<T>();
        }

        public static T[] ReadStructArray<T>(this BinaryReader br, uint count) where T : struct
        {
            Debug.Assert(count < 1000); // pls no
            var arr = new T[count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = br.ReadStruct<T>();
            return arr;
        }

        public static string ReadNXString(this BinaryReader br)
        {
            var length = br.ReadUInt16();
            var bytes = br.ReadBytes(length);
            var str = Encoding.ASCII.GetString(bytes);
            br.ReadByte(); // \0
            if (br.BaseStream.Position % 2 != 0)
                br.ReadByte(); // fix align
            return str;
        }

        public static string ReadStringBytesUntil(this BinaryReader br, byte end = 0)
        {
            StringBuilder str = new StringBuilder();
            byte b;
            while ((b = br.ReadByte()) != end)
                str.Append((char)b);
            return str.ToString();
        }
    }
}