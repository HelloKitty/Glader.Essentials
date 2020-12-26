using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contracts for types that can provide a <see cref="Span{T}"/>.
	/// Generally used to represent the object as a <see cref="Span{T}"/>.
	/// </summary>
	/// <typeparam name="T">The element type of the span.</typeparam>
	public interface ISpanProvidable<T>
	{
		/// <summary>
		/// Provides a <see cref="Span{T}"/>.
		/// </summary>
		/// <returns>A valid <see cref="Span{T}"/></returns>
		Span<T> GetSpan();
	}
}
