using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for DB tables that can be queried for their entire contents.
	/// </summary>
	/// <typeparam name="TContentType"></typeparam>
	public interface IEntireTableQueryable<TContentType>
	{
		/// <summary>
		/// Retrieves every <typeparamref name="TContentType"/> entry in the table.
		/// </summary>
		/// <param name="token">Cancel token.</param>
		/// <returns>An array of all entries in the table.</returns>
		Task<TContentType[]> RetrieveAllAsync(CancellationToken token = default);
	}
}
