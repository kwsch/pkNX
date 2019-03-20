using System;
using System.IO;

namespace pkNX.Containers
{
    public static class Container
    {
        /// <summary>
        /// Gets a new <see cref="IFileContainer"/> for the provided <see cref="path"/> and type.
        /// </summary>
        /// <param name="path">File location</param>
        /// <param name="t">File type</param>
        public static IFileContainer GetContainer(string path, ContainerType t)
        {
            switch (t)
            {
                case ContainerType.GARC: return new GARC(path);
                case ContainerType.Mini: return MiniUtil.GetMini(path);
                case ContainerType.SARC: return new SARC(path);
                case ContainerType.Folder: return new FolderContainer(path);
                case ContainerType.SingleFile: return new SingleFileContainer(path);
                case ContainerType.GFPack: return new GFPack(path);
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }

        /// <summary>
        /// Gets a <see cref="IFileContainer"/> for the stream.
        /// </summary>
        /// <param name="path">Path to the binary data</param>
        public static IFileContainer GetContainer(string path)
        {
            var fs = new FileStream(path, FileMode.Open);
            var container = GetContainer(fs);
            if (!(container is LargeContainer)) // not kept
                fs.Dispose();
            if (container == null)
                return null;

            container.FilePath = path;
            return container;
        }

        /// <summary>
        /// Gets a <see cref="IFileContainer"/> for the stream.
        /// </summary>
        /// <param name="stream">Stream for the binary data</param>
        public static IFileContainer GetContainer(Stream stream)
        {
            var br = new BinaryReader(stream);
            var container = GetContainer(br);
            if (!(container is LargeContainer)) // not kept
                br.Dispose();
            return container;
        }

        /// <summary>
        /// Gets a <see cref="IFileContainer"/> for the stream within the <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">Reader for the binary data</param>
        public static IFileContainer GetContainer(BinaryReader br)
        {
            IFileContainer container;
            if ((container = GARC.GetGARC(br)) != null)
                return container;
            if ((container = MiniUtil.GetMini(br)) != null)
                return container;
            if ((container = SARC.GetSARC(br)) != null)
                return container;
            return null;
        }
    }
}
