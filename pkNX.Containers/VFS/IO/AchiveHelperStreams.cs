// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers.VFS;

internal sealed class WrappedStream : Stream
{
    private readonly Stream _baseStream;
    private readonly bool _closeBaseStream;

    // Delegate that will be invoked on stream disposing
    private readonly Action<BaseArchiveEntry?>? _onClosed;

    // Instance that will be passed to _onClose delegate
    private readonly BaseArchiveEntry? _archiveEntry;
    private bool _isDisposed;

    internal WrappedStream(Stream baseStream, bool closeBaseStream)
        : this(baseStream, closeBaseStream, null, null) { }

    private WrappedStream(Stream baseStream, bool closeBaseStream, BaseArchiveEntry? entry, Action<BaseArchiveEntry?>? onClosed)
    {
        _baseStream = baseStream;
        _closeBaseStream = closeBaseStream;
        _onClosed = onClosed;
        _archiveEntry = entry;
        _isDisposed = false;
    }

    internal WrappedStream(Stream baseStream, BaseArchiveEntry entry, Action<BaseArchiveEntry?>? onClosed)
        : this(baseStream, false, entry, onClosed) { }

    public override long Length
    {
        get
        {
            ThrowIfDisposed();
            return _baseStream.Length;
        }
    }

    public override long Position
    {
        get
        {
            ThrowIfDisposed();
            return _baseStream.Position;
        }
        set
        {
            ThrowIfDisposed();
            ThrowIfCantSeek();

            _baseStream.Position = value;
        }
    }

    public override bool CanRead => !_isDisposed && _baseStream.CanRead;

    public override bool CanSeek => !_isDisposed && _baseStream.CanSeek;

    public override bool CanWrite => !_isDisposed && _baseStream.CanWrite;

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().ToString(), "A stream from BaseArchiveEntry has been disposed.");
    }

    private void ThrowIfCantRead()
    {
        if (!CanRead)
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support reading.");
    }

    private void ThrowIfCantWrite()
    {
        if (!CanWrite)
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support writing.");
    }

    private void ThrowIfCantSeek()
    {
        if (!CanSeek)
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        ThrowIfCantRead();

        return _baseStream.Read(buffer, offset, count);
    }

    public override int Read(Span<byte> buffer)
    {
        ThrowIfDisposed();
        ThrowIfCantRead();

        return _baseStream.Read(buffer);
    }

    public override int ReadByte()
    {
        ThrowIfDisposed();
        ThrowIfCantRead();

        return _baseStream.ReadByte();
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ThrowIfCantRead();

        return _baseStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ThrowIfCantRead();

        return _baseStream.ReadAsync(buffer, cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        ThrowIfDisposed();
        ThrowIfCantSeek();

        return _baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        ThrowIfDisposed();
        ThrowIfCantSeek();
        ThrowIfCantWrite();

        _baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        _baseStream.Write(buffer, offset, count);
    }

    public override void Write(ReadOnlySpan<byte> source)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        _baseStream.Write(source);
    }

    public override void WriteByte(byte value)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        _baseStream.WriteByte(value);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        return _baseStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        return _baseStream.WriteAsync(buffer, cancellationToken);
    }

    public override void Flush()
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        _baseStream.Flush();
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ThrowIfCantWrite();

        return _baseStream.FlushAsync(cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _onClosed?.Invoke(_archiveEntry);

            if (_closeBaseStream)
                _baseStream.Dispose();

            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}

internal sealed class SubReadStream : Stream
{
    private readonly long _startInSuperStream;
    private long _positionInSuperStream;
    private readonly long _endInSuperStream;
    private readonly Stream _superStream;
    private bool _canRead;
    private bool _isDisposed;

    public SubReadStream(Stream superStream, long startPosition, long maxLength)
    {
        _startInSuperStream = startPosition;
        _positionInSuperStream = startPosition;
        _endInSuperStream = startPosition + maxLength;
        _superStream = superStream;
        _canRead = true;
        _isDisposed = false;
    }

    public override long Length
    {
        get
        {
            ThrowIfDisposed();

            return _endInSuperStream - _startInSuperStream;
        }
    }

    public override long Position
    {
        get
        {
            ThrowIfDisposed();

            return _positionInSuperStream - _startInSuperStream;
        }
        set
        {
            ThrowIfDisposed();

            throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
        }
    }

