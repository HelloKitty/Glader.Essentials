using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	//From SwanSong
	/// <summary>
	/// <see cref="Lazy{T}"/>-based implementation of <see cref="IDeferredDependency{TDependencyType}"/>.
	/// </summary>
	/// <typeparam name="TDependencyType"></typeparam>
	public sealed class LazilyDeferredDependency<TDependencyType> : IDeferredDependency<TDependencyType>
	{
		private Lazy<TDependencyType> DeferredDependency { get; }

		/// <inheritdoc />
		public TDependencyType Dependency => DeferredDependency.Value;

		public LazilyDeferredDependency(Lazy<TDependencyType> deferredDependency)
		{
			DeferredDependency = deferredDependency ?? throw new ArgumentNullException(nameof(deferredDependency));
		}
	}
}
