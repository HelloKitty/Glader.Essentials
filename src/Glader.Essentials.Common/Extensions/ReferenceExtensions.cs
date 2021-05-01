using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	public static class ReferenceExtensions
	{
		/// <summary>
		/// Indicates if a provided reference value is null.
		/// </summary>
		/// <typeparam name="T">The generic type of the reference value.</typeparam>
		/// <param name="reference">The reference value.</param>
		/// <returns>True if the provided reference is null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNull<T>(this T reference)
			where T : class
		{
			return reference == null;
		}

		/// <summary>
		/// Indicates if a provided reference value is not null.
		/// </summary>
		/// <typeparam name="T">The generic type of the reference value.</typeparam>
		/// <param name="reference">The reference value.</param>
		/// <returns>True if the provided reference is not null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNotNull<T>(this T reference)
			where T : class
		{
			//Just invert
			return !reference.IsNull<T>();
		}
	}
}
