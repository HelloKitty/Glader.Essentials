using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	//From GladMMO
	public static class EntityMappableExtensions
	{
		/// <summary>
		/// Replaces an existing Entity with key <see cref="key"/> with the value <see cref="obj"/>
		/// if it already exists. Throws if the data does not exist.
		/// </summary>
		/// <typeparam name="TReturnType">The type of object to add.</typeparam>
		/// <typeparam name="TKeyType"></typeparam>
		/// <param name="collection">The entity collection.</param>
		/// <param name="key">The key.</param>
		/// <param name="obj">The object to add.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReplaceObject<TKeyType, TReturnType>(this IEntityGuidMappable<TKeyType, TReturnType> collection, TKeyType key, TReturnType obj)
		{
			//No null checking because we hope to inline this.
			try
			{
				//We strictly enforce that the entity be known/existing in this component collection.
				if(collection.ContainsKey(key))
					collection[key] = obj; //Replaces the existing object.
				else
					CreateEntityDoesNotExistException<TKeyType, TReturnType>(key);
			}
			catch(Exception e)
			{
				CreateEntityCollectionException<TKeyType, TReturnType>(key, e);
			}
		}

		/// <summary>
		/// Adds the entity object with the provided key <see cref="key"/> if it doesn't exists.
		/// If the key already exists it will throw.
		/// </summary>
		/// <typeparam name="TReturnType">The type of object to add.</typeparam>
		/// <typeparam name="TKeyType"></typeparam>
		/// <param name="collection">The entity collection.</param>
		/// <param name="key">The entity guid.</param>
		/// <param name="obj">The object to add.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddObject<TKeyType, TReturnType>(this IEntityGuidMappable<TKeyType, TReturnType> collection, TKeyType key, TReturnType obj)
		{
			//No null checking because we hope to inline this.
			try
			{
				collection.Add(key, obj); //Does NOT replace the entity if there is one.
			}
			catch(Exception e)
			{
				CreateEntityCollectionException<TKeyType, TReturnType>(key, e);
			}
		}

		private static void CreateEntityDoesNotExistException<TKeyType, TReturnType>(TKeyType key)
		{
			if(key == null) throw new ArgumentNullException(nameof(key), $"Found that provided entity guid in {nameof(CreateEntityCollectionException)} was null.");

			throw new InvalidOperationException($"Entity does not exist in Collection {typeof(TReturnType).Name} from Entity: {key}.");
		}

		private static void CreateEntityCollectionException<TKeyType, TReturnType>(TKeyType key, Exception e)
		{
			if(key == null) throw new ArgumentNullException(nameof(key), $"Found that provided entity guid in {nameof(CreateEntityCollectionException)} was null.");
			if(e == null) throw new ArgumentNullException(nameof(e), $"Found that provided inner exception in {nameof(CreateEntityCollectionException)} was null.");

			throw new InvalidOperationException($"Failed to access {typeof(TReturnType).Name} from Entity: {key}. Error: {e.Message}");
		}

		/// <summary>
		/// Retrieve the object of type <typeparamref name="TReturnType"/>
		/// from the entity mapped collection.
		/// </summary>
		/// <typeparam name="TReturnType">The entity mapped object.</typeparam>
		/// <typeparam name="TKeyType"></typeparam>
		/// <param name="collection">The entity collection.</param>
		/// <param name="key">The entity guid.</param>
		/// <exception cref="InvalidOperationException">Throws if the entity does not have data mapped to it.</exception>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReturnType RetrieveEntity<TKeyType, TReturnType>(this IReadonlyEntityGuidMappable<TKeyType, TReturnType> collection, TKeyType key)
		{
			//No null checking because we hope to inline this
			try
			{
				return collection[key];
			}
			catch(Exception e)
			{
				CreateEntityCollectionException<TKeyType, TReturnType>(key, e);
			}

			Debug.Assert(false, "Should never reach this point in RetrieveEntity.");
			//Should never be reached.
			return default(TReturnType);
		}
	}
}