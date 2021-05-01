using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Indicates if the provided type parameter is exactly the specified generic type parameter.
		/// (Warning: If B : A then B.Is{A} is false. Only exact matching.
		/// </summary>
		/// <typeparam name="T">The parameter type to check.</typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this Type type)
		{
			return type == typeof(T);
		}
	}
}
