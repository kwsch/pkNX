using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Containers.VFS;

public abstract class BaseArchiveEntry
{
    // Initializes a ZipArchiveEntry instance for an existing archive entry.
    internal BaseArchiveEntry(BaseArchive archive, ulong uniqueIdentifier)
    {
        Archive = archive;
        UniqueIdentifier = uniqueIdentifier;
        _originallyInArchive = true;
    }

    /// <summary>
    /// Deletes the entry from the archive.
    /// </summary>
    /// <exception cref="IOException">The entry is already open for reading or writing.</exception>
    /// <exception cref="NotSupportedException">The IArchive that this entry belongs to was opened in a mode other than ArchiveOpenMode.Update. </exception>
    /// <exception cref="ObjectDisposedException">The IArchive that this entry belongs to has been disposed.</exception>
    public void Delete()
    {
        if (_currentlyOpenForWrite)
            throw new IOException("Unable to delete the entry: It's already opened for write");

        if (Archive.OpenMode != ArchiveOpenMode.Update)
            throw new NotSupportedException("Delete is only supported in ArchiveOpenMode.Update mode");

        Archive.ThrowIfDisposed();

        Archive.RemoveEntry(this);
        Archive = null!;
        UnloadStreams();
    }

    /// <summary>
    /// Opens the entry. If the archive that the entry belongs to was opened in Read mode, the returned stream will be readable, and it may or may not be seekable. If Create mode, the returned stream will be writable and not seekable. If Update mode, the returned stream will be readable, writable, seekable, and support SetLength.
    /// </summary>
    /// <returns>A Stream that represents the contents of the entry.</returns>
    /// <exception cref="IOException">The entry is already currently open for writing. -or- The entry has been deleted from the archive. -or- The archive that this entry belongs to was opened in ArchiveMode.Create, and this entry has already been written to once.</exception>
    /// <exception cref="InvalidDataException">The entry is missing from the archive or is corrupt and cannot be read. -or- The entry has been compressed using a compression method that is not supported.</exception>
    /// <exception cref="ObjectDisposedException">The ZipArchive that this entry belongs to has been disposed.</exception>
    public Stream Open()
    {
        ThrowIfInvalidArchive();

        switch (Archive.OpenMode)
        {
            case ArchiveOpenMode.Read:
                ThrowIfCantOpen(true);
                return OpenInReadMode();
            case ArchiveOpenMode.Create:
                return OpenInWriteMode();
            case ArchiveOpenMode.Update:
            default:
                Debug.Assert(Archive.OpenMode == ArchiveOpenMode.Update);
                return OpenInUpdateMode();
        }
    }

    private Stream GetDataCompressor(Stream backingStream, bool leaveBackingStreamOpen, EventHandler? onClose)
    {
        // stream stack: backingStream -> DeflateStream -> CheckSumWriteStream

        // By default we compress with deflate, except if compression level is set to NoCompression then stored is used.
        // Stored is also used for empty files, but we don't actually call through this function for that - we just write the stored value in the header
        // Deflate64 is not supported on all platforms
        /*Debug.Assert(CompressionMethod == CompressionMethodValues.Deflate
            || CompressionMethod == CompressionMethodValues.Stored);

        bool isIntermediateStream = true;
        Stream compressorStream;
        switch (CompressionMethod)
        {
            case CompressionMethodValues.Stored:
                compressorStream = backingStream;
                isIntermediateStream = false;
                break;
            case CompressionMethodValues.Deflate:
            case CompressionMethodValues.Deflate64:
            default:
                compressorStream = new DeflateStream(backingStream, _compressionLevel ?? CompressionLevel.Optimal, leaveBackingStreamOpen);
                break;

        }
        bool leaveCompressorStreamOpenOnClose = leaveBackingStreamOpen && !isIntermediateStream;
        var checkSumStream = new CheckSumAndSizeWriteStream(
            compressorStream,
            backingStream,
            leaveCompressorStreamOpenOnClose,
            this,
            onClose,
            (long initialPosition, long currentPosition, uint checkSum, Stream backing, ZipArchiveEntry thisRef, EventHandler? closeHandler) =>
            {
                thisRef._crc32 = checkSum;
                thisRef._uncompressedSize = currentPosition;
                thisRef._compressedSize = backing.Position - initialPosition;
                closeHandler?.Invoke(thisRef, EventArgs.Empty);
            });

        return checkSumStream;*/
        return backingStream;
    }

    private Stream GetDataDecompressor(Stream compressedStreamToRead)
    {
        Stream? uncompressedStream;
        /*switch (CompressionMethod)
        {
            case CompressionMethodValues.Deflate:
                uncompressedStream = new DeflateStream(compressedStreamToRead, CompressionMode.Decompress, _uncompressedSize);
                break;
            case CompressionMethodValues.Stored:
            default:
                // we can assume that only deflate/deflate64/stored are allowed because we assume that
                // IsOpenable is checked before this function is called
                Debug.Assert(CompressionMethod == CompressionMethodValues.Stored);

                uncompressedStream = compressedStreamToRead;
                break;
        }*/

        uncompressedStream = compressedStreamToRead;
        return uncompressedStream;
    }

    private Stream OpenInReadMode()
    {
        Stream compressedStream = new SubReadStream(Archive.ArchiveStream, OffsetOfCompressedData, _compressedSize);
        return GetDataDecompressor(compressedStream);
    }

