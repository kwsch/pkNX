using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using pkNX.Containers;

namespace pkNX
{
    /// <summary>
    /// More complex container format for large long lived archives.
    /// </summary>
    public abstract class LargeContainer : IDisposable, IFileContainer
    {
        public virtual int Count => Files.Length;

        public Task<byte[][]> GetFiles() => new Task<byte[][]>(() => { CacheAll(); return Files; });
        public Task<byte[]> GetFile(int file, int subFile = 0) => new Task<byte[]>(() => GetEntry(file, subFile));
        public Task SetFile(int file, byte[] value, int subFile = 0) => new Task(() => SetEntry(file, value, subFile));

        public string Extension => Path.GetExtension(FilePath);
        public string FileName => Path.GetFileName(FilePath);
        public string FilePath { get; set; }
        public bool Modified { get; set; }

        protected byte[][] Files { get; set; }

        /// <summary>
        /// Packs the <see cref="LargeContainer"/> to the specified writing stream.
        /// </summary>
        /// <param name="bw">Stream Writer to write contents to.</param>
        /// <param name="handler">Manager for monitoring progress.</param>
        /// <param name="token">Cancellation object</param>
        /// <returns>Awaitable Task</returns>
        protected abstract Task Pack(BinaryWriter bw, ContainerHandler handler, CancellationToken token);

        #region File Reading

        protected BinaryReader Reader { get; private set; }
        private Stream Stream;

        protected void OpenBinary(string path)
        {
            path = FileMitm.GetRedirectedReadPath(path);
            Stream = new FileStream(path, FileMode.Open);
            Reader = new BinaryReader(Stream);
            Initialize();
        }

        protected void OpenRead(BinaryReader br)
        {
            Reader = br;
            Stream = br.BaseStream;
            Initialize();
        }

        protected abstract void Initialize();

        protected abstract int GetFileOffset(int file, int subFile = 0);

        public BinaryReader Seek(int file, long offset = 0, int subFile = 0)
        {
            offset += GetFileOffset(file, subFile);
            Reader.BaseStream.Position = offset;
            return Reader;
        }

        public abstract byte[] GetEntry(int index, int subFile);

        public virtual void SetEntry(int index, byte[] value, int subFile)
        {
            Files[index] = value;
            Modified |= value != null && !this[index].SequenceEqual(value);
        }

        private byte[] GetCachedValue(int i, int subFile)
        {
            return Files[i] ??= GetEntry(i, subFile);
        }

        #endregion

        public byte[] this[int index]
        {
            get => (byte[]) GetCachedValue(index, 0).Clone();
            set => SetEntry(index, value, 0);
        }

        public void CacheAll()
        {
            for (int i = 0; i < Files.Length; i++)
            {
                if (Files[i] == null)
                {
                    Files[i] = GetCachedValue(i, 0);
                }
            }

            Reader = null;
            Stream.Close();
        }

        public void CancelEdits()
        {
            if (Reader == null)
                throw new ArgumentNullException(nameof(Reader));

            for (int i = 0; i < Files.Length; i++)
                Files[i] = null;

            Modified = false;
        }

        public abstract void Dump(string path, ContainerHandler handler);

        public async Task SaveAs(string path, ContainerHandler handler, CancellationToken token = new CancellationToken())
        {
            bool sameLocation = path == FilePath && Reader != null;
            var writePath = sameLocation ? Path.GetTempFileName() : path;

            path = FileMitm.GetRedirectedWritePath(path);
            var stream = new FileStream(path, FileMode.CreateNew);
            using (var bw = new BinaryWriter(stream))
                await Pack(bw, handler, token).ConfigureAwait(false);

            if (token.IsCancellationRequested)
            {
                stream.Close();
                File.Delete(path);
                return;
            }

            if (sameLocation && path != writePath)
            {
                if (File.Exists(path))
                    File.Delete(path);
                File.Move(writePath, path);
            }

            Stream = stream;
            Reader = new BinaryReader(Stream);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stream.Dispose();
            Reader.Dispose();
        }
    }
}
