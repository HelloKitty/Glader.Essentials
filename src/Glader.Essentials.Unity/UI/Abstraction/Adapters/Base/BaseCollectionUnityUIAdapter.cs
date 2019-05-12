using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Provider the ability for ordered collections of element Type <typeparamref name="TAdaptedToType"/>
	/// to be adapted and registered together.
	/// </summary>
	public abstract class BaseCollectionUnityUIAdapter<TAdaptedToType> : BaseUnityUI<IReadOnlyCollection<TAdaptedToType>>, IReadOnlyCollection<TAdaptedToType>
	{
		protected abstract IEnumerable<TAdaptedToType> Elements { get; }

		/// <inheritdoc />
		public IEnumerator<TAdaptedToType> GetEnumerator()
		{
			return ((IEnumerable<TAdaptedToType>)Elements).GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Elements.GetEnumerator();
		}

		/// <inheritdoc />
		public int Count => Elements.Count();
	}
}