    private Stream OpenInWriteMode()
    {
        if (EverOpenedForWrite)
            throw new IOException("CreateModeWriteOnceAndOneEntryAtATime");

        // we assume that if another entry grabbed the archive stream, that it set this entry's _everOpenedForWrite property to true by calling WriteLocalFileHeaderIfNeeded
        Archive.DebugAssertIsStillArchiveStreamOwner(this);

        EverOpenedForWrite = true;
        Stream compressorStream = GetDataCompressor(Archive.ArchiveStream, true, (o, _) =>
        {
            // release the archive stream
            var entry = (BaseArchiveEntry)o!;
            entry.Archive.ReleaseArchiveStream(entry);
            entry._outstandingWriteStream = null;
        });
        _outstandingWriteStream = new DirectToArchiveWriterStream(compressorStream, this);

        return new WrappedStream(baseStream: _outstandingWriteStream, closeBaseStream: true);
    }

    private Stream OpenInUpdateMode()
    {
        if (_currentlyOpenForWrite)
            throw new IOException("UpdateModeOneStream");

        ThrowIfCantOpen(true);

        EverOpenedForWrite = true;
        _currentlyOpenForWrite = true;
        // always put it at the beginning for them
        UncompressedData.Seek(0, SeekOrigin.Begin);
        return new WrappedStream(UncompressedData, this, thisRef =>
        {
            // once they close, we know uncompressed length, but still not compressed length
            // so we don't fill in any size information
            // those fields get figured out when we call GetCompressor as we write it to
            // the actual archive
            thisRef!._currentlyOpenForWrite = false;
        });
    }

    // returns false if fails, will get called on every entry before closing in update mode
    // can throw InvalidDataException
    protected internal abstract bool LoadCompressedBytesIfNeeded();


    // does almost everything you need to do to forget about this entry
    // writes the local header/data, gets rid of all the data,
    // closes all of the streams except for the very outermost one that
    // the user holds on to and is responsible for closing
    //
    // after calling this, and only after calling this can we be guaranteed
    // that we are reading to write the central directory
    //
    // should only throw an exception in extremely exceptional cases because it is called from dispose
    internal abstract void WriteAndFinishLocalEntry();
    protected abstract void ThrowIfUnsupportedCompressionType();


    private void UnloadStreams()
    {
        _storedUncompressedData?.Dispose();
        _compressedBytes = null;
        _outstandingWriteStream = null;
    }

    private void CloseStreams()
    {
        // if the user left the stream open, close the underlying stream for them
        _outstandingWriteStream?.Dispose();
    }

    internal void ThrowIfCantOpen(bool needToDecompress)
    {
        if (!_originallyInArchive)
            return; // We can always open if it was a newly added entry

        if (needToDecompress)
            ThrowIfUnsupportedCompressionType();

        Debug.Assert(Archive.ArchiveReader != null);
        Archive.ArchiveStream.Seek(OffsetOfCompressedData, SeekOrigin.Begin);

        if (OffsetOfCompressedData + _compressedSize > Archive.ArchiveStream.Length)
            throw new InvalidDataException("Corrupted data detected! The specified offset + file size would read out of bounds.");
    }

    private void ThrowIfInvalidArchive()
    {
        if (Archive == null)
            throw new InvalidOperationException("Archive was null");
        Archive.ThrowIfDisposed();
    }

    /// <summary>
    /// The Archive that this entry belongs to. If this entry has been deleted, this will return null.
    /// </summary>
    public BaseArchive Archive { get; private set; }

    /// <summary>
    /// The unique identifier for this entry. Eg. a hash of the file path.
    /// </summary>
    public ulong UniqueIdentifier { get; set; }

    protected long OffsetOfCompressedData { get; set; }

    private MemoryStream UncompressedData
    {
        get
        {
            if (_storedUncompressedData != null)
                return _storedUncompressedData;

            // this means we have never opened it before

            // if _uncompressedSize > int.MaxValue, it's still okay, because MemoryStream will just
            // grow as data is copied into it
            _storedUncompressedData = new MemoryStream((int)_uncompressedSize);

            if (!_originallyInArchive)
                return _storedUncompressedData;

            using Stream decompressor = OpenInReadMode();
            try
            {

                /*Debug.Assert(Archive.ArchiveReader != null);
                Archive.ArchiveReader.BaseStream.Position = OffsetOfCompressedData;
                var bytes = Archive.ArchiveReader.ReadBytes((int)_compressedSize);
                _storedUncompressedData.Write(bytes, 0, bytes.Length);*/


                decompressor.CopyTo(_storedUncompressedData);
            }
            catch (InvalidDataException)
            {
                // this is the case where the archive say the entry is deflate, but deflateStream
                // throws an InvalidDataException. This property should only be getting accessed in
                // Update mode, so we want to make sure _storedUncompressedData stays null so
                // that later when we dispose the archive, this entry loads the compressedBytes, and
                // copies them straight over
                _storedUncompressedData.Dispose();
                _storedUncompressedData = null;
                _currentlyOpenForWrite = false;
                EverOpenedForWrite = false;
                throw;
            }

            return _storedUncompressedData;
        }
    }

    /// <summary>
    /// The compressed size of the entry. If the archive that the entry belongs to is in Create mode, attempts to get this property will always throw an exception. If the archive that the entry belongs to is in update mode, this property will only be valid if the entry has not been opened.
    /// </summary>
    /// <exception cref="InvalidOperationException">This property is not available because the entry has been written to or modified.</exception>
    public long CompressedLength
    {
        get
        {
            if (EverOpenedForWrite)
                throw new InvalidOperationException("Can't retrieve length after the entry has been written to");
            return _compressedSize;
        }
    }

    internal bool EverOpenedForWrite { get; private set; }

    private bool _currentlyOpenForWrite;
    private readonly bool _originallyInArchive;

    protected long _compressedSize;
    protected long _uncompressedSize;
    private byte[][]? _compressedBytes;
    private MemoryStream? _storedUncompressedData;
    private Stream? _outstandingWriteStream;
}
