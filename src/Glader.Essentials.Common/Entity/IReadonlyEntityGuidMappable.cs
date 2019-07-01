using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IReadonlyEntityGuidMappable<in TKeyType, out TValueType>
	{
		/// <summary>
		/// Gets the element that has the specified key in the read-only dictionary.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
		/// <returns>The element that has the specified key in the read-only dictionary.</returns>
		TValueType this[TKeyType key] { get; }

		/// <summary>
		/// Determines whether the read-only dictionary contains an element that has the specified key.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		bool ContainsKey(TKeyType key);
	}
}