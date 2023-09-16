using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace pkNX.Containers.VFS;

public abstract class BaseArchive : IDisposable
{
    private ArchiveStreamWrapper ArchiveStreamWrapper { get; }

    /// <summary>
    /// The ArchiveMode that the archive was initialized with.
    /// </summary>
    public ArchiveOpenMode OpenMode { get; }

    private readonly List<BaseArchiveEntry> _entries = new();
    private readonly ReadOnlyCollection<BaseArchiveEntry> _entriesCollection;
    private readonly Dictionary<ulong, BaseArchiveEntry> _entriesDictionary = new();

    private BaseArchiveEntry? _archiveStreamOwner;
    private bool _readEntries;
    private readonly bool _leaveOpen;
    private bool _isDisposed;
    protected long _expectedNumberOfEntries = -1; // todo

    internal BinaryReader? ArchiveReader { get; }

    internal Stream ArchiveStream => ArchiveStreamWrapper.Stream;

    /// <summary>
    /// Initializes a new instance of archive on the given stream in the specified mode, specifying whether to leave the stream open.
    /// </summary>
    /// <exception cref="ArgumentException">The stream is already closed. -or- mode is incompatible with the capabilities of the stream.</exception>
    /// <exception cref="ArgumentNullException">The stream is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">mode specified an invalid value.</exception>
    /// <exception cref="InvalidDataException">The contents of the stream could not be interpreted as a Zip file. -or- mode is Update and an entry is missing from the archive or is corrupt and cannot be read. -or- mode is Update and an entry is too large to fit into memory.</exception>
    /// <param name="stream">The input or output stream.</param>
    /// <param name="openMode">See the description of the ArchiveMode enum. Read requires the stream to support reading, Create requires the stream to support writing, and Update requires the stream to support reading, writing, and seeking.</param>
    /// <param name="leaveOpen">true to leave the stream open upon disposing the archive, otherwise false.</param>
    protected BaseArchive(Stream stream, ArchiveOpenMode openMode = ArchiveOpenMode.Read, bool leaveOpen = false)
    {
        ArchiveStreamWrapper = new ArchiveStreamWrapper(stream, openMode, leaveOpen);

        OpenMode = openMode;
        _entriesCollection = new(_entries);
        _leaveOpen = leaveOpen;

        if (openMode != ArchiveOpenMode.Create)
            ArchiveReader = new BinaryReader(ArchiveStream);

        // TODO: assert Initialize has been called
    }

    protected void Initialize()
    {
        switch (OpenMode)
        {
            case ArchiveOpenMode.Create:
                _readEntries = true;
                break;
            case ArchiveOpenMode.Read:
                ReadHeader();
                break;
            case ArchiveOpenMode.Update:
            default:
                if (ArchiveStream.Length == 0)
                {
                    _readEntries = true;
                }
                else
                {
                    ReadHeader();
                    EnsureDirectoryRead();

                    foreach (BaseArchiveEntry entry in _entries)
                        entry.ThrowIfCantOpen(false);
                }
                break;
        }
    }

    /// <summary>
    /// The collection of entries that are currently in the archive. This may not accurately represent the actual entries that are present in the underlying file or stream.
    /// </summary>
    /// <exception cref="NotSupportedException">The archive does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">The archive has already been closed.</exception>
    /// <exception cref="InvalidDataException">The Zip archive is corrupt and the entries cannot be retrieved.</exception>
    public ReadOnlyCollection<BaseArchiveEntry> Entries
    {
        get
        {
            if (OpenMode == ArchiveOpenMode.Create)
                throw new NotSupportedException("Cannot read archive entries when archive is opened in create mode.");

            ThrowIfDisposed();

            EnsureDirectoryRead();
            return _entriesCollection;
        }
    }

    /// <summary>
    /// Creates an empty entry in the Zip archive with the specified entry name. There are no restrictions on the names of entries. The last write time of the entry is set to the current time. If an entry with the specified name already exists in the archive, a second entry will be created that has an identical name.
    /// </summary>
    /// <exception cref="ArgumentException">entryName is a zero-length string.</exception>
    /// <exception cref="ArgumentNullException">entryName is null.</exception>
    /// <exception cref="NotSupportedException">The archive does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">The archive has already been closed.</exception>
    /// <param name="entryName">A path relative to the root of the archive, indicating the name of the entry to be created.</param>
    /// <param name="compressionLevel">The level of the compression (speed/memory vs. compressed size trade-off).</param>
    /// <returns>A wrapper for the newly created file entry in the archive.</returns>
    public abstract BaseArchiveEntry CreateEntry(string entryName, int compressionLevel = 9);

    protected void ThrowIfCantCreateEntry(string entryPath)
    {
        if (string.IsNullOrEmpty(entryPath))
            throw new ArgumentException("Entry must have a name", nameof(entryPath));

        if (OpenMode == ArchiveOpenMode.Read)
            throw new NotSupportedException("Cannot create new archive entries when archive is opened in read mode.");

        ThrowIfDisposed();
    }

