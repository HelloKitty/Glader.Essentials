using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Glader.Essentials
{
	/// <summary>
	/// Simple implementation of <see cref="IGenericRepositoryCrudable{TKey,TModel}"/>.
	/// Based on EF Core's <see cref="DbContext"/>.
	/// </summary>
	/// <typeparam name="TKey">The Model key type.</typeparam>
	/// <typeparam name="TModelType">The model type.</typeparam>
	public class GeneralGenericCrudRepositoryProvider<TKey, TModelType> : IGenericRepositoryCrudable<TKey, TModelType> 
		where TModelType : class
	{
		/// <summary>
		/// Internal model table set.
		/// </summary>
		protected DbSet<TModelType> ModelSet { get; }

		/// <summary>
		/// Internal db context.
		/// </summary>
		protected DbContext Context { get; }

		/// <inheritdoc />
		public GeneralGenericCrudRepositoryProvider(DbSet<TModelType> modelSet, DbContext context)
		{
			ModelSet = modelSet ?? throw new ArgumentNullException(nameof(modelSet));
			Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <inheritdoc />
		public async Task<bool> ContainsAsync(TKey key, CancellationToken token = default)
		{
			return await RetrieveAsync(key, token) != null;
		}

		/// <inheritdoc />
		public async Task<bool> TryCreateAsync(TModelType model, CancellationToken token = default)
		{
			//TODO: Should we validate no key already exists?
			ModelSet.Add(model);
			return await SaveAndCheckResultsAsync(token);
		}

		private async Task<bool> SaveAndCheckResultsAsync(CancellationToken token = default)
		{
			return await Context.SaveChangesAsync(token) != 0;
		}

		/// <inheritdoc />
		public virtual async Task<TModelType> RetrieveAsync(TKey key, CancellationToken token = default, bool includeNavigationProperties = false)
		{
			if (includeNavigationProperties)
			{
				TModelType model = await RetrieveAsync(key, token, false);

				foreach(var navigation in Context.Entry(model).Navigations)
				{
					navigation.Load();
				}

				return model;
			}
			else
				return await ModelSet.FindAsync(new object[1] { key }, token);
		}

		/// <inheritdoc />
		public async Task<bool> TryDeleteAsync(TKey key, CancellationToken token = default)
		{
			//If it doesn't exist then this will just fail, so get out soon.
			if(!await ContainsAsync(key, token))
				return false;

			TModelType modelType = await RetrieveAsync(key, token);
			ModelSet.Remove(modelType);

			return await SaveAndCheckResultsAsync(token);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TKey key, TModelType model, CancellationToken token = default)
		{
			if(!await ContainsAsync(key, token))
				throw new InvalidOperationException($"Cannot update model with Key: {key} as it does not exist.");

			//TODO: is this slow? Is there a better way to deal with tracked entities?
			Context.Entry(await RetrieveAsync(key, token)).State = EntityState.Detached;
			ModelSet.Update(model);

			await SaveAndCheckResultsAsync(token);
		}
	}
}
