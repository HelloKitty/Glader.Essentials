using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// A large generic array pool that supports pooling of any size arrays.
	/// By default, .NET Shared Array Pool will only use DefaultMaxArrayLength = 1024 * 1024.
	/// See: https://github.com/dotnet/runtime/blob/6221ddb3051463309801c9008f332b34361da798/src/libraries/System.Private.CoreLib/src/System/Buffers/ConfigurableArrayPool.cs#L9
	/// </summary>
	public static class LargeArrayPool<T>
	{
		// See: https://github.com/dotnet/runtime/blob/6221ddb3051463309801c9008f332b34361da798/src/libraries/System.Private.CoreLib/src/System/Buffers/ConfigurableArrayPool.cs#L11C9-L14C66
		/// <summary>The default maximum length of each array in the pool (2^20).</summary>
		private const int DefaultMaxArrayLength = 0x40000000; // .NET doc says this is the max. See: https://github.com/dotnet/runtime/blob/6221ddb3051463309801c9008f332b34361da798/src/libraries/System.Private.CoreLib/src/System/Buffers/ConfigurableArrayPool.cs#L29C71-L29C81

		/// <summary>The default maximum number of arrays per bucket that are available for rent.</summary>
		private const int DefaultMaxNumberOfArraysPerBucket = 50;

		// TODO: Fall back to normal array pool for smaller array allocs.
		/// <summary>
		/// Shared large array pool buffer with maximum array size of 0x40000000 or 1,073,741,824 or 2^30.
		/// (Max size is limited by .NET)
		/// </summary>
		public static ArrayPool<T> Shared { get; } = ArrayPool<T>.Create(DefaultMaxArrayLength, DefaultMaxNumberOfArraysPerBucket);
	}
}