    public override bool CanRead => _superStream.CanRead && _canRead;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().ToString(), "A stream from BaseArchiveEntry has been disposed.");
    }

    private void ThrowIfCantRead()
    {
        if (!CanRead)
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support reading.");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        // parameter validation sent to _superStream.Read
        int origCount = count;

        ThrowIfDisposed();
        ThrowIfCantRead();

        if (_superStream.Position != _positionInSuperStream)
            _superStream.Seek(_positionInSuperStream, SeekOrigin.Begin);
        if (_positionInSuperStream + count > _endInSuperStream)
            count = (int)(_endInSuperStream - _positionInSuperStream);

        Debug.Assert(count >= 0);
        Debug.Assert(count <= origCount);

        int ret = _superStream.Read(buffer, offset, count);

        _positionInSuperStream += ret;
        return ret;
    }

    public override int Read(Span<byte> destination)
    {
        // parameter validation sent to _superStream.Read
        int origCount = destination.Length;
        int count = destination.Length;

        ThrowIfDisposed();
        ThrowIfCantRead();

        if (_superStream.Position != _positionInSuperStream)
            _superStream.Seek(_positionInSuperStream, SeekOrigin.Begin);
        if (_positionInSuperStream + count > _endInSuperStream)
            count = (int)(_endInSuperStream - _positionInSuperStream);

        Debug.Assert(count >= 0);
        Debug.Assert(count <= origCount);

        int ret = _superStream.Read(destination[..count]);

        _positionInSuperStream += ret;
        return ret;
    }

    public override int ReadByte()
    {
        byte b = default;
        return Read(new Span<byte>(ref b)) == 1 ? b : -1;
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ValidateBufferArguments(buffer, offset, count);
        return ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ThrowIfCantRead();
        return Core(buffer, cancellationToken);

        async ValueTask<int> Core(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            if (_superStream.Position != _positionInSuperStream)
            {
                _superStream.Seek(_positionInSuperStream, SeekOrigin.Begin);
            }

            if (_positionInSuperStream > _endInSuperStream - buffer.Length)
            {
                buffer = buffer[..(int)(_endInSuperStream - _positionInSuperStream)];
            }

            int ret = await _superStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);

            _positionInSuperStream += ret;
            return ret;
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
    }

    public override void SetLength(long value)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("SetLength requires a stream that supports seeking and writing.");
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("This stream from BaseArchiveEntry does not support writing.");
    }

    public override void Flush()
    {
        ThrowIfDisposed();
        throw new NotSupportedException("This stream from BaseArchiveEntry does not support writing.");
    }

    // Close the stream for reading.  Note that this does NOT close the superStream (since
    // the substream is just 'a chunk' of the super-stream
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _canRead = false;
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}

internal sealed class DirectToArchiveWriterStream : Stream
{
    private long _position;
    private readonly Stream _stream;
    private bool _everWritten;
    private bool _isDisposed;
    private readonly BaseArchiveEntry _entry;
    private bool _canWrite;

    // makes the assumption that somewhere down the line, stream is eventually writing directly to the archive
    // this class calls other functions on BaseArchiveEntry that write directly to the archive
    public DirectToArchiveWriterStream(Stream stream, BaseArchiveEntry entry)
    {
        _position = 0;
        _stream = stream;
        _everWritten = false;
        _isDisposed = false;
        _entry = entry;
        _canWrite = true;
    }

    public override long Length
    {
        get
        {
            ThrowIfDisposed();
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
        }
    }
    public override long Position
    {
        get
        {
            ThrowIfDisposed();
            return _position;
        }
        set
        {
            ThrowIfDisposed();
            throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
        }
    }

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => _canWrite;

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().ToString(), "A stream from BaseArchiveEntry has been disposed.");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("This stream from BaseArchiveEntry does not support reading.");
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("This stream from BaseArchiveEntry does not support seeking.");
    }

    public override void SetLength(long value)
    {
        ThrowIfDisposed();
        throw new NotSupportedException("SetLength requires a stream that supports seeking and writing.");
    }

    // careful: assumes that write is the only way to write to the stream, if writebyte/beginwrite are implemented
    // they must set _everWritten, etc.
    public override void Write(byte[] buffer, int offset, int count)
    {
        ValidateBufferArguments(buffer, offset, count);

        ThrowIfDisposed();
        Debug.Assert(CanWrite);

        // if we're not actually writing anything, we don't want to trigger the header
        if (count == 0)
            return;

        if (!_everWritten)
        {
            _everWritten = true;
            // write local header, we are good to go
            //_entry.WriteLocalFileHeader(isEmptyFile: false);
        }

        _stream.Write(buffer, offset, count);
        _position += count;
    }

    public override void Write(ReadOnlySpan<byte> source)
    {
        ThrowIfDisposed();
        Debug.Assert(CanWrite);

        // if we're not actually writing anything, we don't want to trigger the header
        if (source.Length == 0)
            return;

        if (!_everWritten)
        {
            _everWritten = true;
            // write local header, we are good to go
            //_entry.WriteLocalFileHeader(isEmptyFile: false);
        }

        _stream.Write(source);
        _position += source.Length;
    }

    public override void WriteByte(byte value) =>
        Write(new ReadOnlySpan<byte>(in value));

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ValidateBufferArguments(buffer, offset, count);
        return WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        Debug.Assert(CanWrite);

        return !buffer.IsEmpty ?
            Core(buffer, cancellationToken) :
            default;

        async ValueTask Core(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            if (!_everWritten)
            {
                _everWritten = true;
                // write local header, we are good to go
                //_entry.WriteLocalFileHeader(isEmptyFile: false);
            }

            await _stream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
            _position += buffer.Length;
        }
    }

    public override void Flush()
    {
        ThrowIfDisposed();
        Debug.Assert(CanWrite);

        _stream.Flush();
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        Debug.Assert(CanWrite);

        return _stream.FlushAsync(cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _stream.Dispose(); // now we have size/crc info

            if (!_everWritten)
            {
                // write local header, no data, so we use stored
                //_entry.WriteLocalFileHeader(isEmptyFile: true);
            }
            else
            {
                // go back and finish writing
                /*if (_entry.Archive.ArchiveStream.CanSeek)
                    // finish writing local header if we have seek capabilities
                    _entry.WriteCrcAndSizesInLocalHeader();
                else
                    // write out data descriptor if we don't have seek capabilities
                    _entry.WriteDataDescriptor();*/
            }
            _canWrite = false;
            _isDisposed = true;
        }

        base.Dispose(disposing);
    }
}
