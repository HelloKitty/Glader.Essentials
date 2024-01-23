using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for collections that can have entries
	/// removed via a <see cref="TKey"/>.
	/// </summary>
	public interface IEntityCollectionRemovable<in TKey>
	{
		// We now spit out the object too, just incase some processing is needed to be done on it.
		/// <summary>
		/// Attempts to remove the collection based
		/// on the provided <see cref="entityGuid"/> key.
		/// </summary>
		/// <param name="entityGuid">The key.</param>
		/// <param name="obj">The removed object if it existed.</param>
		/// <returns>True if removed.</returns>
		bool RemoveEntityEntry(TKey entityGuid, out object obj);
	}

	public static class IEntityCollectionRemovableExtensions
	{
		// This now exists to ensure compatibility with the original API which did not return or allow access to the removed object.
		/// <summary>
		/// Attempts to remove the collection based
		/// on the provided <see cref="entityGuid"/> key.
		/// </summary>
		/// <param name="removable">The removable.</param>
		/// <param name="entityGuid">The key.</param>
		/// <returns>True if removed.</returns>
		public static bool RemoveEntityEntry<TKey>(this IEntityCollectionRemovable<TKey> removable, TKey entityGuid)
		{
			if (removable == null) throw new ArgumentNullException(nameof(removable));
			return removable.RemoveEntityEntry(entityGuid, out var _);
		}
	}
}