using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class GARC : LargeContainer
    {
        private GARCHeader Header;
        private FATO FATO;
        private FATB FATB;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GARC(BinaryReader br) => OpenRead(br);
        public GARC(string file) => OpenBinary(file);
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public GARC(IReadOnlyList<string> files, GARCVersion version = GARCVersion.VER_6)
        {
            Header = new GARCHeader(version);
            FATO = new FATO(files.Count);
            FATB = new FATB(files);
            Files = new byte[]?[files.Count];
        }

        protected override void Initialize()
        {
            Reader!.BaseStream.Position = 0;
            Header = new GARCHeader(Reader);
            FATO = new FATO(Reader);
            FATB = new FATB(Reader, Header.DataOffset);
            Files = new byte[]?[FATO.EntryCount];
        }

        protected override int GetFileOffset(int file, int subFile = 0)
        {
            var f = FATB[file].SubEntries[subFile];
            return f.Start;
        }

        public override byte[] GetEntry(int index, int subFile)
        {
            var f = FATB[index].SubEntries[subFile];
            if (f.File is byte[] data)
                return data;

            data = f.GetFileData(Reader!.BaseStream);
            f.File = data; // cache for future fetches
            return data;
        }

        public override void SetEntry(int index, byte[]? value, int subFile)
        {
            Modified |= value != null && !GetEntry(index, subFile).SequenceEqual(value);
            var f = FATB[index].SubEntries[subFile];
            f.File = value;
        }

        protected override Task Pack(BinaryWriter bw, ContainerHandler handler, CancellationToken token)
        {
            return Task.Run(() => PackGARC(bw, handler, token), token);
        }

        private void PackGARC(BinaryWriter bw, ContainerHandler handler, CancellationToken token)
        {
            StartPack();
            handler.Initialize(Count);

            WriteIntro(bw);
            int dataOffset = (int)bw.BaseStream.Position;
            for (int i = 0; i < Count; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                WriteEntry(bw, i, dataOffset);
            }
            WriteIntro(bw, true);
            bw.Flush();
        }

        private void StartPack()
        {
            Header.ContentLargestUnpadded = 0;
            Header.ContentLargestPadded = 0;
        }

        private void WriteIntro(BinaryWriter bw, bool lastPass = false)
        {
            Header.Write(bw);
            FATO.Write(bw);
            FATB.Write(bw);
            if (lastPass)
                Header.DataOffset = (int)bw.BaseStream.Position;
        }

        private void WriteEntry(BinaryWriter bw, int i, int DataOffset)
        {
            FATO[i].Offset = (uint)bw.BaseStream.Position;
            FATB[i].WriteEntries(bw, Reader!.BaseStream, Header.ContentPadToNearest, DataOffset,
                ref Header.ContentLargestUnpadded, ref Header.ContentLargestPadded);
        }

        public override void Dump(string path, ContainerHandler handler)
        {
            string format = this.GetFileFormatString();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            handler.Initialize(Count);
            for (int i = 0; i < Count; i++)
            {
                FATB[i].Dump(path, i, format, Reader!.BaseStream, Header.DataOffset);
                handler.StepFile(i+1);
            }
        }

        public static GARC? GetGARC(BinaryReader br)
        {
            if (br.BaseStream.Length < 20)
                return null;
            br.BaseStream.Position = 0;
            if (br.ReadUInt32() != GARCHeader.MAGIC)
                return null;
            br.BaseStream.Position = 10;
            var ver = br.ReadUInt16();

            if (ver is 0x0600 or 0x0400)
                return new GARC(br);
            return null;
        }
    }
}
