using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// In-memory <see cref="ConcurrentDictionary{TKey,TValue}"/>-based <see cref="IGenericRepositoryCrudableNonAsync{TKey,TModel}"/> implementation.
	/// </summary>
	/// <typeparam name="TKeyType"></typeparam>
	/// <typeparam name="TModelType"></typeparam>
	public abstract class BaseInMemoryRepositoryCrudableNonAsync<TKeyType, TModelType> : IGenericRepositoryCrudableNonAsync<TKeyType, TModelType>
	{
		private ConcurrentDictionary<TKeyType, TModelType> InternalMap { get; }

		protected BaseInMemoryRepositoryCrudableNonAsync(ConcurrentDictionary<TKeyType, TModelType> internalMap)
		{
			InternalMap = internalMap ?? throw new ArgumentNullException(nameof(internalMap));
		}

		protected BaseInMemoryRepositoryCrudableNonAsync(IEqualityComparer<TKeyType> comparer)
			: this(new ConcurrentDictionary<TKeyType, TModelType>(comparer))
		{
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));
		}

		protected BaseInMemoryRepositoryCrudableNonAsync()
			: this(new ConcurrentDictionary<TKeyType, TModelType>())
		{
			
		}

		/// <inheritdoc />
		public bool Contains(TKeyType key)
		{
			return InternalMap.ContainsKey(key);
		}

		/// <inheritdoc />
		public bool TryCreate(TKeyType key, TModelType model)
		{
			return InternalMap.TryAdd(key, model);
		}

		/// <inheritdoc />
		public TModelType Retrieve(TKeyType key)
		{
			return InternalMap[key];
		}

		/// <inheritdoc />
		public bool TryDelete(TKeyType key)
		{
			return InternalMap.TryRemove(key, out var _);
		}

		/// <inheritdoc />
		public void Update(TKeyType key, TModelType model)
		{
			InternalMap[key] = model;
		}
	}
}
