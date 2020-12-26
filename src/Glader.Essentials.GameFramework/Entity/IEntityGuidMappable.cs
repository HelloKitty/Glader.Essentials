using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IEntityGuidMappable<in TKeyType, TValueType> : IReadonlyEntityGuidMappable<TKeyType, TValueType>, IEntityCollectionRemovable<TKeyType>
	{
		/// <summary>
		/// Gets the element that has the specified key in the read-only dictionary.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
		/// <returns>The element that has the specified key in the read-only dictionary.</returns>
		new TValueType this[TKeyType key] { get; set; }

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="IEntityGuidMappable{TKeyType,TValueType}"/>
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="IEntityGuidMappable{TKeyType,TValueType}"/></exception>
		void Add(TKeyType key, TValueType value);
	}
}