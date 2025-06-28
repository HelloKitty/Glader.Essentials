using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Decorator for wrapping <see cref="Stream"/> with reliable counting semantics.
	/// (Ex. <see cref="NetworkStream"/> will not allow for calling Position).
	/// See: <see cref="BytesRead"/>.
	/// </summary>
	public class CountingStream : Stream
	{
		private readonly Stream _inner;
		private long _bytesRead;

		/// <summary>
		/// Represents the bytes read from the stream.
		/// </summary>
		public long BytesRead => Interlocked.Read(ref _bytesRead);

		/// <inheritdoc />
		public CountingStream(Stream inner)
		{
			_inner = inner ?? throw new ArgumentNullException(nameof(inner));
		}

		/// <inheritdoc />
		public override int Read(byte[] buffer, int offset, int count)
		{
			var n = _inner.Read(buffer, offset, count);
			if(n > 0) Interlocked.Add(ref _bytesRead, n);
			return n;
		}

		/// <inheritdoc />
		public override async Task<int> ReadAsync(
			byte[] buffer, int offset, int count, CancellationToken token)
		{
			var n = await _inner.ReadAsync(buffer, offset, count, token).ConfigureAwait(false);
			if(n > 0) Interlocked.Add(ref _bytesRead, n);
			return n;
		}

		/// <inheritdoc />
		public override bool CanRead => _inner.CanRead;

		/// <inheritdoc />
		public override bool CanSeek => false;

		/// <inheritdoc />
		public override bool CanWrite => _inner.CanWrite;

		/// <inheritdoc />
		public override long Length => _inner.Length;

		/// <inheritdoc />
		public override long Position
		{
			get => BytesRead;
			set => throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override void Flush() => _inner.Flush();

		/// <inheritdoc />
		public override long Seek(long o, SeekOrigin w)
			=> throw new NotSupportedException();

		/// <inheritdoc />
		public override void SetLength(long value)
			=> _inner.SetLength(value);

		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
			=> _inner.Write(buffer, offset, count);
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if(disposing) _inner.Dispose();
			base.Dispose(disposing);
		}

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
		/// <inheritdoc />
		public override int Read(Span<byte> buffer)
		{
			var n = _inner.Read(buffer);
			if(n > 0) Interlocked.Add(ref _bytesRead, n);
			return n;
		}

		/// <inheritdoc />
		public override ValueTask<int> ReadAsync(
			Memory<byte> buffer, CancellationToken ct = default)
		{
			return new ValueTask<int>(ReadAsyncImpl(buffer, ct));

			async Task<int> ReadAsyncImpl(Memory<byte> buff, CancellationToken cancel)
			{
				var n = await _inner.ReadAsync(buff, cancel).ConfigureAwait(false);
				if(n > 0) Interlocked.Add(ref _bytesRead, n);
				return n;
			}
		}
#endif
	}
}
