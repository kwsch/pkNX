using System;
using System.IO;

namespace pkNX.Containers;

/// <summary>
/// // Mini Packing Util
/// </summary>
public static class MiniUtil
{
    public static byte[] PackMini(string folder, ReadOnlySpan<char> identifier) => PackMini(Directory.GetFiles(folder), identifier);

    public static byte[] PackMini(ReadOnlySpan<string> files, ReadOnlySpan<char> identifier)
    {
        byte[][] fileData = new byte[files.Length][];
        for (int i = 0; i < fileData.Length; i++)
            fileData[i] = FileMitm.ReadAllBytes(files[i]);
        return PackMini(fileData, identifier);
    }

    public static byte[] PackMini(byte[][] fileData, ReadOnlySpan<char> identifier)
    {
        // Create new Binary with the relevant header bytes
        byte[] data = new byte[4];
        data[0] = (byte)identifier[0];
        data[1] = (byte)identifier[1];
        Array.Copy(BitConverter.GetBytes((ushort)fileData.Length), 0, data, 2, 2);

        int count = fileData.Length;
        int dataOffset = 4 + 4 + (count * 4);

        // Start the data filling.
        using MemoryStream dataout = new();
        using MemoryStream offsetMap = new();
        using BinaryWriter bd = new(dataout);
        using BinaryWriter bo = new(offsetMap);
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

        using var newPack = new MemoryStream();
        using var header = new MemoryStream(data);
        header.WriteTo(newPack);
        offsetMap.WriteTo(newPack);
        dataout.WriteTo(newPack);
        return newPack.ToArray();
    }

    public static byte[][] UnpackMini(string file, ReadOnlySpan<char> identifier)
    {
        byte[] fileData = FileMitm.ReadAllBytes(file);
        return UnpackMini(fileData, identifier);
    }

    public static byte[][] UnpackMini(byte[] fileData, ReadOnlySpan<char> identifier)
    {
        if (fileData.Length < 4)
            throw new ArgumentOutOfRangeException(nameof(fileData));

        if (identifier.Length == 2)
        {
            if (identifier[0] != fileData[0] || identifier[1] != fileData[1])
                throw new FormatException("Prefix does not match.");
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
        path = FileMitm.GetRedirectedReadPath(path);
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var br = new BinaryReader(fs);
        return GetMini(br) ?? throw new FormatException($"The file at {path} is not a {nameof(Mini)} file.");
    }

    public static Mini? GetMini(BinaryReader br)
    {
        var ident = GetIsMini(br);
        if (string.IsNullOrEmpty(ident))
            return null;

        br.BaseStream.Position = 0;
        var data = br.ReadBytes((int)br.BaseStream.Length);
        var unpack = UnpackMini(data, ident);
        return new Mini(unpack, ident);
    }

    public static string GetIsMini(BinaryReader br)
    {
        if (br.BaseStream.Length < 12)
            return string.Empty;
        br.BaseStream.Position = 0;
        var ident = br.ReadBytes(2);
        var count = br.ReadUInt16();

        int finalLengthOfs = 4 + (count * 4);
        if (br.BaseStream.Length < finalLengthOfs + 4)
            return string.Empty;
        br.BaseStream.Position = 4 + (count * 4);
        var len = br.ReadUInt32();
        if (len != br.BaseStream.Length)
            return string.Empty;
        return $"{(char)ident[0]}{(char)ident[1]}";
    }

    public static string GetIsMini(string path)
    {
        try
        {
            path = FileMitm.GetRedirectedWritePath(path);
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);
            return GetIsMini(br);
        }
        catch { return string.Empty; }
    }

    public static string GetIsMini(byte[] data)
    {
        try
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            return GetIsMini(br);
        }
        catch { return string.Empty; }
    }
}

/// <summary>
/// Utility class for writing BinLinker files.
/// </summary>
public static class BinLinkerWriter
{
    public static byte[] Compress<T>(T[] arr, ReadOnlySpan<byte> ident, Func<T, byte[]> sel, int padTo = 0)
    {
        // Convert th
        var result = new byte[arr.Length][];
        for (int i = 0; i < arr.Length; i++)
            result[i] = sel(arr[i]);
        return Write16(result, ident, padTo);
    }

    /// <summary>
    /// Writes to a new file with the given identifier.
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="identifier">Identifier for the file</param>
    /// <param name="padTo">Padding size for each file</param>
    /// <returns>Writeable byte array</returns>
    public static byte[] Write32(byte[][] data, ReadOnlySpan<byte> identifier, int padTo = 4)
    {
        using var ms = new MemoryStream(4096);
        using var bw = new BinaryWriter(ms);

        bw.Write(identifier[..2]);
        bw.Write((ushort)data.Length);
        const int start = 0;

        // Preallocate the offset map
        int count = data.Length;
        int dataOffset = 4 + ((count + 1) * sizeof(uint));
        for (int i = 0; i < count; i++)
            bw.Write((uint)0);
        bw.Write((uint)0);

        // Write each file, then update the offset map
        for (int i = 0; i < count; i++)
        {
            // Write File Offset
            var fileOffset = bw.BaseStream.Length - start;
            bw.Seek(start + 4 + (i * sizeof(uint)), SeekOrigin.Begin);
            bw.Write((uint)fileOffset);
            // Write File to Stream
            bw.Seek(0, SeekOrigin.End);
            bw.Write(data[i]);
            if (padTo != 0)
            {
                while ((ms.Position - start) % padTo != 0)
                    bw.Write((byte)0);
            }
        }

        // Cap the File
        {
            var fileOffset = bw.BaseStream.Length - start;
            bw.Seek(start + 4 + (count * sizeof(uint)), SeekOrigin.Begin);
            bw.Write((uint)fileOffset);
        }

        // Return the byte array
        return ms.ToArray();
    }

    /// <inheritdoc cref="Write32(byte[][], ReadOnlySpan{byte}, int)"/>
    public static byte[] Write16(byte[][] data, ReadOnlySpan<byte> identifier, int padTo = 2)
    {
        using var ms = new MemoryStream(4096);
        using var bw = new BinaryWriter(ms);
        const int start = 0;

        bw.Write(identifier[..2]);
        bw.Write((ushort)data.Length);

        // Preallocate the offset map
        int count = data.Length;
        int dataOffset = 4 + ((count + 1) * sizeof(ushort));
        for (int i = 0; i < count; i++)
            bw.Write((ushort)0);
        bw.Write((ushort)0);

        // Write each file, then update the offset map
        for (int i = 0; i < count; i++)
        {
            // Write File Offset
            var fileOffset = bw.BaseStream.Length - start;
            bw.Seek(start + 4 + (i * sizeof(ushort)), SeekOrigin.Begin);
            bw.Write((ushort)fileOffset);
            // Write File to Stream
            bw.Seek(0, SeekOrigin.End);
            bw.Write(data[i]);
            if (padTo != 0)
            {
                while ((ms.Position - start) % padTo != 0)
                    bw.Write((byte)0);
            }
        }

        // Cap the File
        {
            var fileOffset = bw.BaseStream.Length - start;
            bw.Seek(start + 4 + (count * sizeof(ushort)), SeekOrigin.Begin);
            bw.Write((ushort)fileOffset);
        }

        // Return the byte array
        return ms.ToArray();
    }
}
