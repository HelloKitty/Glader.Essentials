﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a simple generic crud repository.
	/// (Create, read, update and delete)
	/// </summary>
	/// <typeparam name="TKey">The key type of the model.</typeparam>
	/// <typeparam name="TModel">The model type.</typeparam>
	public interface IGenericRepositoryCrudable<in TKey, TModel>
	{
		/// <summary>
		/// Returns true if a model with the provided <see cref="key"/>
		/// is contained within the repository.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="token">Cancel token.</param>
		/// <returns>True if the Repository contains the model.</returns>
		Task<bool> ContainsAsync(TKey key, CancellationToken token = default); 

		/// <summary>
		/// Tries to crate a new entry of the provided <see cref="model"/>
		/// in the repository.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<bool> TryCreateAsync(TModel model, CancellationToken token = default);

		/// <summary>
		/// Attempts to read/retreieve the model from the
		/// repository.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<TModel> RetrieveAsync(TKey key, CancellationToken token = default, bool includeNavigationProperties = false);

		//TODO: Add Update

		/// <summary>
		/// Attempts to remove a model with the <see cref="key"/>
		/// if it exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> TryDeleteAsync(TKey key, CancellationToken token = default);

		/// <summary>
		/// Attempts to update a model with the provided <see cref="key"/>.
		/// Setting the new model to the provided <see cref="model"/> if it exists.
		/// To create use Create, not update.
		/// </summary>
		/// <param name="key">The key for the entity.</param>
		/// <param name="model">The model to replace the current model.</param>
		/// <returns>Awaitable.</returns>
		Task UpdateAsync(TKey key, TModel model, CancellationToken token = default);
	}
}
