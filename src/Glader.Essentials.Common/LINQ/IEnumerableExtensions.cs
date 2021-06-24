using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// LINQ-like implementation of <see cref="List{T}"/>'s ForEach method.
		/// Iterates the provided <see cref="enumerable"/> and calls the provided <see cref="action"/>
		/// </summary>
		/// <typeparam name="T">Element type.</typeparam>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="action">Action to call for each element.</param>
		/// <returns>The enumerable.</returns>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));

			if (enumerable == null)
				return null;

			foreach (var t in enumerable)
				action(t);

			return enumerable;
		}
	}
}
