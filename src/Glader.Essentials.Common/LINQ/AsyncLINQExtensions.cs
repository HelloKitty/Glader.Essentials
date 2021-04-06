using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	public static class AsyncLINQExtensions
	{
		/// <summary>
		/// Converts the source <see cref="IEnumerable{T}"/> of tasks
		/// to an array of awaited results.
		/// </summary>
		/// <typeparam name="T">The awaited result type.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>Awaitable that yields an array of awaited results from the source.</returns>
		public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<Task<T>> source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			return await Task.WhenAll(source);
		}
	}
}
