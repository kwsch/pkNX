using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using pkNX.Containers;

namespace pkNX.WinForms
{
    public enum RipResultCode
    {
        UnknownFormat,
        Success,
        FileExist,
        BadSize,
        ReadError,
    }

    public static class FileRipper
    {
        public static ContainerHandler DefaultHandler { private get; set; } = new ContainerHandler();

        public static readonly List<Func<BinaryReader, uint, string, ContainerHandler, FileRipperResult>> Loaders =
            new List<Func<BinaryReader, uint, string, ContainerHandler, FileRipperResult>>
            {
                GFPackDump,
                NSODump,
            };

        private static FileRipperResult NSODump(BinaryReader br, uint header, string path, ContainerHandler handler)
        {
            if (header != NSOHeader.ExpectedMagic)
                return new FileRipperResult(RipResultCode.UnknownFormat);

            br.BaseStream.Position = 0;
            var nso = new NSO(br);

            var dir = Path.GetDirectoryName(path);
            var folder = Path.GetFileNameWithoutExtension(path);
            var resultPath = Path.Combine(dir, folder);

            if (resultPath == path)
                resultPath += "_nso";
            Directory.CreateDirectory(resultPath);

            File.WriteAllBytes(Path.Combine(resultPath, "text.bin"), nso.DecompressedText);
            File.WriteAllBytes(Path.Combine(resultPath, "data.bin"), nso.DecompressedData);
            File.WriteAllBytes(Path.Combine(resultPath, "ro.bin"), nso.DecompressedRO);

            return new FileRipperResult(RipResultCode.Success) { ResultPath = resultPath };
        }

        private static FileRipperResult GFPackDump(BinaryReader br, uint header, string path, ContainerHandler handler)
        {
            if (header != 0x584C_4647)
                return new FileRipperResult(RipResultCode.UnknownFormat);

            br.BaseStream.Position = 0;
            var gfp = new GFPack(br);

            var dir = Path.GetDirectoryName(path);
            var folder = Path.GetFileNameWithoutExtension(path);
            var resultPath = Path.Combine(dir, folder);
            Directory.CreateDirectory(resultPath);

            gfp.Dump(resultPath, handler);

            return new FileRipperResult(RipResultCode.Success) {ResultPath = resultPath};
        }

        public static FileRipperResult TryOpenFile(string path, ContainerHandler handler = null)
        {
            if (!File.Exists(path))
                return new FileRipperResult(RipResultCode.FileExist);

            if (handler == null)
                handler = DefaultHandler;

            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);
                if (br.BaseStream.Length < 4)
                    return new FileRipperResult(RipResultCode.BadSize);
                var header = br.ReadUInt32();
                var result = TryLoadFile(br, header, path, handler);
                if (result.Code == RipResultCode.Success)
                    return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new FileRipperResult(RipResultCode.ReadError);
            }
            return new FileRipperResult(RipResultCode.UnknownFormat);
        }

        private static FileRipperResult TryLoadFile(BinaryReader br, uint header, string path, ContainerHandler handler)
        {
            foreach (var method in Loaders)
            {
                try
                {
                    var result = method(br, header, path, handler);
                    if (result.Code == RipResultCode.Success)
                        return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return new FileRipperResult(RipResultCode.UnknownFormat);
        }
    }

    public class FileRipperResult
    {
        public readonly RipResultCode Code;
        public string ResultPath;

        public FileRipperResult(RipResultCode code) => Code = code;
    }
}
