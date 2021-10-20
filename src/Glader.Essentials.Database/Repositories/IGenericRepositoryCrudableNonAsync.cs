using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a simple generic crud repository using non-async.
	/// (Create, read, update and delete)
	/// </summary>
	/// <typeparam name="TKeyType">The key type of the model.</typeparam>
	/// <typeparam name="TModelType">The model type.</typeparam>
	public interface IGenericRepositoryCrudableNonAsync<in TKeyType, TModelType>
	{
		/// <summary>
		/// Returns true if a model with the provided <see cref="key"/>
		/// is contained within the repository.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True if the Repository contains the model.</returns>
		bool Contains(TKeyType key);

		/// <summary>
		/// Tries to crate a new entry of the provided <see cref="model"/>
		/// in the repository.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		bool TryCreate(TKeyType key, TModelType model);

		/// <summary>
		/// Attempts to read/retreieve the model from the
		/// repository.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		TModelType Retrieve(TKeyType key);

		//TODO: Add Update

		/// <summary>
		/// Attempts to remove a model with the <see cref="key"/>
		/// if it exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool TryDelete(TKeyType key);

		/// <summary>
		/// Attempts to update a model with the provided <see cref="key"/>.
		/// Setting the new model to the provided <see cref="model"/> if it exists.
		/// To create use Create, not update.
		/// </summary>
		/// <param name="key">The key for the entity.</param>
		/// <param name="model">The model to replace the current model.</param>
		/// <returns>Awaitable.</returns>
		void Update(TKeyType key, TModelType model);
	}
}
