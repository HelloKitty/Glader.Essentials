using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IReadonlyEntityGuidMappable<in TKeyType, TValueType>
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

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.
		/// This parameter is passed uninitialized.</param>
		/// <returns>True if the it contains an element with the specified key; otherwise, false.</returns>
		bool TryGetValue(TKeyType key, out TValueType value);

		/*/// <summary>
		/// Retrieves the entry similar to the indexer but
		/// as the <see cref="KeyValuePair{TKey,TValue}"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
		/// <returns>The <see cref="KeyValuePair{TKey,TValue}"/> that has the specified key in the read-only dictionary.</returns>
		KeyValuePair<TKeyType, TValueType> GetAsKeyValuePair(TKeyType key);*/
	}
}
 