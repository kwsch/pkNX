using System;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    /// <summary>
    /// Array of Files
    /// </summary>
    public interface IFileContainer
    {
        /// <summary>
        /// Path the <see cref="IFileContainer"/> was loaded from.
        /// </summary>
        string? FilePath { get; set; }

        /// <summary>
        /// Indication if the contents of the <see cref="IFileContainer"/> have been modified.
        /// </summary>
        bool Modified { get; set; }

        /// <summary>
        /// Count of files inside the <see cref="IFileContainer"/>.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// File access for individually indexed files.
        /// </summary>
        /// <param name="index">File number to fetch</param>
        /// <returns>Data representing the file at the specified index.</returns>
        byte[] this[int index] { get; set; }

        Task<byte[][]> GetFiles();
        Task<byte[]> GetFile(int file, int subFile = 0);
        Task SetFile(int file, byte[] value, int subFile = 0);
        Task SaveAs(string path, ContainerHandler handler, CancellationToken token);

        void Dump(string path, ContainerHandler handler);
        void CancelEdits();
    }

    public static class FileContainerExtensions
    {
        public static string GetFileFormatString(this IFileContainer c) => "D" + Math.Ceiling(Math.Log10(c.Count));
    }
}
