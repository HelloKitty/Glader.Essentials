using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Generic dictionary with <see cref="TKey"/> key types.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <typeparam name="TKey"></typeparam>
	public class EntityGuidDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IReadonlyEntityGuidMappable<TKey, TValue>, IEntityGuidMappable<TKey, TValue>
	{
		public EntityGuidDictionary()
		{

		}

		public EntityGuidDictionary(IEqualityComparer<TKey> keyComparer)
			: base(keyComparer)
		{

		}

		public EntityGuidDictionary(int capacity)
			: base(4, capacity)
		{

		}

		public EntityGuidDictionary(int capacity, IEqualityComparer<TKey> keyComparer)
			: base(4, capacity, keyComparer)
		{

		}

		public EntityGuidDictionary(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{

		}

		/// <inheritdoc />
		public bool RemoveEntityEntry(TKey entityGuid)
		{
			return TryRemove(entityGuid, out var temp);
		}
	}
}