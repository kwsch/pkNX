using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace pkNX.IO.Archives;

public interface IArchive
{
    /// <summary>
    /// The ArchiveMode that the archive was initialized with.
    /// </summary>
    ArchiveOpenMode OpenMode { get; }

    BaseArchiveEntry CreateEntry(string entryName, int compressionLevel = 9);
}

public class BaseArchive : IDisposable
{
    private ArchiveStreamWrapper ArchiveStreamWrapper { get; }

    /// <summary>
    /// The ArchiveMode that the archive was initialized with.
    /// </summary>
    public ArchiveOpenMode OpenMode { get; }

    private readonly List<BaseArchiveEntry> _entries = new();
    private readonly ReadOnlyCollection<BaseArchiveEntry> _entriesCollection;
    private readonly Dictionary<string, BaseArchiveEntry> _entriesDictionary = new();

    private BaseArchiveEntry? _archiveStreamOwner;
    private bool _readEntries;
    private readonly bool _leaveOpen;
    private long _centralDirectoryStart; //only valid after ReadCentralDirectory
    private bool _isDisposed;
    private uint _numberOfThisDisk; //only valid after ReadCentralDirectory
    private long _expectedNumberOfEntries;

    internal BinaryReader? ArchiveReader { get; }

    internal Stream ArchiveStream => ArchiveStreamWrapper.Stream;
    internal uint NumberOfThisDisk => _numberOfThisDisk;

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
    public BaseArchive(Stream stream, ArchiveOpenMode openMode = ArchiveOpenMode.Read, bool leaveOpen = false)
    {
        ArchiveStreamWrapper = new ArchiveStreamWrapper(stream, openMode, leaveOpen);

        OpenMode = openMode;
        _entriesCollection = new(_entries);
        _leaveOpen = leaveOpen;

        switch (openMode)
        {
            case ArchiveOpenMode.Create:
                _readEntries = true;
                break;
            case ArchiveOpenMode.Read:
                ArchiveReader = new BinaryReader(ArchiveStream);

                ReadEndOfCentralDirectory();
                break;
            case ArchiveOpenMode.Update:
            default:
                ArchiveReader = new BinaryReader(ArchiveStream);

                if (ArchiveStream.Length == 0)
                {
                    _readEntries = true;
                }
                else
                {
                    ReadEndOfCentralDirectory();
                    EnsureCentralDirectoryRead();

                    foreach (BaseArchiveEntry entry in _entries)
                        entry.ThrowIfNotOpenable(false, true);
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

            EnsureCentralDirectoryRead();
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
    public BaseArchiveEntry CreateEntry(string entryName, int compressionLevel = 9)
    {
        if (string.IsNullOrEmpty(entryName))
            throw new ArgumentException(SR.CannotBeEmpty, nameof(entryName));

        if (OpenMode == ArchiveOpenMode.Read)
            throw new NotSupportedException("Cannot create new archive entries when archive is opened in read mode.");

        ThrowIfDisposed();


        BaseArchiveEntry entry = new BaseArchiveEntry(this, entryName, compressionLevel);

        AddEntry(entry);

        return entry;
    }

    /// <summary>
    /// Retrieves a wrapper for the file entry in the archive with the specified name. Names are compared using ordinal comparison. If there are multiple entries in the archive with the specified name, the first one found will be returned.
    /// </summary>
    /// <exception cref="ArgumentException">entryName is a zero-length string.</exception>
    /// <exception cref="ArgumentNullException">entryName is null.</exception>
    /// <exception cref="NotSupportedException">The archive does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">The archive has already been closed.</exception>
    /// <exception cref="InvalidDataException">The Zip archive is corrupt and the entries cannot be retrieved.</exception>
    /// <param name="entryName">A path relative to the root of the archive, identifying the desired entry.</param>
    /// <returns>A wrapper for the file entry in the archive. If no entry in the archive exists with the specified name, null will be returned.</returns>
    public BaseArchiveEntry? GetEntry(string entryName)
    {
        ArgumentNullException.ThrowIfNull(entryName);

        if (OpenMode == ArchiveOpenMode.Create)
            throw new NotSupportedException("Cannot read archive entries when archive is opened in create mode.");

        EnsureCentralDirectoryRead();
        _entriesDictionary.TryGetValue(entryName, out BaseArchiveEntry? result);
        return result;
    }

    private void AddEntry(BaseArchiveEntry entry)
    {
        _entries.Add(entry);
        _entriesDictionary.TryAdd(entry.FullName, entry);
    }

    internal void RemoveEntry(BaseArchiveEntry entry)
    {
        _entries.Remove(entry);
        _entriesDictionary.Remove(entry.FullName);
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
                throw new IOException(SR.CreateModeCreateEntryWhileOpen);
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

    private void EnsureCentralDirectoryRead()
    {
        if (!_readEntries)
        {
            ReadCentralDirectory();
            _readEntries = true;
        }
    }

    private void ReadCentralDirectory()
    {
        try
        {
            // assume ReadEndOfCentralDirectory has been called and has populated _centralDirectoryStart

            ArchiveStream.Seek(_centralDirectoryStart, SeekOrigin.Begin);

            long numberOfEntries = 0;

            Debug.Assert(ArchiveReader != null);
            //read the central directory
            ZipCentralDirectoryFileHeader currentHeader;
            bool saveExtraFieldsAndComments = OpenMode == ArchiveOpenMode.Update;
            while (ZipCentralDirectoryFileHeader.TryReadBlock(ArchiveReader,
                                                    saveExtraFieldsAndComments, out currentHeader))
            {
                AddEntry(new BaseArchiveEntry(this, currentHeader));
                numberOfEntries++;
            }

            if (numberOfEntries != _expectedNumberOfEntries)
                throw new InvalidDataException(SR.NumEntriesWrong);
        }
        catch (EndOfStreamException ex)
        {
            throw new InvalidDataException(SR.Format(SR.CentralDirectoryInvalid, ex));
        }
    }

    // This function reads all the EOCD stuff it needs to find the offset to the start of the central directory
    // This offset gets put in _centralDirectoryStart and the number of this disk gets put in _numberOfThisDisk
    // Also does some verification that this isn't a split/spanned archive
    // Also checks that offset to CD isn't out of bounds
    private void ReadEndOfCentralDirectory()
    {
        try
        {
            // This seeks backwards almost to the beginning of the EOCD, one byte after where the signature would be
            // located if the EOCD had the minimum possible size (no file zip comment)
            ArchiveStream.Seek(-ZipEndOfCentralDirectoryBlock.SizeOfBlockWithoutSignature, SeekOrigin.End);

            // If the EOCD has the minimum possible size (no zip file comment), then exactly the previous 4 bytes will contain the signature
            // But if the EOCD has max possible size, the signature should be found somewhere in the previous 64K + 4 bytes
            if (!ZipHelper.SeekBackwardsToSignature(ArchiveStream,
                    ZipEndOfCentralDirectoryBlock.SignatureConstant,
                    ZipEndOfCentralDirectoryBlock.ZipFileCommentMaxLength + ZipEndOfCentralDirectoryBlock.SignatureSize))
                throw new InvalidDataException(SR.EOCDNotFound);

            long eocdStart = ArchiveStream.Position;

            Debug.Assert(ArchiveReader != null);
            // read the EOCD
            ZipEndOfCentralDirectoryBlock eocd;
            bool eocdProper = ZipEndOfCentralDirectoryBlock.TryReadBlock(ArchiveReader, out eocd);
            Debug.Assert(eocdProper); // we just found this using the signature finder, so it should be okay

            if (eocd.NumberOfThisDisk != eocd.NumberOfTheDiskWithTheStartOfTheCentralDirectory)
                throw new InvalidDataException(SR.SplitSpanned);

            _numberOfThisDisk = eocd.NumberOfThisDisk;
            _centralDirectoryStart = eocd.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;

            if (eocd.NumberOfEntriesInTheCentralDirectory != eocd.NumberOfEntriesInTheCentralDirectoryOnThisDisk)
                throw new InvalidDataException(SR.SplitSpanned);

            _expectedNumberOfEntries = eocd.NumberOfEntriesInTheCentralDirectory;

            _archiveComment = eocd.ArchiveComment;

            TryReadZip64EndOfCentralDirectory(eocd, eocdStart);

            if (_centralDirectoryStart > ArchiveStream.Length)
            {
                throw new InvalidDataException(SR.FieldTooBigOffsetToCD);
            }
        }
        catch (EndOfStreamException ex)
        {
            throw new InvalidDataException(SR.CDCorrupt, ex);
        }
        catch (IOException ex)
        {
            throw new InvalidDataException(SR.CDCorrupt, ex);
        }
    }

    // Tries to find the Zip64 End of Central Directory Locator, then the Zip64 End of Central Directory, assuming the
    // End of Central Directory block has already been found, as well as the location in the stream where the EOCD starts.
    private void TryReadZip64EndOfCentralDirectory(ZipEndOfCentralDirectoryBlock eocd, long eocdStart)
    {
        // Only bother looking for the Zip64-EOCD stuff if we suspect it is needed because some value is FFFFFFFFF
        // because these are the only two values we need, we only worry about these
        // if we don't find the Zip64-EOCD, we just give up and try to use the original values
        if (eocd.NumberOfThisDisk == ZipHelper.Mask16Bit ||
            eocd.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber == ZipHelper.Mask32Bit ||
            eocd.NumberOfEntriesInTheCentralDirectory == ZipHelper.Mask16Bit)
        {
            // Read Zip64 End of Central Directory Locator

            // This seeks forwards almost to the beginning of the Zip64-EOCDL, one byte after where the signature would be located
            ArchiveStream.Seek(eocdStart - Zip64EndOfCentralDirectoryLocator.SizeOfBlockWithoutSignature, SeekOrigin.Begin);

            // Exactly the previous 4 bytes should contain the Zip64-EOCDL signature
            // if we don't find it, assume it doesn't exist and use data from normal EOCD
            if (ZipHelper.SeekBackwardsToSignature(ArchiveStream,
                    Zip64EndOfCentralDirectoryLocator.SignatureConstant,
                    Zip64EndOfCentralDirectoryLocator.SignatureSize))
            {
                Debug.Assert(ArchiveReader != null);

                // use locator to get to Zip64-EOCD
                Zip64EndOfCentralDirectoryLocator locator;
                bool zip64eocdLocatorProper = Zip64EndOfCentralDirectoryLocator.TryReadBlock(ArchiveReader, out locator);
                Debug.Assert(zip64eocdLocatorProper); // we just found this using the signature finder, so it should be okay

                if (locator.OffsetOfZip64EOCD > long.MaxValue)
                    throw new InvalidDataException(SR.FieldTooBigOffsetToZip64EOCD);

                long zip64EOCDOffset = (long)locator.OffsetOfZip64EOCD;

                ArchiveStream.Seek(zip64EOCDOffset, SeekOrigin.Begin);

                // Read Zip64 End of Central Directory Record

                Zip64EndOfCentralDirectoryRecord record;
                if (!Zip64EndOfCentralDirectoryRecord.TryReadBlock(ArchiveReader, out record))
                    throw new InvalidDataException(SR.Zip64EOCDNotWhereExpected);

                _numberOfThisDisk = record.NumberOfThisDisk;

                if (record.NumberOfEntriesTotal > long.MaxValue)
                    throw new InvalidDataException(SR.FieldTooBigNumEntries);

                if (record.OffsetOfCentralDirectory > long.MaxValue)
                    throw new InvalidDataException(SR.FieldTooBigOffsetToCD);

                if (record.NumberOfEntriesTotal != record.NumberOfEntriesOnThisDisk)
                    throw new InvalidDataException(SR.SplitSpanned);

                _expectedNumberOfEntries = (long)record.NumberOfEntriesTotal;
                _centralDirectoryStart = (long)record.OffsetOfCentralDirectory;
            }
        }
    }

    private void WriteFile()
    {
        // if we are in create mode, we always set readEntries to true in Init
        // if we are in update mode, we call EnsureCentralDirectoryRead, which sets readEntries to true
        Debug.Assert(_readEntries);

        if (OpenMode == ArchiveOpenMode.Update)
        {
            List<BaseArchiveEntry> markedForDelete = new List<BaseArchiveEntry>();
            foreach (BaseArchiveEntry entry in _entries)
            {
                if (!entry.LoadLocalHeaderExtraFieldAndCompressedBytesIfNeeded())
                    markedForDelete.Add(entry);
            }
            foreach (BaseArchiveEntry entry in markedForDelete)
                entry.Delete();

            ArchiveStream.Seek(0, SeekOrigin.Begin);
            ArchiveStream.SetLength(0);
        }

        foreach (BaseArchiveEntry entry in _entries)
        {
            entry.WriteAndFinishLocalEntry();
        }

        long startOfCentralDirectory = ArchiveStream.Position;

        foreach (BaseArchiveEntry entry in _entries)
        {
            entry.WriteCentralDirectoryFileHeader();
        }

        long sizeOfCentralDirectory = ArchiveStream.Position - startOfCentralDirectory;

        WriteArchiveEpilogue(startOfCentralDirectory, sizeOfCentralDirectory);
    }

    // writes eocd, and if needed, zip 64 eocd, zip64 eocd locator
    // should only throw an exception in extremely exceptional cases because it is called from dispose
    private void WriteArchiveEpilogue(long startOfCentralDirectory, long sizeOfCentralDirectory)
    {
        // determine if we need Zip 64
        if (startOfCentralDirectory >= uint.MaxValue
            || sizeOfCentralDirectory >= uint.MaxValue
            || _entries.Count >= ZipHelper.Mask16Bit
#if DEBUG_FORCE_ZIP64
                || _forceZip64
#endif
            )
        {
            // if we need zip 64, write zip 64 eocd and locator
            long zip64EOCDRecordStart = ArchiveStream.Position;
            Zip64EndOfCentralDirectoryRecord.WriteBlock(ArchiveStream, _entries.Count, startOfCentralDirectory, sizeOfCentralDirectory);
            Zip64EndOfCentralDirectoryLocator.WriteBlock(ArchiveStream, zip64EOCDRecordStart);
        }

        // write normal eocd
        ZipEndOfCentralDirectoryBlock.WriteBlock(ArchiveStream, _entries.Count, startOfCentralDirectory, sizeOfCentralDirectory, _archiveComment);
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
                    WriteFile();
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
