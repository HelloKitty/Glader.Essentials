using System;
using System.Collections.Generic;
using System.Linq;
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

		/// <summary>
		/// Extension that attempts to downcast the provided <see cref="collection"/>
		/// to an array if it is an array. Otherwise it will call <see cref="Enumerable"/>.ToArray().
		/// </summary>
		/// <typeparam name="T">The element type of the collection.</typeparam>
		/// <param name="collection">The collection to cast.</param>
		/// <returns></returns>
		public static T[] ToArrayTryAvoidCopy<T>(this IEnumerable<T> collection)
		{
			if(collection == null) throw new ArgumentNullException(nameof(collection));

			if(collection is T[] downcastedArray)
				return downcastedArray;

			return collection.ToArray();
		}
	}
}