    public abstract ulong GetUniqueIdentifierFromPath(string entryPath);

    /// <summary>
    /// Retrieves a wrapper for the file entry in the archive with the specified name. Names are compared using ordinal comparison. If there are multiple entries in the archive with the specified name, the first one found will be returned.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="entryPath"/> is a zero-length string.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="entryPath"/> is null.</exception>
    /// <exception cref="NotSupportedException">The archive does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">The archive has already been closed.</exception>
    /// <exception cref="InvalidDataException">The Zip archive is corrupt and the entries cannot be retrieved.</exception>
    /// <param name="entryPath">A path relative to the root of the archive, identifying the desired entry.</param>
    /// <returns>A wrapper for the file entry in the archive. If no entry in the archive exists with the specified name, null will be returned.</returns>
    public BaseArchiveEntry? GetEntry(string entryPath)
    {
        ArgumentNullException.ThrowIfNull(entryPath);

        if (OpenMode == ArchiveOpenMode.Create)
            throw new NotSupportedException("Cannot read archive entries when archive is opened in create mode.");

        EnsureDirectoryRead();
        ulong id = GetUniqueIdentifierFromPath(entryPath);
        _entriesDictionary.TryGetValue(id, out BaseArchiveEntry? result);
        return result;
    }

    protected void AddEntry(BaseArchiveEntry entry)
    {
        _entries.Add(entry);
        _entriesDictionary.TryAdd(entry.UniqueIdentifier, entry);
    }

    internal void RemoveEntry(BaseArchiveEntry entry)
    {
        _entries.Remove(entry);
        _entriesDictionary.Remove(entry.UniqueIdentifier);
    }


    [Conditional("DEBUG")]
    internal void DebugAssertIsStillArchiveStreamOwner(BaseArchiveEntry entry) => Debug.Assert(_archiveStreamOwner == entry);

    internal void AcquireArchiveStream(BaseArchiveEntry entry)
    {
        // if a previous entry had held the stream but never wrote anything, we write their local header for them
        if (_archiveStreamOwner != null)
        {
            if (!_archiveStreamOwner.EverOpenedForWrite)
            {
                _archiveStreamOwner.WriteAndFinishLocalEntry();
            }
            else
            {
                throw new IOException("CreateModeCreateEntryWhileOpen");
            }
        }

        _archiveStreamOwner = entry;
    }

    internal void ReleaseArchiveStream(BaseArchiveEntry entry)
    {
        Debug.Assert(_archiveStreamOwner == entry);

        _archiveStreamOwner = null;
    }

    internal void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    private void EnsureDirectoryRead()
    {
        if (_readEntries)
            return;

        try
        {
            // Assume read header was called
            ReadDirectory();
            Debug.Assert(_expectedNumberOfEntries != -1, "The number of expected entries should have been set when reading the header.");
            if (_entries.Count != _expectedNumberOfEntries)
                throw new InvalidDataException("Unexpected number of entries");
        }
        catch (EndOfStreamException ex)
        {
            throw new InvalidDataException("Unexpected end of stream", ex);
        }

        _readEntries = true;
    }

    protected abstract void ReadHeader();
    protected abstract void ReadDirectory();
    protected abstract void WriteFile();

    private void PrepareAndWriteFile()
    {
        // if we are in create mode, we always set readEntries to true in Init
        // if we are in update mode, we call EnsureCentralDirectoryRead, which sets readEntries to true
        Debug.Assert(_readEntries);

        if (OpenMode == ArchiveOpenMode.Update)
        {
            var markedForDelete = _entries.Where(entry => !entry.LoadCompressedBytesIfNeeded());
            foreach (BaseArchiveEntry entry in markedForDelete)
                entry.Delete();

            ArchiveStream.Seek(0, SeekOrigin.Begin);
            ArchiveStream.SetLength(0);
        }

        WriteFile();
    }


    /// <summary>
    /// Finishes writing the archive and releases all resources used by the archive object, unless the object was constructed with leaveOpen as true. Any streams from opened entries in the archive still open will throw exceptions on subsequent writes, as the underlying streams will have been closed.
    /// </summary>
    public void Dispose()
    {
        try
        {
            switch (OpenMode)
            {
                case ArchiveOpenMode.Read:
                    break;
                case ArchiveOpenMode.Create:
                case ArchiveOpenMode.Update:
                default:
                    Debug.Assert(OpenMode is ArchiveOpenMode.Update or ArchiveOpenMode.Create);
                    PrepareAndWriteFile();
                    break;
            }
        }
        finally
        {
            CloseStreams();
            _isDisposed = true;
        }
        GC.SuppressFinalize(this);
    }

    private void CloseStreams()
    {
        if (!_leaveOpen)
            ArchiveReader?.Dispose();

        ArchiveStreamWrapper.Dispose();
    }
}
