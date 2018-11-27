using System;
using System.IO;

namespace pkNX.Containers
{
    /// <summary>
    /// // Mini Packing Util
    /// </summary>
    public static class MiniUtil
    {
        public static byte[] PackMini(string folder, string identifier) => PackMini(Directory.GetFiles(folder), identifier);

        public static byte[] PackMini(string[] files, string identifier)
        {
            byte[][] fileData = new byte[files.Length][];
            for (int i = 0; i < fileData.Length; i++)
                fileData[i] = FileMitm.ReadAllBytes(files[i]);
            return PackMini(fileData, identifier);
        }

        public static byte[] PackMini(byte[][] fileData, string identifier)
        {
            // Create new Binary with the relevant header bytes
            byte[] data = new byte[4];
            data[0] = (byte)identifier[0];
            data[1] = (byte)identifier[1];
            Array.Copy(BitConverter.GetBytes((ushort)fileData.Length), 0, data, 2, 2);

            int count = fileData.Length;
            int dataOffset = 4 + 4 + (count * 4);

            // Start the data filling.
            using (MemoryStream dataout = new MemoryStream())
            using (MemoryStream offsetMap = new MemoryStream())
            using (BinaryWriter bd = new BinaryWriter(dataout))
            using (BinaryWriter bo = new BinaryWriter(offsetMap))
            {
                // For each file...
                for (int i = 0; i < count; i++)
                {
                    // Write File Offset
                    uint fileOffset = (uint)(dataout.Position + dataOffset);
                    bo.Write(fileOffset);

                    // Write File to Stream
                    bd.Write(fileData[i]);

                    // Pad the Data MemoryStream with Zeroes until len%4=0;
                    while (dataout.Length % 4 != 0)
                        bd.Write((byte)0);
                    // File Offset will be updated as the offset is based off of the Data length.
                }
                // Cap the File
                bo.Write((uint)(dataout.Position + dataOffset));

                using (var newPack = new MemoryStream())
                using (var header = new MemoryStream(data))
                {
                    header.WriteTo(newPack);
                    offsetMap.WriteTo(newPack);
                    dataout.WriteTo(newPack);
                    return newPack.ToArray();
                }
            }
        }

        public static byte[][] UnpackMini(string file, string identifier = null)
        {
            byte[] fileData = FileMitm.ReadAllBytes(file);
            return UnpackMini(fileData, identifier);
        }

        public static byte[][] UnpackMini(byte[] fileData, string identifier = null)
        {
            if (fileData == null || fileData.Length < 4)
                return null;

            if (identifier?.Length == 2)
            {
                if (identifier[0] != fileData[0] || identifier[1] != fileData[1])
                    return null;
            }

            int count = BitConverter.ToUInt16(fileData, 2); int ctr = 4;
            int start = BitConverter.ToInt32(fileData, ctr); ctr += 4;
            byte[][] returnData = new byte[count][];
            for (int i = 0; i < count; i++)
            {
                int end = BitConverter.ToInt32(fileData, ctr); ctr += 4;
                int len = end - start;
                byte[] data = new byte[len];
                Buffer.BlockCopy(fileData, start, data, 0, len);
                returnData[i] = data;
                start = end;
            }
            return returnData;
        }

        public static Mini GetMini(string path)
        {
            path = FileMitm.GetRedirectedPath(path);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
                return GetMini(br);
        }

        public static Mini GetMini(BinaryReader br)
        {
            var ident = GetIsMini(br);
            if (ident == null)
                return null;

            br.BaseStream.Position = 0;
            var data = br.ReadBytes((int)br.BaseStream.Length);
            var unpack = UnpackMini(data, ident);
            return new Mini(unpack, ident);
        }

        public static string GetIsMini(BinaryReader br)
        {
            if (br.BaseStream.Length < 12)
                return null;
            br.BaseStream.Position = 0;
            var ident = br.ReadBytes(2);
            var count = br.ReadUInt16();

            int finalLengthOfs = 4 + (count * 4);
            if (br.BaseStream.Length < finalLengthOfs + 4)
                return null;
            br.BaseStream.Position = 4 + (count * 4);
            var len = br.ReadUInt32();
            if (len != br.BaseStream.Length)
                return null;
            return $"{(char)ident[0]}{(char)ident[1]}";
        }

        public static string GetIsMini(string path)
        {
            try
            {
                path = FileMitm.GetRedirectedPath(path);
                using (var fs = new FileStream(path, FileMode.Open))
                using (var br = new BinaryReader(fs))
                    return GetIsMini(br);
            }
            catch { return null; }
        }

        public static string GetIsMini(byte[] data)
        {
            try
            {
                using (var ms = new MemoryStream(data))
                using (var br = new BinaryReader(ms))
                    return GetIsMini(br);
            }
            catch { return null; }
        }
    }
}
