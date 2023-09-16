using System;
using System.Diagnostics;
using System.IO;

namespace pkNX.Containers.VFS;

public class ArchiveStreamWrapper : IDisposable
{
    /// <summary>
    /// The ArchiveMode that the archive was initialized with.
    /// </summary>
    public ArchiveOpenMode OpenMode { get; }
    public Stream Stream { get; }

    public event Action<ArchiveStreamWrapper>? OnStreamClosing;

    private readonly bool _leaveOpen;
    private readonly Stream? _backingStream;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of archive on the given stream in the specified mode, specifying whether to leave the stream open.
    /// </summary>
    /// <exception cref="ArgumentException">The stream is already closed. -or- mode is incompatible with the capabilities of the stream.</exception>
    /// <exception cref="ArgumentNullException">The stream is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">mode specified an invalid value.</exception>
    /// <param name="stream">The input or output stream.</param>
    /// <param name="openMode">See the description of the ArchiveMode enum. Read requires the stream to support reading, Create requires the stream to support writing, and Update requires the stream to support reading, writing, and seeking.</param>
    /// <param name="leaveOpen">true to leave the stream open upon disposing the archive, otherwise false.</param>
    public ArchiveStreamWrapper(Stream stream, ArchiveOpenMode openMode = ArchiveOpenMode.Read, bool leaveOpen = false)
    {
        OpenMode = openMode;
        _leaveOpen = leaveOpen;
        Stream = stream;

        ValidateStreamForArchiveOpenMode(Stream);

        if (Stream.CanSeek)
            return;

        switch (openMode)
        {
            case ArchiveOpenMode.Create:
                Stream = new PositionPreservingWriteOnlyStreamWrapper(Stream);
                break;
            case ArchiveOpenMode.Read:
                try
                {
                    _backingStream = Stream;
                    Stream = new MemoryStream();
                    _backingStream.CopyTo(Stream);
                    Stream.Seek(0, SeekOrigin.Begin);
                }
                catch
                {
                    if (_backingStream != null)
                        Stream.Dispose();

                    throw;
                }

                break;
            case ArchiveOpenMode.Update:
            default:
                Debug.Assert(openMode == ArchiveOpenMode.Update);
                break;
        }
    }

    /// <summary>Validate the given stream for the specified archive open mode</summary>
    /// <exception cref="ArgumentException">The stream is already closed. -or- mode is incompatible with the capabilities of the stream.</exception>
    /// <exception cref="ArgumentOutOfRangeException">mode specified an invalid value.</exception>
    /// <param name="stream">The input or output stream.</param>
    private void ValidateStreamForArchiveOpenMode(Stream stream)
    {
        switch (OpenMode)
        {
            case ArchiveOpenMode.Create:
                if (!stream.CanWrite)
                    throw new ArgumentException("The provided stream can not be used to create a new archive", nameof(stream));
                break;
            case ArchiveOpenMode.Read:
                if (!stream.CanRead)
                    throw new ArgumentException("The provided stream can not be used to read an archive", nameof(stream));
                break;
            case ArchiveOpenMode.Update:
                if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek)
                    throw new ArgumentException("The provided stream can not be used to update an archive", nameof(stream));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(OpenMode));
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            try
            {
                OnStreamClosing?.Invoke(this);
            }
            finally
            {
                CloseStreams();
                _isDisposed = true;
            }
        }
        GC.SuppressFinalize(this);
    }

    private void CloseStreams()
    {
        if (!_leaveOpen)
        {
            Stream.Dispose();
            _backingStream?.Dispose();
        }
        else
        {
            if (_backingStream != null)
            {
                // The original stream was assigned to BackingStream and Stream was a copy created for seeking
                Stream.Dispose();
            }
        }
    }
}
